using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.code.structure
{
    public class StatementNode : ASTNonTerminalNode
    {
        private ASTNode? ExpressionOrStatement => _children.Any() ? _children[0] : null;
        
        public StatementNode(IEnumerable<ASTNode> child) : base(child, "statement")
        {
            
        }
    }
}