
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
                "let sum = 0;" + //(0,3)
                "let i = 0;" +  //(0,4)

                "while(i <= 10){" + //(0,4)
                "   i = i + 1;" + //(1,4) (1,4)
                "   if(i==7){" + //(1,4)
                "       for(var j = 0; i < 8; j = j + 1){" + //j:(0,0) i：(3,4) for需要创新帧再eval第一个块
                "           {" +
                "               i = i + 1;" + //i:(4,4)
                "           }" +
                "           if(j > 5){break;}" + //j:(0,0)
                "       }" +
                "       let l = lambda(x, y)=>{x + y + i;};" +//[x:(0,0), y(0,1)  i:(3,4) // l:(0,0)
                "       let a = l(sum, i);" + //l(0,0) sum:(2,3) i(2,4) a:(0,1)
                "       #Console.WriteLine(\"output by native function : \" + sum + i);" + //call c# native function
                "       a = fact(5);" + //a:(0,1) fact(2,2)
                "       break;" +
                "   }" +
                "   sum = sum + i;" +//sum(1,3), i(1,4)
                "   continue;" +
                "   break;" +
                "}" +
                "" +
                "dyfn fact(n){" + //hoisting  fact:(0,2)
                "   if(n==1) return 1;" + //n(0,0) 
                "   return n*fact(n-1);" + //fact:(1,2)
                "}" +
                "dyfn f1(){" + //f1=(0,1)
                "   return f2();" + //f2=(1,0)
                "}" +
                "dyfn f2(){" + //f2=(0,0)
                "   return f1();" + //f1=(1,1)
                "}" + 
                ""); 
             
            var tokenList = new List<Token>();

            //create parser
        /*    Lr1Parser parser = Lr1ParserBuilder.ConfigureFromPackages(lexer.TokenNames, new[] { "YaccLexCS.config" });
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
                if(token.Type == "STRING")
                {
                    token.SourceText = token.SourceText.Trim('\"');
                }
                tokenList.Add(token);
                parser.ParseFromCurrentState(token);
            });
           
            tokenList.PrintEnumerationToConsole();
            //return;
            parser.ParseFromCurrentState(new Token("$", "$"));
            
            var root = parser.GetCurrentStack().Peek();

            //root.GetTreeShapeDescribe().PrintToConsole();
            //root.Eval(runtimeContext);
            //root.GetTreeShapeDescribe().PrintToConsole();
            // return;



            var lexicalAST = InterpreterHelper.ToLexivalRepresentAst(root);
            lexicalAST.Eval(runtimeContext);


            $"=================convert lexical representation finsh=====================".PrintToConsole();
        }

       
    }
}
