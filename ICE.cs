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
            // useVenv 매개변수 추가됨!
            void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived);
            void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived);
            void Stop();
        }

        public class LocalExecutor : ICommandExecutor
        {
            private Process _process;
            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                RunProcess(path, "manage.py train", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived)
            {
                string args = $"manage.py drive --model \"{modelPath}\"";
                RunProcess(path, args, onLogReceived);
            }

            private void RunProcess(string path, string arguments, Action<string> onLogReceived)
            {
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo { FileName = "python", Arguments = arguments, WorkingDirectory = path, UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, CreateNoWindow = true }
                };
                _process.OutputDataReceived += (s, e) => { if (e.Data != null) onLogReceived(e.Data); };
                _process.ErrorDataReceived += (s, e) => { if (e.Data != null) onLogReceived(e.Data); };
                _process.Start();
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
            }
            public void Stop() => _process?.Kill();
        }

        public class RemoteExecutor : ICommandExecutor
        {
            private SshClient _ssh;
            private ShellStream _shell;

            public RemoteExecutor(string host, string user, string pass)
            {
                _ssh = new SshClient(host, user, pass);
                _ssh.Connect();
                _shell = _ssh.CreateShellStream("donkey", 80, 24, 800, 600, 1024);
            }

            public void ExecuteTrain(string path, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/env/bin/activate && " : "";
                RunRemoteCommand(path, $"{venvCmd}python manage.py train", onLogReceived);
            }

            public void ExecuteTest(string path, string modelPath, bool useVenv, Action<string> onLogReceived)
            {
                string venvCmd = useVenv ? "source ~/env/bin/activate && " : "";
                RunRemoteCommand(path, $"{venvCmd}python manage.py drive --model {modelPath}", onLogReceived);
            }

            private void RunRemoteCommand(string path, string command, Action<string> onLogReceived)
            {
                _shell.WriteLine($"cd {path} && {command}");
                Task.Run(() => { while (true) { var line = _shell.ReadLine(); if (line != null) onLogReceived(line); } });
            }
            public void Stop() { _shell?.Dispose(); _ssh?.Disconnect(); _ssh?.Dispose(); }
        }
    }
}