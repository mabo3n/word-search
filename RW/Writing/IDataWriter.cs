using System.Collections.Generic;

namespace GutenbergAnalysis.RW.Writing
{
    public interface IDataWriter<T>
    {
        public void Write(IEnumerable<T> data);
    }
}
