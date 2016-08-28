namespace Energy.Solving.PathFinding
{
    public class Edge
    {
        public Node From { get; }
        public Node To { get; }
        public double Distance { get; set; }
        public int Conduction { get; set; }

        public Edge(Node from, Node to)
        {
            From = from;
            To = to;
        }
    }
}