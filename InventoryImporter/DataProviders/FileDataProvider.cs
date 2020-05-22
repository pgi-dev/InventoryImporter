using InventoryImporter.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InventoryImporter.DataProviders
{
    public class FileDataProvider : IDataProvider
    {
        public event EventHandler<string> LineRead;

        public async Task ProcessData(string path)
        {
            if(!File.Exists(path))
            {
                return;
            }

            using (var content = File.OpenRead(path))
            using (var reader = new StreamReader(content))
            {
                string line = null;
                while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    LineRead(this, line);
                }
            } 
        }
    }
}
