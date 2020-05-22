using InventoryImporter;
using InventoryImporter.Interfaces;
using NUnit.Framework;

namespace Tests
{
    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("ItemName;ItemId;W1,1", 1)]
        [TestCase("ItemName;ItemId;W1,1|W2,2", 2)]
        [TestCase("ItemName;ItemId;W1,1|W2,2|W3,3", 3)]
        [TestCase("Cherry Hardwood Arched Door - PS;COM-100001;WH-A,5|WH-B,10", 2)]
        public void Parse_When_InputIsCorrect_Returns_ValidDto(string validInput, int availabilities)
        {
            // Arrange
            IParser parser = new RegexParser();

            // Act
            var result = parser.Parse(validInput);

            // Assert
            Assert.IsNotNull(result, "Result is null");
            Assert.IsNotEmpty(result.Name, "ItemName is empty");
            Assert.IsNotEmpty(result.Id, "ItemId is empty");
            Assert.NotZero(result.Availabilities.Count, "Availabilities not found");
            Assert.AreEqual(availabilities, result.Availabilities.Count, "Availabilities count incorrect");
            result.Availabilities.ForEach(_ =>
            {
                Assert.IsNotEmpty(_.WarehouseId, "WarehouseIs is empty");
                Assert.NotZero(_.Quantity);
            });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(";;|")]
        [TestCase("ItemName")]
        [TestCase("ItemName;ItemId;W1,")]
        [TestCase("ItemName;ItemId;||")]
        [TestCase("ItemName;ItemId;,|,|,")]
        public void Parse_When_InputIsIncorrect_Returns_Null(string validInput)
        {
            // Arrange
            IParser parser = new RegexParser();

            // Act
            var result = parser.Parse(validInput);

            // Assert
            Assert.IsNull(result, "Result is not null");
        }
    }
}