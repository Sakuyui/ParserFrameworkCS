using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.util;

namespace YaccLexCS.ycomplier
{
    public static class YCompilerConfigurator
    {
        public static void GenerateGrammarDefinitionFileFrom(string inputFilePath, string outputPath)
        {
            var f = File.OpenText(inputFilePath);
            var line = "";
            var beginWord = "";
            var state = 0;
            Dictionary<string, List<string>> definitions = new Dictionary<string, List<string>>();
            var curName = "";
            while ((line = f.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length >= 1)
                {
                    
                    if (line[0] == '#')
                    {
                        beginWord = line[1..];
                        $"begin from {beginWord}".PrintToConsole();
                        continue;
                    }
                    if (state == 1)
                    {
                        if (line[0] == '}')
                        {
                            state = 0;
                            
                        }
                        if (line[^1] == '}')
                        {
                            state = 0;
                            // line.Replace("}", "").PrintToConsole();
                            continue;
                        }
                        //line.PrintToConsole();
                        definitions[curName].Add(line);
                    }
                    if (line[0] == '{')
                    {
                        state = 1;
                        
                    }
                    if (state == 0 && line[^1] == '{')
                    {
                        state = 1;
                        curName = line.Replace("{", "").Trim();
                        var name = curName.Replace("\r", "").Replace("\n", "");
                        if (!definitions.ContainsKey(name)) 
                            definitions[name] = new List<string>();
                        $"entry {curName}".PrintToConsole();
                    }
                   
                  
                }

                foreach (var kv in definitions)
                {
                    //output
                    var sb = new StringBuilder();
                    sb.Append("using YaccLexCS.ycomplier;\r\n"+
                              "using YaccLexCS.ycomplier.attribution;\r\n\r\n"+
                              "namespace YaccLexCS.config{\r\n" +
                              "\t[GrammarConfiguration]\r\n" +
                              $"\tpublic static class Grammar{kv.Key}"+"{"+"\r\n");
                    void AddAMethod(string desc)
                    {
                        var param = desc.Trim().Split(":")[1].Trim().Split("|");
                        var right = "";
                        var left = "\"" + desc.Trim().Split(":")[0].Trim() + "\"";
                        foreach (var p in param)
                        {
                            right += "\"" + p.Trim() + "\"";
                            right += ",";
                        }
                        var className = left.Trim('"') == beginWord ? "BeginningGrammarDefinition" : "GrammarDefinition";
                        sb.Append("\r\n" + $"\t\t[{className}({left},{right.Trim(',')})]\r\n");
                        sb.Append($"\t\tpublic static void {left.Trim('"')}(CompilerContext context)" + "{\r\n\r\n\t\t}\r\n\r\n");
                    }
                    foreach (var v in kv.Value)
                    {
                        AddAMethod(v);
                    }

                    sb.Append("\t}\r\n}");
                    //sb.ToString().PrintToConsole();
                    if(!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }

                    var fileName = outputPath + $"\\Grammar{kv.Key}.cs";
                    if (File.Exists(fileName)) ;
                    File.WriteAllText(fileName, sb + "");
                }

            }
           
        }
        public static IEnumerable<Type> ScanTokenConfiguration(IEnumerable<string> packetName)
        {
            return ReflectionTool.ScanConfigurationClass<TokenConfiguration>(packetName);
        }
        public static IEnumerable<Type> ScanGrammarConfiguration(IEnumerable<string> packetName)
        {
            return ReflectionTool.ScanConfigurationClass<GrammarConfiguration>(packetName);
        }
        public static IEnumerable<(GrammarDefinition tokenDef, MethodInfo methodInfo)> GetAllGrammarDefinitions(IEnumerable<Type> types)
        {
            return ReflectionTool.GetAllDefinitions<GrammarDefinition>(types);
        }
        public static IEnumerable<(TokenDefinition tokenDef, MethodInfo methodInfo)> GetAllTokenDefinitions(IEnumerable<Type> types)
        {
            return ReflectionTool.GetAllDefinitions<TokenDefinition>(types);
        }
    }
}