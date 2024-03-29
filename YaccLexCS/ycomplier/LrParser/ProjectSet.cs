﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YaccLexCS.ycomplier.util;

namespace YaccLexCS.ycomplier.LrParser
{
    public class ProjectSet : IEnumerable<Lr1Item>
    {
        private readonly HashSet<Lr1Item> _items = new();

        public ProjectSet(IEnumerable<Lr1Item> items)
        {
            _items.AddRange(items);
        }

       
        public IEnumerator<Lr1Item> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public override string ToString()
        {
            return _items.ToEnumerationString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ProjectSet ApplyClosure(CfgProducerDefinition definition)
        {
            var changed = true;
            var memo = new HashSet<Lr1Item>(); //记忆化，防止重复记录相同项目
            

            while (changed)
            {
                changed = false;
                var curSet = _items.ToList();
                foreach (var item in curSet)
                {
                    if (memo.Contains(item) || item.IsReductionItem())
                    {
                        continue;
                    }
                    

                    //获取目前点号对着的下一个记号
                    var next = item.ProduceItems[item.DotPos];
                    
                    //如果是终结符号，不需要引入新item.
                    if(definition.Terminations.Contains(next))
                        continue;
              
                    //导出导出式子
                    memo.Add(item); //记录当前item
                    //使用dfs引入新项目
                    ClosureDfs(next, definition, _items, item, 
                        definition.Terminations.Contains(next)? 
                            new HashSet<string>() 
                        :
                           item.DotPos == item.ProduceItems.Count - 1 ?
                               item.SearchWordList.ToHashSet(null):
                               CfgTools
                               .GetSequenceFirstSet(definition, new []{item.ProduceItems[item.DotPos + 1]}.ToList())    
                    );
                }
                
                if (curSet.Count != _items.Count) //如果存在新增的项目
                    changed = true;
            }
           
            var g = 
                _items.GroupBy(e => e, new CustomerEqualityComparer<Lr1Item>((t1, t2) => 
                    t1.DotPos == t2.DotPos && t1.StartWord == t2.StartWord &&
                    t1.ProduceItems.SequenceEqual(t2.ProduceItems), 
                    item => item.ProduceItems.ToEnumerationString().GetHashCode() + item.DotPos)).ToHashSet(null);
            _items.Clear();
            _items.UnionWith(g.Select(e => e.ToList()).Select(e =>
            {
                var newSearchWords = new HashSet<string>();
                foreach (var w in e)
                {
                    newSearchWords.AddRange(w.SearchWordList);
                }

                var item = new Lr1Item(e[0].StartWord, e[0].ProduceItems, newSearchWords.ToList())
                {
                    DotPos = e[0].DotPos
                };
                return item;
            }));
           
            return this;
        }


        public override bool Equals(object? obj)
        {
            if (obj is ProjectSet projectSet)
            {
                return projectSet._items.SetEquals(_items);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _items.Aggregate(0, (a, b) => a + b.GetHashCode());
        }

        public string GetProjectItemsDesc()
        {
            var s = "";
            foreach (var i in _items)
            {
                var s1 = i.ProduceItems.Take(i.DotPos).Aggregate("", (a, b) => a +" " + b);
                s1 += "." + i.ProduceItems.Skip(i.DotPos).Aggregate("", (a, b) => a +" " + b);
                s += "+ " + i.StartWord + " --> " + s1  + "  " + i.SearchWordList.ToEnumerationString();
                s += "\r\n";
            }

            return s;
        }
        public void ClosureDfs(string begin, CfgProducerDefinition definition, HashSet<Lr1Item> items, Lr1Item lr1Items, HashSet<string> forwardSearch)
        {
            if (definition.Terminations.Contains(begin))
                return;
            if (!definition.NonTerminations.Contains(begin))
            {
                throw new Exception("unexpected code");
            }
            
            //$"DFS for {lr1Items.StartWord}, {forwardSearch.GetMultiDimensionString()}".PrintToConsole();
            
            //("in => " + begin).PrintToConsole();
            foreach (var item in definition.ProduceMappingList[begin])
            {
                var l = new Lr1Item(begin, item, forwardSearch.ToList());
                if (l.ProduceItems.Count >= 2)
                {
                    var complete = CfgTools.GetSequenceFirstSet(definition, l.ProduceItems.Skip(1).ToList());
                    foreach(var c in complete)
                    {
                        l.SearchWordList.Add(c);
                    }
                }
                if (items.Contains(l)) continue;
                
                _items.Add(l);
                var next = l.ProduceItems[0];
                ClosureDfs(next, definition, _items, l, 
                    definition.Terminations.Contains(next)? 
                        new HashSet<string>() 
                        :
                        l.DotPos == l.ProduceItems.Count - 1 ?
                            l.SearchWordList.ToHashSet(null):
                            CfgTools.GetSequenceFirstSet(definition, new []{l.ProduceItems[l.DotPos + 1]}.ToList())    
                );

            }
            //_items.PrintMultiDimensionCollectionToConsole();
           
        }
    }
}