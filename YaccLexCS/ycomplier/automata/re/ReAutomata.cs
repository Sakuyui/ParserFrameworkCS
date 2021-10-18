namespace YaccLexCS.ycomplier.automata
{
    public class ReEdge : AutomataEdge
    {
      

        public override string ToString()
        {
            return $"[RE Edge ({FromNode.NodeId} -> {ToNode.NodeId})] use strategy = {IsCanTrans}";
        }

        public ReEdge(AutomataNode fromNode, AutomataNode toNode, InTransEvent eventTransInEdge, ITransStrategy transStrategy) : base(fromNode, toNode, eventTransInEdge, transStrategy)
        {
        }

        public ReEdge(AutomataNode fromNode, AutomataNode toNode, ITransStrategy transStrategy) : base(fromNode, toNode, transStrategy)
        {
        }
    }
    
    
    public class ReAutomata : Automata
    {
        public ReAutomata()
        {
           
            AddNode(new AutomataNode(0));
            AddNode(new AutomataNode(1));
            SetStartState(0).InitState();
            
            AddEdge(new ReEdge( GetNode(0), GetNode(1), CommonTransitionStrategy.EpsilonTrans.Instance));
            this.PrintToConsole();
            ApplyClosure();
            this.PrintToConsole();
        }
    }
}