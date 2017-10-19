using System;
using Xunit;

namespace BatchUpdater.Core.Tests
{
    public class UpdateQueryTests
    {
        private static TestEntity TestEntity =>
            new TestEntity
            {
                Id = Guid.NewGuid(),
                Value1 = "randomValue1",
                Value2 = 12.5M
            };

        [Fact]
        public void TestUpdateQueryWithOneProperty()
        {
            var entity = TestEntity;

            var query = entity.Update(x => x.Id)
                .WithTableName("TestEntity")
                .WithProperty(x => x.Value1)
                .GetQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"Value1\" = 'randomValue1' WHERE \"Id\" = '{entity.Id}';", 
                query);
        }

        [Fact]
        public void TestUpdateQueryWithOneTwoProperty()
        {
            var entity = TestEntity;

            var query = entity.Update(x => x.Id)
                .WithTableName("TestEntity")
                .WithProperty(x => x.Value1)
                .WithProperty(x => x.Value2)
                .GetQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"Value1\" = 'randomValue1', \"Value2\" = 12.5 WHERE \"Id\" = '{entity.Id}';",
                query);
        }
    }
}
