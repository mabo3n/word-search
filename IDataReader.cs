using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace gutenberg_analysis
{
    public interface IDataReader<T>
    {
        public IEnumerable<T> Enumerate();
    }
}
