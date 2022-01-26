
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

        [GrammarDefinition("definition",  "function_definition", "class_definition")]
        public static void definition()
        {
        }

        [GrammarDefinition("function_definition",  "DYFN ID LP params_list RP block", "DYFN ID LP RP block")]
        public static void function_definition()
        {
        }


        [GrammarDefinition("class_definition", "CLASS ID COLON id_list LC class_body RC", "CLASS ID LC class_body RC")]
        public static void class_definition()
        {
        }

        [GrammarDefinition("class_body", "field_or_function_list")]
        public static void class_body()
        {
        }

        [GrammarDefinition("field_or_function_list", "field_or_function_list field_or_function", "field_or_function")]
        public static void field_or_function_list()
        {
        }

        [GrammarDefinition("field_or_function", "class_field", "class_function", "access_control_list class_field", "access_control_list class_function")]
        public static void field_or_function()
        {
        }

        [GrammarDefinition("class_field", "ID ID SEMICOLON", "ID SEMICOLON", "ID ID ASSIGN expression SEMICOLON", "ID ASSIGN expression SEMICOLON")]
        public static void class_field()
        {
        }

        [GrammarDefinition("class_function", "ID LP params_list RP block", "ID LP RP block")]
        public static void class_function()
        {
        }

        [GrammarDefinition("access_control_list", "access_control_list access_control_word", "access_control_word")]
        public static void access_control_list()
        {
        }

        [GrammarDefinition("access_control_word", "PRIVATE", "PUBLIC", "PROTECTED", "STATIC")]
        public static void access_control_word()
        {
        }


    }
}