using BetterTouchpadBehavior.Extensions;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace BetterTouchpad.XamlBehavior.WPF
{
    public class BetterTouchpadBehavior : Behavior<FrameworkElement>
    {
        const int WM_MOUSEWHEEL = 0x020A;
        const int WM_MOUSEHWHEEL = 0x020E;

        public ICommand ScrollVerticalCommand
        {
            get { return (ICommand)GetValue(ScrollVerticalCommandProperty); }
            set { SetValue(ScrollVerticalCommandProperty, value); }
        }

        public static readonly DependencyProperty ScrollVerticalCommandProperty =
            DependencyProperty.Register("ScrollVerticalCommand", typeof(ICommand), typeof(BetterTouchpadBehavior), new PropertyMetadata(null));

        public ICommand ScrollHorizontalCommand
        {
            get { return (ICommand)GetValue(ScrollHorizontalCommandProperty); }
            set { SetValue(ScrollHorizontalCommandProperty, value); }
        }

        public static readonly DependencyProperty ScrollHorizontalCommandProperty =
            DependencyProperty.Register("ScrollHorizontalCommand", typeof(ICommand), typeof(BetterTouchpadBehavior), new PropertyMetadata(null));

        public ICommand PinchZoomCommand
        {
            get { return (ICommand)GetValue(PinchZoomCommandProperty); }
            set { SetValue(PinchZoomCommandProperty, value); }
        }

        public static readonly DependencyProperty PinchZoomCommandProperty =
            DependencyProperty.Register("PinchZoomCommand", typeof(ICommand), typeof(BetterTouchpadBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            Application.Current.MainWindow.SourceInitialized -= MainWindow_SourceInitialized;
            Application.Current.MainWindow.SourceInitialized += MainWindow_SourceInitialized;
        }

        protected override void OnDetaching()
        {
            Application.Current.MainWindow.SourceInitialized -= MainWindow_SourceInitialized;
        }

        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            if (PresentationSource.FromDependencyObject(AssociatedObject) is not HwndSource handle)
                return;

            handle.AddHook(Hook);
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) => msg switch
        {
            WM_MOUSEWHEEL => HandleVerticalScroll(wParam, lParam),
            WM_MOUSEHWHEEL => HandleHorizontalScroll(wParam, lParam),
            _ => IntPtr.Zero,
        };

        private IntPtr HandleVerticalScroll(IntPtr wParam, IntPtr lParam)
        {
            if (!AssociatedObject.IsMouseOver)
                return IntPtr.Zero;

            var arg = BuildArgument(wParam, lParam);

            if (ScrollVerticalCommand?.CanExecute(arg) == true)
                ScrollVerticalCommand.Execute(arg);

            return (IntPtr)1;
        }

        private IntPtr HandleHorizontalScroll(IntPtr wParam, IntPtr lParam)
        {
            if (!AssociatedObject.IsMouseOver)
                return IntPtr.Zero;

            var arg = BuildArgument(wParam, lParam);

            if (ScrollHorizontalCommand?.CanExecute(arg) == true)
                ScrollHorizontalCommand.Execute(arg);

            return (IntPtr)1;
        }

        private object BuildArgument(IntPtr wParam, IntPtr lParam)
        {
            int delta = (short)wParam.GetHiword();
            int modifiers = (short)wParam.GetLoword();

            int x = (short)lParam.GetLoword();
            int y = (short)lParam.GetHiword();

            return new { Delta = delta, X = x, Y = y };
        }
    }
}
