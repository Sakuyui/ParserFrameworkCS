
using System;
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.runtime;
using YaccLexCS.ycomplier.attribution;
namespace YaccLexCS.config
{
    [GrammarConfiguration]
    public static class GDefinition
    {

        [GrammarDefinition("definition",  "function_definition")]
        public static void definition()
        {
        }

        [GrammarDefinition("function_definition",  "DYFN ID LP params_list RP block")]
        public static void function_definition()
        {
        }
    }
}