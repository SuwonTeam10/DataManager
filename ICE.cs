using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Renci.SshNet;

namespace DataManager
{
    public class ICE
    {
        public interface ICommandExecutor
        {
            void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived);
            void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived);
            void Stop();
            void Cancel();
        }

        // 1. 로컬(윈도우 내 우분투 WSL)용 실행기
        public class LocalExecutor : ICommandExecutor
        {
            private Process _process;

            // 윈도우 경로를 리눅스(WSL) 경로로 자동 변환해 주는 마법의 함수 (예: C:\data -> /mnt/c/data)
            private string ConvertToWslPath(string winPath)
            {
                string wslPath = winPath.Replace("\\", "/");
                if (wslPath.Length >= 2 && wslPath[1] == ':')
                {
                    wslPath = $"/mnt/{char.ToLower(wslPath[0])}{wslPath.Substring(2)}";
                }
                return wslPath;
            }

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                string wslPath = ConvertToWslPath(path);
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // [만능 패치] 원격과 동일하게 WSL 내부에서도 자동 복구 적용!
                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string patchCmd = $"cat << 'EOF' > /tmp/patch_mro.py\n{pyCode}\nEOF\n{venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR='data/'; elif [ -f data/data3/manifest.json ]; then DIR='data/data3/'; elif [ -f data3/manifest.json ]; then DIR='data3/'; else DIR='data/'; fi";

                string trainCmd = $"{venvCmd}python train.py --tubs $DIR --model models/mypilot.h5";

                // 명령어들을 하나로 묶음
                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {trainCmd}";

                // 핵심: 윈도우 python이 아니라 wsl(우분투)을 호출하여 bash 쉘 안에서 실행!
                RunProcess("wsl", $"bash -ic \"{fullCmd}\"", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived)
            {
                string wslPath = ConvertToWslPath(path);
                string wslModelPath = ConvertToWslPath(modelPath);
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string patchCmd = $"cat << 'EOF' > /tmp/patch_mro.py\n{pyCode}\nEOF\n{venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";
                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";

                string testCmd = $"{venvCmd}python manage.py drive --model '{wslModelPath}'";

                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {testCmd}";

                RunProcess("wsl", $"bash -ic \"{fullCmd}\"", onLogReceived);
            }

            private void RunProcess(string fileName, string arguments, Action<string> onLogReceived)
            {
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName, 
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                _process.OutputDataReceived += (s, e) => { if (e.Data != null) onLogReceived(e.Data); };
                _process.ErrorDataReceived += (s, e) => { if (e.Data != null) onLogReceived(e.Data); };
                _process.Start();
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
            }

            public void Stop() => Cancel();

            public void Cancel()
            {
                try { if (_process != null && !_process.HasExited) _process.Kill(); } catch { }
            }
        }

        // 2. 원격용 실행기 (SSH 기능 전용)
        public class RemoteExecutor : ICommandExecutor
        {
            private SshClient _ssh;
            private ShellStream _shell;

            public RemoteExecutor(string host, string user, string pass)
            {
                _ssh = new SshClient(host, user, pass);
                _ssh.Connect();
                _shell = _ssh.CreateShellStream("e2e_env", 80, 24, 800, 600, 1024);
            }

            // ★ 추가: 원격 서버에 Ctrl+C 신호를 보내 파이썬만 멈춤 (연결은 유지)
            public void Cancel()
            {
                if (_shell != null) _shell.Write("\x03");
            }

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                // 1. 가상환경 활성화 
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // 2. 패치: MRO 에러 자동 해결 스크립트
                // 사용자가 Donkeycar를 어디에 설치했든(경로 무관), 파이썬이 알아서 설치 위치를 역추적하여 
                // 파이썬 3.11+ 버전 충돌(MRO) 에러를 일으키는 클래스 3개 수정
                string pyCode =
                    "import donkeycar, os\n" +
                    "p=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\n" +
                    "if os.path.exists(p):\n" +
                    "  c=open(p).read()\n" +
                    "  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n" +
                    "  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n" +
                    "  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n" +
                    "  open(p,'w').write(c)";
                string patchCmd = $"cat << 'EOF' > /tmp/patch_mro.py\n{pyCode}\nEOF\n{venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                // 3.  패치 2: 의존성(패키지) 자동 복구
                // 남들의 컴퓨터에 imgaug가 없거나, numpy 버전이 너무 높아 충돌할 경우를 대비해
                // -q(quiet) 옵션으로 필요한 버전을 맞춘다. (정상이면 패스)
                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";

                // 4. 패치 3: 주행 데이터(Tub) 폴더 동적 탐색
                // 압축을 어떻게 풀었든 상관없이, manifest.json 파일이 존재하는 진짜 폴더(data, data3 등)를 
                // 리눅스 쉘이 스스로 찾아 DIR 변수에 담는다
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR=\"data/\"; " +
                                     "elif [ -f data/data3/manifest.json ]; then DIR=\"data/data3/\"; " +
                                     "elif [ -f data3/manifest.json ]; then DIR=\"data3/\"; " +
                                     "else DIR=\"data/\"; fi";

                // 5. 찾아낸 동적 경로($DIR)를 주입하여 학습 최종 실행
                string trainCmd = $"{venvCmd}python train.py --tubs $DIR --model models/mypilot.h5";

                // 위 4단계를 하나의 파이프라인(&&)으로 묶어 서버에 전송 (경로는 사용자가 UI에서 선택한 path 적용)
                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {trainCmd}", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // 1. [만능 패치 1] MRO 에러 자동 해결 스크립트 (테스트 시에도 동일하게 파이썬 모듈을 부르므로 필수)
                string pyCode =
                    "import donkeycar, os\n" +
                    "p=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\n" +
                    "if os.path.exists(p):\n" +
                    "  c=open(p).read()\n" +
                    "  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n" +
                    "  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n" +
                    "  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n" +
                    "  open(p,'w').write(c)";
                string patchCmd = $"cat << 'EOF' > /tmp/patch_mro.py\n{pyCode}\nEOF\n{venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                // 2. [만능 패치 2] 의존성(패키지) 자동 복구
                // 테스트(drive)할 때도 TensorFlow를 쓰기 때문에 numpy 버전 충돌을 무조건 막아야 합니다.
                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";

                // 3. 모델 테스트 실행 (사용자가 선택한 모델 파일 경로 주입)
                string testCmd = $"{venvCmd}python manage.py drive --model {modelPath}";

                // 패치 -> 패키지 확인 -> 모델 테스트를 논스톱으로 실행!
                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {testCmd}", onLogReceived);
            }

            private void RunRemoteCommand(string path, string command, Action<string> onLogReceived)
            {
                _shell.WriteLine($"cd {path} && {command}");
                Task.Run(() => { while (true) { var line = _shell.ReadLine(); if (line != null) onLogReceived(line); } });
            }

            public void Stop()
            {
                if (_shell != null) _shell.Dispose();
                if (_ssh != null) { _ssh.Disconnect(); _ssh.Dispose(); }
            }
        }
    }
}