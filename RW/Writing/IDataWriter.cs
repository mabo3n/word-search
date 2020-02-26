using System.Collections.Generic;
using GutenbergAnalysis.Records;

namespace GutenbergAnalysis.RW.Writing
{
    public interface IDataWriter<T> where T : Record
    {
        public void Write(T data);
    }
}
