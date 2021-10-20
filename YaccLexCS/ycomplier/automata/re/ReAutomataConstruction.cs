using System;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.automata
{
    public class ReAutomataConstruction
    {
        public static Automata BuildReParserAutomata()
        {
             var a = new Automata();
            var node0 = new AutomataNode(0);
            var node1 = new AutomataNode(1);
            var node2 = new AutomataNode(2);
            var node3 = new AutomataNode(3);
            var node4 = new AutomataNode(4);


           

            var e1 = new ReEdge(node0, node0, AddSingleCharCompareNode ,CommonTransitionStrategy.NormalCharacterTrans.Instance);
            var e2 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('+'));
            var e3 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('*'));
            var e4 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('.'));
            var e5 = new ReEdge(node0, node0, EnterLeftBrace, new CommonTransitionStrategy.EqualJudgeTrans<char>('('));
            var e6 = new ReEdge(node0, node0, EnterRightBrace,new CommonTransitionStrategy.EqualJudgeTrans<char>(')'));
            var e7 = new ReEdge(node0, node1, new CommonTransitionStrategy.EqualJudgeTrans<char>('['));
            var e8 = new ReEdge(node0, node2, new CommonTransitionStrategy.EqualJudgeTrans<char>('{'));
            
            var e9 = new ReEdge(node2, node2, new CommonTransitionStrategy.CharacterRangeTrans('1', '9'));
            var e10 = new ReEdge(node2, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('}'));
            
           
            var e11 = new ReEdge(node1, node1, new CommonTransitionStrategy.EqualJudgeTrans<char>('^'));
            var e12 = new ReEdge(node1, node1, new CommonTransitionStrategy.EqualJudgeTrans<char>('-'));
            var e13 = new ReEdge(node1, node1, CommonTransitionStrategy.NormalCharacterTrans.Instance);
            var e14 = new ReEdge(node1, node3, new CommonTransitionStrategy.EqualJudgeTrans<char>('\\'));
            
            var e15 = new ReEdge(node1, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>(']'));
           
            
            var e16 = new ReEdge(node0, node1, new CommonTransitionStrategy.EqualJudgeTrans<char>('\\'));
            var e17 = new ReEdge(node1, node4, new CommonTransitionStrategy.CharacterRangeTrans((char)0, (char)255));
            
            var e18 = new ReEdge(node4, node0, 
                new CommonTransitionStrategy.CustomTrans((ctx, item, _) => 
                    item == null && ctx["stateStack"] is Stack<object> s && s.Any() && (int)s.Peek() == 0));

            var e19 = new ReEdge(node4, node1, 
                new CommonTransitionStrategy.CustomTrans((ctx, item, _) => 
                    item == null && ctx["stateStack"] is Stack<object> s && s.Any() && (int)s.Peek() == 1));


            //var e15 = new ReEdge(node4, node0, CommonTransitionStrategy.EpsilonTrans.Instance);

            
            a.AddNodes(new []{node0, node1, node2, node3, node4});
            a.AddEdges(new []{e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, e16, e17, e18, e19});
            a.SetStartState(0).InitState();
            return a;
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
            ConcatAutomata(curAutomata, resultAutomata);
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

        private static Automata OrMergeAutomata(List<Automata> orExpAutomata)
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
                    automata.AddNode(new AutomataNode((int) node.NodeId + c));
                }
                endSet.Add(c + a.Nodes.Count);
                foreach (var e in a.Edges)
                {
                    var e2 = e;
                    e2.FromNode += c;
                    automata.AddEdge();
                }
            }
            
            
        }

        public static string ConcatAutomata(string a1, string a2)
        {
            $"Concat {a1} with {a2}".PrintToConsole();
            return a1 + a2;
        }
        public static Automata ConcatAutomata(Automata a1, Automata a2)
        {
            $"Concat {a1} \n with {a2}\n".PrintToConsole();
            var c = a1.Nodes.Count;
            var newNodes = a2.Nodes.Select(e => new AutomataNode(c + (int) e.NodeId));
            a1.Nodes.AddRange(newNodes);
            a1.PrintToConsole();
            return null;
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