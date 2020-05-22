using Inventory.Domain.Dto;

namespace InventoryImporter.Interfaces
{
    public interface IParser
    {
        ItemDto Parse(string data);
    }
}
