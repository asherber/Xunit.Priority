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
using Xunit.v3;
using Xunit.Sdk;
using System.Reflection;

namespace Xunit.v3.Priority
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        private static ConcurrentDictionary<string, int> _defaultPriorities = new ConcurrentDictionary<string, int>();

        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases) where TTestCase : ITestCase
        {
            var groupedTestCases = new Dictionary<int, List<ITestCase>>();
            var defaultPriorities = new Dictionary<Type, int>();

            foreach (IXunitTestCase testCase in testCases)
            {
                var defaultPriority = DefaultPriorityForClass(testCase);
                var priority = PriorityForTest(testCase, defaultPriority);
                
                if (!groupedTestCases.ContainsKey(priority))
                    groupedTestCases[priority] = new List<ITestCase>();

                groupedTestCases[priority].Add(testCase);
            }

            var result = new List<TTestCase>();

            var orderedKeys = groupedTestCases.Keys.OrderBy(k => k);            
            foreach (var list in orderedKeys.Select(priority => groupedTestCases[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.MethodName, y.TestMethod.MethodName));
                result.AddRange(list.Cast<TTestCase>());                
            }

            return result;
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
                var defaultAttribute = testClass.GetCustomAttributes<DefaultPriorityAttribute>().SingleOrDefault();
                result = defaultAttribute?.Priority ?? int.MaxValue;
                _defaultPriorities[testClass.Name] = result;
            }

            return result;
        }
    }
}
