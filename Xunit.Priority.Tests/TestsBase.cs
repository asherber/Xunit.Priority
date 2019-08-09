using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Xunit.Priority.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public abstract class TestsBase
    {
        private FieldInfo _counterField;

        public TestsBase()
        {
            _counterField = this.GetType().GetField("_counter", BindingFlags.Static | BindingFlags.NonPublic);

            if (GetCounter() == null)
            {
                var facts = this.GetType().GetMethods()
                    .Where(m => m.GetCustomAttribute<FactAttribute>() != null);
                SetCounter(new bool[facts.Count()]);

            }
        }

        private bool[] GetCounter() => _counterField.GetValue(this) as bool[];

        private void SetCounter(bool[] value) => _counterField.SetValue(this, value);

        protected void VerifyAndFlip(int testNumber)
        {
            var counter = GetCounter();
            counter.Take(testNumber).Should().AllBeEquivalentTo(true);
            counter.Skip(testNumber).Should().AllBeEquivalentTo(false);

            counter[testNumber] = true;
            SetCounter(counter);
        }
    }
}
