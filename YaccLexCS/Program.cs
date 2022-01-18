
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
           
            var context = new CompilerContext();
            
            //在指定命名空间扫描配置
            var lexer = Lexer.ConfigureFromPackages(new []{"YaccLexCS.config"}, context);
          
            //create input stream
            var r = (TextReader) new StringReader("sum = 0;i = 0;while(1){i = i + 1; sum = sum + i;}");
            
            var tokenList = new List<Token>();
            
            //create parser
            /*Lr1Parser parser = Lr1ParserBuilder
                .ConfigureFromPackages(lexer.TokenNames, new[] {"YaccLexCS.config"});*/

            Lr1Parser parser = Lr1ParserBuilder
                .DeSerializeFromFile("1.bin",lexer.TokenNames, new[] { "YaccLexCS.config" });
            //parser.Serialize("1.bin");
            parser.SetContext(context);


            //在流中词法分析。
            lexer.ParseInStream(r, token =>  //callback function
            {
                if (token.Type == "Skip") return;
                tokenList.Add(token);
                parser.ParseFromCurrentState(token);
            });

            parser.ParseFromCurrentState(new Token("$", "$"));
            //tokenList.PrintCollectionToConsole();
            
            var root = parser.GetCurrentStack().Peek();
            root.GetTreeShapeDescribe().PrintToConsole();
           
            root.Eval(context);
            

            return;
            
            //it can also used to parsed a whole text if you want. Simply use it as follow. This Function will return a IEnumerable<Token>,
            //you can also provide a callback function to process a token as soon as a token is parsed.
            
            var tokens = lexer.ParseWholeText("while i < 10 {\n    sum = sum + i\n i = i + 1 \n } sum")
                .Where(e => e.Type != "Skip");
            
            tokens.PrintEnumerationToConsole();
            
        }

        private static void TokenCallBack(Token token)
        {
            if (token.Type != "Skip") token.PrintToConsole();
        }
    }
}
