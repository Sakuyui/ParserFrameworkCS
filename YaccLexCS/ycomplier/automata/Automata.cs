using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.automata
{

    public class AutomataContext
    {
        public dynamic CurInput;
        public void ResetContext()
        {
            
        }
    }
    public class Automata
    {
        public IEnumerable<object> StartState = new HashSet<object>();
        public IEnumerable<object> AcceptState = new HashSet<object>();
        public HashSet<object> CurrentStateCollection = new();
        
        public HashSet<object> StartNodes => StartState.ToHashSet();

        public List<AutomataNode> Nodes = new(); //节点集合
        public List<AutomataEdge> Edges = new(); //边集合
        
        private Dictionary<object, List<AutomataEdge>> _nodeNext = new(); // map node id -> edges that from this node
        private Dictionary<object, AutomataNode> _nodeMap = new(); //map node id -> node


        protected AutomataContext _context = new();
        public Automata SetStartState(params object[] idSet)
        {
            StartState = new HashSet<object>(idSet);
            return this;
        }
        
        public Automata SetAcceptState(params object[] idSet)
        {
            AcceptState = new HashSet<object>(idSet);
            return this;
        }

        public Automata InitState()
        {
            return ResetAutomata();
        }

        public Automata ResetAutomata()
        {
            _context.ResetContext();
            CurrentStateCollection.Clear();
            foreach (var s in StartState)
                CurrentStateCollection.Add(s);
            return this;
        }

        public void AddNode(AutomataNode node)
        {
            if (_nodeMap.ContainsKey(node.NodeId))
                throw new Exception("Repeat same node id");
            _nodeNext[node.NodeId] = new List<AutomataEdge>();
            _nodeMap[node.NodeId] = node;
            Nodes.Add(node);
        }



        public bool IsAccepted()
        {
            return CurrentStateCollection.Any(s => AcceptState.Contains(s));
        }
        public void ApplyClosure()
        {
            var set = new HashSet<object>(CurrentStateCollection);
            foreach (var node in CurrentStateCollection)
            {
                var e = _nodeNext[node];
                foreach (var edge in e.Where(edge => edge.IsCanTrans.Judge(null, null)))
                {
                    set.Add(edge.ToNode.NodeId);
                }
            }

            CurrentStateCollection = set;
        }
        public void AddEdge(AutomataEdge edge)
        {
            Edges.Add(edge);
            if(!_nodeMap.ContainsKey(edge.FromNode.NodeId))
                _nodeMap.Add(edge.FromNode.NodeId, edge.FromNode);
            if(!_nodeMap.ContainsKey(edge.ToNode.NodeId))
                _nodeMap.Add(edge.ToNode.NodeId, edge.ToNode);
            if (!_nodeNext.ContainsKey(edge.FromNode.NodeId))
                _nodeNext.Add(edge.FromNode.NodeId, new List<AutomataEdge>());
            if(!_nodeNext[edge.FromNode.NodeId].Contains(edge))
                _nodeNext[edge.FromNode.NodeId].Add(edge);
        }
        
        public Automata()
        {
            
        }
        
        

        public override string ToString()
        {
            var str = "===============================================\n";
            str += $"[Cur state: {CurrentStateCollection.Aggregate((a, b) => a + ", " + b)}]\n";
            str += $"start from node_id {StartState.Aggregate((a, b) => a + ", " + b)}\n";
            var i = 0;
            foreach(var node in Nodes)
            {
                str += $"*** Node ID = {node.NodeId}:\n";
                str = _nodeNext[node.NodeId].Aggregate(str, (current, e) => current + $"\t* Edge_{i++}: {e}\n");
                
                str += "\n";
            }
            return str;
        }

        public AutomataNode GetNode(object id)
        {
            return _nodeMap[id];
        }
    }
}