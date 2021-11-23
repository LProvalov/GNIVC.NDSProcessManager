using NDSProcessesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDSProcessesManager.Utilities
{
    public class ManagerOfProcesses
    {
        private List<Tuple<int, NDSProcess>> _processes = new List<Tuple<int, NDSProcess>>();

        public bool StartProcess(int groupNumber, NDSProcess ndsProcess)
        {
            try
            {
                if (ndsProcess.StartProcess())
                {
                    _processes.Add(new Tuple<int, NDSProcess>(groupNumber, ndsProcess));
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void StopAll()
        {
            if (_processes != null && _processes.Count > 0)
            {
                var stoppingProcess = new List<NDSProcess>();
                foreach (var process in _processes)
                {
                    try
                    {
                        process.Item2.KillProcess();
                    }
                    catch
                    {
                        // TODO: logging error
                        if (!process.Item2.HasExited)
                        {
                            stoppingProcess.Add(process.Item2);
                        }
                    }
                    finally
                    {
                        if (stoppingProcess.Count > 0)
                        {
                            foreach(var p in stoppingProcess)
                            {
                                p.KillProcess();
                            }
                        }
                    }
                }

                if (stoppingProcess.Count > 0)
                {
                    // TODO: try to do smth with this processes
                }
            }
        }

        public void StopGroup(int groupId)
        {
            if (groupId >= 0)
            {
                foreach(var process in _processes)
                {
                    if (process.Item1 == groupId)
                    {
                        process.Item2.KillProcess();
                    }
                }
            }
        }

        public void Stop(int PID)
        {
            var process = _processes.First(p => p.Item2.PID == PID).Item2;
            if (process != null)
            {
                process.KillProcess();
            }
        }
    }
}
