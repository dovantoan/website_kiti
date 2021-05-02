using System;
namespace Shared
{

    public class TransactionException : Error
    {
        public string transactionName;

        public TransactionException() : base() { }

        public TransactionException(string transactionName)
          : base()
        {
            this.transactionName = transactionName;
        }

        public TransactionException(string transactionName, string message)
          : base(message)
        {
            this.transactionName = transactionName;
        }

        public TransactionException(string transactionName, string message, Exception inner)
          : base(message, inner)
        {
            this.transactionName = transactionName;
        }

        public TransactionException(string message, Exception inner) : base(message, inner) { }

        public override string Message
        {
            get
            {
                string result;
                result = base.Message;
                if (transactionName != null && transactionName.Length > 0)
                {
                    result = string.Concat(transactionName, ": ", result);
                }
                return result;
            }
        }
    }
}
