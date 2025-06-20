![Icon](https://raw.githubusercontent.com/asherber/Xunit.Priority/main/media/xunit-priority-64.png)

# Xunit.Priority [![Build status](https://github.com/asherber/Xunit.Priority/actions/workflows/CI.yml/badge.svg)](https://github.com/asherber/Xunit.Priority/actions)

| Package                | Supported xUnit Version | NuGet                                                                                  |
|------------------------|------------------------|----------------------------------------------------------------------------------------|
| Xunit.Priority         | xunit v2               | [![NuGet](https://img.shields.io/nuget/v/Xunit.Priority.svg)](https://nuget.org/packages/Xunit.Priority)         |
| Xunit.v3.Priority   | xunit v3               | [![NuGet](https://img.shields.io/nuget/v/Xunit.v3.Priority.svg)](https://nuget.org/packages/Xunit.v3.Priority)   |

Provides an `ITestCaseOrderer` that allows you to control the order of execution of Xunit tests within a class.

Based closely on the code at https://github.com/xunit/samples.xunit/tree/main/TestOrderExamples/TestCaseOrdering

**Note** that the Xunit folks [have](https://github.com/xunit/xunit/issues/980#issuecomment-248213473) [stated](https://github.com/xunit/xunit/issues/1301#issuecomment-305323239) that they don't believe that well-written unit tests should be dependent on being run in a particular order, which is why this functionality is not available as part of the core package. Nevertheless, there are some testing scenarios which are not strict unit testing and which may require test ordering.

## Usage

Add the following attribute to classes for which you want tests run in order:

```csharp
// for Xunit.Priority
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]

// for Xunit.v3.Priority
[TestCaseOrderer(typeof(PriorityOrderer))]
```

Then decorate your test methods with the `Priority` attribute.

```csharp
[Fact, Priority(-10)]
public void FirstTestToRun() { }

[Fact, Priority(0)]
public void SecondTestToRun() { }

[Fact, Priority(10)]
public void ThirdTestToRunA() { }

[Fact, Priority(10)]
public void ThirdTestToRunB() { }

[Fact]
public void TestsWithNoPriorityRunLast() { }
```

Priorities are evaluated in numeric order (including 0 and negative numbers). If there are multiple tests with the same priority, those tests will be run in alphabetical order.

By default, tests with no explicit `Priority` attribute are assigned priority `int.MaxValue` and will be run last. You can change this by setting a `DefaultPriority` attribute on your test class.

```csharp
[DefaultPriority(0)]
public class MyTests
{
    [Fact]
    public void SomeTest() { }

    [Fact]
    public void SomeOtherTest() { }

    [Fact, Priority(10)]
    public void RunMeLast() { }
}
```
