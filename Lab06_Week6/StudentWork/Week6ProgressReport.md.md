# Week 6 Test Progress Report

## Reporting point

Release/build: AB-1.4-RC1 (a231a607fe946cba086fd6e2ae79dd320f044d16)
Evidence source: CheckpointA.csv, TestPortfolio.csv
Definitions used for planned, executed, passed and blocked: Planned = all 20 portfolio test cases. Executed = tests with a Passed or Failed result only (excludes Blocked and Not Run, per lab definition). Passed/Blocked/Not Run counted directly from CheckpointA.csv status column.

## Metrics

| Metric | Formula and values | Result | Interpretation |
|---|---|---:|---|
| Execution progress | 15 executed ÷ 20 planned × 100 | 75% | Three-quarters of the portfolio has produced a Pass/Fail result; the remaining quarter is either blocked (SMS) or not yet started, not a reflection of quality |
| Pass rate | 11 passed ÷ 15 executed × 100 | 73.3% | Of tests actually run, roughly 3 in 4 passed — but this treats a Critical concurrency failure and a Critical authorisation failure the same as any low-risk failure |
| Blocked proportion | 2 blocked ÷ 20 planned × 100 | 10% | One-tenth of the portfolio (both SMS-dependent) cannot produce evidence at all until the sandbox recovers — this is an environment gap, not a quality signal |
| High/Critical coverage | 9 executed High/Critical ÷ 13 planned High/Critical × 100 | 69.2% | Nearly a third of the highest-risk scenarios (including TC-014, a Critical data-integrity test) have no result yet, which matters more than the overall pass rate for the release decision |

## Status and forecast



Important evidence: 11 of 20 tests passed, 4 failed (2 Critical: ANO-02 concurrency exception on TC-008, ANO-04 authorisation bypass on TC-011; 2 High: ANO-01 duplicate booking on TC-007, ANO-03 fixture-caused failure on TC-010), 2 blocked by the SMS sandbox outage (TC-015, TC-016), and 3 not yet run (TC-013, TC-014, TC-018).

Main blockers: (1) SMS sandbox outage with unconfirmed recovery time, blocking TC-015/016. (2) TC-014's failure-injection database setup has not yet been prepared. (3) TC-018 was deliberately deferred while cancellation-related failures (TC-009–011) were triaged.

Forecast against the plan: Execution progress (75%) is broadly on pace against the 9.92-hour estimate, but two Critical-risk defects remain open and High/Critical coverage sits at only 69.2% — below what the exit criteria require. Without developer time directed at the two Critical defects and tester time directed at TC-014, the cycle is unlikely to meet its exit criteria before tomorrow's 4pm decision point.

## Control actions


| Action | Signal that triggered it | Expected benefit | Trade-off or new risk | Owner |
|---|---|---|---|---|
| Escalate the SMS sandbox outage to the provider and engage the environment specialist as soon as they're available (1pm) | ANO-05: DNS failure confirmed from two independent sources; provider status notice confirms outage; 10% of portfolio blocked | Recovers the ability to execute TC-015/016, closing a scope gap before exit-criteria evaluation | Diverts the environment specialist's limited hours away from other environment tasks (e.g. preparing TC-014's failure-injection setup); effort may be wasted if the sandbox doesn't recover this cycle | Environment specialist |
| Reallocate the developer's 2 available hours to prioritise the two Critical defects (ANO-02, ANO-04) ahead of the High-severity ANO-01 | Two Critical-risk failures (TC-008, TC-011) identified at triage; release policy requires no unresolved Critical defect | Maximises the chance both Critical defects are resolved and confirmed within the fixed developer window, directly supporting the release decision | ANO-01 (duplicate booking) and ANO-03 (fixture problem) get deprioritised this cycle and may remain open into the completion report as residual risk | Development team |
| Prioritise early execution of the blocked/unexecuted Critical test TC-014 ahead of lower-risk Not Run tests (TC-013, TC-018) | High/Critical coverage is only 69.2%; TC-014 is the one remaining Critical-risk test with no result at all | Closes the largest gap in Critical-risk coverage, directly supporting the exit criterion requiring all Critical tests executed | Preparing the failure-injection setup takes tester time away from TC-013 and TC-018, which risks pushing those further toward Not Run at completion | Tester A / Tester B |

## Communication required


Notify the release decision-maker that two Critical-risk defects remain open and High/Critical coverage is at 69.2%, ahead of the planned progress checkpoint discussion.

