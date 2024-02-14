namespace YaccLexCS.runtime.structures.task_builder
{
    internal class TaskRegister
    {
        private TaskRegisterKind kind;
        private int number;

        public TaskRegister(TaskRegisterKind kind, int number)
        {
            this.kind = kind;
            this.number = number;
        }
    }
}