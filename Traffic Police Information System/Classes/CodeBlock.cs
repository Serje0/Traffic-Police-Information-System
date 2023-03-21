using System;

namespace Traffic_Police_Information_System.Classes
{
    public class CodeBlock
    {
        private bool isLocked;
        private DateTime unlockTime;

        public CodeBlock()
        {
            isLocked = false;
        }

        public void Block()
        {
            isLocked = true;
            unlockTime = DateTime.Now.AddSeconds(10);            
        }

        public void Reset()
        {
            if (isLocked && DateTime.Now < unlockTime)
            {
                isLocked = false;
            }
        }

        public bool IsLocked
        {
            get { return isLocked && DateTime.Now < unlockTime; }
        }
    }
}
