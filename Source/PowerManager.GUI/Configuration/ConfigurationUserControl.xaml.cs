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

namespace PowerManager.GUI
{
    /// <summary>
    /// Logica di interazione per ConfigurationUserControl.xaml
    /// </summary>
    public partial class ConfigurationUserControl : UserControl
    {
        public ConfigurationUserControl()
        {
            InitializeComponent();
        }

		/// <summary>
		/// Shutdown Date Time Checked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShutdownOnDateTime_Checked(object sender, RoutedEventArgs e)
		{
			if (true
				&& ShutdownOnDateTime.IsChecked.HasValue
				&& ShutdownOnDateTime.IsChecked.Value)
			{
				ShutdownDateTime.Minimum = DateTime.Now + new TimeSpan(0, 1, 0);

				if (ShutdownDateTime.Value < ShutdownDateTime.Minimum)
					ShutdownDateTime.Value = ShutdownDateTime.Minimum;
			}
		}
	}
}
