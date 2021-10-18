using System;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS.ycomplier.automata
{
    public interface ITransStrategy
    {
        bool Judge(AutomataContext? ctx, object? tryInputItem, params object[] objs);
    }
    
    public static class CommonTransitionStrategy
    {
        public class EpsilonTrans : ITransStrategy
        {
            public static EpsilonTrans Instance = new Lazy<EpsilonTrans>(() => new EpsilonTrans()).Value;
            private EpsilonTrans(){}

            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[] objs)
            {
                return true;
            }
        }

        
        
        public class EqualJudgeTrans<T> : ITransStrategy
        {
            private readonly T _target;
            public EqualJudgeTrans(T targetChar)
            {
                _target = targetChar;
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[] objs)
            {
                return tryInputItem is T c && c.Equals(_target);
            }
        }
        
        public class CharacterRangeTrans : ITransStrategy
        {
            private readonly char _targetFrom;
            private readonly char _targetTo;

            public CharacterRangeTrans (char cFrom, char cTo)
            {
                if (cTo > cFrom) throw new ArgumentException("CharacterRangeTrans init with to > from params");
                _targetFrom = cFrom;
                _targetTo = cTo;
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[] objs)
            {
                return tryInputItem is char c && c >= _targetFrom && c <= _targetTo;
            }
        }


        public class ExclusionElementsTrans<T> : ITransStrategy
        {
            private HashSet<T> _exclusionElements;

            public ExclusionElementsTrans(IEnumerable<T> elements)
            {
                _exclusionElements = elements.ToHashSet();
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[] objs)
            {
                return tryInputItem is T e && !_exclusionElements.Contains(e);
            }
        }
        
        public class TargetElementsTrans<T> : ITransStrategy
        {
            private HashSet<T> _elements;

            public TargetElementsTrans(IEnumerable<T> es)
            {
                _elements = es.ToHashSet();
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[] objs)
            {
                return tryInputItem is T e && _elements.Contains(e);
            }
        }
    }
}