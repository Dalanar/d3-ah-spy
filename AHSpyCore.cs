using System;

namespace D3AHSpy
{
    class AHSpyCore
    {
        public int GetBuyoutPrice1(int dwProcessID)
        {
            int nBuyout = -1;
            MemoryReader test = new MemoryReader();
            test.AttachToProcess(dwProcessID);
            System.Diagnostics.Process lol = System.Diagnostics.Process.GetProcessById(dwProcessID);
            test.GetValueInt(new IntPtr(lol.MainModule.BaseAddress.ToInt32() + 0x00FC7590), out nBuyout);
            test.DetachFromProcess();
            return nBuyout;
        }

        public int GetBuyoutPrice2(int nAddress, int dwProcessID)
        {
            int nBuyout = -1;
            MemoryReader test = new MemoryReader();
            test.AttachToProcess(dwProcessID);
            test.GetValueInt(new IntPtr(nAddress), out nBuyout);
            test.DetachFromProcess();
            return nBuyout;
        }

        public int GetBuyoutPrice3(int nAddress, int dwProcessID)
        {
            int nBuyout = -1;
            MemoryReader test = new MemoryReader();
            test.AttachToProcess(dwProcessID);
            test.GetValueInt(new IntPtr(nAddress + 0xC), out nBuyout);
            test.DetachFromProcess();
            return nBuyout;
        }

        public int GetBuyoutPrice4(int nAddress, int dwProcessID)
        {
            int nBuyout = -1;
            MemoryReader test = new MemoryReader();
            test.AttachToProcess(dwProcessID);
            test.GetValueInt(new IntPtr(nAddress + 0xD8), out nBuyout);
            test.DetachFromProcess();
            return nBuyout;
        }
    }
}
