﻿using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.code
{
    public class ASTNonTerminalNode : ASTNode
    {
        protected List<ASTNode> _children;

        public ASTNonTerminalNode(IEnumerable<ASTNode> child, string nodeName) : base(nodeName)
        {
            _children = child.ToList();
        }
        public override ASTNode? Child(int i)
        {
            return _children[i];
        }
        

        public override IEnumerable<ASTNode> Children()
        {
            return _children;
        }

        public override string Location()
        {
            var w = _children.Where(c => c.Location() != "");
            return w.Any() ? w.First().Location() : "";
        }
    }
}