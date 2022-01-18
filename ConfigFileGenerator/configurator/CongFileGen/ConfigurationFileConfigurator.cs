using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ConfigFileGenerator.configurator.CongFileGen
{
    public class ConfigurationFileConfigurator
    {
        private readonly ConfigFileParser _configFileParser;
  
        public ConfigurationFileConfigurator(string filePath)
        {
            _configFileParser = new ConfigFileParser(filePath);
            _configFileParser.Init();
        }

        public string GenTokenConfigFile()
        {
            return TokensConfigFileGen.GenFileContentString(_configFileParser.ParseTokensConfig());
        }
        public Dictionary<string, string> GenGrammarConfigFiles()
        {
            var gs = _configFileParser.ParseTokensGrammar();
            return gs.ToDictionary(k => k.FileName, GrammarConfigFileGen.GenFileContentString);
        }
        public string GenGrammarConfigFile()
        {
            return TokensConfigFileGen.GenFileContentString(_configFileParser.ParseTokensConfig());
        }

        public static Dictionary<string, (string folder, List<string> cfgs)> GetCFGList(IEnumerable<TokenMethod> ts, List<GrammarFileStrut> gs){
            var terminalWords = ts.SelectMany(t => t.Descs.Select(d => d.Name)).ToHashSet();
            var memo = new Dictionary<string, (string folder, List<string> cfgs)>(); //nodename -> method list
            var res = gs
                .ToDictionary(f => f.FileName, f => new List<ASTNodeStrut>());
            
            foreach (var g in gs)
            {
                foreach(var cfgs in g.Descs)
                {
                    //这里是为了合并
                    /*
                     *A -> B | C
                     * A -> D
                     * 这样出现了相同名的话，就合并。 A -> [B, C , D]
                     * 
                     */
                    if (!memo.ContainsKey(cfgs.WordName))
                        memo[cfgs.WordName] = (g.FileName, new List<string>(cfgs.Descs));
                    else
                        memo[cfgs.WordName].cfgs.AddRange(cfgs.Descs);

                }
            }

            return memo;
            void CheckIntegrality()
            {
                var allWords = memo.Keys.Concat(memo.SelectMany(t => 
                        t.Value.cfgs.SelectMany(w => w.Split(" ").Select(sw => sw.Trim()).Where(sw => sw != "")))).ToHashSet()
                    .Except(terminalWords);
                if (allWords.Count() != memo.Count)
                    throw new Exception("not all node are appear: " + allWords.Except(memo.Keys).GetCollectionString());
            }
          

        }
        public static Dictionary<string, string> GenAstNodes(IEnumerable<TokenMethod> ts, List<GrammarFileStrut> gs){
            var terminalWords = ts.SelectMany(t => t.Descs.Select(d => d.Name)).ToHashSet();
            var memo = GetCFGList(ts, gs);
            var filesResult = new Dictionary<string, string>();
            void CheckIntegrality()
            {
                var allWords = memo.Keys.Concat(memo.SelectMany(t => 
                        t.Value.cfgs.SelectMany(w => w.Split(" ").Select(sw => sw.Trim()).Where(sw => sw != "")))).ToHashSet()
                    .Except(terminalWords);
                if (allWords.Count() != memo.Count)
                    throw new Exception("not all node are appear: " + allWords.Except(memo.Keys).GetCollectionString());
            }
            CheckIntegrality();
            foreach (var k in memo)
            {
                var (folder, cfgs) = k.Value;
                var nodeName = k.Key;
                //save to //folder//nodeName.cs
                cfgs.Count.PrintToConsole();
                var r= ASTNodeClassFileGen.GenASTNode(nodeName, cfgs);
                filesResult[folder + "::" + nodeName] = r;
                //这里应该根据不同进行进行传入参数的匹配
            }

            return filesResult;

        }

        public Dictionary<string, string> GenAstNodes()
        {
            return GenAstNodes(_configFileParser.ParseTokensConfig(), _configFileParser.ParseTokensGrammar());
        }
    }
}