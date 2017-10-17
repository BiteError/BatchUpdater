using System;
using Xunit;

namespace BatchUpdater.Core.Tests
{
    public class UpdateQueryTests
    {
        [Fact]
        public void TestUpdateQuery()
        {
            var entity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Value1 = "randomValue1",
                Value2 = "randomValue2"
            };

            var query = entity.Update(x => x.Id)
                .WithTableName("TestEntity")
                .WithProperty(x => x.Value1)
                .GetQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"Value1\" = 'randomValue1' WHERE \"Id\" = '{entity.Id}';", 
                query);
        }
    }
}
