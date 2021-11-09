using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CIExam.Complier;
using YaccLexCS.ycomplier.code;
using YaccLexCS.ycomplier.LrParser;

namespace YaccLexCS.ycomplier
{
    
    public abstract class Parser
    {
        protected CompilerContext? Context ;
        protected readonly CfgProducerDefinition Definitions;

        public Parser SetContext(CompilerContext context)
        {
            Context = context;
            return this;
        }

        protected Parser(CfgProducerDefinition definitions)
        {
            
            Definitions = definitions;
        }
        protected Parser(CfgProducerDefinition definitions, CompilerContext context)
        {
            Definitions = definitions;
            Context = context;
        }
        public abstract void Parse();
        public abstract void ParseFromCurrentState(Token token);
        public abstract Parser InitParser();
        public object SetParserContextVariable(string name, object obj)
        {
            this[name] = obj;
            return obj;
        }
        protected object? this[string key]
        {
            set
            {
                if (Context != null) Context[key] = value;
            }
            get => Context?[key];
        }

        public bool CykJudge(string input) {
            var n = input.Length;
            var cykTable = new List<List<string>>();
            var ts = input.Select(e => e + "").ToList();

            string GetProductLeftSymbol(string t)
            {
                var s = new HashSet<string>();
                foreach (var g in Definitions.Grammars.Where(g => g.ProduceItem == t))
                {
                    s.Add(g.LeftSymbol);
                }
                return s.Any() ? s.Aggregate((a, b) => a + "|" + b) : "";
            }

            var initList = ts.Select(GetProductLeftSymbol).ToList();
            initList.PrintEnumerationToConsole();
            cykTable.Add(initList);
            
            var times = n - 1;
            for (var i = 0; i < times; i++)
            {
                var last = cykTable[^1];
                var curLayer = new List<string>();
                $"step : {i}, last layer = {last.GetMultiDimensionString()}".PrintToConsole();
                for (var j = 0; j < n - 1 - i; j++)
                {
                    //填充当前层的第J项。但是没这么容易
                    var join = new List<string>();
                    for (var k = 1; k <= i + 1; k++)
                    {
                        //第i层代表符号长i + 1。 所以符号长k 的 和符号长 i + 2 - k的连接
                        var joinLeft = cykTable[k - 1][j]; //符号长为k的层，从当前项开始能够解析的符号
                        var joinRight = cykTable[i + 1 - k][j + k]; //符号长为k的层，从当前项过去k个字符后开始能够解析的符号
                        $"join {joinLeft} with {joinRight}".PrintToConsole();
                        var tmpJoin = (from x in joinLeft.Split("|")
                            join y in joinRight.Split("|")
                                on 1 equals 1
                            select x + y);
                        join.AddRange(tmpJoin);
                    }
                    $"join in layer {i + 2},{j} = {join.GetMultiDimensionString()}".PrintToConsole();
                  
                    join.PrintEnumerationToConsole();
                    
                    var item = join.Select(GetProductLeftSymbol).Where(e => e != "")
                        .Select(e => e.Split("|")).SelectMany(e => e).Distinct().OrderBy(c => c).ToArray();
                    $"items = {item.GetMultiDimensionString()}".PrintToConsole();
                    curLayer.Add(item.Any() ? item.Aggregate((a, b) => a + "|" + b) : "");
                    "".PrintToConsole();
                }
                cykTable.Add(curLayer);
            }
            $"final layer = {cykTable[^1].GetMultiDimensionString()}".PrintToConsole();
            return cykTable[^1].SelectMany(e => e.Split("|")).Contains(Definitions.StartWord);
        }
    }



    public class Lr1Parser : Parser
    {
       
        private readonly Stack<ASTNode> _codeStack = new();
        private readonly Stack<int> _stateStack = new();
        public Lr1Table? Lr1Table { get; private set; }
        private Dictionary<int, MethodInfo>? _methodMap = null;
        private Dictionary<string, Type>? _typeMap = null;
        public Lr1Parser(CfgProducerDefinition definitions) : base(definitions)
        {
        }

        public Lr1Parser(CompilerContext context, CfgProducerDefinition definitions) : base(definitions, context)
        {
        }

        public override void Parse()
        {
            throw new System.NotImplementedException();
        }

        public Stack<ASTNode> GetCurrentStack() => _codeStack;
        public override void ParseFromCurrentState(Token token)
        {
            
            if (Context == null)
                throw new Exception(
                    "parser should have a Context, may use SetContext(new ParserContext()) to solve this problem.");
            "".DebugOutPut();
            
            ("code peek = " + _codeStack.Peek() + " , state = " + _stateStack.Peek() + ", input = " + token).DebugOutPut();
            //根据状态栈顶，和当前输入确定。
            var t = Lr1Table?.Transition[_stateStack.Peek()][token.Type] + "";
            
            if (t == "ACC")
            {
                "================== ACC!!! =================".PrintToConsole();
                return;
            }
            token.PrintToConsole();
            if (t[0] == 's') //如果开头是s就移进 = 新状态压栈 + 新符号从输入队列弹出压栈
            {
                //shift
                var nextState = int.Parse(t[1..]);
                $"shift to {nextState}".DebugOutPut();
                _stateStack.Push(nextState);
                _codeStack.Push(new ASTTerminalNode(token));
                $"code Stack = {_codeStack.ToEnumerationString()}".PrintToConsole();
                $"state Stack = {_stateStack.GetMultiDimensionString()}".PrintToConsole();
                return;
            }
            if (t[0] == 'r')
            {
                //reduction 规约后一定对应一个新状态
                var grammarID = int.Parse(t[1..]);
                $"reduction by ({grammarID}) : {Definitions.Grammars[grammarID]}".PrintToConsole();

                //callback
                if (_methodMap != null && _methodMap.ContainsKey(grammarID))
                {
                    var m = _methodMap[grammarID];
                    if (m.GetParameters().Any())
                    {
                        Context["parser_reduction"] = Definitions.Grammars[grammarID];
                        m.Invoke(null, new object[] {Context});
                        
                    }
                    else
                    {
                        _methodMap[grammarID].Invoke(null, Array.Empty<object>());
                    }
                }
                    
                var g = Definitions.Grammars[grammarID];
                    
               
                var itemList = g.ProduceItem.Split(" ").ToList();
                itemList.PrintEnumerationToConsole();

                var nodeList = new List<ASTNode>();
                foreach (var node in itemList.Select(_ => _codeStack.Pop()))
                {
                    nodeList.Add(node);
                    _stateStack.Pop();
                }
                
                _codeStack.Push(new ASTNonTerminalNode(nodeList ,g.LeftSymbol));

                var gotoNext = Lr1Table?.Goto[_stateStack.Peek()][_codeStack.Peek().NodeName];
                do
                {
                    $"goto => {_stateStack.Peek()}".PrintToConsole();
                    _stateStack.Push(int.Parse(gotoNext.ToString()));
                    gotoNext = Lr1Table?.Goto[_stateStack.Peek()][_codeStack.Peek().NodeName];
                } while (gotoNext != null);




                $"code Stack = {_codeStack.ToEnumerationString()}".PrintToConsole();
                $"state Stack = {_stateStack.GetMultiDimensionString()}".PrintToConsole();
                ParseFromCurrentState(token);
            }

        }

        public void InitGrammarMapping(Dictionary<string, MethodInfo> produceMethodMapping)
        {
            _methodMap ??= new Dictionary<int, MethodInfo>();
            _methodMap.Clear();
            
            foreach (var (grammar, method) in produceMethodMapping)
            {
                var index = Definitions.Grammars.FindIndex(produce => produce.LeftSymbol + "->" + produce.ProduceItem == grammar);
                _methodMap[index] = method;
                
            }

            return;
        }

        public void InitTypeMapping(Dictionary<string, Type> map)
        {
            _typeMap ??= new Dictionary<string, Type>();
            _typeMap.Clear();
            _typeMap = map;
        }
        public void SetTypeMapping(Dictionary<string, Type> typeMapping)
        {
            
        }
        public override Lr1Parser InitParser()
        {
            "".DebugOutPut();
            $"Grammar List = {Definitions.Grammars.GetMultiDimensionString()}\n".DebugOutPut();
            Definitions.AddGrammar(new ProducerDefinitionItem("<S'>->"+Definitions.StartWord));
      
            Definitions.InitProduceMapping();
            "==========================================P SET===================================".DebugOutPut();
            if (Definitions.ProduceMappingList != null)
                foreach (var (key, value) in Definitions.ProduceMappingList)
                {
                    ("[" + key + "] => " + value.Select(e => e.ToEnumerationString()).ToEnumerationString())
                        .DebugOutPut();
                }


            "==========================================FIRST SET===================================".DebugOutPut();
            
            
            
            "============================================I(0)=======================================".PrintToConsole();
            var curPSet = new ProjectSet(new []{new Lr1Item("<S'>", 
                new List<string>(new []{Definitions.StartWord}),
                new List<string>(new []{"$"}))});
         
            curPSet.ApplyClosure(Definitions);
            curPSet.GetProjectItemsDesc().DebugOutPut();
            
            var fi = CfgTools.GetFirstSet(Definitions);
            foreach (var (key, value) in fi)
            {
                $"first({key}) = [{value.AggregateOneOrMore((a, b) => a + ","+ b)}]".PrintToConsole();
            }
            
           
            var projects = new List<ProjectSet> {curPSet};
            var changed = true;
            
            Lr1Table = new Lr1Table(Definitions);
            Lr1Table.AddRow();
            var products = Definitions.Grammars.Select((e, i)=> (i, e))
                .ToDictionary(k => k.i, v => v.e);
            
            var last = 0;
            while (changed)
            {
                changed = false;
                
                var count = projects.Count;
                for (var i = last; i < count; i++)
                {
                    var p = projects[i];
                    MoveProject(new KeyValuePair<int, ProjectSet>(i, p), projects, Lr1Table, products);
                }

                if (projects.Count != last)
                {
                    changed = true;
                    last = count;
                }
            }
            //throw new Exception();
            Lr1Table.Goto.DebugOutPut();
            Lr1Table.Transition.DebugOutPut();
            Lr1Table.OutputToFilesAsCsv("d:\\pl\\goto.csv", "d:\\pl\\trans.csv");
            
            "=================================Init Finish, Goto and transition Table Generated===============================".PrintToConsole();
            "==============================Project sets===================".PrintToConsole();
            $"count = {projects.Count}".DebugOutPut();

            if(File.Exists("d:\\pl\\projects.txt"))
                File.Delete("d:\\pl\\projects.txt");
            var f = File.Open("d:\\pl\\projects.txt", FileMode.OpenOrCreate);
            projects.ElementInvoke((p, i) =>
            {
                var ps = p.GetProjectItemsDesc();
                f.Write($"I({i}): {ps}".ToCharArray().Select(e => (byte)e).Append((byte)'\n').ToArray());
            });
            
            f.Close();


            "==============================Project sets Out to File===================".PrintToConsole();
            
            
            _codeStack.Push(new ASTTerminalNode(new Token("$", "$")));
            _stateStack.Push(0);

            
            "============================== Can Begin Parse ===================".PrintToConsole();


            return this;
        }

        private void MoveProject(KeyValuePair<int, ProjectSet> projectSet, List<ProjectSet> result, Lr1Table table, 
            Dictionary<int, ProducerDefinitionItem> definitionItems)
        {
            var (curId, value) = projectSet;
            $">> From I({curId}) Move".DebugOutPut();
            
            var cSet = value.Where(e => !e.IsReductionItem())
                .Select(e => e.ProduceItems[e.DotPos]).ToHashSet(null);

            if (!cSet.Any())
            {
                return;
            }

            
           
            //projectSet.Value.PrintToConsole();
            
            //解决规约填表
            if (value.Any(p => p.IsReductionItem()))
            {
                var reduction= value.Where(p => p.IsReductionItem()).ToList();
                foreach (var r in reduction)
                {
                    var forwardSearch = r.SearchWordList;
                    
                    foreach (var f in forwardSearch)
                    {
                        if (f == "$" && r.StartWord == "<S'>")
                        {
                            table.Transition[curId][f] = "ACC";
                        }
                        else
                        {
                            $"cur = {r}".DebugOutPut();
                            table.Transition[curId][f] = "r" + definitionItems.First(d =>
                            {
                                var p = d.Value;
                                if (r.StartWord != p.LeftSymbol)
                                    return false;
                                //$"true with {p}".PrintToConsole();
                                //$"now {p.ProduceItem.GetMultiDimensionString()}, {r.ProduceItems[0].GetMultiDimensionString()}".PrintToConsole();
                                return p.ProduceItem == r.ProduceItems.Aggregate("",(a, b) => a + " " + b).Trim();
                            }).Key;
                        }
                      
                    }
                    
                  
                }
            }
            
            //move
            var pSet = value.Where(p => !p.IsReductionItem()).ToList();

            foreach (var c in cSet)
            {
                //$"input - {c}".DebugOutPut();
                var items = pSet.Where(item => item.ProduceItems[item.DotPos] == c)
                    .Select(item => item.MoveForward()).ToList();
                
                
                var ps = new ProjectSet(items);
                //$"after {ps}".DebugOutPut();
                

                //$"closure before {result.Count}".PrintToConsole();
                ps.ApplyClosure(Definitions);
                //$"closure after {result.Count}".PrintToConsole();
                //判重
                var newCode = result.Count;
                if (!result.Contains(ps))
                {
                   
                    result.Add(ps);
                    table.AddRow();
                }
                else
                {
                    newCode = result.FindIndex(e => e.Equals(ps));
                    //code.PrintToConsole();
                }
                
                //处理移进和Goto
                if (Definitions.NonTerminations.Contains(c))
                {
                    table.Goto[curId][c] = newCode;
                }else if (Definitions.Terminations.Contains(c))
                {
                    table.Transition[curId][c] = "s" + newCode;
                }
                
                //存在规约项目，那么要填表
                if (ps.Any(p => p.IsReductionItem()))
                {
                    
                    //需要规约的项目
                    var reduction= ps.Where(p => p.IsReductionItem()).ToList();
                    
                    //对于项目集内每个项
                    foreach (var r in reduction)
                    {
                        //$"reduction {r}".PrintToConsole();
                        var forwardSearch = r.SearchWordList;
                        foreach (var f in forwardSearch)
                        {
                            
                            //$"{f}".PrintToConsole();
                            var k = definitionItems.First(d =>
                            {
                                var p = d.Value;
                                if (r.StartWord != p.LeftSymbol)
                                    return false;
                                //$"a: {p.ProduceItem}, b:{r.ProduceItems.Aggregate("", (a, b) => a + b)}"
                                //    .PrintToConsole();
                                //p.ProduceItem.PrintToConsole();
                                //r.ProduceItems.Aggregate("", (a, b) => a + " " + b).Trim().PrintToConsole();
                                return p.ProduceItem.Equals(r.ProduceItems.Aggregate("", (a, b) => a + " " + b).Trim());
                            }).Key;
                            
                            //"归约" + definitionItems[k] + " " + k + " " + f).PrintToConsole();
                            table.Transition[newCode][f] = r.StartWord == "<S'>" ? "ACC": "r" + k;
                        }
                    }
                }
            }
          
        }
    }
   
}