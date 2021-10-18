namespace YaccLexCS.ycomplier.automata
{
    public static class CommonTransitionStrategy
    {
        public static AutomataEdge.TransStrategy EpsilonTrans => (_, _, _) => true;

        public static AutomataEdge.TransStrategy SingleCharacterTrans => (p,e,o) =>
        {
           //var cxt = (AutomataContext)p[0];
            var c = (char) cxt.CurInput;
            return true;
        }
    }
}