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
            void ExecuteTest(string path, string modelPath, string testPath, bool useVenv, Action<string> onLogReceived);
            void Stop();
            void Cancel();
        }

        // ==========================================
        // 1. 로컬(윈도우 내 우분투 WSL)용 실행기
        // ==========================================
        public class LocalExecutor : ICommandExecutor
        {
            private Process _process;

            private string ConvertToWslPath(string winPath)
            {
                string wslPath = winPath.Replace("\\", "/");

                // 1. 일반 윈도우 드라이브 경로 처리 (예: C:\data -> /mnt/c/data)
                if (wslPath.Length >= 2 && wslPath[1] == ':')
                {
                    wslPath = $"/mnt/{char.ToLower(wslPath[0])}{wslPath.Substring(2)}";
                }
                // 2. 윈도우11 WSL 네트워크 경로 처리 (예: \\wsl.localhost\Ubuntu-22.04\home\... -> /home/...)
                else if (wslPath.StartsWith("//wsl$/") || wslPath.StartsWith("//wsl.localhost/"))
                {
                    string withoutPrefix = wslPath.Replace("//wsl.localhost/", "").Replace("//wsl$/", "");
                    int firstSlashIndex = withoutPrefix.IndexOf('/');
                    if (firstSlashIndex != -1) wslPath = withoutPrefix.Substring(firstSlashIndex);
                }

                return wslPath;
            }

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                string wslPath = ConvertToWslPath(path);
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // Base64 압축 인젝션 기법 도입 (터미널 씹힘 원천 차단)
                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string base64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyCode));
                string patchCmd = $"echo {base64Code} | base64 -d > /tmp/patch_mro.py && {venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR='data/'; elif [ -f data/data3/manifest.json ]; then DIR='data/data3/'; elif [ -f data3/manifest.json ]; then DIR='data3/'; else DIR='data/'; fi";
                string trainCmd = $"{venvCmd}python train.py --tubs $DIR --model models/mypilot.h5";

                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {trainCmd}";
                RunProcess("wsl", $"bash -ic \"{fullCmd}\"", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, string testPath, bool useVenv, Action<string> onLogReceived)
            {
                string wslPath = ConvertToWslPath(path);
                string wslModelPath = ConvertToWslPath(modelPath);
                string wslTestPath = ConvertToWslPath(testPath);
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                string pyMroCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string pyMroBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyMroCode));
                string patchCmd = $"echo {pyMroBase64} | base64 -d > /tmp/patch_mro.py && {venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";

                string pyEvalCode =
                    "import os, sys, json, glob\n" +
                    "from donkeycar.parts.keras import KerasPilot\n" +
                    "from PIL import Image\n" +
                    "import numpy as np\n" +
                    "kl = KerasPilot()\n" +
                    "kl.load(sys.argv[1])\n" +
                    "tub_path = sys.argv[2]\n" +
                    "records = []\n" +
                    "for cat in glob.glob(os.path.join(tub_path, '**', 'catalog_*.catalog'), recursive=True):\n" +
                    "  for line in open(cat, 'r'):\n" +
                    "    if line.strip():\n" +
                    "      d = json.loads(line)\n" +
                    "      if d.get('cam/image_array'): records.append((os.path.join(os.path.dirname(cat), 'images', d['cam/image_array']), d.get('user/angle', 0.0)))\n" +
                    "if not records:\n" +
                    "  for j in glob.glob(os.path.join(tub_path, '**', 'record_*.json'), recursive=True):\n" +
                    "    d = json.load(open(j, 'r'))\n" +
                    "    if d.get('cam/image_array'): records.append((os.path.join(os.path.dirname(j), d['cam/image_array']), d.get('user/angle', 0.0)))\n" +
                    "print(f'[Info] 총 {len(records)}개의 이미지를 테스트합니다.', flush=True)\n" +
                    "for img_path, real_angle in records:\n" +
                    "  if os.path.exists(img_path):\n" +
                    "    pred = kl.run(np.asarray(Image.open(img_path)))\n" +
                    "    print(f'image={img_path} real={real_angle:.2f} predict={pred[0] if isinstance(pred, tuple) else pred:.2f}', flush=True)";

                string pyEvalBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyEvalCode));
                string evalScriptCmd = $"echo {pyEvalBase64} | base64 -d > /tmp/eval_model.py";

                string testCmd = $"{venvCmd}python /tmp/eval_model.py '{wslModelPath}' '{wslTestPath}' && rm /tmp/eval_model.py";

                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {evalScriptCmd} && {testCmd}";
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
            public void Cancel() { try { if (_process != null && !_process.HasExited) _process.Kill(); } catch { } }
        }

        // ==========================================
        // 2. 원격용 실행기 (SSH 기능 전용)
        // ==========================================
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

            public void Cancel() { if (_shell != null) _shell.Write("\x03"); }

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // Base64 압축 인젝션
                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string base64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyCode));
                string patchCmd = $"echo {base64Code} | base64 -d > /tmp/patch_mro.py && {venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR=\"data/\"; elif [ -f data/data3/manifest.json ]; then DIR=\"data/data3/\"; elif [ -f data3/manifest.json ]; then DIR=\"data3/\"; else DIR=\"data/\"; fi";
                string trainCmd = $"{venvCmd}python train.py --tubs $DIR --model models/mypilot.h5";

                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {trainCmd}", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, string testPath, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                string pyMroCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string pyMroBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyMroCode));
                string patchCmd = $"echo {pyMroBase64} | base64 -d > /tmp/patch_mro.py && {venvCmd}python /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";

                string pyEvalCode =
                    "import os, sys, json, glob\n" +
                    "from donkeycar.parts.keras import KerasPilot\n" +
                    "from PIL import Image\n" +
                    "import numpy as np\n" +
                    "kl = KerasPilot()\n" +
                    "kl.load(sys.argv[1])\n" +
                    "tub_path = sys.argv[2]\n" +
                    "records = []\n" +
                    "for cat in glob.glob(os.path.join(tub_path, '**', 'catalog_*.catalog'), recursive=True):\n" +
                    "  for line in open(cat, 'r'):\n" +
                    "    if line.strip():\n" +
                    "      d = json.loads(line)\n" +
                    "      if d.get('cam/image_array'): records.append((os.path.join(os.path.dirname(cat), 'images', d['cam/image_array']), d.get('user/angle', 0.0)))\n" +
                    "if not records:\n" +
                    "  for j in glob.glob(os.path.join(tub_path, '**', 'record_*.json'), recursive=True):\n" +
                    "    d = json.load(open(j, 'r'))\n" +
                    "    if d.get('cam/image_array'): records.append((os.path.join(os.path.dirname(j), d['cam/image_array']), d.get('user/angle', 0.0)))\n" +
                    "print(f'[Info] 총 {len(records)}개의 이미지를 테스트합니다.', flush=True)\n" +
                    "for img_path, real_angle in records:\n" +
                    "  if os.path.exists(img_path):\n" +
                    "    pred = kl.run(np.asarray(Image.open(img_path)))\n" +
                    "    print(f'image={img_path} real={real_angle:.2f} predict={pred[0] if isinstance(pred, tuple) else pred:.2f}', flush=True)";

                string pyEvalBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyEvalCode));
                string evalScriptCmd = $"echo {pyEvalBase64} | base64 -d > /tmp/eval_model.py";

                // 서버 환경에서는 testPath로 윈도우 경로(C:\)가 넘어오면 작동하지 않으므로 주의가 필요합니다.
                string testCmd = $"{venvCmd}python /tmp/eval_model.py '{modelPath}' '{testPath}' && rm /tmp/eval_model.py";

                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {evalScriptCmd} && {testCmd}", onLogReceived);
            }

            private void RunRemoteCommand(string path, string command, Action<string> onLogReceived)
            {
                _shell.WriteLine($"cd {path} && {command}");
                Task.Run(() => { while (true) { var line = _shell.ReadLine(); if (line != null) onLogReceived(line); } });
            }

            public void Stop() { if (_shell != null) _shell.Dispose(); if (_ssh != null) { _ssh.Disconnect(); _ssh.Dispose(); } }
        }
    }
}
