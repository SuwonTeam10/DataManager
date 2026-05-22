using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace DataManager
{
    public class ICE
    {
        public interface ICommandExecutor
        {
            void ExecuteTrain(string path, Action<string> onLogReceived);
            void Stop();
        }

        public class LocalExecutor : ICommandExecutor
        {
            private Process _process;
            public void ExecuteTrain(string path, Action<string> onLogReceived)
            {
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "python",
                        Arguments = "manage.py train",
                        WorkingDirectory = path,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                _process.OutputDataReceived += (s, e) => onLogReceived(e.Data);
                _process.ErrorDataReceived += (s, e) => onLogReceived(e.Data);
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

            public void ExecuteTrain(string path, Action<string> onLogReceived)
            {
                _shell.WriteLine($"cd {path} && python manage.py train");

                Task.Run(() => {
                    while (true)
                    {
                        var line = _shell.ReadLine();
                        if (line != null) onLogReceived(line);
                    }
                });
            }

            public void Stop()
            {
                _shell?.Dispose();
                _ssh?.Disconnect();
                _ssh?.Dispose();
            }
        }
    }
}