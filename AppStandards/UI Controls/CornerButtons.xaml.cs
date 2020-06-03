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

namespace AppStandards.UIControls
{
    /// <summary>
    /// Interaction logic for CornerButtons.xaml
    /// </summary>
    internal partial class CornerButtons : UserControl
    {
        #region Fields
        /// <summary>
        /// Returns the <see cref="Window"/> that is the parent of the <see cref="CornerButtons"/>.
        /// </summary>
        private Window _parentWindow
        {
            get
            {
                return Window.GetWindow(this);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether or not the user can cancel out of the dialog.
        /// </summary>
        public bool CanCancel
        {
            get { return (bool)GetValue(CanCancelProperty); }
            set { SetValue(CanCancelProperty, value); }
        }

        /// <summary>
        /// The dependency property for <see cref="CanCancel"/>.
        /// <para>Using a DependencyProperty as the backing store for CanCancel enables animation, styling, binding, etc...</para>
        /// </summary>
        public static readonly DependencyProperty CanCancelProperty =
            DependencyProperty.Register("CanCancel", typeof(bool), typeof(CornerButtons), new PropertyMetadata(true));
        #endregion

        public CornerButtons()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the parent window.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CustomChrome.Close(_parentWindow);
        }

        /// <summary>
        /// Maximizes or restores the parent window.
        /// </summary>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomChrome.Maximize(_parentWindow);
        }

        /// <summary>
        /// Minimizes the parent window.
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            CustomChrome.Minimize(_parentWindow);
        }

        public class CustomChrome
        {
            /// <summary>
            /// Closes the specified <see cref="Window"/>.
            /// </summary>
            /// <param name="Window">The <see cref="Window"/> to close.</param>
            public static void Close(Window Window)
            {
                SystemCommands.CloseWindow(Window);
            }

            /// <summary>
            /// Maximizes or restores the specified <see cref="Window"/>, depending on the <see cref="WindowState"/>.
            /// </summary>
            /// <param name="Window">The <see cref="Window"/> to maximize or restore.</param>
            public static void Maximize(Window Window)
            {
                AdjustWindowSize(Window);
            }

            /// <summary>
            /// Minimizes the specified <see cref="Window"/>.
            /// </summary>
            /// <param name="Window">The <see cref="Window"/> to minimize.</param>
            public static void Minimize(Window Window)
            {
                SystemCommands.MinimizeWindow(Window);
            }

            /// <summary>
            /// Maximizes or restores the specified <see cref="Window"/>, depending on the <see cref="WindowState"/>.
            /// </summary>
            /// <param name="Window">The <see cref="Window"/> to maximize or restore.</param>
            internal static void AdjustWindowSize(Window Window)
            {
                if (Window.WindowState == WindowState.Maximized)
                {
                    SystemCommands.RestoreWindow(Window);
                }
                else
                {
                    SystemCommands.MaximizeWindow(Window);
                }

            }
        }
    }
}
