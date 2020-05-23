using Inventory.Domain;
using InventoryImporter.Interfaces;
using System;
using System.Threading.Tasks;

namespace InventoryImporter
{
    public class Importer
    {
        private readonly IDataProvider dataProvider;
        private readonly IParser parser;
        private readonly IRepository repository;

        public Importer(IDataProvider dataProvider, IParser parser, IRepository repository)
        {
            this.dataProvider = dataProvider;
            this.parser = parser;
            this.repository = repository;
        }

        public async Task<ImportResult> Import(string uri)
        {
            int recordsTotal = 0, recordFailed = 0, recordSucceeded = 0;

            EventHandler<string> handler = async (sender, e) =>
            {
                if (e.StartsWith("#") || string.IsNullOrWhiteSpace(e))
                {
                    return;
                }

                recordsTotal++;

                var itemDto = parser.Parse(e);
                if (itemDto != null)
                {
                    recordSucceeded++;
                    await repository.AddInventoryItem(itemDto);
                }
                else
                {
                    recordFailed++;
                }
            };

            try
            {
                dataProvider.LineRead += handler;
                await dataProvider.ProcessData(uri);

                return new ImportResult
                {
                    Success = true,
                    Message = $"Processed records: {recordsTotal}, succeeded: {recordSucceeded}, failed: {recordFailed}"
                };
            }
            catch(Exception ex)
            {
                return new ImportResult
                {
                    Success = false,
                    Message = $"Error processing data: {ex.Message}"
                };
            }
            finally
            {
                dataProvider.LineRead -= handler;
            }
        }
    }
}
