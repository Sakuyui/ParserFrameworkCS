using System;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.automata
{
    public class ReAutomataConstruction
    {
   

        public static object? EnterPlusChar(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
         
            var bStack = (Stack<char>)context["stack_Brace"];
            
            
            var strStack = (Stack<string>)context["tmp_strStack"];
            var andStack = (Stack<Automata>) context["stack_AndAutomata"];
            
            var cur = (string) context["tmp_cur"];
            var curAutomata = (Automata) context["automata"];
            $"meet +, cur automata = \n {curAutomata}".PrintToConsole();
            
            
            var orExpAutomataStack =  (Stack<List<Automata>>) context["stack_OrAutomata"];
            var orExpAutomata = (List<Automata>) context["orExpAutomata"];
            var orExpStack =  (Stack<List<string>>) context["tmp_OrExpStack"];
            var orExp = (List<string>) context["tmp_OrExp"];
            var lastNodeStack = (Stack<AutomataNode>) context["stack_lastNode"];
            var lastNode = (AutomataNode) context["lastNode"];
            
            var lastResult = (AutomataNode) context["lastResult"];
            $"lastResult Node = {lastResult.NodeId}".PrintToConsole();
            $"last Node = {lastNode.NodeId}".PrintToConsole();
            curAutomata.AddEdge(new ReEdge(lastNode, lastResult, CommonTransitionStrategy.EpsilonTrans.Instance));
            curAutomata.PrintToConsole();
            context["lastResult"] = lastNode;
            
            return null;
        }

        public static object? EnterLeftMBrace(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            $"meet [, begin a new exp, to get char range state".PrintToConsole();
            context["v_charRange_desc"] = "";
            var curAutomata = (Automata) context["automata"];
            context["lastResult"] = curAutomata.NodeMap[curAutomata.Nodes.Count - 1];
            //var bStack = (Stack<char>)context["stack_Brace"];
            //bStack.Push('(');
            
            return null;
        }

        public static object? ProcessSlashReturn(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            var stack = (Stack<object>) context["stateStack"];
            stack.Pop();
            return null;
        }

        public static object? ProcessSlashChar(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            var stack = (Stack<object>) context["stateStack"];
            if ((int)stack.Peek() == 1)
            {
                context["v_charRange_desc"] = context["v_charRange_desc"] + "" + input;
                $"now = {context["v_charRange_desc"]}".PrintToConsole();
            }else if ((int) stack.Peek() == 0)
            {
                context["tmp_cur"] = (string)context["tmp_cur"] + (char)input;
                AddSingleCharCompareNode(input, objs);
                ((Automata) context["automata"]).PrintToConsole();
            }

            return null;
        }
        public static object? ProcessCharsSet(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            $"\n process {context["v_charRange_desc"]}".PrintToConsole();
            var desc = (string)context["v_charRange_desc"];
            List<(char f, char e)> ranges = new();
            HashSet<char> singleChar = new();
            for (var i = 0; i < desc.Length;)
            {
                if (i == desc.Length - 1)
                {
                    singleChar.Add(desc[i++]);
                }else if (i < desc.Length - 1 && desc[i + 1] != '-')
                {
                    singleChar.Add(desc[i++]);
                }
                else
                {
                    if (!(i < desc.Length - 2))
                        throw new Exception("error format in [...]");
                    ranges.Add((desc[i], desc[i + 2]));
                    i += 3;
                }
            }
            

            var targetRange = new List<(int start, int end)>();
            //区间合并
            foreach (var r in ranges.OrderBy(e => e.f))
            {
                if (targetRange.Count == 0)
                {
                    targetRange.Add(r);
                }
                else
                {
                    var last = targetRange[^1];
                    if (last.end < r.f)
                    {
                        targetRange.Add(r);
                    }
                    else
                    {
                        targetRange[^1] = (targetRange[^1].start, r.e); //merge
                    }
                }
            }
            //创建迁移判断策略
            var customTrans = new CommonTransitionStrategy.CustomTrans(delegate(AutomataContext? ctx, object? item, object[] objects)
                {
                    if (item == null) return false;
                    return singleChar.Contains((char) item) || targetRange.Any(r => (char) item >= r.start && (char) item <= r.end);
                });
            
            targetRange.PrintEnumerationToConsole();
            singleChar.PrintEnumerationToConsole();
            
            //connect
            var lastNode = (AutomataNode) context["lastNode"];
            var automata = (Automata) context["automata"];

            var node = new AutomataNode(automata.Nodes.Count);
            automata.AddNode(node);
            automata.AddEdge(new ReEdge(lastNode, node, customTrans));
            context["lastNode"] = node;

            $"add a char set {desc}".PrintToConsole();

            context["tmp_cur"] = (string)context["tmp_cur"] + (char)input;
            automata.PrintToConsole();

            return null;

        }
        public static object? InReadingCharSet(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            $"get char {input} (reading char set state)".PrintToConsole();
            context["v_charRange_desc"] = context["v_charRange_desc"] + "" + input;
            context["tmp_cur"] = (string)context["tmp_cur"] + (char)input;
            $"now = {context["v_charRange_desc"]}".PrintToConsole();
            //var bStack = (Stack<char>)context["stack_Brace"];
            //bStack.Push('(');
            
            return null;
        }
        public static object? EnterRightBrace(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
         
            var bStack = (Stack<char>)context["stack_Brace"];
            
            
            var strStack = (Stack<string>)context["tmp_strStack"];
            var andStack = (Stack<Automata>) context["stack_AndAutomata"];
            
            var cur = (string) context["tmp_cur"];
            var curAutomata = (Automata) context["automata"];
            $"meet ), return from subroutine, cur automata = \n {curAutomata}".PrintToConsole();
            
            
            var orExpAutomataStack =  (Stack<List<Automata>>) context["stack_OrAutomata"];
            var orExpAutomata = (List<Automata>) context["orExpAutomata"];
            var orExpStack =  (Stack<List<string>>) context["tmp_OrExpStack"];
            var orExp = (List<string>) context["tmp_OrExp"];
            var lastNodeStack = (Stack<AutomataNode>) context["stack_lastNode"];
            var lastNode = (AutomataNode) context["lastNode"];
            
            
            orExp.Add(cur);
            orExpAutomata.Add(curAutomata);
            var result = OrMergeAutomata(orExp);
            var resultAutomata = OrMergeAutomata(orExpAutomata);
            
            $"build finish..begin merge result = {result}".PrintToConsole();
            
            orExp.Clear();
            orExp = null;
            context["tmp_OrExp"] = orExpStack.Pop();
            orExp = (List<string>) context["tmp_OrExp"];
            
            if (!bStack.Any())
                throw new Exception("brace exception");
            bStack.Pop();
            cur = strStack.Pop();
            $"restore str = {cur}\n".PrintToConsole();
            curAutomata = andStack.Pop();
            context["automata"] = curAutomata;
            var r = ConcatAutomata(cur, result);
            context["lastResult"] = curAutomata.NodeMap[curAutomata.AcceptState.First()];
            resultAutomata = ConcatAutomata(curAutomata, resultAutomata);
            context["lastNode"] = resultAutomata.NodeMap[resultAutomata.Nodes.Count - 1];
            $"lastResultNode = {((AutomataNode)context["lastResult"]).NodeId}".PrintToConsole();
            
            //         // orExp.Add(cur);
            //         //
            //         // ReAutomata.BuildAutomataFromTopExp(cur);
            //         //
            //         // var result = OrMergeAutomata(orExp);
            //         // $"build finish..begin merge result = {result}".PrintToConsole();
            //         //
            //         // result.PrintToConsole();
            //         // orExp.Clear();
            //         // orExp = orExpStack.Pop();
            //         //
            //         //
            //         // if (!bStack.Any())
            //         //     throw new Exception("brace exception");
            //         // bStack.Pop();
            //         // cur = strStack.Pop();
            //         // $"restore str = {cur}\n".PrintToConsole();
            //         //
            //         //
            //         // var r = ConcatAutomata(cur, result);
            //         // lastResult = result;
            //         // $"last result = {lastResult}".PrintToConsole();
            //         // cur = r;
            
            return null;
        }

        
        private static Automata OrMergeAutomata(IReadOnlyCollection<Automata> orExpAutomata)
        {
            if (!orExpAutomata.Any())
            {
                return null;
            }

            if (orExpAutomata.Count == 1)
                return orExpAutomata.First();

            var automata = new Automata();
            var iNode = new AutomataNode(0);
            automata.AddNode(iNode);
            var endSet = new List<int>();
            foreach (var a in orExpAutomata)
            {
                var c = a.Nodes.Count;
                foreach (var node in a.Nodes)
                {
                    var es = a.NodeNext[node.NodeId];
                    automata.AddNode(new AutomataNode((int) node.NodeId + c));
                    
                }
                endSet.Add(c + a.Nodes.Count);
                foreach (var e in a.Edges)
                {
                    automata.AddEdge(new ReEdge(automata.NodeMap[(int)e.FromNode.NodeId + c]
                        , automata.NodeMap[(int)e.ToNode.NodeId + c], e.EventTransInEdge, e.IsCanTrans));
                }
            }

            var end = automata.Nodes.Count;
            automata.AddNode(new AutomataNode(end));
            foreach (var e in endSet)
            {
                automata.AddEdge(new ReEdge(automata.NodeMap[e], automata.NodeMap[end], 
                    CommonTransitionStrategy.EpsilonTrans.Instance));
            }
            return automata;

        }

        public static string ConcatAutomata(string a1, string a2)
        {
            $"Concat {a1} with {a2}".PrintToConsole();
            return a1 + a2;
        }
        public static Automata? ConcatAutomata(Automata? a1, Automata? a2)
        {
            if (a1 == null)
                return a2;
            if (a2 == null)
                return a1;
            
            $"Concat {a1} \n with {a2}\n".PrintToConsole();
            var c = a1.Nodes.Count;
            var newNodes = a2.Nodes.Select(e => new AutomataNode(c + (int) e.NodeId)).ToArray();
            
            a1.AddNodes(newNodes);
            foreach(var node in newNodes)
            {
                var es = a2.NodeNext[(int) node.NodeId - c];
                foreach (var e in es)
                {
                    a1.AddEdge(new ReEdge(a1.NodeMap[(int)e.FromNode.NodeId + c], a1.NodeMap[(int)e.ToNode.NodeId + c]
                        , e.EventTransInEdge, e.IsCanTrans));
                }
                
            }

            foreach (var acc in a1.AcceptState)
            {
                foreach (var s in a2.StartNodes)
                {
                    a1.AddEdge(new ReEdge(a1.NodeMap[acc], a1.NodeMap[c + (int)s], CommonTransitionStrategy.EpsilonTrans.Instance));
                }
                
            }
            a1.AcceptState.Clear();
            a1.AcceptState.Add(a1.Nodes.Count - 1);
            a1.PrintToConsole();
            return a1;
        }
        public static string OrMergeAutomata(IEnumerable<string> automatas)
        {
            var str = $"[automata: {automatas.Aggregate("",(a,b) => a + " | " + b)}]";
            return str;
        }
        public static object? EnterLeftBrace(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            $"meet (, begin a new exp, save cur = {context["tmp_cur"]} to stack".PrintToConsole();
            var bStack = (Stack<char>)context["stack_Brace"];
            bStack.Push('(');
            
            var strStack = (Stack<string>)context["tmp_strStack"];
            var andStack = (Stack<Automata>) context["stack_AndAutomata"];
            
            var cur = (string) context["tmp_cur"];
            var curAutomata = (Automata) context["automata"];

            curAutomata.SetAcceptState(curAutomata.NodeMap[curAutomata.Nodes.Count - 1].NodeId);
            strStack.Push(cur);
            andStack.Push(curAutomata);
            
            var orExpAutomataStack =  (Stack<List<Automata>>) context["stack_OrAutomata"];
            var orExpAutomata = (List<Automata>) context["orExpAutomata"];
            var orExpStack =  (Stack<List<string>>) context["tmp_OrExpStack"];
            var orExp = (List<string>) context["tmp_OrExp"];
            orExpAutomataStack.Push(orExpAutomata);
            orExpStack.Push(orExp);

            
            context["orExpAutomata"] = new List<Automata>();
            context["tmp_OrExp"] = new List<string>();
            
            context["tmp_cur"] = "";
            var initNode = new AutomataNode(0);
            context["automata"] = new Automata().AddNode(initNode).SetStartState(0);
            var lastNodeStack = (Stack<AutomataNode>) context["stack_lastNode"];
            var lastNode = (AutomataNode) context["lastNode"];
            lastNodeStack.Push(lastNode);
            context["lastNode"] = initNode;
            
            strStack.GetMultiDimensionString().PrintToConsole();
            andStack.Count.PrintToConsole();

            return null;
        }
        
        public static object? AddSingleCharCompareNode(object input, object[] objs)
        {
            var context = (AutomataContext) objs[0];
            var lastNode = (AutomataNode) context["lastNode"];
            var automata = (Automata) context["automata"];

            var node = new AutomataNode(automata.Nodes.Count);
            automata.AddNode(node);
            automata.AddEdge(new ReEdge(lastNode, node, new CommonTransitionStrategy.EqualJudgeTrans<char>((char) input)));
            context["lastNode"] = node;

            $"add a normal char {input}".PrintToConsole();

            context["tmp_cur"] = (string)context["tmp_cur"] + (char)input;
            
            //automata.PrintToConsole();
            return null;
        }
    }
}