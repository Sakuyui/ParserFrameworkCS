
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.code;
using YaccLexCS.ycomplier.code.structure;
namespace YaccLexCS.code.structure
{
		[GrammarConfiguration]
		public class TerminalExpressionNode : ASTNonTerminalNode
		{
				public override dynamic Eval(CompilerContext context)
				{
						return EvaluationConfiguration.ClassNameMapping[GetType().Name].Invoke(null, new object[]{this, context});
				}
				public TerminalExpressionNode(IEnumerable<ASTNode> child) : base(child, "terminal_expression")
				{
				}
		}
}