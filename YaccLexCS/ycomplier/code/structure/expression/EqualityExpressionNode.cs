
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.code;
using YaccLexCS.ycomplier.code.structure;
namespace YaccLexCS.code.structure
{
		[GrammarConfiguration]
		public class EqualityExpressionNode : ASTNonTerminalNode
		{
				public override dynamic Eval(ycomplier.RuntimeContext context)
				{
						return EvaluationConfiguration.ClassNameMapping[GetType().Name].Invoke(null, new object[]{this, context});
				}
				public EqualityExpressionNode(IEnumerable<ASTNode> child) : base(child, "equality_expression")
				{
				}
		}
}