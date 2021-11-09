using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class BlockNode : ASTNonTerminalNode
    {
        private StatementListNode? Statements => 
            (StatementListNode?) (_children.Count switch
            {
                3 => _children[1],
                2 => new StatementListNode(),
                _ => null
            });

        public BlockNode(IEnumerable<ASTNode> children) : base(children, "block")
        {
            
        }

        public override string ToString()
        {
            return Statements.ToString() ?? "null";
        }
    }
}