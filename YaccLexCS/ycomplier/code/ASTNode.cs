using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.code
{
    public abstract class ASTree : IEnumerable<ASTree>
    {
        public abstract ASTree? Child(int i);
        public int ChildrenCount => Children().Count();
        public abstract IEnumerable<ASTree> Children();
        public abstract string Location();

       
        public IEnumerator<ASTree> GetEnumerator()
        {
            return Children().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ASTree? this[int i] => Child(i);
    }
}