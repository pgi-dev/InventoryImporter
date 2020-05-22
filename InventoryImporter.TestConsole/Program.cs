using Inventory.Domain;
using InventoryImporter.DataProviders;
using InventoryImporter.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InventoryImporter.TestConsole
{
    class Program
    {
        // Use this inputs for testing
        // https://gist.githubusercontent.com/pgi-dev/0b7ae70fbd16a1aec0ad1f5404e79ec9/raw/ef7036a7cd55f3e7ca6351da46fb549874f346d7/ImportData.txt
        // D:\Projects\InventoryImporter\InventoryImporter.TestConsole\Sample\ImportData.txt

        static async Task Main(string[] args)
        {
            IDataProvider dataProvider = null;
           
            Console.WriteLine("Provide data source, web url or file path:");

            var uri = Console.ReadLine();
            if (Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                dataProvider = new WebDataProvider();
                Console.WriteLine("Processing Web content...");
            }
            else if(File.Exists(uri))
            {
                dataProvider = new FileDataProvider();
                Console.WriteLine("Processing File content...");
            }
            else
            {
                Console.WriteLine("Invalid resource...");
            }

            Console.WriteLine();

            if (dataProvider != null)
            {
                IParser parser = new RegexParser();
                IRepository repository = new Repository(new DataContext());

                var importer = new Importer(dataProvider, parser, repository);
                var result = await importer.Import(uri);

                Console.WriteLine(result.Message);
                Console.WriteLine();

                if (result.Success)
                {
                    var dump = repository.DumpContent();
                    Console.Write(dump);
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
