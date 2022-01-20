using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YaccLexCS.ycomplier.code;

namespace YaccLexCS.runtime.types
{
    //a structure lazy function call
    public class FunctionThunk : IInvokable
    {
        private List<ASTNode> auguments = new();
        private List<FunctionThunk> functions = new();
    }
}
