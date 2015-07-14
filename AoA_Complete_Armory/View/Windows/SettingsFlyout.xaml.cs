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
using System.Windows.Forms;
using System.Windows.Shapes;
using DM.Armory.Properties;

namespace DM.Armory.View.Windows
{
    /// <summary>
    /// Interaction logic for SettingsFlyout.xaml
    /// </summary>
    public partial class SettingsFlyout
    {
        public event EventHandler OnRequestReloading = delegate { };

        public SettingsFlyout()
        {

            InitializeComponent();
            DirectoryText.Text = Settings.Default.PathToGameFolder;
            CloseCommand = new SendRequestComand(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();

            DialogResult res = diag.ShowDialog();
            if (res == DialogResult.OK)
            {
                DirectoryText.Text = diag.SelectedPath;
            }
        }

        public void SendOnrequestReloading()
        {
            if (Settings.Default.PathToGameFolder != DirectoryText.Text)
            {
                Settings.Default.PathToGameFolder = DirectoryText.Text;
                Settings.Default.Save();
                OnRequestReloading(this, null);
            }
            
        }


        public class SendRequestComand : ICommand
        {
            private  SettingsFlyout flyout;

            public SendRequestComand(SettingsFlyout flyout)
            {
                this.flyout = flyout; 
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                flyout.SendOnrequestReloading();
            }
        }
    }
}
