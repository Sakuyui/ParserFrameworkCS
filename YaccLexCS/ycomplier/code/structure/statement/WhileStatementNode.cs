using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class WhileStatementNode : ASTNonTerminalNode
    {
        public ExpressionNode? Condition => _children.Count == 5 ? (ExpressionNode?)_children[2] : null;
        public BlockNode? Block => _children.Count == 5 ? (BlockNode?)_children[4] : null;
        public WhileStatementNode(IEnumerable<ASTNode> child) : base(child, "while_statement")
        {
        }
    }
}