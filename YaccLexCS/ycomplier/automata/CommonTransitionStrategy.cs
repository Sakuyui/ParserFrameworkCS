using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace YaccLexCS.ycomplier.automata
{
    public interface ITransStrategy
    {
        bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs);
    }
    
    public static class CommonTransitionStrategy
    {
        public class EpsilonTrans : ITransStrategy
        {
            public static EpsilonTrans Instance = new Lazy<EpsilonTrans>(() => new EpsilonTrans()).Value;
            private EpsilonTrans(){}

            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
            {
                return tryInputItem == null;
            }
        }

        
        
        public class EqualJudgeTrans<T> : ITransStrategy
        {
            private readonly T _target;
            public EqualJudgeTrans(T targetChar)
            {
                _target = targetChar;
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
            {
                return tryInputItem is T c && c.Equals(_target);
            }

            public override string ToString()
            {
                return $"Equal = {_target} trans";
            }
        }
        
        public class CharacterRangeTrans : ITransStrategy
        {
            private readonly char _targetFrom;
            private readonly char _targetTo;

            public CharacterRangeTrans (char cFrom, char cTo)
            {
                if (cTo < cFrom) throw new ArgumentException("CharacterRangeTrans init with (to < from) params");
                _targetFrom = cFrom;
                _targetTo = cTo;
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
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
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
            {
                return tryInputItem is T e && !_exclusionElements.Contains(e);
            }
        }



        public class CustomTrans : ITransStrategy
        {
            private readonly Dictionary<object, object> _map = new Dictionary<object, object>();

            public object this[object obj]
            {
                get => _map[obj];
                set => _map[obj] = value;
            }

            private readonly TransitionStrategy _transitionStrategy;
            public delegate bool TransitionStrategy(AutomataContext? ctx, object? tryInputItem, params object[] objs);
            public CustomTrans([NotNull]TransitionStrategy transitionStrategy)
            {
                _transitionStrategy = transitionStrategy;
            }

            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
            {
                return _transitionStrategy.Invoke(ctx, tryInputItem, objs);
            }
        }
        public class NormalCharacterTrans : ITransStrategy
        {
            private static readonly 
                ExclusionElementsTrans<char> Trans = new(new []{'(', ')', '|', '*', '\\', '.', '[', ']', '{', '}', '+', '^'});
            
            public static NormalCharacterTrans Instance = 
                new Lazy<NormalCharacterTrans>(() => new NormalCharacterTrans()).Value;
            private NormalCharacterTrans(){}
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
            {
                return Trans.Judge(ctx, tryInputItem, objs);
            }
        }
        public class TargetElementsTrans<T> : ITransStrategy
        {
            private HashSet<T> _elements;

            public TargetElementsTrans(IEnumerable<T> es)
            {
                _elements = es.ToHashSet();
            }
            
            public bool Judge(AutomataContext? ctx, object? tryInputItem, params object[]? objs)
            {
                return tryInputItem is T e && _elements.Contains(e);
            }
        }
    }
}