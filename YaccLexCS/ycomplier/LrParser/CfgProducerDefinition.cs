﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIExam.Complier;

namespace YaccLexCS.ycomplier.LrParser
{
    public class CfgProducerDefinition : IEnumerable<ProducerDefinitionItem>
    {
        public string StartWord = "";
        public readonly List<ProducerDefinitionItem> Grammars = new();

        public readonly Dictionary<string, List<ProducerDefinitionItem>> StartWordsMapping =
            new();

        public readonly HashSet<string> Terminations = new();
        public HashSet<string> NonTerminationWords => StartWordsMapping.Keys.ToHashSet();

        public Dictionary<string, List<List<string>>> ProduceMapping;
        public List<ProducerDefinitionItem> this[string nT] => StartWordsMapping[nT];
        public void AddTerminations(string expression)
        {
            foreach (var e in expression.Split("|"))
                Terminations.Add(e);
        }

        public void InitProduceMapping()
        {
            ProduceMapping = SplitProduceWord();
        }
        
        
        private Dictionary<string, List<List<string>>> SplitProduceWord()
        {
            var res = new Dictionary<string, List<List<string>>>();
            var kSet = Terminations.Union(NonTerminationWords).ToHashSet(null);
            
            kSet.DebugPrintCollectionToConsole();
            
            //对每条产生式处理，转换为列表
            foreach (var p in this)
            {
                List<string> list = p.ProduceItem.Trim().Split(" ").ToList();
                
                
                if (!res.ContainsKey(p.LeftSymbol))
                    res[p.LeftSymbol] = new List<List<string>>();
                res[p.LeftSymbol].Add(list);
            }

            return res;
        }


        public CfgProducerDefinition(IEnumerable<string> grammarsDefinition, string terminationsExp, string startWord)
        {
            AddGrammars(grammarsDefinition.SelectMany(ProducerDefinitionItem.ParseProducerExpression));
            Grammars.PrintEnumerationToConsole();
            AddTerminations(terminationsExp);
            StartWord = startWord;
        }
        public CfgProducerDefinition(string grammarDefinition)
        {
            AddGrammar(new ProducerDefinitionItem(grammarDefinition));
        }
        public CfgProducerDefinition(){}

        public void AddGrammar(ProducerDefinitionItem item)
        {
            Grammars.Add(item);
            if (!NonTerminationWords.Contains(item.LeftSymbol))
                StartWordsMapping[item.LeftSymbol] = new List<ProducerDefinitionItem>();
            StartWordsMapping[item.LeftSymbol].Add(item);
        }
        public void AddGrammars(IEnumerable<ProducerDefinitionItem> item)
        {
            foreach(var g in item)
                AddGrammar(g);
        }

        public IEnumerator<ProducerDefinitionItem> GetEnumerator()
        {
            return Grammars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}