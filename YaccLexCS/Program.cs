
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
using YaccLexCS.ycomplier.LrParser;
using YaccLexCS.ycomplier.util;

using Regex = System.Text.RegularExpressions.Regex;


namespace YaccLexCS
{
    public class Program
    {
        
        
        public static void Main()
        {


            var context = new ParserContext();
            //在指定命名空间扫描配置
            var lexer = Lexer.ConfigureFromPackages(new []{"YaccLexCS.config"}, context);
            
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




            var gs = YCompilerConfigurator.GetAllGrammarDefinitions(
            YCompilerConfigurator.ScanGrammarConfiguration(new []{"YaccLexCS.config.grammars"})).ToList();
            
            foreach (var valueTuple in gs)
            {
                valueTuple.tokenDef.PrintToConsole();
            }
            
            var cfgDefSet = gs.Select(g => g.tokenDef.Name).ToHashSet();
            var tokenNames = lexer.TokenNames;
            
            var symbolsAppearInCfg = gs.SelectMany(e => 
                e.tokenDef.CfgItem.SelectMany(s => s.Split("|").SelectMany(item => item.Split(" "))));
            
            //check
            void CheckDef()
            {
                var except = symbolsAppearInCfg.Except(tokenNames).Except(cfgDefSet);
                
                if (except.Any())
                {
                    throw new Exception("CFG Definition uncompleted, unrecognized symbols :" + except.ToEnumerationString());
                }
            }
            CheckDef();

            var cfg = gs.GroupBy(e => e.tokenDef.Name).Select(g => (g.Key, g.ToList()))
                .ToDictionary(k => k.Key, v => v.Item2);
            var grammars = cfgDefSet.Select(l =>
            {
                var c = cfg[l];
                var s = c.SelectMany(e => e.tokenDef.CfgItem).ToList();
                return l + "->" + s.AggregateOneOrMore((a, b) => a + "|" + b);
            }).ToArray();
            var terminations = tokenNames.AggregateOneOrMore((a, b) => a +"|" + b);
            grammars.PrintEnumerationToConsole();
            
            terminations.PrintToConsole();
            var grammarSet = new ProducerDefinition(grammars, terminations, "program");
            grammarSet.NonTerminationWords.PrintEnumerationToConsole();
            grammarSet.Terminations.PrintEnumerationToConsole();
            var parser = new GrammarParser(grammarSet);
            parser.InitParse();
            
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
