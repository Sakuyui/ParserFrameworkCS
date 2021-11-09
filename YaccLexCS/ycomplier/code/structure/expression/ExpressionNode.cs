using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class ExpressionNode : ASTNonTerminalNode
    {
        private ASTNode? _expression => _children.Count == 1 ? _children[0] : null;
        public ExpressionNode(IEnumerable<ASTNode> child) : base(child, "expression")
        {
            
        }
    }
}