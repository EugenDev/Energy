using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Energy.UI.Model;

namespace Energy.UI.Helpers
{
    public static class ModelBindingHelper
    {
        public static ModelBase GetBoundItem(DependencyObject currentElement)
        {
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(currentElement);

                if (parent == null)
                    return null;

                if (parent is ContentPresenter)
                    return (parent as ContentPresenter).DataContext as ModelBase;

                currentElement = parent;
            }
        }
    }
}
