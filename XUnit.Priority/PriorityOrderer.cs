using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Priority
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        private static string _priorityAttribute = typeof(PriorityAttribute).AssemblyQualifiedName;
        private static string _priority = nameof(PriorityAttribute.Priority);

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

            foreach (TTestCase testCase in testCases)
            {
                var attr = testCase.TestMethod.Method.GetCustomAttributes(_priorityAttribute).SingleOrDefault();
                int priority = attr?.GetNamedArgument<int>(_priority) ?? int.MaxValue;
                
                if (!sortedMethods.ContainsKey(priority))
                    sortedMethods[priority] = new List<TTestCase>();

                sortedMethods[priority].Add(testCase);
            }

            foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                foreach (TTestCase testCase in list)
                    yield return testCase;
            }
        }
    }
}
