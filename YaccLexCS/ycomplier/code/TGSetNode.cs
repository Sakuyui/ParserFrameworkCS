using YaccLexCS.runtime.structures.task_builder;

namespace YaccLexCS.ycomplier.code.structure
{
    internal class TGSetNode : TaskGraphVertex
    {
        private TaskRegister var_reg;
        private TaskRegister reg;

        public TGSetNode(TaskRegister var_reg, TaskRegister reg)
        {
            this.var_reg = var_reg;
            this.reg = reg;
        }
    }
}