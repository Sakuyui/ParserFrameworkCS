namespace YaccLexCS.ycomplier.automata
{
    
   
    public abstract class AutomataEdge
    {

        public AutomataNode FromNode;
        public AutomataNode ToNode;
        
        public delegate object? InTransEvent(object input, params object[] objs);
       // public delegate bool TransStrategy(AutomataContext ctx, AutomataEdge edge, params object[] objs);

        public InTransEvent EventTransInEdge;
        public readonly ITransStrategy IsCanTrans;
        public AutomataEdge( AutomataNode fromNode, AutomataNode toNode,InTransEvent eventTransInEdge, ITransStrategy transStrategy)
        {
            EventTransInEdge = eventTransInEdge;
            IsCanTrans = transStrategy;
            FromNode = fromNode;
            ToNode = toNode;
        }

        public AutomataEdge( AutomataNode fromNode, AutomataNode toNode, ITransStrategy transStrategy)
        {
            IsCanTrans = transStrategy;
            FromNode = fromNode;
            ToNode = toNode;
        }
    }
}