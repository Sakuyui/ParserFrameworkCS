using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class AssignExpressionNode : ASTNonTerminalNode
    {
        
        public AssignExpressionNode(IEnumerable<ASTNode> child) : base(child, "assign_expression")
        {
        }
    }
}