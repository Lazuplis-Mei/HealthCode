using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCodeLib
{
    public class Graph
    {
        private class Node
        {
            public int Id;
            public readonly HashSet<int> ContactId;
            public Node(int id)
            {
                this.Id = id;
                ContactId = new HashSet<int>();
            }
        }

        private readonly List<Node> nodes;
        private readonly List<int> accessedId;
        private const int MAXCOUNT = 1000000;

        public Graph()
        {
            nodes = new List<Node>();
            accessedId = new List<int>();
        }

        private Node? GetNode(int id)
        {
            return nodes.Find(n => n.Id == id);
        }

        public void AddContact(int id1, int id2)
        {
            var node = GetNode(id1);
            node ??= new Node(id1);
            nodes.Add(node);
            node.ContactId.Add(id2);

            node = GetNode(id2);
            node ??= new Node(id2);
            nodes.Add(node);
            node.ContactId.Add(id1);
        }

        public void ClearData()
        {
            nodes.Clear();
        }

        private int GetInfactionCount(int sourceId, int targetId)
        {
            if (sourceId == targetId)
                return 0;

            int count = MAXCOUNT;
            var node = GetNode(sourceId);

            if (node is not null)
            {
                accessedId.Add(sourceId);
                foreach (var id in node.ContactId)
                {
                    if (accessedId.Contains(id))
                        continue;
                    count = Math.Min(count, GetInfactionCount(id, targetId));
                }
                accessedId.Remove(sourceId);
                return Math.Min(count + 1, MAXCOUNT);
            }

            return MAXCOUNT;
        }

        public double GetInfactionRate(int sourceId, int targetId)
        {
            var count = GetInfactionCount(sourceId, targetId);
            return count == MAXCOUNT ? 0 : Math.Pow(0.5, count);
        }
        
    }
}
