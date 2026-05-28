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

                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string base64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyCode));
                string patchCmd = $"echo {base64Code} | base64 -d > /tmp/patch_mro.py && {venvCmd}python -u /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR='data/'; elif [ -f data/data3/manifest.json ]; then DIR='data/data3/'; elif [ -f data3/manifest.json ]; then DIR='data3/'; else DIR='data/'; fi";
                string ensureModelsDirCmd = "mkdir -p models";

                // ★ 과거에 새겨진 100번 강제 문신(찌꺼기)을 지워서 원래의 '조기 종료'로 원상복구!
                string cleanConfigCmd = "if [ -f myconfig.py ]; then sed -i '/EARLY_STOP_PATIENCE/d' myconfig.py; fi";

                string trainCmd = $"{venvCmd}python -u train.py --tubs $DIR --model models/mypilot.h5 && echo '---TRAINING_COMPLETE---'";

                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {ensureModelsDirCmd} && {cleanConfigCmd} && {trainCmd}";

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
                string patchCmd = $"echo {pyMroBase64} | base64 -d > /tmp/patch_mro.py && {venvCmd}python -u /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";

                string pyEvalCode =
                    "import os, sys, json, glob\n" +
                    "os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'\n" +
                    "import numpy as np\n" +
                    "from PIL import Image\n" +
                    "import tensorflow as tf\n" +
                    "model_path = os.path.abspath(sys.argv[1])\n" +
                    "if not os.path.exists(model_path):\n" +
                    "  print(f'[NO_MODEL] {model_path}', flush=True)\n" +
                    "  sys.exit(0)\n" +
                    "try:\n" +
                    "  model = tf.keras.models.load_model(model_path, compile=False)\n" +
                    "except Exception as e:\n" +
                    "  print(f'Error: 모델 로드 실패 - {e}', flush=True)\n" +
                    "  sys.exit(1)\n" +
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
                    "    img_arr = np.expand_dims(np.asarray(Image.open(img_path)), axis=0)\n" +
                    "    pred = model(img_arr, training=False)\n" +
                    "    pred_angle = float(np.array(pred[0] if isinstance(pred, (list, tuple)) else pred).flatten()[0])\n" +
                    "    print(f'image={img_path} real={real_angle:.2f} predict={pred_angle:.2f}', flush=True)\n" +
                    "print('Finished', flush=True)";

                string pyEvalBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyEvalCode));
                string evalScriptCmd = $"echo {pyEvalBase64} | base64 -d > /tmp/eval_model.py";

                string testCmd = $"{venvCmd}python -u /tmp/eval_model.py '{wslModelPath}' '{wslTestPath}' && rm /tmp/eval_model.py";

                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {evalScriptCmd} && {testCmd}";
                RunProcess("wsl", $"bash -ic \"{fullCmd}\"", onLogReceived);
            }

            private void RunProcess(string fileName, string arguments, Action<string> onLogReceived)
            {
                try
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
                catch (Exception ex)
                {
                    // ★ wsl 자체가 실행 안 될 때 침묵하지 않고 에러를 뱉어냅니다!
                    onLogReceived($"[Error] 로컬(WSL) 환경 실행 실패! WSL이 설치되어 있는지 확인하세요.\n상세오류: {ex.Message}");
                }
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

                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string base64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyCode));
                string patchCmd = $"echo {base64Code} | base64 -d > /tmp/patch_mro.py && {venvCmd}python -u /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";
                string findTubsCmd = "if [ -f data/manifest.json ]; then DIR=\"data/\"; elif [ -f data/data3/manifest.json ]; then DIR=\"data/data3/\"; elif [ -f data3/manifest.json ]; then DIR=\"data3/\"; else DIR=\"data/\"; fi";
                string ensureModelsDirCmd = "mkdir -p models";

                string cleanConfigCmd = "if [ -f myconfig.py ]; then sed -i '/EARLY_STOP_PATIENCE/d' myconfig.py; fi";

                string trainCmd = $"{venvCmd}python -u train.py --tubs $DIR --model models/mypilot.h5 && echo '---TRAINING_COMPLETE---'";

                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {findTubsCmd} && {ensureModelsDirCmd} && {cleanConfigCmd} && {trainCmd}", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, string testPath, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                string pyMroCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string pyMroBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyMroCode));
                string patchCmd = $"echo {pyMroBase64} | base64 -d > /tmp/patch_mro.py && {venvCmd}python -u /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";

                string pyEvalCode =
                    "import os, sys, json, glob\n" +
                    "os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'\n" +
                    "import numpy as np\n" +
                    "from PIL import Image\n" +
                    "import tensorflow as tf\n" +
                    "model_path = os.path.abspath(sys.argv[1])\n" +
                    "if not os.path.exists(model_path):\n" +
                    "  print(f'[NO_MODEL] {model_path}', flush=True)\n" +
                    "  sys.exit(0)\n" +
                    "try:\n" +
                    "  model = tf.keras.models.load_model(model_path, compile=False)\n" +
                    "except Exception as e:\n" +
                    "  print(f'Error: 모델 로드 실패 - {e}', flush=True)\n" +
                    "  sys.exit(1)\n" +
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
                    "    img_arr = np.expand_dims(np.asarray(Image.open(img_path)), axis=0)\n" +
                    "    pred = model(img_arr, training=False)\n" +
                    "    pred_angle = float(np.array(pred[0] if isinstance(pred, (list, tuple)) else pred).flatten()[0])\n" +
                    "    print(f'image={img_path} real={real_angle:.2f} predict={pred_angle:.2f}', flush=True)\n" +
                    "print('Finished', flush=True)";

                string pyEvalBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyEvalCode));
                string evalScriptCmd = $"echo {pyEvalBase64} | base64 -d > /tmp/eval_model.py";

                string testCmd = $"{venvCmd}python -u /tmp/eval_model.py '{modelPath}' '{testPath}' && rm /tmp/eval_model.py";

                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {evalScriptCmd} && {testCmd}", onLogReceived);
            }

            private void RunRemoteCommand(string path, string command, Action<string> onLogReceived)
            {
                try
                {
                    _shell.WriteLine($"cd {path} && {command}");
                    Task.Run(() => {
                        try { while (true) { var line = _shell.ReadLine(); if (line != null) onLogReceived(line); } }
                        catch { /* 통신 끊김 무시 */ }
                    });
                }
                catch (Exception ex)
                {
                    onLogReceived($"[Error] 원격 서버에 명령을 전송할 수 없습니다.\n상세오류: {ex.Message}");
                }
            }

            public void Stop() { if (_shell != null) _shell.Dispose(); if (_ssh != null) { _ssh.Disconnect(); _ssh.Dispose(); } }
        }
    }
}