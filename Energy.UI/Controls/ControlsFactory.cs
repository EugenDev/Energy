using System;
using System.Windows;
using System.Windows.Controls;

namespace Energy.UI.Controls
{
    public class ControlsFactory
    {
        private readonly Action<object, RoutedEventArgs> _wantDeleteAction;

        public ControlsFactory(Action<object, RoutedEventArgs> wantDeleteAction)
        {
            _wantDeleteAction = wantDeleteAction;
        }

        private ContextMenu CreateContextMenu()
        {
            var result = new ContextMenu();
            var deleteItem = new MenuItem {Header = "Удалить"};
            deleteItem.Click += new RoutedEventHandler(_wantDeleteAction);
            result.Items.Add(deleteItem);
            return result;
        }
        
        public ControlBase CreateControl(ElementType elementType, string name)
        {
            if (elementType == ElementType.Station)
                return new Station(name) { ContextMenu = CreateContextMenu() };

            if (elementType == ElementType.Consumer)
                return new Consumer(name) { ContextMenu = CreateContextMenu() };

            throw new InvalidOperationException();
        }
    }
}
