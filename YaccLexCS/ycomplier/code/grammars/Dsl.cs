
using System;
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.runtime;
using YaccLexCS.ycomplier.attribution;
namespace YaccLexCS.config
{
    [GrammarConfiguration]
    public static class GrammarDsl
    {

        [GrammarDefinition("block",  "LB statements RB")]
        public static void block(CompilerContext context)
        {
        }

        [GrammarDefinition("statements",  "statement statements")]
        public static void statements(CompilerContext context)
        {
        }

        [GrammarDefinition("statement",  "expression SEMICOLON")]
        public static void statement(CompilerContext context)
        {
        }

        [GrammarDefinition("expression",  "bracket_expression", "query_expression", "host_expression")]
        public static void expression(CompilerContext context)
        {
        }

        [GrammarDefinition("bracket_expression",  "LP expression RP")]
        public static void bracket_expression()
        {
        }

        [GrammarDefinition("host_expression",  "HOST_EXPRESSION")]
        public static void host_expression()
        {
        }
    }
}