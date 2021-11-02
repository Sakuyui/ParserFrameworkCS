using System;
using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code
{
    public class ASTTerminalNode : ASTNode
    {
        public Token Token { get; }

        public ASTTerminalNode(Token token) : base(token.Type)
        {
            Token = token;
        }

        public override ASTNode? Child(int i)
        {
            return null;
        }
        

        public override IEnumerable<ASTNode> Children()
        {
            return Array.Empty<ASTNode>();
        }

        public override string Location()
        {
            return $"at line {Token.LineNum}";
        }
    }
}