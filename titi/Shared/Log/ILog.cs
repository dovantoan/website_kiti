using System;
namespace Shared
{
    /// <summary>
    /// Provide method to logging application
    /// </summary>
    public interface ILog
    {

        #region Properties
        /// <summary>Get or set indent level</summary>
        /// <example>
        /// <code>
        /// ILog log;
        /// //...init log
        /// log.WriteLine("List of errors:");
        /// log.Indent();
        /// log.WriteLine("Error 1: File not found");
        /// log.WriteLine("Error 2: Directory not found");
        /// log.Unindent();
        /// log.WriteLine("End of list of errors");
        /// </code>
        /// This example produces the following output:
        /// <code>
        /// List of errors:
        ///   Error 1: File not found
        ///   Error 2: Directory not found
        /// End of list of errors
        /// </code>
        /// </example>
        int IndentLevel
        {
            get;
            set;
        }
        /// <summary>Get or set indent size.</summary>
        /// <remarks>Size is number of space <c>' '</c> character.</remarks>
        int IndentSize
        {
            get;
            set;
        }
        /// <summary>Check if log is closed.</summary>
        bool IsClose
        {
            get;
        }
        #endregion Properties

        #region Methods
        /// <summary>Write log header.</summary>
        void WriteHeader();
        /// <summary>Write an exception to log file.</summary>
        /// <param name="e">Exception to log.</param>
        void Write(Exception e);
        /// <summary>Write a message to log file. </summary>
        /// <param name="value">Message to write</param>
        void WriteLine(string value);
        /// <summary>Write a message to log file. </summary>
        /// <param name="value">Message to write</param>
        void Write(string value);
        /// <summary>Increases the current <see cref="IndentLevel"/> by one.</summary>
        void Indent();
        /// <summary>Decreases the current <see cref="IndentLevel"/> by one.</summary>
        void Unindent();
        /// <summary>Open log file.</summary>
        void Open();
        /// <summary>Flush the output buffer and close the log file.</summary>
        void Close();
        /// <summary>
        /// Flushes the output buffer, 
        /// and causes buffered data to be written to the log file.
        /// </summary>
        void Flush();
        #endregion Methods
    }

    public class LogNull : ILog
    {

        #region Fields
        private static readonly LogNull instance;
        #endregion Fields

        #region Methods
        static LogNull()
        {
            instance = new LogNull();
        }
        private LogNull() { }

        public int IndentLevel
        {
            get { return 0; }
            set { }
        }
        public int IndentSize
        {
            get { return 0; }
            set { }
        }
        public bool IsClose
        {
            get { return false; }
        }
        public static LogNull Value
        {
            get { return instance; }
        }
        public void WriteHeader() { }
        public void Write(Exception e) { }
        public void WriteLine(string value) { }
        void Shared.ILog.Write(string value) { }
        public void Indent() { }
        public void Unindent() { }
        public void Open() { }
        public void Close() { }
        public void Flush() { }
        #endregion Methods
    }
}
