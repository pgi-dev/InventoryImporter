
using System.Collections.Generic;

namespace Inventory.Domain.Model
{
    public class Warehouse
    {
        public string Id { get; set; }
        public List<InventoryItem> Items { get; set; }

        public Warehouse()
        {
            Items = new List<InventoryItem>();
        }
    }
}
