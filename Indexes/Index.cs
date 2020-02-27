using GutenbergAnalysis.Records;
using GutenbergAnalysis.RW.Reading;
using GutenbergAnalysis.RW.Writing;

namespace GutenbergAnalysis.Indexes
{
    public abstract class Index<TSource, T>
        where TSource : Record
        where T : Record
    {
        public IDataReader<TSource> sourceData;
        public IDataReader<T> dataReader;
        public IDataWriter<T> dataWriter;

        public void Create()
        {
            foreach (var item in sourceData.Enumerate())
            {

            }
        }
    }
}
