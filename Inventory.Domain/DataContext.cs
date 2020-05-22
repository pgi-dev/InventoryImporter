using Inventory.Domain.Model;
using System.Collections.Generic;

namespace Inventory.Domain
{
    public class DataContext
    {
        public List<Warehouse> Warehouses { get; private set; }

        public DataContext()
        {
            Warehouses = new List<Warehouse>(); 
        }
    }
}
