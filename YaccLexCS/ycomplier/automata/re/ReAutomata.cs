using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YaccLexCS.ycomplier.automata
{
    public class ReEdge : AutomataEdge
    {
      

        public override string ToString()
        {
            return $"[RE Edge ({FromNode.NodeId} -> {ToNode.NodeId})] use strategy = {IsCanTrans}";
        }

        public ReEdge(AutomataNode fromNode, AutomataNode toNode, InTransEvent eventTransInEdge, ITransStrategy transStrategy) : base(fromNode, toNode, eventTransInEdge, transStrategy)
        {
        }

        public ReEdge(AutomataNode fromNode, AutomataNode toNode, ITransStrategy transStrategy) : base(fromNode, toNode, transStrategy)
        {
        }
    }


    public class ReAutomata : Automata
    {
        public static Automata TopRegexAutomata = new Lazy<Automata>(BuildTopLevelParserAutomata).Value;
        public static Automata RegexAutomata = new Lazy<Automata>(ReParserBuilder.BuildReParserAutomata).Value;
        public static Automata BuildTopLevelParserAutomata()
        {
            var a = new Automata();
            var node0 = new AutomataNode(0);
            var node1 = new AutomataNode(1);
            var node2 = new AutomataNode(2);
            var node3 = new AutomataNode(3);
            //var node4 = new AutomataNode(4);
            var node5 = new AutomataNode(5);


           

            var e1 = new ReEdge(node0, node0, ReAutomataConstruction.AddSingleCharCompareNode ,CommonTransitionStrategy.NormalCharacterTrans.Instance);
            
            var e2 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('+'));
            var e3 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('*'));
            var e4 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('.'));
            var e5 = new ReEdge(node0, node1, new CommonTransitionStrategy.EqualJudgeTrans<char>('{'));
            var e6 = new ReEdge(node0, node3, new CommonTransitionStrategy.EqualJudgeTrans<char>('['));
            
            var e7 = new ReEdge(node1, node1, new CommonTransitionStrategy.CharacterRangeTrans('1', '9'));
            var e8 = new ReEdge(node1, node2, new CommonTransitionStrategy.EqualJudgeTrans<char>('}'));
            
           
            var e9 = new ReEdge(node3, node3, new CommonTransitionStrategy.EqualJudgeTrans<char>('^'));
            var e10 = new ReEdge(node3, node3, new CommonTransitionStrategy.EqualJudgeTrans<char>('-'));
            var e11 = new ReEdge(node3, node3, CommonTransitionStrategy.NormalCharacterTrans.Instance);
            
            var e12 = new ReEdge(node3, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>(']'));
           
            
            var e13 = new ReEdge(node0, node5, new CommonTransitionStrategy.EqualJudgeTrans<char>('\\'));
            var e14 = new ReEdge(node5, node0, new CommonTransitionStrategy.CharacterRangeTrans((char)0, (char)255));

            //var e15 = new ReEdge(node4, node0, CommonTransitionStrategy.EpsilonTrans.Instance);

            
            a.AddNodes(new []{node0, node1, node2, node3, node5});
            a.AddEdges(new []{e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14});
            a.SetStartState(0).InitState();
            return a;
        }
         //ensure the expression provided is a top level expression
         public static void BuildAutomataFromTopExp(string exp)
         {
             $"\\=======================Build Automata======================\n>> Try Build {exp}".PrintToConsole();
             var automata = TopRegexAutomata;
             automata.ResetAutomata();
             
             var initNode = new AutomataNode(0);
             var targetAutomata = new Automata();
             targetAutomata.AddNode(initNode);
             targetAutomata.SetStartState(0);
             
             automata.Context["initNode"] = initNode;
             automata.Context["lastNode"] = initNode;
             automata.Context["automata"] = targetAutomata;
             
             automata.ParseFromCurrentStates(exp.ToCharArray().Cast<object>());
             $"\\=======================Build Automata {exp} Finish ======================\n".PrintToConsole();
         }


         public static void BuildAutomataFromExp(string exp)
         {
             var sb = new StringBuilder(exp);
            
            
            var automata = RegexAutomata;
            automata.ResetAutomata();
             
            var initNode = new AutomataNode(0);
            var targetAutomata = new Automata();
            targetAutomata.AddNode(initNode);
            targetAutomata.SetStartState(0);
             
            automata.Context["initNode"] = initNode;
            automata.Context["lastNode"] = initNode;
            automata.Context["automata"] = targetAutomata; // The automata current in building process.
            automata.Context["preContNode"] = null;
            automata.Context["lastResult"] = null;
            automata.Context["orExpAutomata"] = new List<Automata>();

            automata.Context["stack_lastNode"] = new Stack<AutomataNode>();
            automata.Context["stack_OrAutomata"] = new Stack<List<Automata>>();
            automata.Context["stack_AndAutomata"] = new Stack<Automata>();
            automata.Context["stack_Brace"] = new Stack<char>();
            
            automata.Context["tmp_cur"] = "";
            automata.Context["tmp_strStack"] = new Stack<string>();
            automata.Context["tmp_OrExp"] = new List<string>();
            automata.Context["tmp_OrExpStack"] = new Stack<List<string>>();
            
            while (sb.Length > 0)
            {
                var c = sb[0];
                sb.Remove(0, 1);
                automata.ParseFromCurrentStates(c);
            }
            
            // while (sb.Length > 0)
            // {
            //     var c = sb[0];
            //     sb.Remove(0, 1);
            //     automata.ParseFromCurrentStates(c);
            //     if (c == '(')
            //     {
            //         // $"meet (, begin a new exp, save cur = {cur} to stack".PrintToConsole();
            //         // bStack.Push('(');
            //         // strStack.Push(cur);
            //         // orExpStack.Push(orExp);
            //         // orExp = new List<string>();
            //         // cur = "";
            //         // ShowStrStack();
            //     }else if (c == ')')
            //     {
            //         // $"\n meet ), merge cur string and all element in or stack".PrintToConsole();
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
            //     }
            //     else if (c == '|')
            //     {
            //         // $"meet |, cur str = {cur}, save to 'or stack' ".PrintToConsole();
            //         // orExp.Add(cur);
            //         // cur ="";
            //         // ShowOrStack();
            //     }
            //     else if (cur == "" && (c == '+' || c == '*'))
            //     {
            //         // $"meet {c}, build ({lastResult}){c}".PrintToConsole();
            //         // cur += c;
            //     }else
            //     {
            //         //cur += c;
            //         
            //     }
            //     
            // }
            // $"final = {cur}".PrintToConsole();
         }
        public ReAutomata()
        {
           
            // AddNode(new AutomataNode(0));
            // AddNode(new AutomataNode(1));
            // SetStartState(0).InitState();
            //
            // AddEdge(new ReEdge( GetNode(0), GetNode(1), CommonTransitionStrategy.EpsilonTrans.Instance));
            // this.PrintToConsole();
            // ApplyClosure();
            // this.PrintToConsole();
        }
    }
}