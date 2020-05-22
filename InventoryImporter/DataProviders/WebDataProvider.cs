using InventoryImporter.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace InventoryImporter.DataProviders
{
    public class WebDataProvider : IDataProvider
    {
        public event EventHandler<string> LineRead;

        public async Task ProcessData(string uri)
        {
            using (var client = new WebClient())
            using (var content = await client.OpenReadTaskAsync(new Uri(uri)).ConfigureAwait(false))
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
