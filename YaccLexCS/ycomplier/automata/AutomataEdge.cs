namespace YaccLexCS.ycomplier.automata
{
    
    public abstract class AutomataEdge
    {

        public AutomataNode FromNode;
        public AutomataNode ToNode;
        
        public delegate object InTransEvent(params object[] objs);
        public delegate bool TransStrategy(AutomataContext ctx, AutomataEdge edge, params object[] objs);

        public InTransEvent EventTransInEdge;
        public TransStrategy IsCanTrans;
        public AutomataEdge( AutomataNode fromNode, AutomataNode toNode,InTransEvent eventTransInEdge, TransStrategy transStrategy)
        {
            EventTransInEdge = eventTransInEdge;
            IsCanTrans = transStrategy;
            FromNode = fromNode;
            ToNode = toNode;
        }

        public AutomataEdge( AutomataNode fromNode, AutomataNode toNode, TransStrategy transStrategy)
        {
            IsCanTrans = transStrategy;
            FromNode = fromNode;
            ToNode = toNode;
        }
    }
}