using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Xunit.Priority.Tests
{

    [TestCaseOrderer("Xunit.Priority.PriorityOrderer", "xunit.priority")]
    public class PriorityOrderExamples
    {
        public static bool Test1Called;
        public static bool Test2Called;
        public static bool Test3ACalled;
        public static bool Test3BCalled;

        [Fact, Priority(0)]
        public void Test2()
        {
            Test2Called = true;

            Assert.True(Test1Called);
            Assert.False(Test3ACalled);
            Assert.False(Test3BCalled);
        }

        [Fact]
        public void Test3B()
        {
            Test3BCalled = true;

            Assert.True(Test1Called);
            Assert.True(Test2Called);
            Assert.True(Test3ACalled);
        }

        [Fact]
        public void Test3A()
        {
            Test3ACalled = true;

            Assert.True(Test1Called);
            Assert.True(Test2Called);
            Assert.False(Test3BCalled);
        }

        [Fact]
        [Priority(-5)]
        public void Test1()
        {
            Test1Called = true;

            Assert.False(Test3ACalled);
            Assert.False(Test3BCalled);
            Assert.False(Test2Called);
        }

    }

}
