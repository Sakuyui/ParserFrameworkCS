using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YaccLexCS.ycomplier.conf.attribution;

namespace YaccLexCS.ycomplier.util
{
    public class ReflectionUtil
    {
        public static IEnumerable<(TokenDefinition tokenDef, MethodInfo methodInfo)> GetAllTokenDefinition(Type type)
        {
            //list all method
            var methods = type.GetMethods();
            //filter
            var result = methods
                .Where(m => m.GetCustomAttributes(typeof(TokenDefinition)).Any() && m.IsStatic)
                .SelectMany(m => m.GetCustomAttributes<TokenDefinition>().Select(attr => (attr, m)));
            return result;
        }
    }
}