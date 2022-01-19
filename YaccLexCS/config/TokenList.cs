using System;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config
{
    
    [TokenConfiguration]
    public static class TokenList
    {
        
        [TokenDefinition("ADD", "+")]
        [TokenDefinition("SUB", "-")]
        [TokenDefinition("MUL", "*")]
        [TokenDefinition("MOD", "%")]
        [TokenDefinition("DIV", "/")]
       
        public static void Operator(RuntimeContext content)
        {
            $"meet operator {content.TokenText}".PrintToConsole();
        }
        [TokenDefinition("LOGICAL_OR", @"\|\|", true)]
        [TokenDefinition("LOGICAL_AND", "&&")]
        [TokenDefinition("NE", "!=")]
        [TokenDefinition("LT", "<")]
        [TokenDefinition("LE", "<=")]
        [TokenDefinition("GT", ">")]
        [TokenDefinition("GE", ">=")]

        public static void Relation(RuntimeContext content)
        {

        }
        
        [TokenDefinition("COMMA", ",")]
        public static void Romma(RuntimeContext content)
        {

        }

        [TokenDefinition("EQ", "==")]
        public static void Eq(RuntimeContext content)
        {
            $"meet operator {content.TokenText}".PrintToConsole();
        }
        
        [TokenDefinition("ASSIGN", "=")]
        public static void Assign(RuntimeContext content)
        {
            $"meet operator {content.TokenText}".PrintToConsole();
        }
        [TokenDefinition("STRING", "\".*\"", true)]
        public static void StringType(RuntimeContext context)
        {
            var v = context.TokenText;
            $"get string = {v}".PrintToConsole();
            context.TokenVal = v;
        }
        
        
        [TokenDefinition("CR", "\n")]
        [TokenDefinition("LP", "(")]
        [TokenDefinition("RP", ")")]
        [TokenDefinition("LC", "{")]
        [TokenDefinition("RC", "}")]
        [TokenDefinition("SEMICOLON", ";")]
        public static void SpecialCharacter()
        {
            $"meet special character".PrintToConsole();
        }
        
        [TokenDefinition("Skip", @"[ \r\t]", true)]
        public static void Skip(){}
        
        
        [TokenDefinition("WHILE", "while")]
        [TokenDefinition("IF", "if")]
        [TokenDefinition("ELSE", "else")]
        [TokenDefinition("ELSIF", "elsif")]
        [TokenDefinition("FOR", "for")]
        [TokenDefinition("FLASE","false")]
        [TokenDefinition("TRUE_T", "true")]
        [TokenDefinition("NULL", "null")]
        [TokenDefinition("CONTINUE", "continue")]
        [TokenDefinition("RETURN", "return")]
        [TokenDefinition("BREAK", "break")]
        [TokenDefinition("LET", "let")]
        [TokenDefinition("VAR", "var")]
        public static void KeyWord()
        {
            
        }

        [TokenDefinition("ID", @"[A-Z_a-z]+", true, 1)]
        public static void Id(RuntimeContext context)
        {
            
        }
        [TokenDefinition("DOUBLE_LITERAL", @"[0-9]+\.[0-9]+|[1-9][0-9]*|0", true)]
        public static void DoubleLiteral(RuntimeContext context)
        {
            $"DOUBLE_LITERAL with val = {context.TokenText}".PrintToConsole();
            var val = double.Parse(context.TokenText);
            context.TokenVal = val;
        }

        [TokenDefinition("ERR", ".", true, 255)]
        public static void Error()
        {
            $"error token!".PrintToConsole();
            throw new Exception();
        }
        
    }
    
    // [TokenConfiguration]
    // public static class TokenList
    // {
    //     [TokenDefinition("ADD", "+")]
    //     [TokenDefinition("SUB", "-")]
    //     [TokenDefinition("MUL", "*")]
    //     [TokenDefinition("DIV", "/")]
    //     public static void Operator(ParserContext content)
    //     {
    //         $"meet operator {content.TokenText}".PrintToConsole();
    //     }
    //
    //     [TokenDefinition("CR", "\n")]
    //     [TokenDefinition("LBRACE", "(")]
    //     [TokenDefinition("RBRACE", ")")]
    //     public static void SpecialCharacter()
    //     {
    //         $"meet special character".PrintToConsole();
    //     }
    //     
    //     [TokenDefinition("Skip", @"[ \t]", true)]
    //     public static void Skip(){}
    //     
    //     [TokenDefinition("DOUBLE_LITERAL", @"[0-9]+\.[0-9]+|[1-9][0-9]*|0", true)]
    //     public static void DoubleLiteral(ParserContext context)
    //     {
    //         $"DOUBLE_LITERAL with val = {context.TokenText}".PrintToConsole();
    //         var val = double.Parse(context.TokenText);
    //         context.TokenVal = val;
    //     }
    //
    //     [TokenDefinition("ERR", ".", true, 255)]
    //     public static void Error()
    //     {
    //         $"error token!".PrintToConsole();
    //         throw new Exception();
    //     }
    // }
}