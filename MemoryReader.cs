using System;
using System.Runtime.InteropServices;

namespace D3AHSpy
{
    class MemoryReader : Logger
    {
        #region Win32API
        [Flags]
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hProcess);
        #endregion

        private int m_dwProcessID;
        private IntPtr m_hAttachedProcess;

        public MemoryReader()
        {
            m_dwProcessID = 0;
            m_hAttachedProcess = new IntPtr(0);
        }

        ~MemoryReader()
        {
            DetachFromProcess();
        }

        /// <summary>
        /// Attach to process to start memory reading.
        /// </summary>
        /// <param name="dwProcessID">Target process ID. Must be > 0</param>
        /// <returns>0 if all was OK and we are attached to process</returns>
        public int AttachToProcess(int dwProcessID)
        {
            int nReturn = 0;

            try
            {
                if (dwProcessID <= 0)
                {
                    ArgumentException exInvalidArg = new ArgumentException("Wrong process ID");
                    exInvalidArg.Data.Add("Custom", 1);
                    exInvalidArg.Data.Add("nDebugLevel", 1);
                    throw exInvalidArg;
                }

                m_dwProcessID = dwProcessID;
                m_hAttachedProcess = OpenProcess(ProcessAccessFlags.All, false, m_dwProcessID);

                if (m_hAttachedProcess.ToInt64() == 0)
                {
                    Exception exUnexpectedResult = new Exception("Unexpected return value from OpenProcess");
                    exUnexpectedResult.Data.Add("Custom", 1);
                    exUnexpectedResult.Data.Add("nDebugLevel", 2);
                    throw exUnexpectedResult;
                }

                WriteLog(0, "Successfully attached to process ID " + dwProcessID.ToString());
            }
            catch (Exception ex)
            {
                int nLevel = 2;

                if (ex.Data.Contains("Custom"))
                {
                    if (ex.Data.Contains("nDebugLevel"))
                    {
                        nLevel = Convert.ToInt32(ex.Data["nDebugLevel"]);
                    }
                }

                nReturn = -1;

                WriteLog(nLevel, "Exception while executing AttachToProcess in " + ex.TargetSite.ToString() + "@" + ex.Source + " : " + ex.Message);
                m_dwProcessID = 0;
                CloseHandle(m_hAttachedProcess);
            }

            return nReturn;
        }

        /// <summary>
        /// Determines if this class attached to process
        /// </summary>
        public bool IsAttached()
        {
            return ((m_dwProcessID != 0) && (m_hAttachedProcess.ToInt64() != 0));
        }

        /// <summary>
        /// Detaches from attached process
        /// </summary>
        public void DetachFromProcess()
        {
            if (m_hAttachedProcess.ToInt64() != 0)
                CloseHandle(m_hAttachedProcess);

            m_dwProcessID = 0;
        }

        private int ReadData(IntPtr lAddress, int nSize, out byte[] buffer)
        {
            int nReturn = 0;
            buffer = new byte[nSize];
            try
            {
                if (!IsAttached())
                {
                    Exception exNotInited = new Exception("MemoryReader is not attached to any process");
                    exNotInited.Data.Add("Custom", 1);
                    exNotInited.Data.Add("nDebugLevel", 2);
                    throw exNotInited;
                }

                int nBytesRead;
                if (!ReadProcessMemory(m_hAttachedProcess, lAddress, buffer, nSize, out nBytesRead))
                {
                    Exception exError = new Exception("ReadProcessMemory failed. LastError=" + Marshal.GetLastWin32Error().ToString());
                    exError.Data.Add("Custom", 1);
                    exError.Data.Add("nDebugLevel", 2);
                    throw exError;
                }

                if (nSize != nBytesRead)
                {
                    Exception exError = new Exception("ReadProcessMemory nSize != nBytesRead");
                    exError.Data.Add("Custom", 1);
                    exError.Data.Add("nDebugLevel", 1);
                    throw exError;
                }

            }
            catch(Exception ex)
            {
                int nLevel = 2;

                if (ex.Data.Contains("Custom"))
                {
                    if (ex.Data.Contains("nDebugLevel"))
                    {
                        nLevel = Convert.ToInt32(ex.Data["nDebugLevel"]);
                    }
                }

                nReturn = -1;

                WriteLog(nLevel, "Exception while executing ReadData in " + ex.TargetSite.ToString() + "@" + ex.Source + " : " + ex.Message);
            }


            return nReturn;
        }

        public int GetValueInt(IntPtr nAddress, out int nValue)
        {
            int nRet = -1;
            byte[] buffer;
            nRet = ReadData(nAddress, 4, out buffer);
            nValue = 0;
            for (int i = 0; i < 4; i++)
            {
                nValue += buffer[i] << i * 8;
            }
            return nRet;
        }
    }
}
