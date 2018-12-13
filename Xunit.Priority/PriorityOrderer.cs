/**
 * Copyright 2018 Aaron Sherber
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
