using YaccLexCS.runtime.structures.task_builder;

namespace YaccLexCS.ycomplier.code.structure
{
    internal class TGOutNode : TaskGraphVertex
    {
        private TaskRegister reg;

        public TGOutNode(TaskRegister reg)
        {
            this.reg = reg;
        }
    }
}