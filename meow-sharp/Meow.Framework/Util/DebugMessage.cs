namespace Meow.Framework.Util
{
    /// <summary>
    /// Debug message type
    /// </summary>
    public enum DebugMessageType
    {
        /// <summary>
        /// Informational message
        /// </summary>
        Info,

        /// <summary>
        /// Warning message
        /// </summary>
        Warning,

        /// <summary>
        /// Error message
        /// </summary>
        Error
    }

    /// <summary>
    /// Represents a debugging message
    /// </summary>
    public struct DebugMessage
    {
        /// <summary>
        /// Message type
        /// </summary>
        public DebugMessageType Type;

        /// <summary>
        /// Message text
        /// </summary>
        public string Message
        {
            get; private set;
        }

        /// <summary>
        /// Create a new debug message
        /// </summary>
        /// <param name="text">Message text</param>
        /// <param name="type">Message info</param>
        public DebugMessage(string text, DebugMessageType type = DebugMessageType.Info)
        {
            Type = type;
            Message = text;
        }
    }
}
