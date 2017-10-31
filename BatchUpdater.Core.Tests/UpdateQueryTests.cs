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
                StringValue = "randomValue1",
                DecimalValue = 12.5M,
                CreatedOn = DateTime.UtcNow.AddMinutes(-44),
                TestEnumValue = TestEntity.TestEnum.Value2
            };

        private QueryBuilderFactory factory 
            => 
            new QueryBuilderFactory()
            .WithConfig(new QueryBuilderConfig()
                .WithDialect(new PgDialect())
                .RegisterType<TestEntity>("TestEntity", "dbo"));

        [Fact]
        public void StringSettedValueMustBeQuate()
        {
            var entity = TestEntity;

            var query = factory.Create<TestEntity>()
                .Set(x => x.StringValue, entity.StringValue)
                .Where(x => x.Id == entity.Id)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.StringValue)}\" = '{entity.StringValue}' WHERE (\"Id\" = '{entity.Id}');", 
                query);
        }

        [Fact]
        public void NullStringSettedValueMustBeNull()
        {
            var entity = TestEntity;

            entity.StringValue = null;

            var query = factory.Create<TestEntity>()
                .Set(x => x.StringValue, entity.StringValue)
                .Where(x => x.Id == entity.Id)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.StringValue)}\" = NULL WHERE (\"Id\" = '{entity.Id}');",
                query);
        }

        [Fact]
        public void EnumSettedValueMustBeInteger()
        {
            var entity = TestEntity;

            entity.StringValue = null;

            var query = factory.Create<TestEntity>()
                .Set(x => x.TestEnumValue, entity.TestEnumValue)
                .Where(x => x.Id == entity.Id)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.TestEnumValue)}\" = {(int)entity.TestEnumValue} WHERE (\"Id\" = '{entity.Id}');",
                query);
        }

        [Fact]
        public void DecimalSettedValueMustNotBeQuate()
        {
            var entity = TestEntity;

            var query = factory.Create<TestEntity>()
                .Set(x => x.DecimalValue, entity.DecimalValue)
                .Where(x => x.Id == entity.Id)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.DecimalValue)}\" = {entity.DecimalValue} WHERE (\"Id\" = '{entity.Id}');",
                query);
        }

        [Fact]
        public void TestUpdateQueryWithTwoProperty()
        {
            var entity = TestEntity;

            var query = factory.Create<TestEntity>()
                .Set(x => x.StringValue, entity.StringValue)
                .Set(x => x.DecimalValue, entity.DecimalValue)
                .Where(x => x.Id == entity.Id)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.StringValue)}\" = '{entity.StringValue}', \"{nameof(entity.DecimalValue)}\" = {entity.DecimalValue} WHERE (\"Id\" = '{entity.Id}');",
                query);
        }


        [Fact]
        public void TestUpdateQueryWithOneStringPropertyWithReversePredicate()
        {
            var entity = TestEntity;

            var query = factory.Create<TestEntity>()
                .Set(x => x.StringValue, entity.StringValue)
                .Where(x => entity.Id == x.Id)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.StringValue)}\" = '{entity.StringValue}' WHERE ('{entity.Id}' = \"Id\");",
                query);
        }

        [Fact]
        public void TestUpdateQueryWithPredicateGreater()
        {
            var entity = TestEntity;
            var now = DateTime.UtcNow;

            var query = factory.Create<TestEntity>()
                .Set(x => x.StringValue, entity.StringValue)
                .Where(x => x.CreatedOn > now)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.StringValue)}\" = '{entity.StringValue}' WHERE (\"CreatedOn\" > '{now:yyyy-MM-ddTHH:mm:ss.ffffffZ}');",
                query);
        }

        [Fact]
        public void TestUpdateQueryWithPredicateEqualAndGreater()
        {
            var entity = TestEntity;
            var now = DateTime.UtcNow;

            var query = factory.Create<TestEntity>()
                .Set(x => x.StringValue, entity.StringValue)
                .Where(x => x.Id == entity.Id && x.CreatedOn > now)
                .GetUpdateQuery();

            Assert.Equal(
                $"UPDATE dbo.\"TestEntity\" SET \"{nameof(entity.StringValue)}\" = '{entity.StringValue}' WHERE ((\"Id\" = '{entity.Id}') AND (\"CreatedOn\" > '{now:yyyy-MM-ddTHH:mm:ss.ffffffZ}'));",
                query);
        }
    }
}
