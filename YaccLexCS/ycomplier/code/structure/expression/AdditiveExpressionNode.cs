using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class AdditiveExpressionNode : ASTNonTerminalNode
    {
        public AdditiveExpressionNode(IEnumerable<ASTNode> child) : base(child, "additive_expression")
        {
        }
    }
}