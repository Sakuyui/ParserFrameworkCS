namespace YaccLexCS.ycomplier.automata
{
    public class AutomataNode
    {
       
        public delegate object TransToNodeEvent();

        public readonly object NodeId;
        public TransToNodeEvent EventTransToNode;

        public AutomataNode(TransToNodeEvent eventTransToNode, object nodeId)
        {
            EventTransToNode = eventTransToNode;
            NodeId = nodeId;
        }

        public AutomataNode(object nodeId)
        {
            NodeId = nodeId;
        }
    }
}