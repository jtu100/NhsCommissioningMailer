﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Logger
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// </summary>
    public class LogWriter
    {
        private static LogWriter _instance;
        private static Queue<Log> _logQueue;
        private static string logDir = Application.StartupPath;
        private static string logFile = "logFile";
        private static int maxLogAge = 60;
        private static int queueSize = 25;
        private static DateTime _lastFlushed = DateTime.Now;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter() { }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (_instance == null)
                {
                    _instance = new LogWriter();
                    _logQueue = new Queue<Log>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToLog(string message)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (_logQueue)
            {
                // Create the entry and push to the Queue
                Log logEntry = new Log(message);
                _logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (_logQueue.Count >= queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }            
        }

        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - _lastFlushed;
            if (logAge.TotalSeconds >= maxLogAge)
            {
                _lastFlushed = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private void FlushLog()
        {
            while (_logQueue.Count > 0)
            {
                Log entry = _logQueue.Dequeue();
                string logPath = string.Format("{0}\\{1}_{2}.txt", logDir, entry.LogDate, logFile);

        // This could be optimised to prevent opening and closing the file for each write
                using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter log = new StreamWriter(fs))
                    {
                        log.WriteLine(string.Format("{0}\t{1}",entry.LogTime,entry.Message));
                    }
                }
            }            
        }
    }

    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    internal class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }

        public Log(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }
}
