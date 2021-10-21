using System;
using System.Collections.Generic;
using System.Text;

namespace YaccLexCS.ycomplier.automata.re
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
        private static readonly Automata RegexAutomata = new Lazy<Automata>(ReParserBuilder.BuildReParserAutomata).Value;
        
        //ensure the expression provided is a top level expression
    

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
            automata.Context["preContNode"] = null!;
            automata.Context["lastResult"] = null!;
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