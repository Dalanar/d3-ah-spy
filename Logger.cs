using System;

namespace D3AHSpy
{
    class Logger
    {
        protected int m_nLogLevel;

        public Logger()
        {
            m_nLogLevel = -1;
        }

        /// <summary>
        /// Writes a log message.
        /// </summary>
        /// <param name="nSignificance">Message's importance. Values: 
        /// <para>0 - regular info message (to console);</para>
        /// <para>1 - warning, something goes wrong;</para>
        /// <para>2 - error, something terrible happened.</para></param>
        /// <param name="sMessage">String with a message</param>
        protected void WriteLog(int nSignificance, string sMessage)
        {
            if (nSignificance <= m_nLogLevel)
                return;

            
            string sHeader = DateTime.Now.ToString() + " " + this.GetType().ToString()+ ": ";

            switch (nSignificance)
            {
                case 0:
                    Console.WriteLine(sHeader + sMessage);
                    break;
                case 1:
                    Console.WriteLine(sHeader + "!!!WARNING!!! " + sMessage);
                    break;
                case 2:
                    System.Windows.Forms.MessageBox.Show(sHeader + "!!!ERROR!!! " + sMessage);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set logging level to turn off some unused messages
        /// </summary>
        /// <param name="nLogLevel">New log level.
        /// <para>-1 - show all messages;</para>
        /// <para>0 - show warnings and errors;</para>
        /// <para>1 - show only errors.</para></param>
        public void SetLogLevel(int nLogLevel)
        {
            m_nLogLevel = nLogLevel;
        }

    }
}
