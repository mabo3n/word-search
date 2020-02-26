using System.Collections.Generic;

namespace GutenbergAnalysis.RW.Reading
{
    public interface IDataReader<T>
    {
        public IEnumerable<T> Enumerate();
    }
}
