namespace YaccLexCS.ycomplier.code.structure
{
    public class NumberLiteralNode : ASTTerminalNode
    {
        public NumberLiteralNode(Token token) : base(token){}
        public override string ToString() => $"[AST Leaf: {Token}]";
    }
}