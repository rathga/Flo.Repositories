using System;
using System.Runtime.Serialization;

namespace Flo.Repositories
{
    [Serializable]
    internal class NotAuthorisedException : Exception
    {
        public NotAuthorisedException()
        {
        }

        public NotAuthorisedException(string message) : base(message)
        {
        }

        public NotAuthorisedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAuthorisedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}