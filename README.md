# Xunit.Priority

Provides an `ITestCaseOrderer` that allows you to control the order of execution of Xunit tests within a class.

Based closely on the code at https://github.com/xunit/samples.xunit/tree/master/TestOrderExamples/TestCaseOrdering



## Usage

Add the following attribute to classes for which you want tests run in order:

```csharp
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
```

Then decorate your test methods with the `Priority` attribute.

```csharp
[Fact, Priority(10)]
public void FirstTestToRun()
{  
}

[Fact, Priority(20)]
public void SecondTestToRun()
{
}
```

- Priorities are evaluated in numeric order (including negative numbers)
- Multiple tests with the same priority will be run in alphabetical order
- Tests with no explicit `Priority` attribute are assigned priority `int.MaxValue` and will be run last

