using System;
using System.Diagnostics;
using System.IO;

namespace NDSProcessesManager.Models
{
    public class NDSProcess : IDisposable
    {
        private int _process_delay_ms = 0;
        private Process _process = null;
        private bool _isRunning = false;

        public bool IsRunning => _isRunning;
        public int PID 
        {
            get => _process?.Id ?? -1;
        }
        public string ProcessName => _process?.ProcessName ?? null;
        public string ProcessPath => _process?.StartInfo.FileName ?? null;
        public int ProcessDelayMs => _process_delay_ms;

        public NDSProcess(string processPath, int delayMs) : this(processPath, delayMs, false) {}

        public NDSProcess(string processPath, int delayMs, bool isRedirectedStdOut)
        {
            if (string.IsNullOrEmpty(processPath) || !File.Exists(processPath))
            {
                throw new ArgumentException("Value can't null or empty, also file should exist", "processPath");
            }

            _process_delay_ms = delayMs;
            _process = new Process();
            _process.StartInfo.FileName = processPath;

            if (isRedirectedStdOut)
            {
                _process.StartInfo.UseShellExecute = false;
                _process.StartInfo.RedirectStandardOutput = true;
            }
        }

        public bool StartProcess()
        {
            if (_isRunning == false && _process.Start())
            {
                _isRunning = true;
                return true;
            }
            return false;
        }

        public void KillProcess()
        {
            if (_isRunning)
            {
                _process?.Kill();
                if (_process.HasExited)
                {
                    _isRunning = false;
                }
            }
        }

        public bool HasExited => _process?.HasExited ?? false;

        public void Dispose()
        {
            _process?.Dispose();
        }

        public StreamReader StandartOutput => _process.StandardOutput;
        public StreamReader StandartError => _process.StandardError;
    }
}
