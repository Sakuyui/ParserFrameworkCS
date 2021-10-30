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
        // public static void Parse(string str)
        // {
        //     var sb = new StringBuilder(str);
        //     //按匹配优先级分组
        //     var regexes = ReflectionUtil.GetAllTokenDefinition(typeof(TokenList))
        //         .GroupBy(rs => rs.tokenDef.Priority)
        //         .OrderBy(g => g.Key)
        //         .Select(g => g.ToList()).ToArray();
        //
        //     var parserContext = new ParserContext {TokenText = "1.43"};
        //     
        //     
        //     $"parse {sb}".PrintToConsole();
        //     (TokenDefinition definition, MethodInfo info) GetNextToken()
        //     {
        //         if (regexes == null)
        //             throw new Exception();
        //         
        //         (TokenDefinition definition, MethodInfo info) = (null, null);
        //         foreach (var g in regexes)
        //         {
        //             //某个优先级下的所有regex
        //             foreach (var r in g)
        //             {
        //                 
        //                 var re = r.tokenDef.Pattern;
        //                 $"try {re}, {re.IsMatch(sb.ToString())}".PrintToConsole();
        //                 re.Matches(sb + "",0).PrintCollectionToConsole();
        //                 var mm = re.Matches(sb + "", 0);
        //              
        //                 var matches = re.Matches(sb + "", 0).Where(m => m.Index == 0).ToArray();
        //                 
        //                 if(!matches.Any())
        //                     continue;
        //                 
        //                 var m = matches[0];
        //
        //                 void TryUpdate(Match match)
        //                 {
        //                     if (definition == null)
        //                     {
        //                         m = match;
        //                     }
        //                 }
        //                 for (var i = 1; i < matches.Length; i++)
        //                 {
        //                     if(matches[i].Length <= m.Length || matches[i].Index != 0)
        //                         continue;
        //                     m = matches[i];
        //                     definition = r.tokenDef;
        //                     info = r.methodInfo;
        //                 }
        //                 
        //             }
        //             if(definition != null && info != null)
        //                 break;
        //                 
        //         }
        //
        //         if (definition == null || info == null)
        //         {
        //             throw new Exception("can not parse next token");
        //         }
        //
        //         return (definition, info);
        //     }
        //
        //
        //     while (sb.Length > 0)
        //     {
        //         var next = GetNextToken();
        //         $"get token {next.definition.TokenName}".PrintToConsole();
        //         var paramCount = next.info.GetParameters().Length;
        //         next.info.Invoke(null,
        //             paramCount == 0 ? Array.Empty<object>() :
        //             ((paramCount == 1) ? new []{parserContext} : Array.Empty<object>())
        //         );
        //     }
        // }




       
       
        
   


  
        
        public static void Main()
        {

            
            var lexer = Lexer.ConfigureFromPackages(new []{"YaccLexCS"});
            
            lexer.ParseInStream("while i < 10 {\n    sum = sum + i\n i = i + 1 \n } sum").Where(e => e.Type != "Skip").PrintEnumerationToConsole();
            

        }
    }
}
