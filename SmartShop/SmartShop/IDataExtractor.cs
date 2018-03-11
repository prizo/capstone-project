using System.Collections.Generic;

namespace SmartShop
{
    interface IDataExtractor<T>
    {
        IList<T> ExtractData(string document);
    }
}
