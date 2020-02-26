using System.Collections.Generic;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Reading
{
    public interface IDataReader<T> where T : Record
    {
        public IEnumerable<T> Enumerate();
    }
}
