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

        public void AddContact(int leftId, int rightId)
        {
            var leftNode = nodes.Find(n => n.Id == leftId);
            if (leftNode is null)
            {
                leftNode = new Node(leftId);
                nodes.Add(leftNode);
            }
            var rightNode = nodes.Find(n => n.Id == rightId);
            if (rightNode is null)
            {
                rightNode = new Node(rightId);
                nodes.Add(rightNode);
            }
            leftNode.ContactId.Add(rightId);
            rightNode.ContactId.Add(leftId);
        }

        public void ClearData()
        {
            nodes.Clear();
        }

        private int GetInfactionCount(int sourceId, int targetId)
        {
            if (sourceId == targetId) return 0;

            int count = MAXCOUNT;

            Node? startNode = nodes.Find(n => n.Id == sourceId);
            if (startNode is not null)
            {
                int temp = count;
                accessedId.Add(startNode.Id);
                foreach (var id in startNode.ContactId)
                {
                    if (!accessedId.Contains(id))
                    {
                        temp = GetInfactionCount(id, targetId);
                        if (temp < count) count = temp;
                    }
                }
                accessedId.Remove(startNode.Id);
                return count + 1;
            }
            return MAXCOUNT;
        }
        public double GetInfactionRate(int sourceId, int targetId)
        {
            accessedId.Clear();
            var count = GetInfactionCount(sourceId, targetId);
            return count >= MAXCOUNT ? 0 : Math.Pow(0.5, count);
        }
        
    }
}
