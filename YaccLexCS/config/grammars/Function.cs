
using System;
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.runtime;
using YaccLexCS.ycomplier.attribution;
namespace YaccLexCS.config
{
    [GrammarConfiguration]
    public static class GrammarFunction
    {

        [GrammarDefinition("params_list", "params_list COMMA param",  "param")]
        public static void params_list()
        {
        }

        [GrammarDefinition("param",  "typeless_param", "typed_param")]
        public static void param()
        {
        }

        [GrammarDefinition("typeless_param",  "ID")]
        public static void typeless_param()
        {
        }

        [GrammarDefinition("typed_param",  "ID ID")]
        public static void typed_param()
        {
        }
    }
}