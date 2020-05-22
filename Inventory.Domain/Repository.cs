using Inventory.Domain.Dto;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Inventory.Domain.Model;

namespace Inventory.Domain
{
    public interface IRepository
    {
        Task Add(InventoryItemDto item);
        void Truncate();
        string DumpContent();
    }

    public class Repository : IRepository
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }

        public Task Add(InventoryItemDto item)
        {
            foreach (var availability in item.Availabilities)
            {
                var warehouse = context.Warehouses.Find(_ => _.Id == availability.WarehouseId);
                if (warehouse == null)
                {
                    warehouse = new Warehouse
                    {
                        Id = availability.WarehouseId
                    };

                    context.Warehouses.Add(warehouse);
                }

                warehouse.Items.Add(new InventoryItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = availability.Quantity
                });
            }

            return Task.CompletedTask;
        }

        public void Truncate()
        {
            context.Warehouses.Clear();
        }

        public string DumpContent()
        {
            StringBuilder sb = new StringBuilder();

            var query = from w in context.Warehouses
                        orderby w.Items.Sum(_ => _.Quantity) descending, w.Id descending
                        select w;

            query.ToList().ForEach(warehouse =>
            {
                sb.AppendLine($"{warehouse.Id} (total {warehouse.Items.Sum(_ => _.Quantity)})");

                var items = from i in warehouse.Items
                            orderby i.Id
                            select i;

                items.ToList().ForEach(item => sb.AppendLine($"{item.Id}: {item.Quantity}"));

                sb.AppendLine();
            });

            return sb.ToString();
        }
    }
}
