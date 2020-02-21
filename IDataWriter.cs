using System.Collections.Generic;

namespace gutenberg_analysis
{
    public interface IDataWriter<T>
    {
        public void Write(IEnumerable<T> data);
    }
}
