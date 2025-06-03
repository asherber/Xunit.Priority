namespace Xunit.Priority.v3.Tests
{
    public class NoPriorityTests: TestsBase
    {
#pragma warning disable 169
        private static bool[] _counter;
#pragma warning restore 169


        [Fact]
        public void Test3A() => VerifyAndFlip(2);        
        
        [Fact]
        public void Test1() => VerifyAndFlip(0);
        
        [Fact]
        public void Test5() => VerifyAndFlip(5);
        
        [Fact]
        public void Test2() => VerifyAndFlip(1);
        
        [Fact]
        public void Test4() => VerifyAndFlip(4);
        
        [Fact]
        public void Test3B() => VerifyAndFlip(3);        
    }
}
