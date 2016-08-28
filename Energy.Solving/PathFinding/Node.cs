namespace Energy.Solving.PathFinding
{
    public class Node
    {
        public double Distance { get; set; }
        public int Conduction { get; set; }

        public string Name { get; }

        public Node(string name)
        {
            Name = name;
        }
    }
}