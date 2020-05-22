using System.Collections.Generic;

namespace Inventory.Domain.Dto
{
    public class ItemDto
    {
        public string Id{ get; set; }
        public string Name { get; set; }
        public List<AvailabilityDto> Availabilities { get; set; }

        public ItemDto()
        {
            Availabilities = new List<AvailabilityDto>();
        }
    }
}
