using YaccLexCS.ycomplier.code;

namespace YaccLexCS.runtime
{
    public static class InterpreterHelper
    {
		public static Dictionary<string, object> EntreNewBlock(RuntimeContext context)
        {
			return context.GetCurrentCommonFrame().CreateNewStorageBlockForNewCodeBlock();
        }
		public static void LeaveBlock(RuntimeContext context)
        {
			context.GetCurrentCommonFrame().RemoveNewestStorageBlock();
        }
		public static dynamic BasicTypesValueExtract(ASTTerminalNode terminalNode, RuntimeContext context)
        {
            switch (terminalNode.Token.Type)
            {
				case "ID":
					return context.GetCurrentCommonFrame().GetLocalVar(terminalNode.Token.SourceText);
				case "STRING":
					return terminalNode.Token.SourceText;
				case "DOUBLE_LITERAL":
					return double.Parse(terminalNode.Token.SourceText);
				case "TRUE_T":
					return 1;
				case "FALSE_T":
					return 0;
				case "BREAK":
					return SpecialValue.BREAK;
				case "CONTINUE":
					return SpecialValue.CONTINUE;
			}
			return null;
        }
	}
}