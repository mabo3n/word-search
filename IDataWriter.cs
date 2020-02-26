using System.Collections.Generic;

namespace GutenbergAnalysis
{
    public interface IDataWriter<T>
    {
        public void Write(IEnumerable<T> data);
    }
}
