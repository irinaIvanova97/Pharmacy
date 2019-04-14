using System;
using System.Diagnostics;
using System.IO;

namespace Pharmacy
{
    public class Log
    {
        private static Log logInstance = null;

        public static string LogFilePath { get; private set; }
        public string LogFileName { get; private set; }

        public enum LogType
        {
            INFO,
            WARNING,
            ERROR
        }

        private Log()
        {
            LogFileName = "Pharmacy";
        }

        public static Log Instance
        {
            get
            {
                if (logInstance == null)
                    logInstance = new Log();

                return logInstance;
            }
        }

        public void InitializeLog()
        {
            DateTime dt = DateTime.Now;
            LogFileName = string.Format("{0}-{1}.txt", LogFileName, dt.ToString("yyyy-MM-dd-HH-mm-ss"));

            LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
            if(!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);
            }

            // Delete logs older than a month
            string[] files = Directory.GetFiles(LogFilePath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastWriteTime < DateTime.Now.AddMonths(-1))
                    fi.Delete();
            }

            LogFilePath += LogFileName;
            if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath).Close();
            }
        }

        public static void LogInfo(string message, params object[] args)
        {
            StackFrame stackFrame = GetStackFrame();

            message = string.Format(message, args);
            LogMessage
                (
                    message, 
                    LogType.INFO, 
                    stackFrame.GetMethod().DeclaringType.Name, 
                    stackFrame.GetMethod().Name, 
                    stackFrame.GetFileName(), 
                    stackFrame.GetFileLineNumber(), 
                    stackFrame.GetFileColumnNumber() 
                );
        }

        public static void LogWarning(string message, params object[] args)
        {
            StackFrame stackFrame = GetStackFrame();

            message = string.Format(message, args);
            LogMessage
                (
                    message, 
                    LogType.WARNING, 
                    stackFrame.GetMethod().DeclaringType.Name, 
                    stackFrame.GetMethod().Name, 
                    stackFrame.GetFileName(), 
                    stackFrame.GetFileLineNumber(), 
                    stackFrame.GetFileColumnNumber()
                );
        }

        public static void LogError(string message, params object[] args)
        {
            StackFrame stackFrame = GetStackFrame();
            LogError(message, stackFrame, args);
        }

        public static void LogError(string message, StackFrame stackFrame = null, params object[] args)
        {
            if (stackFrame == null)
                stackFrame = GetStackFrame();

            message = string.Format(message, args);
            LogMessage
                (
                    message, 
                    LogType.ERROR, 
                    stackFrame.GetMethod().DeclaringType.Name, 
                    stackFrame.GetMethod().Name, 
                    stackFrame.GetFileName(), 
                    stackFrame.GetFileLineNumber(), 
                    stackFrame.GetFileColumnNumber()
                );
        }

        public static void LogException(Exception exception)
        {
            StackFrame stackFrame = GetStackFrame();

            LogError("Exception: {0}, HRESULT: {1}", stackFrame, exception.Message, exception.HResult.ToString());
        }

        private static StackFrame GetStackFrame()
        {
            StackTrace st = new StackTrace(true);
            StackFrame sf = st.GetFrame(2); // Няколко нива нагоре

            return sf;
        }

        private static void LogMessage
            (
                string message, 
                LogType logType, 
                string className, 
                string method, 
                string filePath, 
                int lineNumber, 
                int columnNumber
            )
        {
            string logTypeString = logType.ToString();

            if (logType == LogType.INFO)
                logTypeString += "   ";

            if (logType == LogType.ERROR)
                logTypeString += "  ";

            string logMessage = string.Format
                (
                    "{0}: [{1}.{2}][{3}:{4}.{5}] {6}",
                    logTypeString,
                    className,
                    method,
                    System.IO.Path.GetFileName(filePath),
                    lineNumber,
                    columnNumber,
                    message
                );

#if DEBUG
            Console.WriteLine(logMessage);
#endif
            try
            {
                File.AppendAllLines(LogFilePath, new string[] { logMessage });
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}, HRESULT: {1}", exception.Message, exception.HResult.ToString());
            }
        }
    }
}
