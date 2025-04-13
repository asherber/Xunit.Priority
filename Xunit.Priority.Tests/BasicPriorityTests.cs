using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

#if V3
using Xunit.v3.Priority;

namespace Xunit.v3.Priority.Tests
#else
using Xunit.Priority;

namespace Xunit.Priority.Tests
#endif
{
    public class BasicPriorityTests: TestsBase
    {
#pragma warning disable 169
        private static bool[] _counter;
#pragma warning restore 169


        [Fact, Priority(10)]
        public void Test3A() => VerifyAndFlip(2);        
        
        [Fact, Priority(-10)]
        public void Test1() => VerifyAndFlip(0);
        
        [Fact]
        public void Test5() => VerifyAndFlip(5);
        
        [Fact, Priority(0)]
        public void Test2() => VerifyAndFlip(1);
        
        [Fact, Priority(20)]
        public void Test4() => VerifyAndFlip(4);
        
        [Fact, Priority(10)]
        public void Test3B() => VerifyAndFlip(3);        
    }
}
