
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.code;
using YaccLexCS.ycomplier.code.structure;
namespace YaccLexCS.code.structure
{
		[GrammarConfiguration]
		public class StatementListNode : ASTNonTerminalNode
		{
				public override dynamic Eval(ycomplier.RuntimeContext context)
				{
						return EvaluationConfiguration.ClassNameMapping[GetType().Name].Invoke(null, new object[]{this, context});
				}
				public StatementListNode(IEnumerable<ASTNode> child) : base(child, "statement_list")
				{
				}
		}
}