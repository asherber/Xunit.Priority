# XUnit.Priority

Originally from https://github.com/xunit/samples.xunit/tree/master/TestOrderExamples/TestCaseOrdering

- Changed `TestPriorityAttribute` to `PriorityAttribute`
- Refactored `PriorityOrderer` 
- Don't need separate `AlphabeticalOrderer`, because `PriorityOrderer` already does that as well

