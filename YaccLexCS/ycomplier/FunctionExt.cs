using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YaccLexCS
{
      public static class FunctionExt
    {

       
        public static void PrintToConsole(this object obj)
        {
            Console.WriteLine(obj.ToString());
        }

        public static void ElementInvoke<T>(this IEnumerable<T> list, Action<T> action)
        {
            Action a;
            foreach (var e in list)
            {
                action.Invoke(e);
            }
        }
        public static void ElementInvoke<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            var enumerable = list as T[] ?? list.ToArray();

            for (var i = 0; i < enumerable.Length; i++)
            {
                action.Invoke(enumerable[i], i);
            }
        }
        public static void EnumerableConditionInvoke<T>(this IEnumerable<T> list,Func<T ,bool> condition, Action<T> action)
        {
            foreach (var e in list)
            {
                if(condition.Invoke(e))
                    action.Invoke(e);
            }
        }

        public static IEnumerable<T2> WindowSelect<T, T2>(this IEnumerable<T> list, int wSize, Func<IEnumerable<T>, int, T2> selFunc)
        {
            var l = list.ToList();

            var right = wSize - 1;
            var window = new Queue<T>();
            for (var i = 0; i <= right; i++)
            {
                window.Enqueue(l[i]);
            }

            var left = 0;
            do
            { 
                var tmp = window.ToList();
                var ans = selFunc.Invoke(tmp, left++);
                window.Dequeue();
                if(right + 1 < l.Count)
                    window.Enqueue(l[right + 1]);
                right++;
                yield return ans;
            }while(right< l.Count);
            
            
        }
        public static IEnumerable<T2> WindowSelect<T, T2>(this IEnumerable<T> list, int wSize, Func<IEnumerable<T>, T2> selFunc, bool equalWidth = false)
        {
            var enumerable = list as T[] ?? list.ToArray();
            var l = enumerable.ToList();

            var right = wSize - 1;
            var window = new Queue<T>();
            for (var i = 0; i <= right; i++)
            {
                window.Enqueue(l[i]);
            }

            if (!equalWidth)
            {
                do
                { 
                    var tmp = window.ToList();
                    var ans = selFunc.Invoke(tmp);
                    window.Dequeue();
                    if(right + 1 < l.Count)
                        window.Enqueue(l[right + 1]);
                    right++;
                    yield return ans;
                }while(right< l.Count);
            }
            else
            {
                var t = Enumerable.Range(0, enumerable.Length).Select(e =>
                {
                    var left = e;
                    var r = e + wSize;
                    if (r >= enumerable.Length)
                    {
                        left = left - (r - enumerable.Length) - 1;
                    }
                    return selFunc.Invoke(enumerable.Skip(left).Take(wSize).ToList());
                });
                foreach (var w in t)
                {
                    yield return w;
                }
            }



        }
        public static IEnumerable<T> SelectByIndexes<T>(this IEnumerable<T> list, params int[] indexes)
        {
            var l = list.ToList();
            foreach (var i in indexes)
            {
                if (i < l.Count)
                {
                    yield return l[i];
                }
            }
        }
   
     

      
        public static IEnumerable<T> SelectByIndexes<T>(this IEnumerable<T> enumerable,IEnumerable<int> indexes)
        {
            var list = enumerable.ToArray();
            var ints = indexes as int[] ?? indexes.ToArray();
            var indexList = ints.ToArray();
            for (var i = 0; i < ints.Length; i++)
            {
                if (indexList[i] < list.Length)
                    yield return list[indexList[i]];
                else
                    yield return default;
            }
        }

      
        public static IEnumerable<int> FindAndGetIndexes<T>(this IEnumerable<T> enumerable, Func<T,bool> condition)
        {
            var bArr = enumerable.ConditionFindWithBoolResult(condition);
            return bArr.ConditionSelect(((b, _) => b)
                , (_, i) => i
            );
        }
        
        
        //比aggregate高级的东西

        public static Key Sum<T, Key>(this IEnumerable<T> enumerable, Func<T, Key> keySelector,
            Func<Key, Key, Key> addStrategy)
        {
            var list = enumerable.ToList();
            switch (list.Count)
            {
                case 0:
                    return default;
                case 1:
                    return keySelector.Invoke(list[0]);
            }

            
            var cur = keySelector.Invoke(list[0]);
            
            for (var i = 1; i < list.Count; i++)
            {
                cur = addStrategy.Invoke(cur, keySelector.Invoke(list[i]));
            }
           
            return cur;
        }
      
        public static void PrintEnumerationToConsole<T>(this IEnumerable<T> list)
        {
            Console.WriteLine(list.ToEnumerationString());
        }
        public static string ToEnumerationString<T>(this IEnumerable<T> list)
        {
            if (!list.Any())
                return "[]";
            var res = list.Aggregate("[", (current, e) => current + (e + ","));
            
            res = res.Substring(0, res.Length - 1) + "]";
            return res;
        }

        public static IEnumerable<bool> ConditionFindWithBoolResult<T>(this IEnumerable<T> list, Func<T,bool> condition)
        {
            var ans = list.Select(condition.Invoke).ToList();
            return ans;
        }
        public static IEnumerable<bool> ConditionFindWithBoolResult<T>(this IEnumerable<T> list, Func<T, int, bool> condition)
        {
            var ans = list.Select(condition.Invoke).ToList();
            return ans;
        }
        public static IEnumerable<T2> ConditionSelect<T, T2>(this IEnumerable<T> enumerable, IEnumerable<bool> conditionBools, Func<T, T2> selectFunc = null)
        {
            var con = conditionBools.ToArray();
            selectFunc ??= t => (T2) (object) t;
            
            var ts = enumerable as T[] ?? enumerable.ToArray();
            for (var i = 0; i < ts.Length; i++)
            {
                if (con[i])
                    yield return selectFunc(ts[i]);
            }
        }
        
        public static IEnumerable<T2> ConditionSelect<T, T2>(this IEnumerable<T> enumerable, IEnumerable<bool> conditionBools, Func<T,int, T2> selectFunc = null)
        {
            var con = conditionBools.ToArray();
            selectFunc ??= (i, t) => (T2) ((i, t) as object);
            
            var ts = enumerable as T[] ?? enumerable.ToArray();
            for (var i = 0; i < ts.Length; i++)
            {
                if (con[i])
                    yield return selectFunc(ts[i], i);
            }
        }
        public static IEnumerable<T> ConditionSelect<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            return ts.ConditionSelect((t, _) => condition.Invoke(t), (t, _) => t);
        }
        public static IEnumerable<T> ConditionSelect<T>(this IEnumerable<T> enumerable, IEnumerable<bool> condition)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            return ts.ConditionSelect(condition, t => t);
        }
        public static IEnumerable<T2> ConditionSelect<T, T2>(this IEnumerable<T> enumerable, Func<T, int, bool> condition, Func<T,int, T2> selectFunc = null)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            return ts.ConditionSelect(ts.ConditionFindWithBoolResult(condition), selectFunc);
        }
        
        public static IEnumerable<T2> ConditionSelect<T, T2>(this IEnumerable<T> enumerable, Func<T, bool> condition, Func<T, T2> selectFunc = null)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            return ts.ConditionSelect(ts.ConditionFindWithBoolResult(condition), selectFunc);
        }
        public static IEnumerable<T2> ConditionSelect<T, T2>(this IEnumerable<T> enumerable, Func<T, bool> condition, Func<T,int, T2> selectFunc)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            return ts.ConditionSelect(ts.ConditionFindWithBoolResult(condition), selectFunc);
        }
        
        public static void PrintCollectionToConsole<T>(this IEnumerable<T> enumerable)
        {
            var str = enumerable.Aggregate("[", (current, e) => current + (e + ","));
            str = str.Substring(0, str.Length - 1) + "]";
            Console.WriteLine(str);
        }

        public static IEnumerable<IEnumerable<T>> SplitCollection<T>(this IEnumerable<T> enumerable, IEnumerable<int> length)
        {
            var ts = enumerable as T[] ?? enumerable.ToArray();
            var ints = length as int[] ?? length.ToArray();
            if (ints.Sum() != ts.Length)
                throw new ArithmeticException();
            var pos = 0;
            foreach (var l in ints)
            {
                var tmp = new List<T>();
                for (var i = 0; i < l; i++)
                {
                    tmp.Add(ts[pos++]);
                }
                yield return tmp;
            }
        }

        public static T CastTo<T>(this object obj)
        {
            return (T) obj;
        }
        public static string GetMultiDimensionString(this IEnumerable enumerable)
        {
            var s = "[";
            var count = 0;
            foreach (var e in enumerable)
            {
                if (e.GetType().GetInterface("IEnumerable") == null)
                {
                    s += e + "";
                }
                else
                {
                    s += GetMultiDimensionString((IEnumerable)e);
                }

                count++;
                s += " ,";
            }

            if (count != 0)
                s = s.Remove(s.Length - 1);
            s += "]";
            return s;
        }
        public static void PrintMultiDimensionCollectionToConsole<T>(this IEnumerable<T> enumerable)
        {
            $"{enumerable.GetMultiDimensionString()}".PrintToConsole();
            /*var str = enumerable.Aggregate("[", (current, e) => current + (e.ToEnumerationString() + ","));
            str = str.Substring(0, str.Length - 1) + "]";
            Console.WriteLine(str);*/
        }
        public static IEnumerable<TResult> ResultSaveAggregate<TSource, TResult>
            (this IEnumerable<TSource> enumerable, Func<TResult,TSource, TResult> func, TResult start)
        {
            var ans = new List<TResult>();
            var cur = start;
            foreach (var e in enumerable)
            {
                var tmp = func.Invoke(cur, e);
                ans.Add(tmp);
                cur = tmp;
            }

            return ans;
        }

        public static void SetWhere<TSource>(this IEnumerable<TSource> enumerable, Action<TSource> func, Func<TSource, bool> logicJudge)
        {
            foreach (var e in enumerable)
            {
                if(logicJudge(e))
                    func(e);
            }
        }

        public static int SearchFirstIndex<TSource>(this IEnumerable<TSource> enumerable, TSource element, int start = 0)
        {
            return enumerable.ToList().IndexOf(element, start);
        }

        public static (int index, TSource data) FindFirst<TSource>(this IEnumerable<TSource> enumerable,
            Func<TSource,int, bool> judge)
        {
            var t = enumerable.ToList();
            for (var i = 0; i < t.Count; i++)
            {
                if (judge(t[i], i))
                    return (i, t[i]);
            }
            return (-1, default);
        }
        
    }
}