
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.code;
using YaccLexCS.ycomplier.code.structure;
namespace YaccLexCS.code.structure
{
		[GrammarConfiguration]
		public class ElsifListNode : ASTNonTerminalNode
		{
				public override dynamic Eval(RuntimeContext context)
				{
						return EvaluationConfiguration.ClassNameMapping[GetType().Name].Invoke(null, new object[]{this, context});
				}
				public ElsifListNode(IEnumerable<ASTNode> child) : base(child, "elsif_list")
				{
				}
		}
}