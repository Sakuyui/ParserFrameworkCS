﻿namespace YaccLexCS.ycomplier.code.structure
{
    public class YBlock
    {
        public string BlockText;

        public YBlock(string blockText)
        {
            BlockText = blockText;
        }

        public override string ToString()
        {
            return BlockText;
        }
    }
}