using System;

namespace yuroca.test.Helpers
{
    //Nulear ciertas informaciones
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        public void Dispose() { }
        private NullScope() { }
    }
}
