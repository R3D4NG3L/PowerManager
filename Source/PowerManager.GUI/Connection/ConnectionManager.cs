using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;

namespace PowerManager.GUI
{
    /// <summary>
    /// Connection Manager
    /// </summary>
    public class ConnectionManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Connection Status Changed
        /// </summary>
        public event EventHandler<ConnectionStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Connection Status
        /// </summary>
        public bool Connected
        {
            get
            {
                // Has been initialized?
                if (!_isConnected.HasValue)
                    return false; // Connection not yet checked, return false by default

                // Return original value
                return _isConnected.Value;
            }
        }

        /// <summary>
        /// Host Address
        /// </summary>
        public string HostAddress
        {
            get => _hostAddress;
            private set
            {
                _hostAddress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HostAddress"));
            }
        }

        /// <summary>
        /// Host Address
        /// </summary>
        private string _hostAddress;

        /// <summary>
        /// Is Connected
        /// </summary>
        private bool? _isConnected;

        /// <summary>
        /// Connection Verifier Thread
        /// </summary>
        private Thread _connectionVerifier;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly Configuration _configuration;

        /// <summary>
        /// Empty Constuctor
        /// </summary>
        public ConnectionManager() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public ConnectionManager(Configuration configuration)
        {
            // Get Configuration
            _configuration = configuration;

            // Start Thread
            _connectionVerifier = new Thread(CheckConnection)
            {
                IsBackground = true,
                Name = "Connection Manager"
            };
            _connectionVerifier.Start();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ConnectionManager()
        {
            _connectionVerifier.Abort();
        }

        /// <summary>
        /// Check Connection
        /// </summary>
        private void CheckConnection()
        {
            var pingSender = new Ping();
            while (true)
            {
                if (!_configuration.ShutdownOnNoConnection)
                    return; // Nothing to do

                try
                {
                    // Send ping request
                    HostAddress = _configuration.HostAddress;
                    var reply = pingSender.Send(HostAddress);

                    // Verify response
                    if (reply.Status == IPStatus.Success)
                        NotifyNewConnection();
                    else
                        NotifyDisconnection();
                }
                catch
                {
                    // No connection available, notify connection status changed
                    NotifyDisconnection();
                }

                // Sleep for 3 seconds
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Notify Reconnection
        /// </summary>
        /// <param name="isOnline">Indicates if the conneciton was online</param>
        private void NotifyNewConnection()
        {
            // New connection available, notify connection status changed
            if (false
                || !_isConnected.HasValue
                || (true
                    && _isConnected.HasValue
                    && !_isConnected.Value
                    )
                )
            {
                _isConnected = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Connected"));
                StatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs(_isConnected.Value));
            }
        }

        /// <summary>
        /// Notify Disconnection
        /// </summary>
        /// <param name="isOnline">Indicates if the conneciton was online</param>
        private void NotifyDisconnection()
        {
            // No connection available, notify connection status changed
            if (false
                || !_isConnected.HasValue
                || (true
                    && _isConnected.HasValue
                    && _isConnected.Value
                    )
                )
            {
                _isConnected = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Connected"));
                StatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs(_isConnected.Value));
            }
        }
    }
}
