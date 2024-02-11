
using System;
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.runtime;
using YaccLexCS.ycomplier.attribution;
namespace YaccLexCS.config
{
    [GrammarConfiguration]
    public static class GrammarCompilationUnit
    {

        [BeginningGrammarDefinition("compilation_unit",  "definition compilation_unit")]
        public static void compilation_unit()
        {
        }

        [GrammarDefinition("definition",  "task_definition_statement")]
        public static void definition()
        {
        }

        [GrammarDefinition("task_definition_statement",  "DEF_TASK IDENTIFIER LP task_param_list RP RIGHT_ARROW typeT block")]
        public static void task_definition_statement()
        {
        }
    }
}