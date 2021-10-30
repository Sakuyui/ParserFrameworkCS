// See https://aka.ms/new-console-template for more information

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
using YaccLexCS.ycomplier.conf.attribution;
using YaccLexCS.ycomplier.util;

using Regex = System.Text.RegularExpressions.Regex;


namespace YaccLexCS
{
    public class Program
    {
        
        
        public static void Main()
        {

            

            //指定されたパッケージネームスからToken Configurationクラスを探す。
            var lexer = Lexer.ConfigureFromPackages(new []{"YaccLexCS"});
            
            //Use lexer in stream. Simply provide a reader of text stream, and a callback function to process a token as soon as the token is parsed.
            var r = (TextReader) new StringReader("while i < 10 {\n    sum = sum + i\n i = i + 1 \n } sum");
            
            //lexer.ParseInStream(r, TokenCallBack); 
            
            lexer.ParseInStream(r, token =>  //lambda expression can also be used.
            {
                if(token.Type != "Skip") token.PrintToConsole();
            });
            
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
