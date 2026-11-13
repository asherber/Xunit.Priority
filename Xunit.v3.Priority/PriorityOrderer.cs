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
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace Xunit.v3.Priority
{
    public class PriorityOrderer : ITestMethodOrderer
    {
        public IReadOnlyCollection<TTestMethod> OrderTestMethods<TTestMethod>(IReadOnlyCollection<TTestMethod?> testMethods) where TTestMethod : ITestMethod
        {
            var methodsByPriority = testMethods.OfType<TTestMethod>().GroupBy(Priority).ToDictionary(g => g.Key);
            return
            [
                .. methodsByPriority.Keys.OrderBy(k => k)
                    .Select(priority => methodsByPriority[priority])
                    .SelectMany(methods => methods.OrderBy(m => m.MethodName, StringComparer.OrdinalIgnoreCase)),
            ];
        }

        private static int Priority<TTestMethod>(TTestMethod testMethod) where TTestMethod : ITestMethod
        {
            if (testMethod.Traits.TryGetValue(PriorityAttribute.TraitKey, out var priority) && priority.Count > 0)
            {
                // Traits are merged in class -> method order, see https://github.com/xunit/xunit/blob/v3-4.0.0/src/xunit.v3.core/Utility/ExtensibilityPointFactory.TestMethod_reflection.cs#L94-L104
                // So if both [DefaultPriority] (class only) and [Priority] (method only) are defined, the value of [Priority] will be last
                return int.Parse(priority.Last());
            }
            return int.MaxValue;
        }
    }
}
