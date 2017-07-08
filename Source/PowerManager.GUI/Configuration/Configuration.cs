using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PowerManager.GUI
{
    /// <summary>
    /// Configuration
    /// </summary>
    public class Configuration
    {
        #region Private Fields
        /// <summary>
        /// Shutdown PC on no connection available
        /// </summary>
        private bool _shutdownOnNoConnection;

        /// <summary>
        /// Host Address to check in order to shutdown pc when no connection is available
        /// </summary>
        private string _hostAddress;

        /// <summary>
        /// Shutdown PC on Date Time
        /// </summary>
        private bool _shutdownOnDateTime;

        /// <summary>
        /// Shutdown PC Date Time
        /// </summary>
        private DateTime _shutdownDateTime;
        #endregion // Private Fields

        #region Public Events
        /// <summary>
        /// On Configuration Changed
        /// </summary>
        public event EventHandler ValueChanged;
        #endregion // Public Events

        #region Public Properties
        /// <summary>
        /// Shutdown PC on no connection available
        /// </summary>
        public bool ShutdownOnNoConnection
        {
            get => _shutdownOnNoConnection;
            set
            {
                _shutdownOnNoConnection = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }


        /// <summary>
        /// Host Address to check in order to shutdown pc when no connection is available
        /// </summary>
        public string HostAddress
        {
            get => _hostAddress;
            set
            {
                _hostAddress = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Shutdown PC on Date Time
        /// </summary>
        public bool ShutdownOnDateTime
        {
            get => _shutdownOnDateTime;
            set
            {
                _shutdownOnDateTime = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Shutdown PC Date Time
        /// </summary>
        public DateTime ShutdownDateTime
        {
            get => _shutdownDateTime;
            set
            {
                _shutdownDateTime = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }
        #endregion // Public Properties
    }
}
