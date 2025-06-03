namespace Xunit.Priority.v3.Tests
{
    [DefaultPriority(0)]
    public class PriorityWithZeroDefault: TestsBase
    {
#pragma warning disable 169
        private static bool[] _counter;
#pragma warning restore 169


        [Fact, Priority(50)]
        public void Test3A() => VerifyAndFlip(5);        
        
        [Fact, Priority(-10)]
        public void Test1() => VerifyAndFlip(1);
        
        [Fact]
        public void Test5() => VerifyAndFlip(3);
        
        [Fact, Priority(0)]
        public void Test2() => VerifyAndFlip(2);
        
        [Fact, Priority(-40)]
        public void Test4() => VerifyAndFlip(0);
        
        [Fact, Priority(10)]
        public void Test3B() => VerifyAndFlip(4);        
    }
}
