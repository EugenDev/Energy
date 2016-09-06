using System.Windows.Media;

namespace Energy.UI.Helpers
{
    public static class ColorGenerator
    {
        private static int _colorIndex;

        private static Color[] _colors = {
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.Aqua,
            Colors.Aquamarine,
            Colors.Bisque,
            Colors.BlueViolet,
            Colors.CadetBlue,
            Colors.Chartreuse,
            Colors.Chocolate,
            Colors.Coral,
            Colors.CornflowerBlue,
            Colors.DarkSeaGreen,
            Colors.DeepPink,
            Colors.Yellow,
            Colors.Thistle,
            Colors.SpringGreen,
            Colors.Olive,
            Colors.Navy,
            Colors.Khaki,
            Colors.Gold
        };

        public static Color GetNext()
        {
            var result = _colors[_colorIndex++];
            _colorIndex %= _colors.Length;
            return result;
        }
    }
}
