using ChatClient.MVMM.Core;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ChatClient.MVMM.View_Model;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Chatnious
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ScrollViewer msgSV = new();
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;

            conBtn.Background = new LinearGradientBrush(Color.FromRgb(250, 115, 255), Color.FromRgb(162, 0, 168),0.5f);
            conBtn.Foreground = new LinearGradientBrush(Color.FromRgb(162, 0, 168), Color.FromRgb(83, 42, 232), 0.5f);

            msg.TargetUpdated += Msg_SourceUpdated;
            
        }

        private void Msg_SourceUpdated(object? sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(msg) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(msg, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendBTN.Command.Execute(msg.Text);
                msg.Text = null;
                msg.ScrollToEnd();
                
            }
        }

    }
}
