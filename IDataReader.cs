using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GutenbergAnalysis
{
    public interface IDataReader<T>
    {
        public IEnumerable<T> Enumerate();
    }
}
