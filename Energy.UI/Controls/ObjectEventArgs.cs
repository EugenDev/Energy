using System;

namespace Energy.UI.Controls
{
    public class ObjectEventArgs<T> : EventArgs
    {
        public T Item { get; set; }

        public ObjectEventArgs(T item)
        {
            Item = item;
        }
    }
}