using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowerManager.GUI
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Configuration Manager
        /// </summary>
        private ConfigurationManager _configurationManager;

        /// <summary>
        /// Connection Manager
        /// </summary>
        private ConnectionManager _connectionManager;

        /// <summary>
        /// Shutdown Scheduled
        /// </summary>
        public bool ShutdownScheduled
        {
            get => _shutdownScheduled;
            private set
            {
                _shutdownScheduled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShutdownScheduled"));
            }
        }

        /// <summary>
        /// Shutdown Time Span
        /// </summary>
        public TimeSpan ShutdownTimeSpan
        {
            get => _shutdownTimeSpan;
            private set
            {
                _shutdownTimeSpan = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShutdownTimeSpan"));
            }
        }

        /// <summary>
        /// Shutdown Date Time
        /// </summary>
        public DateTime ShutdownDateTime
        {
            get => _shutdownDateTime;
            private set
            {
                _shutdownDateTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShutdownDateTime"));
            }
        }

        /// <summary>
        /// Shutdown Time Span
        /// </summary>
        private TimeSpan _shutdownTimeSpan;

        /// <summary>
        /// Shutdown Date Time
        /// </summary>
        private DateTime _shutdownDateTime;

        /// <summary>
        /// Shutdown Scheduled
        /// </summary>
        private bool _shutdownScheduled;

        /// <summary>
        /// Default No Connection Shutdown Timespan
        /// </summary>
        private TimeSpan _defaultNoConnectionShutdownTimespan = new TimeSpan(0, 3, 0);

        /// <summary>
        /// Timer Shutdown
        /// </summary>
        private Timer _timerShutdown;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Set Main Window data context
            this.DataContext = this;

            // Set Data Context for Configuration User Control
            _configurationManager = new ConfigurationManager();
            _configurationManager.Configuration.ShutdownDateTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            ConfigurationUC.DataContext = _configurationManager.Configuration;

            // Set Data Context for Connection Manager User Control
            _connectionManager = new ConnectionManager(_configurationManager.Configuration);
            ConnectionManagerUC.DataContext = _connectionManager;

            // Create Timer
            _timerShutdown = new Timer(1000) { AutoReset = true };
            _timerShutdown.Elapsed += TimerShutdownElapsed;

            // Subscribe to events for shutdown
            _connectionManager.StatusChanged += (o, e) => ManageTimer();
            _configurationManager.Configuration.ValueChanged += (o, e) => ManageTimer();
        }

        /// <summary>
        /// Timer Shutdown Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerShutdownElapsed(object sender, ElapsedEventArgs e)
        {
            // Update Time Span
            ShutdownTimeSpan = ShutdownDateTime - DateTime.Now;

            // Verify if it is time to shutdown
            if (ShutdownTimeSpan <= new TimeSpan(0))
            {
                // Remove flag of shutdown on date time
                _configurationManager.Configuration.ShutdownOnDateTime = false;

                // Send shutdown command
                var psi = new ProcessStartInfo("shutdown", "/s /t 5")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi);
            }

        }

        /// <summary>
        /// Select Shutdown Date Time
        /// </summary>
        private void ManageTimer()
        {
            var dateTimeChoose = DateTime.MaxValue;
            if (_configurationManager.Configuration.ShutdownOnDateTime)
                dateTimeChoose = _configurationManager.Configuration.ShutdownDateTime;

            if (true
                && _configurationManager.Configuration.ShutdownOnNoConnection
                && !_connectionManager.Connected)
            {
                var timespan = dateTimeChoose - DateTime.Now;
                if (timespan > _defaultNoConnectionShutdownTimespan)
                    dateTimeChoose = DateTime.Now + _defaultNoConnectionShutdownTimespan;
            }

            // Verify if shutdown is scheduled
            if (dateTimeChoose != DateTime.MaxValue)
            {
                // Shutdown is scheduled
                ShutdownDateTime = dateTimeChoose;
                ShutdownScheduled = true;
                _timerShutdown.Enabled = true;
            }
            else
            {
                // No shutdown scheduled
                _timerShutdown.Enabled = false;
                ShutdownScheduled = false;
            }
        }
    }
}
