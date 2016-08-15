using System.Windows.Controls;

namespace Energy.UI.Controls
{
    public class LinkToolTip : ToolTip
    {
        public LinkToolTip(Link link)
        {
            Content = new TextBlock {Text = link.Distance.ToString()};
        }
    }
}
