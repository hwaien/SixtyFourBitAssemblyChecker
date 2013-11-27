using System;
using System.Runtime.Serialization;

namespace H2O.Applications.SixtyFourBitAssemblyChecker
{
    [Serializable]
    public class PEHeaderNotFoundException : Exception
    {
        public PEHeaderNotFoundException()
            : this("Can't find assembly's PE header")
        {
        }

        public PEHeaderNotFoundException(string message)
            : base(message)
        {
        }

        public PEHeaderNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PEHeaderNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
