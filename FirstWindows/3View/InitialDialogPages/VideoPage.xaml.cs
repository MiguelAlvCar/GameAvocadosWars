using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GameFrontEnd.View;
using System.Windows.Controls;
using WpfBasicElements;

namespace FirstWindows.View.InitialDialog
{
    /// <summary>
    /// Interaktionslogik für VideoWindow.xaml
    /// </summary>
    public partial class VideoPage : Page
    {
        private Window window;

        public VideoPage()
        {
            InitializeComponent();            
            Mouse.OverrideCursor = Cursors.None;
            Video.MediaEnded += (a, b) => 
            {
                CloseWindow();
            };


            this.Loaded += (sender, e) =>
            {
                window = Window.GetWindow(this);
                window.KeyDown += EnterEscapeDown;
            };
            this.Unloaded += (sender, e) =>
            {
                window.KeyDown -= EnterEscapeDown;
            };
        }

        private void CloseWindow()
        {
            ((IPrincipalWindow)Window.GetWindow(this)).MainFrame.Navigate(new FirstPage());
            Mouse.OverrideCursor = null;
        }        

        private void EnterEscapeDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                CloseWindow();
            }
        }
    }
}
