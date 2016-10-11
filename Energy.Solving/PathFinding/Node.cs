namespace Energy.Solving.PathFinding
{
    public class Node
    {
        public int? Component { get; set; }

        public double? Distance { get; set; }
        public double? Conduction { get; set; }

        public string Name { get; }

        public Node(string name)
        {
            Name = name;
        }
    }
}