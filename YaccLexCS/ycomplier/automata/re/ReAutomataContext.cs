using System.Collections.Generic;

namespace YaccLexCS.ycomplier.automata
{
    public class ReAutomataContext : AutomataContext
    {
        //register
        public AutomataNode InitNode  { get => (AutomataNode) this["initNode"]; set => this["initNode"] = value; }
        public AutomataNode LastNode  { get => (AutomataNode) this["lastNode"]; set => this["lastNode"] = value; }
        public Automata Automata  { get => (Automata) this["automata"]; set => this["automata"] = value; }
        public AutomataNode PreContNode  { get => (AutomataNode) this["preContNode"]; set => this["preContNode"] = value; }
        public Automata LastResult  { get => (Automata) this["lastResult"]; set => this["lastResult"] = value; }
        public List<Automata> OrExpAutomata  { get => (List<Automata>) this["orExpAutomata"]; set => this["orExpAutomata"] = value; }
        
        //stack
        public Stack<AutomataNode> StackLastNode  { get => (Stack<AutomataNode>) this["stack_lastNode"]; set => this["stack_lastNode"] = value; }
        public Stack<List<Automata>> StackOrAutomata  { get => (Stack<List<Automata>>) this["stack_OrAutomata"]; set => this["stack_OrAutomata"] = value; }
        public Stack<Automata> StackAndAutomata { get => (Stack<Automata>) this["stack_AndAutomata"]; set => this["stack_AndAutomata"] = value; }
        public Stack<char> StackBrace  { get => (Stack<char>) this["stack_Brace"]; set => this["stack_Brace"] = value; }
        
        
        
        public string TmpCur  { get => (string) this["tmp_cur"]; set => this["tmp_cur"] = value; }
        public Stack<string> TmpStrStack { get => (Stack<string>) this["tmp_strStack"]; set => this["tmp_strStack"] = value; }
        public List<string> TmpOrExp { get => (List<string>) this["tmp_OrExp"]; set => this["tmp_OrExp"] = value; }
        public Stack<List<string>> TmpOrExpStack  { get => (Stack<List<string>>) this["tmp_OrExpStack"]; set => this["tmp_OrExpStack"] = value; }

        
        
        public ReAutomataContext()
        {
            //register
            InitNode = null;
            LastNode = null;
            Automata = null; // The automata current in building process.
            PreContNode = null;
            LastResult = null;
            OrExpAutomata = new List<Automata>();

            //stack
            StackLastNode = new Stack<AutomataNode>();
            StackOrAutomata = new Stack<List<Automata>>();
            StackAndAutomata = new Stack<Automata>();
            StackBrace = new Stack<char>();
            
            //test
            TmpCur = "";
            TmpStrStack = new Stack<string>();
            TmpOrExp = new List<string>();
            TmpOrExpStack = new Stack<List<string>>();
        }
    }
}