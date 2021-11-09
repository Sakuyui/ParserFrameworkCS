using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class DefinitionOrStatement : ASTNonTerminalNode
    {
        public DefinitionOrStatement(IEnumerable<ASTNode> child) : base(child, "definition_or_statement")
        {
        }
    }
}