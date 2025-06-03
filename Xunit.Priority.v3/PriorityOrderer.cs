/**
 * Copyright 2018-2019 Aaron Sherber
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 *  limitations under the License.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;
using Xunit.v3;

namespace Xunit.Priority.v3
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        private ITestCaseOrderer _testCaseOrdererImplementation;
        public const string Name = "Xunit.Priority.v3.PriorityOrderer";
        public const string Assembly = "Xunit.Priority.v3";

        private static ConcurrentDictionary<string, int> _defaultPriorities = new ConcurrentDictionary<string, int>();

        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases) where TTestCase : ITestCase
        {
            var groupedTestCases = new Dictionary<int, List<IXunitTestCase>>();
            var defaultPriorities = new Dictionary<Type, int>();
            var orderedTestCases = new List<TTestCase>();

            foreach (IXunitTestCase testCase in testCases)
            {
                var defaultPriority = DefaultPriorityForClass(testCase);
                var priority = PriorityForTest(testCase, defaultPriority);

                if (!groupedTestCases.ContainsKey(priority))
                    groupedTestCases[priority] = new List<IXunitTestCase>();

                groupedTestCases[priority].Add(testCase);
            }

            var orderedKeys = groupedTestCases.Keys.OrderBy(k => k);
            foreach (var list in orderedKeys.Select(priority => groupedTestCases[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                orderedTestCases.AddRange(list.Cast<TTestCase>());
            }

            return orderedTestCases;
        }

        private int PriorityForTest(IXunitTestCase testCase, int defaultPriority)
        {
            var priorityAttribute = testCase.TestMethod.Method
                .GetCustomAttributes<PriorityAttribute>()
                .SingleOrDefault();
            return priorityAttribute?.Priority ?? defaultPriority;
        }

        private int DefaultPriorityForClass(IXunitTestCase testCase)
        {
            var testClass = testCase.TestMethod.TestClass.Class;
            if (!_defaultPriorities.TryGetValue(testClass.Name, out var result))
            {
                // Check for DefaultPriority on the class
                var defaultAttribute = testClass.GetCustomAttributes<DefaultPriorityAttribute>().SingleOrDefault();
                result = defaultAttribute?.Priority ?? int.MaxValue;
                _defaultPriorities[testClass.Name] = result;
            }

            return result;
        }
    }
}
