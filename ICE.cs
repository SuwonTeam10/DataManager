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
            // ★ tubPath 인자 추가
            void ExecuteTrain(string path, string tubPath, bool useVenv, Action<string> onLogReceived);
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
                if (wslPath.Length >= 2 && wslPath[1] == ':')
                {
                    wslPath = $"/mnt/{char.ToLower(wslPath[0])}{wslPath.Substring(2)}";
                }
                else if (wslPath.StartsWith("//wsl$/") || wslPath.StartsWith("//wsl.localhost/"))
                {
                    string withoutPrefix = wslPath.Replace("//wsl.localhost/", "").Replace("//wsl$/", "");
                    int firstSlashIndex = withoutPrefix.IndexOf('/');
                    if (firstSlashIndex != -1) wslPath = withoutPrefix.Substring(firstSlashIndex);
                }
                return wslPath;
            }

            public void ExecuteTrain(string path, string tubPath, bool useVenv, Action<string> onLogReceived)
            {
                string wslPath = ConvertToWslPath(path);
                string wslTubPath = ConvertToWslPath(tubPath); // ★ 윈도우 경로를 WSL 경로로 변환
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string base64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyCode));
                string patchCmd = $"echo {base64Code} | base64 -d > /tmp/patch_mro.py && {venvCmd}python -u /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug 'numpy<2.0'";
                string ensureModelsDirCmd = "mkdir -p models";

                // ★ 기존 findTubsCmd(하드코딩 추측)는 버리고, 선택한 경로(wslTubPath)를 직접 주입합니다!
                string resizePyCode =
                    "import os, shutil, sys, json, time\n" +
                    "from PIL import Image\n" +
                    "marker='/tmp/datamanager_train_dir.txt'\n" +
                    $"src_arg='{wslTubPath}'\n" +
                    "dst_arg='.datamanager_train_160x120'\n" +
                    "if not src_arg or not dst_arg:\n" +
                    "  print(f'[Resize Error] resize path missing. src={src_arg!r}, dst={dst_arg!r}', flush=True)\n" +
                    "  sys.exit(1)\n" +
                    "src=os.path.abspath(src_arg)\n" +
                    "dst=os.path.abspath(dst_arg)\n" +
                    "target=(160,120)\n" +
                    "resample=getattr(getattr(Image, 'Resampling', Image), 'LANCZOS', Image.BICUBIC)\n" +
                    "large=[]\n" +
                    "bom_files=[]\n" +
                    "manifest_fix=False\n" +
                    "catalog_manifest_fix=False\n" +
                    "checked=0\n" +
                    "for root, dirs, files in os.walk(src):\n" +
                    "  for name in files:\n" +
                    "    lower=name.lower()\n" +
                    "    path=os.path.join(root,name)\n" +
                    "    if lower.endswith(('.json','.catalog')):\n" +
                    "      try:\n" +
                    "        with open(path, 'rb') as f:\n" +
                    "          if f.read(3) == b'\\xef\\xbb\\xbf': bom_files.append(os.path.relpath(path, src))\n" +
                    "        if lower == 'manifest.json':\n" +
                    "          with open(path, 'r', encoding='utf-8-sig') as f:\n" +
                    "            lines=[line.strip() for line in f if line.strip()]\n" +
                    "          if len(lines) < 5:\n" +
                    "            manifest_fix=True\n" +
                    "          else:\n" +
                    "            json.loads(lines[0]); json.loads(lines[1]); json.loads(lines[2]); json.loads(lines[3]); json.loads(lines[4])\n" +
                    "      except Exception as e:\n" +
                    "        if lower == 'manifest.json': manifest_fix=True\n" +
                    "        print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "    if lower.endswith('.catalog'):\n" +
                    "      manifest_name=os.path.splitext(name)[0]+'.catalog_manifest'\n" +
                    "      manifest_path=os.path.join(root, manifest_name)\n" +
                    "      if not os.path.exists(manifest_path):\n" +
                    "        catalog_manifest_fix=True\n" +
                    "      else:\n" +
                    "        try:\n" +
                    "          with open(manifest_path, 'r', encoding='utf-8-sig') as f:\n" +
                    "            json.loads(f.readline())\n" +
                    "        except Exception:\n" +
                    "          catalog_manifest_fix=True\n" +
                    "    if not lower.endswith(('.jpg','.jpeg','.png','.bmp')): continue\n" +
                    "    checked+=1\n" +
                    "    try:\n" +
                    "      with Image.open(path) as img:\n" +
                    "        if img.width > target[0] or img.height > target[1]:\n" +
                    "          large.append(os.path.relpath(path, src))\n" +
                    "    except Exception as e:\n" +
                    "      print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "if not os.path.exists(os.path.join(src, 'manifest.json')):\n" +
                    "  manifest_fix=True\n" +
                    "if not large and not bom_files and not manifest_fix and not catalog_manifest_fix:\n" +
                    "  open(marker, 'w').write(src_arg)\n" +
                    "  print(f'[Resize] no images larger than 160x120. train with original tub. checked={checked}', flush=True)\n" +
                    "  sys.exit(0)\n" +
                    "if os.path.exists(dst): shutil.rmtree(dst)\n" +
                    "shutil.copytree(src, dst)\n" +
                    "bom_cleaned=0\n" +
                    "for rel in bom_files:\n" +
                    "  path=os.path.join(dst, rel)\n" +
                    "  try:\n" +
                    "    with open(path, 'rb') as f: data=f.read()\n" +
                    "    if data.startswith(b'\\xef\\xbb\\xbf'):\n" +
                    "      with open(path, 'wb') as f: f.write(data[3:])\n" +
                    "      bom_cleaned+=1\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "catalog_paths=sorted([name for name in os.listdir(dst) if name.startswith('catalog_') and name.endswith('.catalog')])\n" +
                    "catalog_manifest_fixed=0\n" +
                    "image_path_fixed=0\n" +
                    "record_offset=0\n" +
                    "for catalog_name in catalog_paths:\n" +
                    "  catalog_path=os.path.join(dst, catalog_name)\n" +
                    "  manifest_name=os.path.splitext(catalog_name)[0]+'.catalog_manifest'\n" +
                    "  catalog_manifest_path=os.path.join(dst, manifest_name)\n" +
                    "  line_lengths=[]\n" +
                    "  try:\n" +
                    "    updated_records=[]\n" +
                    "    changed_catalog=False\n" +
                    "    with open(catalog_path, 'r', encoding='utf-8-sig') as f:\n" +
                    "      for line in f:\n" +
                    "        if not line.strip(): continue\n" +
                    "        record=json.loads(line)\n" +
                    "        image_name=record.get('cam/image_array')\n" +
                    "        if isinstance(image_name, str) and image_name and not os.path.exists(os.path.join(dst, image_name)):\n" +
                    "          candidate=os.path.join('images', os.path.basename(image_name))\n" +
                    "          if os.path.exists(os.path.join(dst, candidate)):\n" +
                    "            record['cam/image_array']=candidate\n" +
                    "            changed_catalog=True\n" +
                    "            image_path_fixed+=1\n" +
                    "        updated_records.append(record)\n" +
                    "    if changed_catalog:\n" +
                    "      with open(catalog_path, 'w', encoding='utf-8', newline='') as f:\n" +
                    "        for record in updated_records:\n" +
                    "          f.write(json.dumps(record, sort_keys=True) + '\\n')\n" +
                    "    with open(catalog_path, 'r', encoding='utf-8-sig', newline='') as f:\n" +
                    "      for line in f:\n" +
                    "        if line.strip(): line_lengths.append(len(line if line.endswith('\\n') else line+'\\n'))\n" +
                    "    manifest_contents={'path': manifest_name, 'created_at': time.time(), 'start_index': record_offset, 'line_lengths': line_lengths}\n" +
                    "    with open(catalog_manifest_path, 'w', encoding='utf-8') as f:\n" +
                    "      f.write(json.dumps(manifest_contents, sort_keys=True) + '\\n')\n" +
                    "    catalog_manifest_fixed+=1\n" +
                    "    record_offset+=len(line_lengths)\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] catalog manifest fix failed: {catalog_path}: {e}', flush=True)\n" +
                    "manifest_fixed=0\n" +
                    "manifest_path=os.path.join(dst, 'manifest.json')\n" +
                    "if manifest_fix:\n" +
                    "  try:\n" +
                    "    raw_lines=[]\n" +
                    "    if os.path.exists(manifest_path):\n" +
                    "      with open(manifest_path, 'r', encoding='utf-8-sig') as f:\n" +
                    "        raw_lines=[line.strip() for line in f if line.strip()]\n" +
                    "    first=json.loads(raw_lines[0]) if raw_lines else {}\n" +
                    "    inputs=first.get('inputs', []) if isinstance(first, dict) else first\n" +
                    "    types=first.get('types', []) if isinstance(first, dict) else (json.loads(raw_lines[1]) if len(raw_lines) > 1 else [])\n" +
                    "    if not inputs: inputs=['cam/image_array','user/angle','user/throttle','user/mode']\n" +
                    "    if not types: types=['image_array','float','float','str']\n" +
                    "    current_index=0\n" +
                    "    for catalog_name in catalog_paths:\n" +
                    "      with open(os.path.join(dst, catalog_name), 'r', encoding='utf-8-sig') as f:\n" +
                    "        current_index += sum(1 for line in f if line.strip())\n" +
                    "    max_len=max(1000, current_index if current_index else 1000)\n" +
                    "    fixed_lines=[inputs, types, {}, {'created_at': time.time()}, {'paths': catalog_paths, 'current_index': current_index, 'max_len': max_len, 'deleted_indexes': []}]\n" +
                    "    with open(manifest_path, 'w', encoding='utf-8') as f:\n" +
                    "      for item in fixed_lines:\n" +
                    "        f.write(json.dumps(item) + '\\n')\n" +
                    "    manifest_fixed=1\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] manifest fix failed: {e}', flush=True)\n" +
                    "resized=0\n" +
                    "for rel in large:\n" +
                    "  path=os.path.join(dst, rel)\n" +
                    "  try:\n" +
                    "    with Image.open(path) as img:\n" +
                    "      img=img.convert('RGB').resize(target, resample)\n" +
                    "      img.save(path)\n" +
                    "      resized+=1\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "open(marker, 'w').write(dst_arg)\n" +
                    "print(f'[Resize] created training tub copy: {dst} / checked={checked} / resized={resized} / bom_cleaned={bom_cleaned} / manifest_fixed={manifest_fixed} / catalog_manifest_fixed={catalog_manifest_fixed} / image_path_fixed={image_path_fixed}', flush=True)\n";

                string resizeBase64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(resizePyCode));
                string resizeTrainTubCmd = $"echo {resizeBase64Code} | base64 -d > /tmp/resize_tub_for_training.py && {venvCmd}python -u /tmp/resize_tub_for_training.py && rm /tmp/resize_tub_for_training.py";

                string cleanConfigCmd = "if [ -f myconfig.py ]; then sed -i '/EARLY_STOP_PATIENCE/d' myconfig.py; fi";

                // ★ docopt 에러 방지를 위해 등호(=)와 따옴표 명시적 추가
                string trainCmd = $"TRAIN_TUB=\\$(cat /tmp/datamanager_train_dir.txt) && echo [Train] tub path: \\$TRAIN_TUB && {venvCmd}python -u train.py --tubs=\"\\$TRAIN_TUB\" --model=models/mypilot.h5; TRAIN_STATUS=\\$?; if [ \"\\$TRAIN_TUB\" = \".datamanager_train_160x120\" ]; then rm -rf \"\\$TRAIN_TUB\"; fi; rm -f /tmp/datamanager_train_dir.txt; if [ \\$TRAIN_STATUS -eq 0 ]; then echo '---TRAINING_COMPLETE---'; else exit \\$TRAIN_STATUS; fi";

                string fullCmd = $"cd '{wslPath}' && {patchCmd} && {ensureEnvCmd} && {resizeTrainTubCmd} && {ensureModelsDirCmd} && {cleanConfigCmd} && {trainCmd}";

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

                // 인성님의 텐서플로우 스케일링 코드(255.0 나누기) 완벽 보존!
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
                    "    img = Image.open(img_path).convert('RGB').resize((160, 120))\n" +
                    "    img_arr = np.expand_dims(np.asarray(img).astype('float32') / 255.0, axis=0)\n" +
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

            public void ExecuteTrain(string path, string tubPath, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/.bashrc && conda activate e2e_env && " : "";

                // ★ 원격 접속용 상대경로 폴더명 추출
                string folderName = System.IO.Path.GetFileName(tubPath.TrimEnd('\\', '/'));
                string remoteTubPath = $"./{folderName}";

                string pyCode = "import donkeycar, os\np=os.path.join(os.path.dirname(donkeycar.__file__), 'pipeline', 'sequence.py')\nif os.path.exists(p):\n  c=open(p).read()\n  c=c.replace('class TfmIterator(Generic[R, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmIterator(SizedIterator):')\n  c=c.replace('class TfmTupleIterator(Generic[X, Y, XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class TfmTupleIterator(SizedIterator):')\n  c=c.replace('class BaseTfmIterator_(Generic[XOut, YOut],  SizedIterator[Tuple[XOut, YOut]]):', 'class BaseTfmIterator_(SizedIterator):')\n  open(p,'w').write(c)";
                string base64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pyCode));
                string patchCmd = $"echo {base64Code} | base64 -d > /tmp/patch_mro.py && {venvCmd}python -u /tmp/patch_mro.py && rm /tmp/patch_mro.py";

                string ensureEnvCmd = $"{venvCmd}pip install -q imgaug \"numpy<2.0\"";
                string ensureModelsDirCmd = "mkdir -p models";

                // ★ 기존의 하드코딩 삭제하고 선택한 원격경로(remoteTubPath) 사용
                string resizePyCode =
                    "import os, shutil, sys, json, time\n" +
                    "from PIL import Image\n" +
                    "marker='/tmp/datamanager_train_dir.txt'\n" +
                    $"src_arg='{remoteTubPath}'\n" +
                    "dst_arg='.datamanager_train_160x120'\n" +
                    "if not src_arg or not dst_arg:\n" +
                    "  print(f'[Resize Error] resize path missing. src={src_arg!r}, dst={dst_arg!r}', flush=True)\n" +
                    "  sys.exit(1)\n" +
                    "src=os.path.abspath(src_arg)\n" +
                    "dst=os.path.abspath(dst_arg)\n" +
                    "target=(160,120)\n" +
                    "resample=getattr(getattr(Image, 'Resampling', Image), 'LANCZOS', Image.BICUBIC)\n" +
                    "large=[]\n" +
                    "bom_files=[]\n" +
                    "manifest_fix=False\n" +
                    "catalog_manifest_fix=False\n" +
                    "checked=0\n" +
                    "for root, dirs, files in os.walk(src):\n" +
                    "  for name in files:\n" +
                    "    lower=name.lower()\n" +
                    "    path=os.path.join(root,name)\n" +
                    "    if lower.endswith(('.json','.catalog')):\n" +
                    "      try:\n" +
                    "        with open(path, 'rb') as f:\n" +
                    "          if f.read(3) == b'\\xef\\xbb\\xbf': bom_files.append(os.path.relpath(path, src))\n" +
                    "        if lower == 'manifest.json':\n" +
                    "          with open(path, 'r', encoding='utf-8-sig') as f:\n" +
                    "            lines=[line.strip() for line in f if line.strip()]\n" +
                    "          if len(lines) < 5:\n" +
                    "            manifest_fix=True\n" +
                    "          else:\n" +
                    "            json.loads(lines[0]); json.loads(lines[1]); json.loads(lines[2]); json.loads(lines[3]); json.loads(lines[4])\n" +
                    "      except Exception as e:\n" +
                    "        if lower == 'manifest.json': manifest_fix=True\n" +
                    "        print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "    if lower.endswith('.catalog'):\n" +
                    "      manifest_name=os.path.splitext(name)[0]+'.catalog_manifest'\n" +
                    "      manifest_path=os.path.join(root, manifest_name)\n" +
                    "      if not os.path.exists(manifest_path):\n" +
                    "        catalog_manifest_fix=True\n" +
                    "      else:\n" +
                    "        try:\n" +
                    "          with open(manifest_path, 'r', encoding='utf-8-sig') as f:\n" +
                    "            json.loads(f.readline())\n" +
                    "        except Exception:\n" +
                    "          catalog_manifest_fix=True\n" +
                    "    if not lower.endswith(('.jpg','.jpeg','.png','.bmp')): continue\n" +
                    "    checked+=1\n" +
                    "    try:\n" +
                    "      with Image.open(path) as img:\n" +
                    "        if img.width > target[0] or img.height > target[1]:\n" +
                    "          large.append(os.path.relpath(path, src))\n" +
                    "    except Exception as e:\n" +
                    "      print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "if not os.path.exists(os.path.join(src, 'manifest.json')):\n" +
                    "  manifest_fix=True\n" +
                    "if not large and not bom_files and not manifest_fix and not catalog_manifest_fix:\n" +
                    "  open(marker, 'w').write(src_arg)\n" +
                    "  print(f'[Resize] no images larger than 160x120. train with original tub. checked={checked}', flush=True)\n" +
                    "  sys.exit(0)\n" +
                    "if os.path.exists(dst): shutil.rmtree(dst)\n" +
                    "shutil.copytree(src, dst)\n" +
                    "bom_cleaned=0\n" +
                    "for rel in bom_files:\n" +
                    "  path=os.path.join(dst, rel)\n" +
                    "  try:\n" +
                    "    with open(path, 'rb') as f: data=f.read()\n" +
                    "    if data.startswith(b'\\xef\\xbb\\xbf'):\n" +
                    "      with open(path, 'wb') as f: f.write(data[3:])\n" +
                    "      bom_cleaned+=1\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "catalog_paths=sorted([name for name in os.listdir(dst) if name.startswith('catalog_') and name.endswith('.catalog')])\n" +
                    "catalog_manifest_fixed=0\n" +
                    "record_offset=0\n" +
                    "for catalog_name in catalog_paths:\n" +
                    "  catalog_path=os.path.join(dst, catalog_name)\n" +
                    "  manifest_name=os.path.splitext(catalog_name)[0]+'.catalog_manifest'\n" +
                    "  catalog_manifest_path=os.path.join(dst, manifest_name)\n" +
                    "  line_lengths=[]\n" +
                    "  try:\n" +
                    "    with open(catalog_path, 'r', encoding='utf-8-sig', newline='') as f:\n" +
                    "      for line in f:\n" +
                    "        if line.strip(): line_lengths.append(len(line if line.endswith('\\n') else line+'\\n'))\n" +
                    "    manifest_contents={'path': manifest_name, 'created_at': time.time(), 'start_index': record_offset, 'line_lengths': line_lengths}\n" +
                    "    with open(catalog_manifest_path, 'w', encoding='utf-8') as f:\n" +
                    "      f.write(json.dumps(manifest_contents, sort_keys=True) + '\\n')\n" +
                    "    catalog_manifest_fixed+=1\n" +
                    "    record_offset+=len(line_lengths)\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] catalog manifest fix failed: {catalog_path}: {e}', flush=True)\n" +
                    "manifest_fixed=0\n" +
                    "manifest_path=os.path.join(dst, 'manifest.json')\n" +
                    "if manifest_fix:\n" +
                    "  try:\n" +
                    "    raw_lines=[]\n" +
                    "    if os.path.exists(manifest_path):\n" +
                    "      with open(manifest_path, 'r', encoding='utf-8-sig') as f:\n" +
                    "        raw_lines=[line.strip() for line in f if line.strip()]\n" +
                    "    first=json.loads(raw_lines[0]) if raw_lines else {}\n" +
                    "    inputs=first.get('inputs', []) if isinstance(first, dict) else first\n" +
                    "    types=first.get('types', []) if isinstance(first, dict) else (json.loads(raw_lines[1]) if len(raw_lines) > 1 else [])\n" +
                    "    if not inputs: inputs=['cam/image_array','user/angle','user/throttle','user/mode']\n" +
                    "    if not types: types=['image_array','float','float','str']\n" +
                    "    current_index=0\n" +
                    "    for catalog_name in catalog_paths:\n" +
                    "      with open(os.path.join(dst, catalog_name), 'r', encoding='utf-8-sig') as f:\n" +
                    "        current_index += sum(1 for line in f if line.strip())\n" +
                    "    max_len=max(1000, current_index if current_index else 1000)\n" +
                    "    fixed_lines=[inputs, types, {}, {'created_at': time.time()}, {'paths': catalog_paths, 'current_index': current_index, 'max_len': max_len, 'deleted_indexes': []}]\n" +
                    "    with open(manifest_path, 'w', encoding='utf-8') as f:\n" +
                    "      for item in fixed_lines:\n" +
                    "        f.write(json.dumps(item) + '\\n')\n" +
                    "    manifest_fixed=1\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] manifest fix failed: {e}', flush=True)\n" +
                    "resized=0\n" +
                    "for rel in large:\n" +
                    "  path=os.path.join(dst, rel)\n" +
                    "  try:\n" +
                    "    with Image.open(path) as img:\n" +
                    "      img=img.convert('RGB').resize(target, resample)\n" +
                    "      img.save(path)\n" +
                    "      resized+=1\n" +
                    "  except Exception as e:\n" +
                    "    print(f'[Resize Warning] {path}: {e}', flush=True)\n" +
                    "open(marker, 'w').write(dst_arg)\n" +
                    "print(f'[Resize] created training tub copy: {dst} / checked={checked} / resized={resized} / bom_cleaned={bom_cleaned} / manifest_fixed={manifest_fixed} / catalog_manifest_fixed={catalog_manifest_fixed}', flush=True)\n";

                string resizeBase64Code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(resizePyCode));
                string resizeTrainTubCmd = $"echo {resizeBase64Code} | base64 -d > /tmp/resize_tub_for_training.py && {venvCmd}python -u /tmp/resize_tub_for_training.py && DIR=$(cat /tmp/datamanager_train_dir.txt) && rm /tmp/resize_tub_for_training.py /tmp/datamanager_train_dir.txt";

                string cleanConfigCmd = "if [ -f myconfig.py ]; then sed -i '/EARLY_STOP_PATIENCE/d' myconfig.py; fi";

                // ★ docopt 에러 방지를 위해 등호(=)와 따옴표 명시적 추가 완료!
                string trainCmd = $"{venvCmd}python -u train.py --tubs=\"$DIR\" --model=\"models/mypilot.h5\"; TRAIN_STATUS=$?; if [ \"$DIR\" = \".datamanager_train_160x120\" ]; then rm -rf \"$DIR\"; fi; if [ $TRAIN_STATUS -eq 0 ]; then echo '---TRAINING_COMPLETE---'; else exit $TRAIN_STATUS; fi";

                RunRemoteCommand(path, $"{patchCmd} && {ensureEnvCmd} && {resizeTrainTubCmd} && {ensureModelsDirCmd} && {cleanConfigCmd} && {trainCmd}", onLogReceived);
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
                    "    img = Image.open(img_path).convert('RGB').resize((160, 120))\n" +
                    "    img_arr = np.expand_dims(np.asarray(img).astype('float32') / 255.0, axis=0)\n" +
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
