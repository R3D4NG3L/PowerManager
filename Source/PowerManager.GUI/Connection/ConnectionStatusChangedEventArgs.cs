using System;

namespace PowerManager.GUI
{
    /// <summary>
    /// Connection Status Changed Event Args
    /// </summary>
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Connected
        /// </summary>
        public bool Connected { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connected">Is Connected</param>
        public ConnectionStatusChangedEventArgs(bool connected)
        {
            Connected = connected;
        }
    }
}
