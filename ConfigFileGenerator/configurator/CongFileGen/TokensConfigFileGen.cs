using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigFileGenerator.configurator.CongFileGen
{
    public static class TokensConfigFileGen
    {
        public static string[] Header =
        {
            "using System;",
            "using YaccLexCS.ycomplier;",
            "using YaccLexCS.ycomplier.attribution;"
        };

        public static string NameSpace = "YaccLexCS.config";
        public static string ClassName = "TokenList";

       
        const string T6 = "\t\t\t\t\t\t";
        const string T4 = "\t\t\t\t";
        const string T2 = "\t\t";
        const string LE = "\r\n";

        private static string MethodGen(TokenMethod method)
        {
            var sb = new StringBuilder();
           
            void AddAttrLine(TokenMethod.TokenAttributeDesc desc) => 
                sb.Append($"{T4}[TokenDefinition(\"{desc.Name}\", {desc.Desc}, {(desc.IsRegex + "").ToLower()}, {desc.Priority})]{LE}");

            foreach (var d in method.Descs) AddAttrLine(d);

            sb.Append($"{T4}public static void {method.MethodName}(){LE}" + T4 + "{" + $"{LE}{T4}" + "}");
            return sb + "";
        }
        public static string GenFileContentString(List<TokenMethod> methods)
        {
            var sb = new StringBuilder();
            sb.Append(Header.Aggregate("", (a, b) => a + "\r\n" + b));
            sb.Append("\r\n");
            sb.Append("namespace " + NameSpace + "\r\n{\r\n");
            sb.Append("\t\t[TokenConfiguration]\r\n");
            sb.Append("\t\tpublic static class " + ClassName + "\r\n\t\t{\r\n");
            foreach (var m in methods) sb.Append("\r\n" + MethodGen(m) + "\r\n");
            sb.Append("\t\t}\r\n");
            sb.Append('}');
            sb.PrintToConsole();
            return sb + "";
        }
    }
}