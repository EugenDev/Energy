using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Energy.UI.Model;

namespace Energy.UI.Helpers
{
    public static class ModelBindingHelper
    {
        public static ModelBase TryGetBoundItem(DependencyObject currentElement)
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

        public static LinkModel TryGetLinkItem(DependencyObject currentElement)
        {
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(currentElement);

                if (parent == null)
                    return null;

                if (parent is ContentPresenter)
                    return (parent as ContentPresenter).DataContext as LinkModel;

                currentElement = parent;
            }
        }

        public static T GetParent<T>(DependencyObject currentElement) 
            where T: DependencyObject
        {
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(currentElement);

                if (parent == null)
                    return null;

                if (currentElement is T)
                    return (T)currentElement;

                currentElement = parent;
            }
        }
    }
}
