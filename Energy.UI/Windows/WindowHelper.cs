using System.Windows;
using System.Windows.Input;

namespace Energy.UI.Windows
{
    public static class WindowHelper
    {
        internal static void SetStartupLocationNearMouse(Window window, FrameworkElement context)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            var mousePoint = context.PointToScreen(Mouse.GetPosition(context));
            window.Top = mousePoint.Y;
            window.Left = mousePoint.X;
        }
    }
}
