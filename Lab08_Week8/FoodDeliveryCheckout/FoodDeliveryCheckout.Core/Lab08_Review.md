# Lab 8 – Review of Copilot-Generated Output

## Overview
This document reviews the GitHub Copilot–assisted implementation and test
generation for the Online Food Delivery Checkout System, as required by
the Lab 8 brief.

## Review Questions

**1. Did Copilot generate tests that match the requirements?**
Yes. The generated tests covered subtotal calculation, delivery fee by
zone, discount by customer type, service fee, GST, and the minimum order
boundary — matching every business rule listed in the brief.

**2. Did Copilot invent any requirements?**
No extra business rules were invented. However, the initial test code
relied on `Assert.ThrowsException<T>()`, an MSTest API that has been
removed in MSTest 4.x. This wasn't an invented requirement, but it shows
Copilot generating code against an outdated version of the testing
framework, which had to be corrected to `Assert.ThrowsExactly<T>()`
after updating the MSTest NuGet package to 4.4.0.

**3. Are the expected values correct?**
Yes, but not by default trust — the three end-to-end totals (34.78, 73.59,
50.13) were manually recalculated step-by-step (subtotal → discount →
service fee → delivery fee → GST → rounding) before being accepted into
the test assertions, confirming Copilot's suggested figures were accurate.

**4. Are the tests using Arrange–Act–Assert?**
Yes. Every test method follows the Arrange–Act–Assert structure with
clear comments delineating each section.

**5. Are the test names clear?**
Yes. Names follow a `Method_Scenario_ExpectedResult` convention, e.g.
`CalculateTotal_SubtotalBelowMinimum_ThrowsInvalidOperationException`,
making intent clear without reading the method body.

**6. Are boundary values tested?**
Yes. The $10 minimum order boundary is explicitly tested
(`CalculateTotal_SubtotalBelowMinimum_ThrowsInvalidOperationException`),
along with zero and negative values for unit price, quantity, and
subtotal.

**7. Are invalid inputs tested?**
Yes. Null/empty/whitespace item names, null and empty item lists, and
negative subtotals are all covered with dedicated tests and
`[DataRow]`-driven cases.

**8. Are the assertions strong enough?**
Mostly yes — `Assert.AreEqual` is used with precise decimal expected
values (not just checking non-null or a rough range), and
`Assert.ThrowsExactly<T>()` verifies the exact exception type rather than
just "an exception occurred." One improvement made during review was
ensuring `decimal` was used throughout for money values (as Copilot
suggested) rather than `double`, avoiding floating-point rounding errors.

**9. Do any tests depend on execution order?**
No. Each test creates its own `CheckoutCalculator` and its own list of
`OrderItem`s in the Arrange step, so tests are fully independent and can
run in any order or in parallel.

## Key Learning
The most significant issue encountered was a breaking change between
MSTest versions: `Assert.ThrowsException` (used in the lab brief's sample
code) was removed entirely in MSTest 4.x in favour of
`Assert.ThrowsExactly`. This reinforced the lab's core lesson — Copilot
(and even official lab material) should be reviewed and verified against
the actual installed tooling version, not accepted blindly.

## Test Summary
- Total tests: 28
- All tests passing
- Test files: `CheckoutCalculatorSubtotalTests.cs`,
  `CheckoutCalculatorDiscountAndFeeTests.cs`,
  `CheckoutCalculatorTotalTests.cs`