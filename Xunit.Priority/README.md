# Xunit.Priority

Originally from https://github.com/xunit/samples.xunit/tree/master/TestOrderExamples/TestCaseOrdering

- Changed `TestPriorityAttribute` to `PriorityAttribute`
- Refactored `PriorityOrderer` 
- Don't need separate `AlphabeticalOrderer`, because `PriorityOrderer` already does that as well



## Usage

Add the following attribute to classes where you want tests run in order:

```csharp
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
```

