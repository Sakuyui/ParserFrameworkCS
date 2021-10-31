using System;
using System.Collections.Generic;

namespace YaccLexCS.ycomplier.code
{
    public class ASTLeaf : ASTree
    {
        public Token Token { get; }

        public ASTLeaf(Token token)
        {
            Token = token;
        }

        public override ASTree? Child(int i)
        {
            return null;
        }
        

        public override IEnumerable<ASTree> Children()
        {
            return Array.Empty<ASTree>();
        }

        public override string Location()
        {
            return $"at line {Token.LineNum}";
        }
    }
}