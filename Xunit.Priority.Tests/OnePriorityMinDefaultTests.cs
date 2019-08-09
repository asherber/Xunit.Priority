using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;
using FluentAssertions;

namespace Xunit.Priority.Tests
{
    [DefaultPriority(int.MinValue)]
    public class OnePriorityMinDefaultTests: TestsBase
    {
#pragma warning disable 169
        private static bool[] _counter;
#pragma warning restore 169


        [Fact]
        public void Test3A() => VerifyAndFlip(2);        
        
        [Fact]
        public void Test1() => VerifyAndFlip(0);
        
        [Fact]
        public void Test5() => VerifyAndFlip(4);
        
        [Fact]
        public void Test2() => VerifyAndFlip(1);
        
        [Fact]
        public void Test4() => VerifyAndFlip(3);
        
        [Fact, Priority(10)]
        public void Test3B() => VerifyAndFlip(5);        
    }
}
