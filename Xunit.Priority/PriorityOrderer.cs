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
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Priority
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public const string Name = "Xunit.Priority.PriorityOrderer";
        public const string Assembly = "Xunit.Priority";

        private static string _priorityAttributeName = typeof(PriorityAttribute).AssemblyQualifiedName;
        private static string _defaultPriorityAttributeName = typeof(DefaultPriorityAttribute).AssemblyQualifiedName;
        private static string _priorityArgumentName = nameof(PriorityAttribute.Priority);

        private static ConcurrentDictionary<string, int> _defaultPriorities = new ConcurrentDictionary<string, int>();

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var groupedTestCases = new Dictionary<int, List<ITestCase>>();
            var defaultPriorities = new Dictionary<Type, int>();

            foreach (var testCase in testCases)
            {
                var defaultPriority = DefaultPriorityForClass(testCase);
                var priority = PriorityForTest(testCase, defaultPriority);
                
                if (!groupedTestCases.ContainsKey(priority))
                    groupedTestCases[priority] = new List<ITestCase>();

                groupedTestCases[priority].Add(testCase);
            }

            var orderedKeys = groupedTestCases.Keys.OrderBy(k => k);            
            foreach (var list in orderedKeys.Select(priority => groupedTestCases[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                foreach (TTestCase testCase in list)
                    yield return testCase;
            }
        }

        private int PriorityForTest(ITestCase testCase, int defaultPriority) 
        {
            var priorityAttribute = testCase.TestMethod.Method.GetCustomAttributes(_priorityAttributeName).SingleOrDefault();
            return priorityAttribute?.GetNamedArgument<int>(_priorityArgumentName) ?? defaultPriority;
        }

        private int DefaultPriorityForClass(ITestCase testCase)
        {
            var testClass = testCase.TestMethod.TestClass.Class;
            if (!_defaultPriorities.TryGetValue(testClass.Name, out var result))
            {
                var defaultAttribute = testClass.GetCustomAttributes(_defaultPriorityAttributeName).SingleOrDefault();
                result = defaultAttribute?.GetNamedArgument<int>(_priorityArgumentName) ?? int.MaxValue;
                _defaultPriorities[testClass.Name] = result;
            }

            return result;
        }
    }
}
