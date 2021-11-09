using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class UnaryExpressionNode : ASTNonTerminalNode
    {
        public UnaryExpressionNode(IEnumerable<ASTNode> child) : base(child, "unary_expression")
        {
        }
    }
}