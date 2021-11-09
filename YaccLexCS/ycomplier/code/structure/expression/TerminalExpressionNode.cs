using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class TerminalExpressionNode : ASTNonTerminalNode
    {
        public TerminalExpressionNode(IEnumerable<ASTNode> child) : base(child, "terminal_expression")
        {
            
        }
    }
}