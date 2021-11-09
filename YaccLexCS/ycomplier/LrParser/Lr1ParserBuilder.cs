using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.ycomplier.LrParser
{
    public static class Lr1ParserBuilder
    {
        public static Lr1Parser ConfigureFromPackages(IEnumerable<string> terminationNames,IEnumerable<string> packageNames)
        {
            var gs = YCompilerConfigurator.GetAllGrammarDefinitions(
            YCompilerConfigurator.ScanGrammarConfiguration(packageNames)).ToList();
            
           
            foreach (var valueTuple in gs)
            {
                valueTuple.PrintToConsole();
            }
            
            var cfgDefSet = gs.Select(g => g.tokenDef.Name).ToHashSet();
            var tokenNames = terminationNames.ToList();
                        
            var symbolsAppearInCfg = gs.SelectMany(e => 
                            e.tokenDef.CfgItem.SelectMany(s => s.Split("|").SelectMany(item => item.Split(" "))));
            string startWord = "";
            
            //check
            void CheckDef()
            {
                var except = symbolsAppearInCfg.Except(tokenNames).Except(cfgDefSet);
                var startGrammar = gs.Select(g => g.tokenDef).FirstOrDefault(d => d is BeginningGrammarDefinition,null);
                if (startGrammar == null)
                    throw new Exception("can find beginning word definition");
                startWord = startGrammar.Name;
                if (startWord == "" || except.Any() || !cfgDefSet.Contains(startWord))
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
            
            var grammarSet = new CfgProducerDefinition(grammars, terminations, startWord);
           


            Dictionary<string, MethodInfo> methodMapping = new();
            Dictionary<string, Type> typeMapping = new();
            foreach (var (tokenDef, methodInfo) in gs)
            {
                var name = tokenDef.Name;
                typeMapping[tokenDef.Name] = tokenDef.Type;
                foreach (var s in tokenDef.CfgItem.Select(i => name + "->" + i))
                {
                    methodMapping[s] = methodInfo;
                }
            }
            
            var parser = new Lr1Parser(grammarSet);
            parser.InitGrammarMapping(methodMapping);
            parser.InitTypeMapping(typeMapping);
            return parser;
        }
    }
}