using YaccLexCS.runtime.structures.task_builder;

namespace YaccLexCS.ycomplier.code.structure
{
    internal class TGNewVariableNode : TaskGraphVertex
    {
        private string getSourceText;
        private TaskRegister taskRegister;

        public TGNewVariableNode(string getSourceText, TaskRegister taskRegister)
        {
            this.getSourceText = getSourceText;
            this.taskRegister = taskRegister;
        }
    }
}