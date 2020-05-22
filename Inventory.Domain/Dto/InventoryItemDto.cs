using System.Collections.Generic;

namespace Inventory.Domain.Dto
{
    public class InventoryItemDto
    {
        public string Id{ get; set; }
        public string Name { get; set; }
        public List<AvailabilityDto> Availabilities { get; set; }

        public InventoryItemDto()
        {
            Availabilities = new List<AvailabilityDto>();
        }
    }
}
