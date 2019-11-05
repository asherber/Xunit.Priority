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

namespace Xunit.Priority
{
    /// <summary>
    /// <para>Indicates relative priority of tests for execution. Tests with the same
    /// priority are run in alphabetical order. </para>
    /// 
    /// <para>Tests with no priority attribute
    /// are assigned a priority of <see cref="int.MaxValue"/>,
    /// unless the class has a <see cref="DefaultPriorityAttribute"/>.</para>
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
