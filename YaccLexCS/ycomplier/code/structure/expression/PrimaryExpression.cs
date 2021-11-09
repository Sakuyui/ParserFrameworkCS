using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class PrimaryExpression : ASTNonTerminalNode
    {
        public PrimaryExpression(IEnumerable<ASTNode> child) : base(child, "primary_expression")
        {
        }
    }
}