using System;
using Energy.UI.Model;

namespace Energy.UI.Controls
{
    public class LinkAddedEventArgs : EventArgs
    {
        public ModelBase From { get; private set; }
        public ModelBase To { get; private set; }

        public LinkAddedEventArgs(ModelBase from, ModelBase to)
        {
            From = from;
            To = to;
        }
    }
}