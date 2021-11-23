using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NDSProcessesManager.Utilities;
using NDSProcessesManager.Services;

namespace NDSProcessesManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessesService _processesService = new ProcessesService(new ManagerOfProcesses());
        private TabControlManager _tabControlManager;

        public MainWindow()
        {
            InitializeComponent();
            _tabControlManager = new TabControlManager(MainTabControl, _processesService);
        }

        private void btn_start_stop_processes_Click(object sender, RoutedEventArgs e)
        {
            btn_start_stop_processes.IsEnabled = false;
            Task t = null;
            if (!_processesService.IsNDSProcessesRunning)
            {
                btn_start_stop_processes.Content = (string)Application.Current.FindResource("btn_starting_processes");
                t = _processesService.StartProcessesAsync();
                t.ContinueWith((obj) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        btn_start_stop_processes.Content = (string)Application.Current.FindResource("btn_stop_processes");
                    });
                });
            }
            else
            {
                t = _processesService.StopProcessesAsync();
                btn_start_stop_processes.Content = (string)Application.Current.FindResource("btn_start_processes");
            }
            t.ContinueWith((obj) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btn_start_stop_processes.IsEnabled = true;
                });
            });
        }

        private void btn_start_stop_rtm_Click(object sender, RoutedEventArgs e)
        {
            btn_start_stop_rtm.IsEnabled = false;
            Task t = null;
            if (!_processesService.IsRTMRunning)
            {
                btn_start_stop_rtm.Content = (string)Application.Current.FindResource("btn_starting_rtm");
                t = _processesService.StartRealTimeMonitorAsync();
                t.ContinueWith((obj) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        btn_start_stop_rtm.Content = (string)Application.Current.FindResource("btn_stop_rtm");
                    });
                });
            }
            else
            {
                btn_start_stop_rtm.Content = (string)Application.Current.FindResource("btn_start_rtm");
                t = _processesService.StopRealTimeMonitorAsync();                
            }
            t.ContinueWith((obj) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btn_start_stop_rtm.IsEnabled = true;
                });
            });
        }

        private void btn_start_stop_client_Click(object sender, RoutedEventArgs e)
        {
            btn_start_stop_client.IsEnabled = false;
            Task t = null;
            if (!_processesService.IsClientRunning)
            {
                btn_start_stop_client.Content = (string)Application.Current.FindResource("btn_starting_client");
                t = _processesService.StartClientAsync();
                t.ContinueWith((obj) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        btn_start_stop_client.Content = (string)Application.Current.FindResource("btn_stop_client");
                    });
                });
            }
            else
            {
                btn_start_stop_client.Content = (string)Application.Current.FindResource("btn_start_client");
                t = _processesService.StopClientAsync();
            }
            t.ContinueWith((obj) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btn_start_stop_client.IsEnabled = true;
                });
            });
        }
    }
}
