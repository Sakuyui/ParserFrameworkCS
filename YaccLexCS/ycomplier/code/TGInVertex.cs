using YaccLexCS.runtime.structures.task_builder;

namespace YaccLexCS.ycomplier.code.structure
{
    internal class TGInVertex : TaskGraphVertex
    {
        private TaskRegister[] registers;

        public TGInVertex(params TaskRegister[] registers) { 
            this.registers = registers;
        }
    }
}