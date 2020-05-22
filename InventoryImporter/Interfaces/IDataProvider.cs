using System;
using System.Threading.Tasks;

namespace InventoryImporter.Interfaces
{
    public interface IDataProvider
    {
        Task ProcessData(string uri);
        event EventHandler<string> LineRead;
    }
}
