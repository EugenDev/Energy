using System;
using System.Collections.Generic;
using System.Linq;

namespace Energy.Solving.PathFinding
{
    public class Graph
    {
        private readonly Dictionary<string, Node> _nodes;
        private readonly List<Edge> _links;
        private readonly Dictionary<string, List<Edge>> _adjasentEdges;

        public Graph()
        {
            _nodes = new Dictionary<string, Node>();
            _links = new List<Edge>();
            _adjasentEdges = new Dictionary<string, List<Edge>>();
        }

        public void AddNode(string name)
        {
            _nodes.Add(name, new Node(name));
        }

        public void LinkNode(string fromName, string toName, double distance, int conduction)
        {
            var edge = new Edge(_nodes[fromName], _nodes[toName])
            {
                Distance = distance,
                Conduction = conduction
            };
            _links.Add(edge);
            AddAdjasent(fromName, edge);
            AddAdjasent(toName, edge);
        }

        private void AddAdjasent(string name, Edge edge)
        {
            if(!_adjasentEdges.ContainsKey(name))
                _adjasentEdges[name] = new List<Edge>();
            _adjasentEdges[name].Add(edge);
        }


        public Dictionary<string, double> GetShortestDistances(string from)
        {
            ClearVisitedNodes();
            var fromNode = _nodes[from];

            var queue = new Queue<Node>();
            queue.Enqueue(fromNode);
            fromNode.Distance = 0;

            while (queue.Count != 0)
            {
                var currentNode = queue.Dequeue();
                var adjEdges = _adjasentEdges[currentNode.Name];
                foreach (var edge in adjEdges)
                {
                    var otherNodeName = edge.From.Name.Equals(currentNode.Name) ? edge.To.Name : edge.From.Name;
                    var node = _nodes[otherNodeName];
                    if(currentNode.Distance + edge.Distance > node.Distance)
                        continue;
                    node.Distance = currentNode.Distance + edge.Distance;
                    queue.Enqueue(node);
                }
            }
            
            return _nodes.ToDictionary(n => n.Key, n => n.Value.Distance);
        }

        public Dictionary<string, int> GetConductions(string from)
        {
            ClearVisitedNodes();
            var fromNode = _nodes[from];

            var queue = new Queue<Node>();
            queue.Enqueue(fromNode);
            fromNode.Distance = 0;
            fromNode.Conduction = 3;

            while (queue.Count != 0)
            {
                var currentNode = queue.Dequeue();
                var adjEdges = _adjasentEdges[currentNode.Name];
                foreach (var edge in adjEdges)
                {
                    var otherNodeName = edge.From.Name.Equals(currentNode.Name) ? edge.To.Name : edge.From.Name;
                    var node = _nodes[otherNodeName];
                    if (Math.Min(currentNode.Conduction, edge.Conduction) <= node.Conduction)
                        continue;
                    node.Conduction = Math.Min(currentNode.Conduction, edge.Conduction);
                    queue.Enqueue(node);
                }
            }

            return _nodes.ToDictionary(n => n.Key, n => n.Value.Conduction);
        }

        private void ClearVisitedNodes()
        {
            foreach (var pair in _nodes)
            {
                pair.Value.Distance = double.MaxValue;
                pair.Value.Conduction = 0;
            }
        }
    }
}
