using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YaccLexCS
{
    public class YCompiler
    {
        private YCompiler()
        {
        }

        public static YCompiler GetNewCompiler()
        {
            return new YCompiler();
        }
        
    }


  
}