
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
        [GrammarDefinition("access_list", "access_list POINT access_name", "access_name")]
        public static void access_list()
        {
        }

        [GrammarDefinition("access_name", "ID")]
        public static void access_name()
        {
        }
       
        [GrammarDefinition("id_list", "id_list COMMA ID", "ID")]
        public static void id_list()
        {
        }

        [GrammarDefinition("augument_list", "augument_list COMMA augument", "augument")]
        public static void augument_list()
        {
        }

        [GrammarDefinition("augument", "expression")]
        public static void augument()
        {
        }

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