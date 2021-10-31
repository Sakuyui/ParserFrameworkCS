using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.code
{
    public class ASTList : ASTree
    {
        protected List<ASTree> _children;

        public ASTList(IEnumerable<ASTree> child)
        {
            _children = child.ToList();
        }
        public override ASTree? Child(int i)
        {
            return _children[i];
        }
        

        public override IEnumerable<ASTree> Children()
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