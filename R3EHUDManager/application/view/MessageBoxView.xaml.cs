using da2mvc.framework.application.view;
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
using System.Windows.Shapes;

namespace R3EHUDManager.application.view
{
    /// <summary>
    /// Logique d'interaction pour MessageBoxView.xaml
    /// </summary>
    public partial class MessageBoxView : ModalWindow
    {

        public static void Show(string title, string description)
        {
            var dialog = new MessageBoxView();
            dialog.SetText(title, description);
            dialog.ShowDialog();
        }

        public MessageBoxView()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            button.Click += ButtonClicked;
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void SetText(string title, string description)
        {
            Title = title;
            textBlock.Text = description;
        }
    }
}
