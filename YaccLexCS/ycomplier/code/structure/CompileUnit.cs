using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code.structure
{
    public class CompileUnit : ASTNonTerminalNode
    {
        public CompileUnit(IEnumerable<ASTNode> child) : base(child, "program")
        {
        }
    }
}