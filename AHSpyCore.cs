using System;

namespace D3AHSpy
{
    class AHSpyCore
    {
        public int GetFirstBuyoutPrice()
        {
            int nBuyout = -1;
            MemoryReader test = new MemoryReader();
            test.AttachToProcess();
            return nBuyout;
        }
    }
}
