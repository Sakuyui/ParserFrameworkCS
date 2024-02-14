using YaccLexCS.runtime.structures.task_builder;

namespace YaccLexCS.ycomplier.code.structure
{
    internal class TGDSLPrimitiveCallNode : TaskGraphVertex
    {
        private string functionName;
        private TaskRegister[] registers;

        public TGDSLPrimitiveCallNode(string functionName, TaskRegister resultReg, params TaskRegister[] registers)
        {
            this.functionName = functionName;
            this.registers = registers;
        }
    }
}