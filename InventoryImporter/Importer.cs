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

        private int recordsTotal;
        private int recordFailed;
        private int recordSucceeded;

        public Importer(IDataProvider dataProvider, IParser parser, IRepository repository)
        {
            this.dataProvider = dataProvider;
            this.parser = parser;
            this.repository = repository;
        }

        public async Task<ImportResult> Import(string uri)
        {
            try
            {
                recordsTotal = recordFailed = recordSucceeded = 0;

                dataProvider.LineRead += DataProvider_LineRead;
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
                dataProvider.LineRead -= DataProvider_LineRead;
            }
        }

        private async void DataProvider_LineRead(object sender, string e)
        {
            if(e.StartsWith("#") || string.IsNullOrWhiteSpace(e))
            {
                return;
            }

            recordsTotal++;

            var itemDto = parser.Parse(e);
            if (itemDto != null)
            {
                recordSucceeded++;
                await repository.Add(itemDto);
            }
            else
            {
                recordFailed++;
            }
        }
    }
}
