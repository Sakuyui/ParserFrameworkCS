using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.ycomplier.conf.attribution;

namespace YaccLexCS.ycomplier
{
    public static class YCompilerConfigurator
    {
        public static IEnumerable<Type> ScanTokenConfiguration(IEnumerable<string> packetName)
        {
            var tokenConfig = 
                (from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && packetName.Contains(t.Namespace) && t.GetCustomAttribute(typeof(TokenConfiguration)) != null
                select t).ToList();
            tokenConfig.ForEach(t => Console.WriteLine(t.Name));
            return tokenConfig;
        }
        
        public static IEnumerable<(TokenDefinition tokenDef, MethodInfo methodInfo)> GetAllTokenDefinitions(IEnumerable<Type> types)
        {
            
            var methods = types.SelectMany(e => e.GetMethods());
            
            var result = methods
                .Where(m => m.GetCustomAttributes(typeof(TokenDefinition)).Any() && m.IsStatic)
                .SelectMany(m => m.GetCustomAttributes<TokenDefinition>().Select(attr => (attr, m)));
            return result;
        }
    }
}