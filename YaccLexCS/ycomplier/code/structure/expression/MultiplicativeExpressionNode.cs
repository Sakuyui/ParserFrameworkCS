using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class MultiplicativeExpressionNode : ASTNonTerminalNode
    {
        public MultiplicativeExpressionNode(IEnumerable<ASTNode> child) : base(child, "multiplicative_expression")
        {
        }
    }
}