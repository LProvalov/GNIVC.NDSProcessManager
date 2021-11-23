using NDSProcessesManager.Services;
using System.Windows.Controls;

namespace NDSProcessesManager
{
    public class TabControlManager
    {
        private TabControl _tabControl;
        public TabControlManager(TabControl tabControl, ProcessesService processesService)
        {
            processesService.ProcessStarted += ProcessesService_ProcessStarted_Handler;
            _tabControl = tabControl;
        }

        private void ProcessesService_ProcessStarted_Handler(Models.NDSProcess ndsProcess)
        {
            if (ndsProcess.IsRunning)
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = $"{ndsProcess.ProcessName} - {ndsProcess.PID}";
                _tabControl.Items.Insert(_tabControl.Items.Count - 1, tabItem);
            }
        }
    }
}
