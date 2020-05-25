using System;

namespace GeneratorLibrary
{
    [Serializable]
    public class PolyException : Exception
    {
        public PolyException() { }
        public PolyException(string message) : base(message) { }
        public PolyException(string message, Exception inner) : base(message, inner) { }
        protected PolyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
