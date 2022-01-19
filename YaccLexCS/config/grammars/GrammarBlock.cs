using System.Collections.Generic;
using YaccLexCS.ycomplier;
using YaccLexCS.ycomplier.attribution;

namespace YaccLexCS.config{
	[GrammarConfiguration]
	public static class GrammarBlock{

		[GrammarDefinition("block","LC statement_list RC","LC RC")]
		public static void block(CompilerContext context)
		{
		}

	}
}