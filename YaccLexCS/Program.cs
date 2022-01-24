
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
        public static ASTNode ToLexivalRepresentAst(ASTNode root)
        {
            void dfs(ASTNode node, Stack<Dictionary<string, (int depth, int order)>> s, int d)
            {

                (int depth, int order) TrackBack(Token t)
                {
                    var find = 0;
                    var id = t.SourceText;
                    foreach (var sFrame in s)
                    {
                        if (sFrame.ContainsKey(id))
                        {
                            var res = (find, sFrame[id].order);

                            $">>>represent {id} with {res}\r\n".PrintToConsole();
                            t.LexivalDistance = res;
                            return (find, sFrame[id].order);
                        }
                        find++;
                    }
                    $"not found {id}, it may in global area".PrintToConsole();
                    t.LexivalDistance = (-1, -1);
                    return (-1, -1);
                }
                List<string> DfsGetParamsList(ASTNode pNode, List<string> ls = null)
                {
                    ls ??= new();
                    if (pNode is TypelessParamNode typelessParamNode)
                    {
                        ls.Add(((ASTTerminalNode)typelessParamNode[0]).Token.SourceText);
                    }
                    else if (pNode is TypedParamNode typedParamNode)
                    {
                        ls.Add(((ASTTerminalNode)typedParamNode[1]).Token.SourceText);
                    }
                    else
                    {
                        foreach (var c in pNode)
                            DfsGetParamsList(c, ls);
                    }
                    return ls;
                }
                if (node is CompileUnitNode cNode)
                {
                    if (cNode.Count() == 1)
                        dfs(cNode[0], s, d);
                    else
                    {
                        var l = node[0];
                        var r = node[1][0]; //希望提前暴露定义
                        if (r is DefinitionNode)
                        {
                            dfs(r, s, d);
                            dfs(l, s, d);
                        }
                        else
                        {
                            dfs(l, s, d);
                            dfs(r, s, d);
                        }
                    }
                }
                else if (node is AssignExpressionNode tNode)
                {
                    if (tNode.Count() == 3)
                    {
                        var token = (tNode[0] as ASTTerminalNode).Token;
                        $"ID in left Assignment {token}".PrintToConsole();
                        var find = 0;
                        TrackBack(token);
                        dfs(tNode[2], s, d);
                    }
                    else
                    {
                        dfs(tNode[0], s, d);
                    }

                }
                else if (node is DefineVarExpressionNode tNode2)
                {
                    var token = (tNode2[1] as ASTTerminalNode).Token;
                    $"ID in Definition {token}".PrintToConsole();
                    $"define {token.SourceText} in depth {d}".PrintToConsole();
                    var top = s.Peek();
                    var l = top.Count();
                    top[token.SourceText] = (0, l);
                    token.LexivalDistance = (0, l);
                    $">>>represent {token} with {top[token.SourceText]}\r\n".PrintToConsole();
                    dfs(tNode2.Last(), s, d);
                }
                else if (node is PrimaryExpressionNode tNode3)
                {
                    if ((tNode3[0] as ASTTerminalNode).Token.Type == "ID")
                    {
                        var token = (tNode3[0] as ASTTerminalNode).Token;
                        $"ID in read {token}".PrintToConsole();
                        TrackBack(token);
                    }

                }
                else if (node is BlockNode tNode4)
                {
                    "!!depth++".PrintToConsole();
                    s.Push(new Dictionary<string, (int depth, int order)>());
                    foreach (var c in tNode4)
                    {
                        dfs(c, s, d + 1);
                    }
                    s.Pop();
                    "depth--".PrintToConsole();
                }
                else if (node is ForStatementNode tNode5)
                {
                    "!!depth++ in for".PrintToConsole();
                    s.Push(new Dictionary<string, (int depth, int order)>());
                    foreach (var c in tNode5.SkipLast(1))
                    {
                        dfs(c, s, d + 1);
                    }
                    if (tNode5.Last()[0] is BlockNode bNode)
                    {
                        foreach (var c in bNode) dfs(c, s, d + 1);
                    }
                    s.Pop();
                    "depth--".PrintToConsole();
                }
                else if (node is AccessExpressionNode node6)
                {
                    if (node6.Count() == 4)
                    {
                        $"call".PrintToConsole();
                        TrackBack((node6[0] as ASTTerminalNode).Token);
                        foreach (var c in node6.Skip(1))
                        {
                            dfs(c, s, d);
                        }
                    }
                    else if (node6.Count() == 3)
                    {
                        dfs(node6[1], s, d);
                    }
                    else if (node6.Count() == 1)
                    {
                        dfs(node6[0], s, d);
                    }
                }
                else if (node is FunctionDefinitionNode node7)
                {

                    $"new function".PrintToConsole();
                    var t = (node7[1] as ASTTerminalNode).Token;
                    var id = t.SourceText;
                    $"ID in Definition {id}".PrintToConsole();
                    $"define {id} in depth {d}".PrintToConsole();
                    t.LexivalDistance = (0, s.Peek().Count());
                    s.Peek()[id] = (0, s.Peek().Count());
                    var ls = DfsGetParamsList(node7[3]);
                    s.Push(new());
                    foreach (var p in ls)
                    {
                        var r = (0, s.Peek().Count());
                        $"define param {p} in {r}".PrintToConsole();
                        s.Peek()[p] = r;
                    }
                    foreach (var c in node7.Last())
                    {
                        dfs(c, s, d);
                    }
                    s.Pop();
                }else if (node is LambdaExpressionNode node8)
                {
                    var ls = DfsGetParamsList(node8[2]);
                    s.Push(new());
                    foreach (var p in ls)
                    {
                        var r = (0, s.Peek().Count());
                        $"define param {p} in {r}".PrintToConsole();
                        s.Peek()[p] = r;
                    }
                    foreach (var c in node8.Last())
                    {
                        if(c is BlockNode block)
                        {
                            foreach(var c2 in c)
                                dfs(c2, s, d);
                        }
                        else
                        {
                            dfs(c, s, d);
                        }
                    }
                    s.Pop();
                }
                else
                {
                    foreach (var c in node)
                    {
                        dfs(c, s, d);
                    }
                }
            }
            var stack = new Stack<Dictionary<string, (int, int)>>();
            stack.Push(new());
            dfs(root, stack, 0);
            return root;
        }
        public static void Main()
        {
            
            var compilerContext = new CompilerContext();
            var runtimeContext = new RuntimeContext();

            //在指定命名空间扫描配置
            var lexer = Lexer.ConfigureFromPackages(new[] { "YaccLexCS.config" }, compilerContext);
            
           
            //create input stream
            var r = (TextReader)new StringReader("" +
                "let sum = 0;" + //(0,1)
                "let i = 0;" +  //(0,2)
                "while(i <= 10){" + //(0,2)
                "   i = i + 1;" + //(1,2) (1,2)
                "   if(i==7){" + //(1,2)
                "       for(var j = 0; i < 8; j = j + 1){" + //j:(0,0) i：(3,2) for需要创新帧再eval第一个块
                "           {" +
                "               i = i + 1;" + //i:(4,2)
                "           }" +
                "           if(j > 5){break;}" + //j:(0,0)
                "       }" +
                "       let l = lambda(x, y)=>{x + y + i;};" +//[x:(0,0), y(0,1)  i:(3,2) // l:(0,0)
                "       let a = l(sum, i);" + //sum:(2,1) i(2,2) a:(0,1)
                "       #Console.WriteLine(\"output by native function : \" + sum + i);" +
                "       a = fact(5);" + //a:(0,1) fact(2,0)
                "       break;" +
                "   }" +
                "   sum = sum + i;" +//sum(1,1), i(1,2)
                "   continue;" +
                "   break;" +
                "}" +
                "" +
                "dyfn fact(n){" + //hosting  fact:(0,0) in depth=0
                "   if(n==1) return 1;" + //n(0,0) 
                "   return n*fact(n-1);" + //fact:(1,0)
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
            root.Eval(runtimeContext);
            //root.GetTreeShapeDescribe().PrintToConsole();
            // return;



            //var lexicalAST = ToLexivalRepresentAst(root);
           // lexicalAST.Eval(runtimeContext);
            
            
            
        }

       
    }
}
