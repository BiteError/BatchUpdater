using System;

namespace BatchUpdater.Core.Tests
{
    public class TestEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string StringValue { get; set; } 
        public decimal DecimalValue { get; set; }
        public TestEnum TestEnumValue { get; set; }
        public enum TestEnum { Value1 = 1, Value2 }
    }
}