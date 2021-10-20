﻿using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.automata
{
    public class ReParserBuilder
    {   
        public static Automata BuildReParserAutomata()
        {
            var a = new Automata();
            var node0 = new AutomataNode(0);
            var node1 = new AutomataNode(1);
            var node2 = new AutomataNode(2);
            var node3 = new AutomataNode(3);
            var node4 = new AutomataNode(4);


           

            var e1 = new ReEdge(node0, node0, ReAutomataConstruction.AddSingleCharCompareNode ,CommonTransitionStrategy.NormalCharacterTrans.Instance);
            var e2 = new ReEdge(node0, node0, ReAutomataConstruction.EnterPlusChar,new CommonTransitionStrategy.EqualJudgeTrans<char>('+'));
            var e3 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('*'));
            var e4 = new ReEdge(node0, node0, new CommonTransitionStrategy.EqualJudgeTrans<char>('.'));
            var e5 = new ReEdge(node0, node0, ReAutomataConstruction.EnterLeftBrace, new CommonTransitionStrategy.EqualJudgeTrans<char>('('));
            var e6 = new ReEdge(node0, node0, ReAutomataConstruction.EnterRightBrace,new CommonTransitionStrategy.EqualJudgeTrans<char>(')'));
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
    }
}