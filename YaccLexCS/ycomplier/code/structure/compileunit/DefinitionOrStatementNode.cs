
using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;
using YaccLexCS.ycomplier.code;
using YaccLexCS.ycomplier.code.structure;
namespace YaccLexCS.code.structure
{
		[GrammarConfiguration]
		public class DefinitionOrStatementNode : ASTNonTerminalNode
		{
				public override dynamic Eval(ycomplier.RuntimeContext context)
				{
						return EvaluationConfiguration.ClassNameMapping[GetType().Name].Invoke(null, new object[]{this, context});
				}
				public DefinitionOrStatementNode(IEnumerable<ASTNode> child) : base(child, "definition_or_statement")
				{
				}
		}
}