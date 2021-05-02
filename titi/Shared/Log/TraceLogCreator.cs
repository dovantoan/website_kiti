using System;
namespace Shared
{
    /// <summary>
    /// Summary description for TraceLogCreator.
    /// </summary>
    public class TraceLogCreator : ILogCreator
    {
        public TraceLogCreator()
        {
        }

        public ILog Create(string fileName)
        {
            return new TraceLog(fileName);
        }
    }
}
