# Pipeline Quality Notes — Week09QualityLab

## Task 1: CI Pipeline for This Lab Project

When a developer pushes code to GitHub, the CI pipeline should automatically run the following sequence:

1. **Checkout the code** — the pipeline pulls the latest pushed commit from the repository.
2. **Restore dependencies** — NuGet packages required by the solution (e.g. MSTest framework, .NET SDK dependencies) are restored so the project can build.
3. **Build the solution** — both `Week09QualityLab.Core` and `Week09QualityLab.Tests` are compiled. If the build fails (e.g. a syntax error or missing reference), the pipeline stops here and reports failure.
4. **Run MSTest tests** — all unit tests in `Week09QualityLab.Tests` are executed automatically, without any manual step.
5. **Collect test results** — the number of tests passed, failed, and skipped is recorded, along with details of any failures (which test, what assertion failed, expected vs actual value).
6. **Report pipeline status** — the pipeline reports an overall pass or fail status back to GitHub, usually visible as a check mark or cross next to the commit/pull request.

**Why failed tests should block merge/release:**
If a test fails, it means the code no longer behaves as expected for at least one requirement (e.g. a VIP customer no longer gets a 20% discount, or a negative price no longer throws an exception). Allowing a merge despite failing tests would let broken or incorrect business logic reach the main branch — and potentially production — without anyone catching it. Blocking the merge forces the developer to fix the issue before it can affect other team members or end users, which is the core purpose of automated testing in a CI/CD pipeline.
## Task 2: Pipeline Stages

1. **Source Code Checkout**
   Pulls the latest code from the GitHub repository. Reduces the risk of testing or building outdated or wrong code.

2. **Dependency Restore**
   Downloads required NuGet packages (e.g. MSTest framework). Reduces the risk of build failures caused by missing or mismatched package versions.

3. **Build Verification**
   Compiles `Week09QualityLab.Core` and `Week09QualityLab.Tests`. Catches syntax errors, type mismatches, and broken references before any tests even run.

4. **Unit Test Execution**
   Runs all MSTest tests (Discount Calculator and Shipping Calculator suites). Catches logic errors and regressions — e.g. a discount rate calculated incorrectly, or an exception no longer being thrown for invalid input.

5. **Test Result Reporting**
   Summarises pass/fail counts and failure details. Gives the team fast, clear visibility into what broke and why, instead of digging through raw logs.

6. **Code Coverage Measurement**
   Measures how much of `DiscountCalculator` and `ShippingCalculator` is exercised by tests. Reduces the risk of untested code paths (e.g. an edge case) silently containing bugs.

7. **Static Code Analysis**
   Scans the code for style violations, code smells, and potential bugs without running it. Reduces the risk of maintainability issues and common programming mistakes slipping through.

8. **Dependency Vulnerability Scanning**
   Checks NuGet packages against known vulnerability databases. Reduces the risk of shipping software with insecure third-party dependencies.

9. **Artefact Creation**
   Packages the built solution (e.g. compiled DLLs) into a deployable artefact. Ensures a consistent, versioned build output is available for release, rather than relying on a developer's local build.

10. **Optional Deployment to a Test Environment**
    Deploys the build to a staging/test environment for further manual or automated verification. Reduces the risk of issues that only appear in a realistic, integrated environment (as opposed to Test Explorer running the tests in isolation).
    ## Task 3: Quality Gates

1. **The solution must build successfully.**
   Useful because a broken build means the code can't even run — merging it would break the pipeline for everyone else immediately.

2. **All MSTest unit tests must pass.**
   Useful because it directly confirms that the business rules (e.g. discount percentages, shipping costs, negative-price rejection) still behave as required after the change.

3. **Test failures must block merge.**
   Useful because it removes the possibility of a human accidentally (or knowingly) merging broken logic under time pressure — the pipeline enforces the standard automatically.

4. **Code coverage should meet a selected threshold (e.g. 80%).**
   Useful because it gives confidence that most of the code — not just the "happy path" — is actually being exercised by tests, reducing the chance of untested logic hiding bugs.

5. **No critical dependency vulnerabilities should exist.**
   Useful because a vulnerable NuGet package could expose the application (or its users' data) to known security exploits, even if the project's own code is fine.

6. **No secrets should be detected in the repository.**
   Useful because accidentally committed secrets (API keys, connection strings, passwords) can be exploited by anyone with repository access, including in public repos or if access is later leaked.

7. **Static analysis should not report blocker-level issues.**
   Useful because it catches serious code-quality problems (e.g. unreachable code, likely null-reference exceptions) that unit tests might not directly catch, especially in code paths not yet covered by tests.
   ## Task 4: Regression Testing in the Pipeline

The tests written in this lab (for `DiscountCalculator` and `ShippingCalculator`) act as regression tests because they lock in the expected behaviour for every requirement — once written, they run automatically on every future change, not just when they were first created.

**How future changes could accidentally break existing behaviour:**
- A developer updating `DiscountCalculator` to add a new customer tier (e.g. "Platinum") might accidentally change the `switch` expression in a way that alters the existing Premium or VIP discount rates.
- A developer refactoring `ShippingCalculator` to add a new shipping type (e.g. "Overnight") might loosen the guard clause that currently rejects unrecognised shipping types, silently allowing invalid input through instead of throwing an exception.
- A change to the negative-price guard clause in `DiscountCalculator` (e.g. changing `< 0` to `<= 0`) would break the assumption that a price of exactly 0 is valid, without anyone necessarily noticing by just reading the code.

**How the pipeline detects this early:**
Because the existing tests (e.g. `CalculateFinalPrice_ShouldApplyTenPercentDiscount_ForPremiumCustomer`, `CalculateShippingCost_ShouldThrowException_WhenShippingTypeIsInvalid`) run automatically on every push, any of the above changes would cause one or more of these tests to fail immediately during the pipeline's test execution stage — long before the change reaches production. This means the developer gets fast feedback on the exact requirement that broke, rather than the bug being discovered later by a user or in manual testing.
## Task 5: Possible Pipeline Failures

1. **Build failure**
   e.g. a syntax error or a missing project reference (like the `CS0234` error encountered in this lab when the Core reference wasn't set up correctly).
   *Action:* The developer should not push further changes until the build is fixed locally. The pipeline should block the merge, and the developer investigates the compiler error message to fix the broken code.

2. **Failing unit test**
   e.g. `CalculateFinalPrice_ShouldApplyTwentyPercentDiscount_ForVipCustomer` fails because someone changed the VIP discount rate incorrectly.
   *Action:* The developer reviews the failure details (expected vs actual value), determines whether the test or the code is wrong, and fixes whichever is incorrect before re-pushing.

3. **Low code coverage**
   e.g. coverage drops below the agreed threshold because a new method was added without tests.
   *Action:* The developer writes additional tests to cover the missing code paths before the change is allowed to merge.

4. **Detected secret**
   e.g. an API key or connection string accidentally committed to the repository.
   *Action:* The secret should be removed from the code immediately, rotated/invalidated (since it may already be compromised once pushed), and the commit history cleaned if necessary. The merge should be blocked until this is resolved.

5. **Vulnerable NuGet package**
   e.g. a dependency has a known critical security vulnerability.
   *Action:* The developer updates the package to a patched version, or finds an alternative package, before the pipeline is allowed to pass.

6. **Static analysis warning/blocker**
   e.g. a likely null-reference exception is flagged in new code.
   *Action:* The developer reviews and fixes the flagged issue, or — if it is a false positive — documents why it is safe and suppresses it explicitly rather than ignoring it silently.

7. **Unstable (flaky) test result**
   e.g. a test passes locally but fails intermittently in the pipeline.
   *Action:* The team should not simply re-run the pipeline until it passes. The test should be investigated to find the root cause (e.g. timing issues, shared state between tests) and fixed to be reliable, since an unreliable test undermines trust in the whole pipeline.
   ## Task 6: Test Data and Test Reliability

**Why test data should be repeatable, isolated, and safe:**
- **Repeatable** — the same input should always produce the same result, so a test either consistently passes or consistently fails for a genuine reason, not because of randomness or changing external state.
- **Isolated** — a test's data and setup should not depend on another test having run first (or in a particular order), and should not leak state that affects other tests.
- **Safe** — test data should never be real, sensitive data (e.g. real customer prices, real personal information), since tests may run in shared or less-secure CI environments.

**Example of poor test data from this lab's problem:**
In `ShippingCalculator`, if a test used `orderAmount = 100m` to check the "free shipping over $100" rule, this is poor test data — it sits exactly on the boundary, and the actual rule uses `> 100m` (strictly greater than), not `>= 100m`. Using exactly `100m` doesn't clearly prove whether the boundary is implemented correctly; a value like `100.01m` (just over) and `100m` (exactly at, expecting normal cost) would both be needed to properly test the boundary. Using only one ambiguous value could make the test pass even if the boundary condition is subtly wrong (e.g. if `>` was mistakenly written as `>=`).

**What a flaky test is:**
A flaky test is one that passes or fails inconsistently without any actual change to the code being tested — for example, a test that sometimes passes and sometimes fails depending on timing, execution order, or shared/leftover state from another test.

**Why flaky tests reduce trust in CI/CD pipelines:**
When a test fails intermittently, developers start assuming failures are "just flaky" rather than investigating them properly — which means genuine bugs can be missed and merged because the failure was dismissed rather than fixed. Over time, this erodes confidence in the pipeline's ability to catch real problems, and teams may start ignoring failing checks altogether, defeating the purpose of having automated quality gates in the first place.