using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;
using Shouldly;

namespace Xunit.Priority.Tests
{

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class PriorityOrdererTests
    {
        public static bool Test1Called;
        public static bool Test2Called;
        public static bool Test3ACalled;
        public static bool Test3BCalled;
        public static bool Test4Called;
        public static bool Test5Called;

        [Fact, Priority(10)]
        public void Test3A()
        {
            Test3ACalled = true;

            Test1Called.ShouldBe(true);
            Test2Called.ShouldBe(true);
            Test3BCalled.ShouldBe(false);
            Test4Called.ShouldBe(false);
            Test5Called.ShouldBe(false);
        }
        
        [Fact, Priority(-10)]
        public void Test1()
        {
            Test1Called = true;

            Test2Called.ShouldBe(false);
            Test3ACalled.ShouldBe(false);
            Test3BCalled.ShouldBe(false);
            Test4Called.ShouldBe(false);
            Test5Called.ShouldBe(false);
        }

        [Fact]
        public void Test5()
        {
            Test5Called = true;

            Test1Called.ShouldBe(true);
            Test2Called.ShouldBe(true);
            Test3ACalled.ShouldBe(true);
            Test3BCalled.ShouldBe(true);
            Test4Called.ShouldBe(true);
        }

        [Fact, Priority(0)]
        public void Test2()
        {
            Test2Called = true;

            Test1Called.ShouldBe(true);
            Test3ACalled.ShouldBe(false);
            Test3BCalled.ShouldBe(false);
            Test4Called.ShouldBe(false);
            Test5Called.ShouldBe(false);
        }

        [Fact, Priority(20)]
        public void Test4()
        {
            Test4Called = true;

            Test1Called.ShouldBe(true);
            Test2Called.ShouldBe(true);
            Test3ACalled.ShouldBe(true);
            Test3BCalled.ShouldBe(true);
            Test5Called.ShouldBe(false);
        }

        [Fact, Priority(10)]
        public void Test3B()
        {
            Test3BCalled = true;

            Test1Called.ShouldBe(true);
            Test2Called.ShouldBe(true);
            Test3ACalled.ShouldBe(true);
            Test4Called.ShouldBe(false);
            Test5Called.ShouldBe(false);
        }
    }
}
