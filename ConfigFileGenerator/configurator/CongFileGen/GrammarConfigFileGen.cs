using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigFileGenerator.configurator.CongFileGen
{
    public class GrammarConfigFileGen
    {
        public static string[] Header =
        {
            "using System;",
            "using System.Collections.Generic;",
            "using YaccLexCS.ycomplier;",
            "using YaccLexCS.ycomplier.attribution;"
        };

        public static string NameSpace = "YaccLexCS.config";
        
        const string T6 = "\t\t\t\t\t\t";
        const string T4 = "\t\t\t\t";
        const string LE = "\r\n";

        private static string MethodGen(GrammarFileStrut.GrammarMethodDesc method)
        {
            var sb = new StringBuilder();
           
            void AddAttrLine(GrammarFileStrut.GrammarMethodDesc desc) 
            {
                sb.Append(!method.IsBeginningWord
                    ? $"{T4}[GrammarDefinition({ExtentQuote(desc.WordName)}, "
                    : $"{T4}[BeginningGrammarDefinition({ExtentQuote(desc.WordName)}, ");
                string ExtentQuote(string s) => "\"" + s + "\"";
                var paramsStr = desc.Descs.Aggregate("", (a, b) => a +  ", " + ExtentQuote(b));
                sb.Append(paramsStr.Trim().Trim(','));
                sb.Append($")]{LE}");
            }
                

            AddAttrLine(method);

            sb.Append($"{T4}public static void {method.Name}(){LE}" + T4 + "{" + $"{LE}{T4}" + "}");
            
            return sb + "";
        }
        public static string GenFileContentString(GrammarFileStrut fileStrut)
        {
            var className = $"Grammar{fileStrut.FileName}";
            
            var sb = new StringBuilder();
            sb.Append(Header.Aggregate("", (a, b) => a + "\r\n" + b));
            sb.Append("\r\n");
            sb.Append("namespace " + NameSpace + "\r\n{\r\n");
            sb.Append("\t\t[GrammarConfiguration]\r\n");
            sb.Append("\t\tpublic static class " + className + "\r\n\t\t{\r\n");
            foreach (var m in fileStrut.Descs) 
                sb.Append("\r\n" + MethodGen(m) + "\r\n");
            sb.Append("\t\t}\r\n");
            sb.Append('}');
            sb.PrintToConsole();
            
            return sb + "";
        }
    }
}