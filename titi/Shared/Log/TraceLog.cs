using System;
using System.Diagnostics;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Summary description for TraceLog.
    /// </summary>
    public class TraceLog : ILog
    {
        private string fileName;
        private TextWriterTraceListener writer;

        public TraceLog(string fileName)
        {
            this.fileName = fileName;
            writer = null;
        }

        public int IndentLevel
        {
            get { return Trace.IndentLevel; }
            set { Trace.IndentLevel = value; }
        }

        public int IndentSize
        {
            get { return Trace.IndentSize; }
            set { Trace.IndentSize = value; }
        }

        public bool IsClose
        {
            get { return writer == null; }
        }

        public void Open()
        {
            if (writer == null)
            {
                writer = new TextWriterTraceListener(this.fileName);
                Trace.Listeners.Add(writer);
            }
        }

        public void Close()
        {
            if (writer != null)
            {
                Trace.Listeners.Remove(writer);
                writer.Close();
                writer = null;
            }
        }

        public void WriteLine(string value)
        {
            Trace.WriteLine(value);
        }

        public void Write(string value)
        {
            Trace.Write(value);
        }

        public void WriteHeader()
        {
            this.WriteLine(string.Format("Append at: {0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            //this.WriteLine("[Time<HH:mm>] [Source File] [Line error] [Description] [(SQLSatement)]");
        }

        public void Write(Exception e)
        {
            StackTrace stacktrace = new StackTrace(e, true);
            if (stacktrace.FrameCount <= 0)
            {
                return;
            }

            string logline = "{0} : {1} : {2} : {3} : {4} : {5}";
            StackFrame stackframe = stacktrace.GetFrame(0);

            StringBuilder summaryError = new StringBuilder();
            summaryError.Append("\r\n");
            summaryError.Append("[BeginLogEx] \r\n");
            summaryError.AppendFormat(logline, DateTime.Now.ToString("t"), stackframe.GetFileName(), stackframe.GetMethod().Name, stackframe.GetFileLineNumber(), e.Message, "");
            summaryError.Append("\r\n");
            summaryError.Append("[Exceptions] \r\n");
            string exceptionLineFormat = "{0}: {1} \r\n";
            Exception tempE = e;
            while (tempE != null)
            {
                summaryError.AppendFormat(exceptionLineFormat, tempE.GetType().Name, tempE.Message);
                tempE = tempE.InnerException;
            }
            summaryError.Append("[BeginStackTrace]\r\n");
            summaryError.Append(e.StackTrace);
            summaryError.Append("\r\n");
            summaryError.Append("[EndStackTrace]\r\n");
            summaryError.Append("[EndLogEx] \r\n");

            this.WriteLine(summaryError.ToString());
            this.Flush();
        }
        public void Indent()
        {
            Trace.Indent();
        }
        public void Unindent()
        {
            Trace.Unindent();
        }
        public void Flush()
        {
            Trace.Flush();
        }
    }
}
