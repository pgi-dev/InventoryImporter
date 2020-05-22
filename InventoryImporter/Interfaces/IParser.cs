using Inventory.Domain.Dto;

namespace InventoryImporter.Interfaces
{
    public interface IParser
    {
        InventoryItemDto Parse(string data);
    }
}
