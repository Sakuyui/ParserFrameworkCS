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
using YaccLexCS.ycomplier.conf.attribution;
using YaccLexCS.ycomplier.util;

using Regex = System.Text.RegularExpressions.Regex;

namespace YaccLexCS
{
    [TokenConfiguration]
    public static class TokenList
    {
        [TokenDefinition("ADD", "+")]
        [TokenDefinition("SUB", "-")]
        [TokenDefinition("MUL", "*")]
        [TokenDefinition("DIV", "/")]
        public static void Operator()
        {
            $"meet operator".PrintToConsole();
        }

        [TokenDefinition("CR", "\n")]
        [TokenDefinition("LBRACE", "(")]
        [TokenDefinition("RBRACE", ")")]
        public static void SpecialCharacter()
        {
            $"meet special character".PrintToConsole();
        }
        
        [TokenDefinition("Skip", @"[ \t]", true)]
        public static void Skip(){}
        
        [TokenDefinition("DOUBLE_LITERAL", @"^(([0-9]+\.[0-9]+)|([1-9][0-9]*)|0)", true)]
        public static void DoubleLiteral(ParserContext context)
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

   
    public class Program
    {
        public static void Parse(string str)
        {
            var sb = new StringBuilder(str);
            //按匹配优先级分组
            var regexes = ReflectionUtil.GetAllTokenDefinition(typeof(TokenList))
                .GroupBy(rs => rs.tokenDef.Priority)
                .OrderBy(g => g.Key)
                .Select(g => g.ToList()).ToArray();

            var parserContext = new ParserContext {TokenText = "1.43"};
            
            
            $"parse {sb}".PrintToConsole();
            (TokenDefinition definition, MethodInfo info) GetNextToken()
            {
                if (regexes == null)
                    throw new Exception();
                
                (TokenDefinition definition, MethodInfo info) = (null, null);
                foreach (var g in regexes)
                {
                    //某个优先级下的所有regex
                    foreach (var r in g)
                    {
                        
                        var re = r.tokenDef.Pattern;
                        $"try {re}, {re.IsMatch(sb.ToString())}".PrintToConsole();
                        re.Matches(sb + "",0).PrintCollectionToConsole();
                        var mm = re.Matches(sb + "", 0);
                     
                        var matches = re.Matches(sb + "", 0).Where(m => m.Index == 0).ToArray();
                        
                        if(!matches.Any())
                            continue;
                        
                        var m = matches[0];

                        void TryUpdate(Match match)
                        {
                            if (definition == null)
                            {
                                m = match;
                            }
                        }
                        for (var i = 1; i < matches.Length; i++)
                        {
                            if(matches[i].Length <= m.Length || matches[i].Index != 0)
                                continue;
                            m = matches[i];
                            definition = r.tokenDef;
                            info = r.methodInfo;
                        }
                        
                    }
                    if(definition != null && info != null)
                        break;
                        
                }

                if (definition == null || info == null)
                {
                    throw new Exception("can not parse next token");
                }

                return (definition, info);
            }


            while (sb.Length > 0)
            {
                var next = GetNextToken();
                $"get token {next.definition.TokenName}".PrintToConsole();
                var paramCount = next.info.GetParameters().Length;
                next.info.Invoke(null,
                    paramCount == 0 ? Array.Empty<object>() :
                    ((paramCount == 1) ? new []{parserContext} : Array.Empty<object>())
                );
            }
        }

        public static void BuildAutomataFromTopExp(string exp)
        {
            $">> Build {exp}".PrintToConsole();
        }

        public static string ConcatAutomata(string a1, string a2)
        {
            $"Concat {a1} with {a2}".PrintToConsole();
            return a1 + a2;
        }
        
        public static string OrMergeAutomata(IEnumerable<string> automatas)
        {
            var str = $"[automata: {automatas.Aggregate((a,b) => a + " | " + b)}]";
            return str;
        }
        public static void AutomataFrom(string s)
        {
            var sb = new StringBuilder(s);
            //stack
            var bStack = new Stack<char>();
            var strStack = new Stack<string>();
            var orExpStack = new Stack<List<string>>();
            
            //register
            var cur = "";
            var orExp = new List<string>();
            
            void ShowStrStack()
            {
                $"current stack: len = {strStack.Count}, content = {strStack.ToEnumerationString()}".PrintToConsole();
                
            }
            void ShowOrStack()
            {
                $"current stack: len = {orExp.Count}, content = {orExp.ToEnumerationString()}".PrintToConsole();
                
            }
            while (sb.Length > 0)
            {
                var c = sb[0];
                sb.Remove(0, 1);
                if (c == '(')
                {
                    $"meet (, begin a new exp, save cur = {cur} to stack".PrintToConsole();
                    bStack.Push('(');
                    strStack.Push(cur);
                    orExpStack.Push(orExp);
                    orExp = new List<string>();
                    cur = "";
                    ShowStrStack();
                }else if (c == ')')
                {
                    $"\n meet ), merge cur string and all element in or stack".PrintToConsole();
                    orExp.Add(cur);
                    
                    BuildAutomataFromTopExp(cur);

                    var result = OrMergeAutomata(orExp);
                    $"build finish..begin merge result = {result}".PrintToConsole();
                 
                    result.PrintToConsole();
                    orExp.Clear();
                    orExp = orExpStack.Pop();
                    
                    
                    if (!bStack.Any())
                        throw new Exception("brace exception");
                    bStack.Pop();
                    cur = strStack.Pop();
                    $"restore str = {cur}\n".PrintToConsole();
                    
                    
                    var r = ConcatAutomata(cur, result);
                    
                    cur = r;
                }
                else if (c == '|')
                {
                    $"meet |, cur str = {cur}, save to 'or stack' ".PrintToConsole();
                    orExp.Add(cur);
                    cur ="";
                    ShowOrStack();
                }
                else
                {
                    cur += c;
                    
                }
                
            }
            $"final = {cur}".PrintToConsole();
        }
        public static void TryParse(string s)
        {
            var sb = new StringBuilder(s);
            //stack
            var bStack = new Stack<char>();
            var strStack = new Stack<string>();
            var orExpStack = new Stack<List<string>>();
            
            //register
            var cur = "";
            var orExp = new List<string>();
            
            void ShowStrStack()
            {
                $"current stack: len = {strStack.Count}, content = {strStack.ToEnumerationString()}".PrintToConsole();
                
            }
            void ShowOrStack()
            {
                $"current stack: len = {orExp.Count}, content = {orExp.ToEnumerationString()}".PrintToConsole();
                
            }
            while (sb.Length > 0)
            {
                var c = sb[0];
                sb.Remove(0, 1);
                if (c == '(')
                {
                    $"meet (, begin a new exp, save cur = {cur} to stack".PrintToConsole();
                    bStack.Push('(');
                    strStack.Push(cur);
                    orExpStack.Push(orExp);
                    orExp = new List<string>();
                    cur = "";
                    ShowStrStack();
                }else if (c == ')')
                {
                    $"\n meet ), merge cur string and all element in or stack".PrintToConsole();
                    orExp.Add(cur);
                    var result = "[exp :" + orExp.Aggregate("", (a, b) => a + " | " + b) + "]";
                    result.PrintToConsole();
                    orExp.Clear();
                    orExp = orExpStack.Pop();
                    
                    
                    if (!bStack.Any())
                        throw new Exception("brace exception");
                    bStack.Pop();
                    cur = strStack.Pop();
                    $"restore str = {cur}".PrintToConsole();
                    var r = cur + result;
                    $"concat {cur} with {result} = ".PrintToConsole();
                    cur = r;
                }
                else if (c == '|')
                {
                    $"meet |, cur str = {cur}, save to 'or stack' ".PrintToConsole();
                    orExp.Add(cur);
                    cur ="";
                    ShowOrStack();
                }
                else
                {
                    cur += c;
                    
                }
                
            }
            $"final = {cur}".PrintToConsole();
        }
        public static void Main()
        {

            ReAutomata reAutomata = new();
            //TryParse("((ab|cd|ef)|aaabc|((ag)(ge)))");
            AutomataFrom(@"(([0-9]+\.[0-9]+)|([1-9][0-9]*)|0)");
            // var tclassType = typeof(TokenList);
            // ReflectionUtil.GetAllTokenDefinition(typeof(TokenList)).PrintEnumerationToConsole();
            // "".PrintToConsole();
            //
            // Parse("1.43+2");

            // ReflectionUtil.GetAllTokenDefinition(typeof(TokenList)).ElementInvoke(e =>
            // {
            //     var p = e.methodInfo.GetParameters(); //get parameters
            //     if(!p.Any())
            //         e.methodInfo.Invoke(null, Array.Empty<object>());
            //     else if (p.Length == 1)
            //         e.methodInfo.Invoke(null, new object?[]{parserContext});
            // });
        }
    }
}
