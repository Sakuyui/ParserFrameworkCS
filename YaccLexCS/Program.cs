
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml.Serialization;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.automata;
using YaccLexCS.ycomplier.automata.re;
using YaccLexCS.ycomplier.code;

using YaccLexCS.ycomplier.LrParser;
using YaccLexCS.ycomplier.util;

using Regex = System.Text.RegularExpressions.Regex;

namespace YaccLexCS
{
    
    
    public class Program
    {
        public static void Main()
        {


            //YCompilerConfigurator.GenerateGrammarDefinitionFileFrom("d:\\d.txt", "c:\\out");

            var context = new RuntimeContext();

            //在指定命名空间扫描配置
            var lexer = Lexer.ConfigureFromPackages(new[] { "YaccLexCS.config" }, context);

            //create input stream
            var r = (TextReader)new StringReader("" +
                "let sum = 0;" +
                "let i = 0;" +
                "while(i <= 10){" +
                "   i = i + 1;" +
                "   if(i==7){" +
                "       for(var j = 0; i < 8; j = j + 1){" +
                "           if(j > 5){break;}" +
                "       }" +
                "       break;" +
                "   }" +
                "   sum = sum + i;" +
                "   continue;" +
                "   break;" +
                "}");
            
            var tokenList = new List<Token>();

            //create parser
            /*Lr1Parser parser = Lr1ParserBuilder.ConfigureFromPackages(lexer.TokenNames, new[] { "YaccLexCS.config" });
            parser.InitParser().SetContext(context);
            if (File.Exists("1.bin")) File.Delete("1.bin");
            parser.Serialize("1.bin");*/
            Lr1Parser parser = Lr1ParserBuilder
                .DeSerializeFromFile("1.bin", lexer.TokenNames, new[] { "YaccLexCS.config" });
            parser.SetContext(context);

            /*
             问题：生成器。对于非终结字符，要多一个类型判断

             */

            //在流中词法分析。
            lexer.ParseInStream(r, token =>  //callback function
            {
                if (token.Type == "Skip") return;
                tokenList.Add(token);
                parser.ParseFromCurrentState(token);
            });
           
            tokenList.PrintEnumerationToConsole();
           
            parser.ParseFromCurrentState(new Token("$", "$"));
            
            var root = parser.GetCurrentStack().Peek();
            root.GetTreeShapeDescribe().PrintToConsole();
            
            root.Eval(context);
            

            return;
            
            
            var tokens = lexer.ParseWholeText("while i < 10 {\n    " +
                "sum = sum + i;\n " +
                "i = i + 1; \n " +
                "continue;" + 
                "break;\n" +
                "} sum")
                .Where(e => e.Type != "Skip");
            
            tokens.PrintEnumerationToConsole();
            
        }

        private static void TokenCallBack(Token token)
        {
            if (token.Type != "Skip") token.PrintToConsole();
        }
    }
}
