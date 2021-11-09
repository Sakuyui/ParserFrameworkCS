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
        [TokenDefinition("LOGIC_OP", @"&&|\|\|", true)]
        public static void Operator(CompilerContext content)
        {
            $"meet operator {content.TokenText}".PrintToConsole();
        }
        
        
        
        
        [TokenDefinition("EQ", "==")]
        public static void Eq(CompilerContext content)
        {
            $"meet operator {content.TokenText}".PrintToConsole();
        }
        
        [TokenDefinition("ASSIGN", "=")]
        public static void Assign(CompilerContext content)
        {
            $"meet operator {content.TokenText}".PrintToConsole();
        }
        [TokenDefinition("STRING", "\".*\"", true)]
        public static void StringType(CompilerContext context)
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
        [TokenDefinition("FOR", "for")]
        public static void KeyWord()
        {
            
        }

        [TokenDefinition("ID", @"[A-Z_a-z]+", true, 1)]
        public static void Id(CompilerContext context)
        {
            
        }
        [TokenDefinition("DOUBLE_LITERAL", @"[0-9]+\.[0-9]+|[1-9][0-9]*|0", true)]
        public static void DoubleLiteral(CompilerContext context)
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