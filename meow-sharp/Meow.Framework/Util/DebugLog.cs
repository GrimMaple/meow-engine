using System.Collections.Generic;
using System.IO;

namespace Meow.Framework.Util
{
    /// <summary>
    /// Debug logging class
    /// </summary>
    public sealed class DebugLog
    {
        List<DebugMessage> messages;
        private static DebugLog instance;

        /// <summary>
        /// Get the shared debug logger
        /// </summary>
        public static DebugLog SharedInstance
        {
            get
            {
                if (instance == null)
                    instance = new DebugLog();

                return instance;
            }
        }

        /// <summary>
        /// Number of messages in the console
        /// </summary>
        public int MessagesCount
        {
            get
            {
                return messages.Count;
            }
        }

        private DebugLog()
        {
            messages = new List<DebugMessage>(1000);
        }

        /// <summary>
        /// Get a message wuth certain index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DebugMessage this[int i]
        {
            get
            {
                return messages[i];
            }
        }

        /// <summary>
        /// Write an info string to debug log
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="parameters">Formatting objects</param>
        public void Write(string format, params object[] parameters)
        {
            Write(DebugMessageType.Info, format, parameters);
        }

        /// <summary>
        /// Write a string of desired type to debug log
        /// </summary>
        /// <param name="type">String type</param>
        /// <param name="format">String format</param>
        /// <param name="parameters">Formatting objects</param>
        public void Write(DebugMessageType type, string format, params object[] parameters)
        {
            messages.Add(new DebugMessage(string.Format(format, parameters), type));
        }

        /// <summary>
        /// Save debug log into file
        /// </summary>
        /// <param name="path">File path</param>
        public void Save(string path)
        {
            string[] content = new string[messages.Count];
            for (int i = 0; i < messages.Count; i++)
                content[i] = messages[i].Message;
            File.WriteAllLines(path, content);
        }
    }
}
