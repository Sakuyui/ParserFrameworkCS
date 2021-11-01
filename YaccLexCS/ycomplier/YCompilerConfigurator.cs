using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.util;

namespace YaccLexCS.ycomplier
{
    public static class YCompilerConfigurator
    {
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