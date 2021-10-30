using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.automata.re;
using YaccLexCS.ycomplier.conf.attribution;

namespace YaccLexCS
{
    public class Lexer
    {
        private Lexer(){}
        public readonly ParserContext ParserContext = new();
        Dictionary<TokenDefinition, MethodInfo> _patternMap = new();
        
        public static Lexer ConfigureFromPackages(IEnumerable<string> scanPackage)
        {
            var lexer = new Lexer();
            YCompilerConfigurator.GetAllTokenDefinitions(
                YCompilerConfigurator.ScanTokenConfiguration(scanPackage)).ElementInvoke(e =>
            {
                var p = e.methodInfo.GetParameters(); //get parameters
                if (!e.tokenDef.UseRegex)
                {
                    lexer._patternMap[e.tokenDef] = e.methodInfo!;
                }
                else
                {
                    var a = ReAutomata.BuildAutomataFromExp(e.tokenDef.SourcePattern);
                    lexer._patternMap[e.tokenDef] = e.methodInfo!;
                }
            });
            return lexer;
        }
        public IEnumerable<Token> ParseInStream(string s) {
            var sb = new StringBuilder(s);
            var cur = "";
        
            void Init()
            {
                foreach (var a in _patternMap)
                {
                    a.Key.Automata?.ResetAutomata();
                }
            }
            
            Init();
        
            var order = _patternMap.GroupBy(e => e.Key.Priority)
                        .OrderBy(g => g.Key).SelectMany(g => g.ToList()).ToList();
            var available = order.ToArray();
        
            void Invoke(MethodBase methodInfo) {
                var p = methodInfo.GetParameters();
                if (!p.Any())
                    methodInfo.Invoke(null, Array.Empty<object>());
                else if(p.Length == 1)
                    methodInfo.Invoke(null, new object?[]{ParserContext});
            }
            
            while (sb.Length > 0) {
                var c = sb[0];

                var cur1 = cur;
                var str = order.Where(e => 
                    !e.Key.UseRegex && (cur1 + c) == e.Key.SourcePattern);
                var t = available.Where(e =>
                            e.Key.UseRegex && e.Key.Automata!.IsCanTransWith(c)).Concat(str)
                    .OrderBy(e => e.Key.Priority).ToArray();
                       
                if(cur == "{")
                    "".PrintToConsole();
                if (!t.Any()) {
                            
                    $"get token {cur}".PrintToConsole();
                    ParserContext.TokenText = cur;
                            
                    cur = "";
                          
                            
                    Invoke(available.First().Value);
                    yield return new Token(ParserContext.TokenText,available.First().Key.TokenName);
                    available = order.ToArray();
                    Init();
                }else {
                    cur += c;
                    sb.Remove(0, 1);
                    t.ElementInvoke(e => {
                        if (e.Key.UseRegex)
                                    e.Key.Automata?.ParseSingleInputFromCurrentStates(c);
                    });
                    available = t.ToArray();
                }
                        
            }
        
            if (!available.Any()) 
                yield break;
            $"get token {cur}".PrintToConsole();
            ParserContext.TokenText = cur;
            Invoke(available.First().Value);
            yield return new Token(ParserContext.TokenText,available.First().Key.TokenName);
        }
    }
}