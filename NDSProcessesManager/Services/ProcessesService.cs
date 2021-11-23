using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using NDSProcessesManager.Utilities;
using NDSProcessesManager.Models;
using System.Linq;

namespace NDSProcessesManager.Services
{
    public class ProcessesService
    {
        private ManagerOfProcesses _processesManager;
        public ProcessesService(ManagerOfProcesses processesManager)
        {
            _processesManager = processesManager;
        }

        // TODO: filling from configuration file
        private List<NDSProcess> _ndsServersProcesses = new List<NDSProcess>()
        {
            new NDSProcess(@"C:\Repos\nds2\mrr\_FullBuild\Debug\NDS2\ServerGC\ServerGC.ConsoleHost.exe", 0),
            new NDSProcess(@"C:\Repos\nds2\mrr\_FullBuild\Debug\NDS2\ServerAP\ServerAP.ConsoleHost.exe", 3000),
            new NDSProcess(@"C:\Repos\nds2\mrr\_FullBuild\Debug\NDS2\ServerIS\ServerIS.ConsoleHost.exe", 0),
            new NDSProcess(@"C:\Repos\nds2\mrr\_FullBuild\Debug\NDS2\NDS2Server\NDS2Server.ConsoleHost.exe", 10000),
        };

        private NDSProcess _rtmProcess = new NDSProcess(@"C:\Repos\nds2\mrr\_FullBuild\Debug\NDS2\DevTools\Abs4.0\RealtimeMonitorTool\CommonComponents.RealtimeMonitor.exe", 0);
        private NDSProcess _clientProcess = new NDSProcess(@"C:\Repos\nds2\mrr\_FullBuild\Debug\NDS2\Client\CommonComponents.UnifiedClient.exe", 0);

        public event Action<NDSProcess> ProcessStarted;

        private bool StartProcess(int groupNumber, NDSProcess process)
        {
            Thread.Sleep(process.ProcessDelayMs);
            try
            {
                return _processesManager.StartProcess(groupNumber, process);   
                //if (process.StartProcess())
                //{
                //    //ProcessStarted?.BeginInvoke(process, null, this);
                //}
            }
            catch (Exception ex)
            {
                //TODO:
                throw ex;
            }
        }

        public Task StartProcessesAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    foreach (var process in _ndsServersProcesses)
                    {
                        StartProcess(1, process);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            });
        }
        public Task StopProcessesAsync()
        {
            return Task.Run(() =>
            {
                _processesManager.StopAll();
            });
        }
        public bool IsNDSProcessesRunning => _ndsServersProcesses.All(p => p.IsRunning);

        public Task StartRealTimeMonitorAsync()
        {
            return Task.Run(() =>
            {
                StartProcess(2, _rtmProcess);
            });
        }
        public Task StopRealTimeMonitorAsync()
        {
            return Task.Run(() =>
            {
                if (_rtmProcess.IsRunning)
                {
                    _processesManager.StopGroup(2);
                }
            });
        }
        public bool IsRTMRunning => _rtmProcess.IsRunning;

        public Task StartClientAsync()
        {
            return Task.Run(() =>
            {
                StartProcess(3, _clientProcess);
            });
        }
        public Task StopClientAsync()
        {
            return Task.Run(() =>
            {
                if (_clientProcess.IsRunning)
                {
                    _processesManager.StopGroup(3);
                }
            });
        }
        public bool IsClientRunning => _clientProcess.IsRunning;

    }
}
