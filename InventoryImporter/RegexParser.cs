
using Inventory.Domain.Dto;
using InventoryImporter.Interfaces;
using System.Text.RegularExpressions;

namespace InventoryImporter
{
    public class RegexParser : IParser
    {
        private Regex _regex = new Regex(@"^(?<name>[^;]+);(?<id>[^;]+);(?<availability>[^,]+,\d+[|]?)+$");
        private Regex _availability = new Regex(@"^(?<warehouse>[^,]+),(?<quantity>\d+)[|]?$");

        public ItemDto Parse(string data)
        {
            ItemDto result = null;

            if (string.IsNullOrWhiteSpace(data))
            {
                return result;
            }

            var match = _regex.Match(data);
            if (match.Success)
            {
                result = new ItemDto();
                result.Name = match.Groups["name"].Value;
                result.Id = match.Groups["id"].Value;

                var availabilities = match.Groups["availability"].Captures;
                for (int i = 0; i < availabilities.Count; i++)
                {
                    var availabilityMatch = _availability.Match(availabilities[i].Value);
                    if (availabilityMatch.Success)
                    {
                        var availabilityDto = new AvailabilityDto
                        {
                            WarehouseId = availabilityMatch.Groups["warehouse"].Value,
                            Quantity = int.Parse(availabilityMatch.Groups["quantity"].Value)
                        };
                        result.Availabilities.Add(availabilityDto);
                    }
                }
            }

            return result;
        }
    }
}
