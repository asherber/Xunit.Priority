using System;

namespace Xunit.Priority
{
    /// <summary>
    /// Indicates relative priority of tests for execution. Tests with the same
    /// priority are run in alphabetical order. Tests with no priority are run last.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PriorityAttribute : Attribute
    {
        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}
