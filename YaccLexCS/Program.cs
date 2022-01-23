
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
using YaccLexCS.code.structure;
using YaccLexCS.runtime;
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
            var compilerContext = new CompilerContext();
            var runtimeContext = new RuntimeContext();

            //在指定命名空间扫描配置
            var lexer = Lexer.ConfigureFromPackages(new[] { "YaccLexCS.config" }, compilerContext);
           
            //create input stream
            var r = (TextReader)new StringReader("" +
                "let sum = 0;" + //(0,0)
                "let i = 0;" +  //(0,1)
                "while(i <= 10){" + //(0,0)
                "   i = i + 1;" + //(1,1)
                "   if(i==7){" + //(1,1)
                "       for(var j = 0; i < 8; j = j + 1){" + //j:(0,0)
                "           {" +
                "               i = i + 1;" + //i:(3,1)
                "           }" +
                "           if(j > 5){break;}" + //j:(1,0)
                "       }" +
                "       let l = lambda(x, y)=>{x + y;};" +//[x:(0,0), y(0,1) // l:(0,0)
                "       let a = l(sum, i);" + //sum:(2,0) i(2,1) a:(0,2)
                "       " +
                "       a = fact(5);" + //a:(0,1)
                "       break;" +
                "   }" +
                "   sum = sum + i;" +//sum(1,0), i(1,1)
                "   continue;" +
                "   break;" +
                "}" +
                "" +
                "dyfn fact(n){" +
                "   if(n==1) return 1;" +
                "   return n*fact(n-1);" +
                "}" +
                ""); 
             
            var tokenList = new List<Token>();

            //create parser
            /*Lr1Parser parser = Lr1ParserBuilder.ConfigureFromPackages(lexer.TokenNames, new[] { "YaccLexCS.config" });
            parser.InitParser().SetContext(compilerContext);
            if (File.Exists("1.bin")) File.Delete("1.bin");
            parser.Serialize("1.bin");
*/
            Lr1Parser parser = Lr1ParserBuilder
                .DeSerializeFromFile("1.bin", lexer.TokenNames, new[] { "YaccLexCS.config" });
            parser.SetContext(compilerContext);
            // parser.Lr1Table.OutputToFilesAsCsv("D:\\goto.csv", "D:\\trans.csv");

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
            //return;
            parser.ParseFromCurrentState(new Token("$", "$"));
            
            var root = parser.GetCurrentStack().Peek();
            root.GetTreeShapeDescribe().PrintToConsole();
            
            void dfs(ASTNode node)
            {
                if(node is AssignExpressionNode tNode)
                {
                    if(tNode.Count() == 3)
                    {
                        (tNode[0] as ASTTerminalNode).Token.PrintToConsole();
                    }
                }
                else if(node is DefineVarExpressionNode tNode2){
                    (tNode2[1] as ASTTerminalNode).Token.PrintToConsole();
                }
                else
                {
                    foreach(var c in node)
                    {
                        dfs(c);
                    }
                }
            }
            dfs(root);
           // root.Eval(runtimeContext);
            

            
            
            
          /*  var tokens = lexer.ParseWholeText("while i < 10 {\n    " +
                "sum = sum + i;\n " +
                "i = i + 1; \n " +
                "continue;" + 
                "break;\n" +
                "} sum")
                .Where(e => e.Type != "Skip");
            
            tokens.PrintEnumerationToConsole();*/
            
        }

        private static void TokenCallBack(Token token)
        {
            if (token.Type != "Skip") token.PrintToConsole();
        }
    }
}
