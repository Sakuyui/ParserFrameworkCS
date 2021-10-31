
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.automata;
using YaccLexCS.ycomplier.automata.re;
using YaccLexCS.ycomplier.util;

using Regex = System.Text.RegularExpressions.Regex;


namespace YaccLexCS
{
    public class Program
    {
        
        
        public static void Main()
        {

            

            //在指定命名空间扫描配置
            var lexer = Lexer.ConfigureFromPackages(new []{"YaccLexCS.config"});
            
            //创建输入流
            var r = (TextReader) new StringReader("while i < 10 {\n    sum = sum + i\n i = i + 1 \n } sum");
            var tokenList = new List<Token>();
            //在流中词法分析。
            lexer.ParseInStream(r, token =>  //callback function
            {
                if(token.Type != "Skip") 
                    tokenList.Add(token);
            });
            
            tokenList.PrintEnumerationToConsole();
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
