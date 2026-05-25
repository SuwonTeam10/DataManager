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
        }

        // 1. 로컬용 실행기 (SSH 기능 없음! Process만 사용)
        public class LocalExecutor : ICommandExecutor
        {
            private Process _process;

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                // 윈도우 로컬 환경에 맞게 명령어 수정 (tubs 경로를 로컬에 맞게 세팅)
                // 윈도우는 리눅스 쉘 명령어(&&)를 못 쓰므로 명확하게 python train.py를 실행합니다.
                RunProcess(path, "train.py --tubs data/ --model models/mypilot.h5", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived)
            {
                RunProcess(path, $"train.py drive --model \"{modelPath}\"", onLogReceived);
            }

            private void RunProcess(string path, string arguments, Action<string> onLogReceived)
            {
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "python",
                        Arguments = arguments,
                        WorkingDirectory = path,
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

            public void Stop() => _process?.Kill();
        }

        // 2. 원격용 실행기 (SSH 기능 전용!)
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

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // 1. MRO 에러 자동 패치 스크립트 (기존 유지)
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

                // 2. 의존성 환경 검증 (기존 유지)
                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";

                // ★ [핵심 추가] 리눅스 쉘 조건문(if-elif)을 활용한 후보 폴더 자동 탐색 로직
                // data/, data/data3/, data3/ 폴더 중 진짜 주행 정보 파일(manifest.json)이 있는 경로를 0.1초 만에 찾아 DIR 변수에 할당합니다.
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR=\"data/\"; " +
                                     "elif [ -f data/data3/manifest.json ]; then DIR=\"data/data3/\"; " +
                                     "elif [ -f data3/manifest.json ]; then DIR=\"data3/\"; " +
                                     "else DIR=\"data/\"; fi";

                // 동적으로 찾아낸 $DIR 변수를 --tubs 옵션에 대입하여 학습을 실행합니다.
                string trainCmd = $"{venvCmd}python train.py --tubs $DIR --model models/mypilot.h5";

                // 서버에서 폴더 이동 -> MRO 패치 -> 환경 검증 -> 진짜 데이터 폴더 탐색 -> 학습 실행을 논스톱으로 처리
                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {trainCmd}", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";
                RunRemoteCommand(path, $"{venvCmd}python manage.py drive --model {modelPath}", onLogReceived);
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