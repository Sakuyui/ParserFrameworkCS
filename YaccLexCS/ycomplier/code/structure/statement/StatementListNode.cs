using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace YaccLexCS.ycomplier.code.structure
{
    public class StatementListNode : ASTNonTerminalNode
    {
        private StatementNode? _statementNode =>
            (StatementNode?) (_children.Count switch
            {
                1 => _children[0],
                2 => _children[1],
                _ => null
            });
        
        public StatementListNode(IEnumerable<ASTNode> child) : base(child, "statement_list")
        {
        }
        public StatementListNode() : base(ArraySegment<ASTNode>.Empty, "statement_list")
        {
        }
    }
}