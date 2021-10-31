using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class NumberLiteralNode : ASTLeaf
    {
        public NumberLiteralNode(Token token) : base(token){}
        public override string ToString() => $"[AST Leaf: {Token}]";
    }

    public class BinaryExprNode : ASTList
    {
        public BinaryExprNode(IEnumerable<ASTree> child) : base(child)
        {
        }

        public Token? Operator => (this[1] as ASTLeaf)?.Token ?? null;
        public ASTree? Left => this[0];
        public ASTree? Right => this[2];
    }
}