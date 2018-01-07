﻿using System;
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
            var dict = new Dictionary<int, List<TTestCase>>();

            foreach (TTestCase testCase in testCases)
            {
                var attr = testCase.TestMethod.Method.GetCustomAttributes(_priorityAttribute).SingleOrDefault();
                int priority = attr?.GetNamedArgument<int>(_priority) ?? int.MaxValue;
                
                if (!dict.ContainsKey(priority))
                    dict[priority] = new List<TTestCase>();

                dict[priority].Add(testCase);
            }

            var orderedKeys = dict.Keys.OrderBy(k => k);
            foreach (var list in orderedKeys.Select(priority => dict[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                foreach (TTestCase testCase in list)
                    yield return testCase;
            }
        }
    }
}