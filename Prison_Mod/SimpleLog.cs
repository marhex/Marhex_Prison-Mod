using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleLogger
{

    public class SimpleLog
    {
        public static String AppErrorLogFileName = "Prison_Mod.Log";

        /// <summary>
        /// Log any messages from the Application
        /// </summary>
        /// <param name="message"></param>
        public static void Info(String pMessage)
        {
            bool Successful = false;

            for (int idx = 0; idx < 10; idx++)
            {
                try
                {
                    // Log message to default log file.
                    StreamWriter str = new StreamWriter
                    (AppErrorLogFileName, true);

                    str.AutoFlush = true;   // Wri9te text with no buffering
                    str.WriteLine("[" + DateTime.Now.ToString() + "]" +  " [Info] " + pMessage);
                    str.Close();

                    Successful = true;
                }
                catch (Exception)
                {
                }

                if (Successful == true)     // Logging successful
                    break;
                else                        // Logging failed, retry in 100 milliseconds
                    Thread.Sleep(10);
            }
        }

        public static void Error(String pMessage)
        {
            bool Successful = false;

            for (int idx = 0; idx < 10; idx++)
            {
                try
                {
                    // Log message to default log file.
                    StreamWriter str = new StreamWriter
                    (AppErrorLogFileName, true);

                    str.AutoFlush = true;   // Wri9te text with no buffering
                    str.WriteLine("[" + DateTime.Now.ToString() + "]" + " [Error] " + pMessage);
                    str.Close();

                    Successful = true;
                }
                catch (Exception)
                {
                }

                if (Successful == true)     // Logging successful
                    break;
                else                        // Logging failed, retry in 100 milliseconds
                    Thread.Sleep(10);
            }
        }

     

        public static void Error(Exception pExeption)
        {
            String str_inner = "";

            try
            {
                str_inner = Environment.NewLine +
                "Inner Exception Msg: " + pExeption.InnerException.Message +
                "Inner Exception Stack: " + pExeption.InnerException.StackTrace;
            }
            catch (Exception)
            {
            }

            Info("Exception: " + pExeption.Message + Environment.NewLine +
                "Stack: " + str_inner);
        }
    } 
}