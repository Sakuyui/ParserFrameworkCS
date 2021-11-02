using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class BinaryExprNode : ASTNonTerminalNode
    {
        public BinaryExprNode(IEnumerable<ASTNode> child) : base(child, "")
        {
        }

        public Token? Operator => (this[1] as ASTTerminalNode)?.Token ?? null;
        public ASTNode? Left => this[0];
        public ASTNode? Right => this[2];
    }
}