# Week 6 Test Completion Report

## 1. Release and scope

Release/build: AB-1.4-RC3 (final rebuild after correcting the TC-017 duplicate-key regression)
Scope tested: 17 of 20 planned test cases executed (Passed or Failed) across booking, cancellation, authorisation, persistence, SMS reminders, usability and audit areas.
Scope not tested or blocked: TC-013 (database restart reliability) remained Blocked because the required restart window was never made available. TC-018 (end-to-end cancellation) and TC-020 (audit-log test) were deferred and not run at the deadline.

## 2. Final results

Definitions and calculations:
- Execution progress: 17 ÷ 20 × 100 = 85%
- Pass rate: 15 ÷ 17 × 100 = 88.2%
- Blocked proportion: 1 ÷ 20 × 100 = 5%
- High/Critical coverage: 11 of 13 planned High/Critical test cases executed ÷ 13 × 100 = 84.6% (TC-013, Blocked, and TC-018, Not Run, are the two High-risk cases not executed)

## 3. Deviations from the cycle addendum
- The SMS sandbox recovered mid-cycle (unlike the "unconfirmed recovery time" assumption in the addendum), allowing TC-015 and TC-016 to be executed. TC-016 revealed a new defect (DEF-003: a provider error incorrectly rolls back an otherwise valid booking), which was not anticipated in the original estimate.
- A second rebuild (AB-1.4-RC3) was required beyond the single AB-1.4-RC2 retest build the estimate assumed, because the AB-1.4-RC2 fix for DEF-001 introduced a regression on TC-017 that itself needed correcting.
- The database restart window needed for TC-013 was never made available, so this environment-dependent test remains untested, contrary to the plan's assumption that environment-dependent tasks would be scheduled early.
- TC-018 and TC-020 were deferred at the deadline rather than executed, indicating the additional regression-fix cycle consumed time that the original 9.92-hour estimate had allocated to executing the full portfolio.


## 4. Exit-criteria assessment

| Exit criterion | Met / Not met | Evidence | Decision consequence |
|---|---|---|---|
| All Critical-risk test cases (TC-008, TC-011, TC-014) executed, with no unresolved Critical/Severity 1 defect remaining | Not met | TC-011 and TC-014 passed, but TC-008 failed - an unhandled exception remains possible under concurrent final-slot requests (ANO-02, still open) | A Critical concurrency defect remains unresolved, which directly blocks an unrestricted exit |
| All High-risk test cases in scope executed, documented pass rate, no unresolved High-severity defect affecting state integrity or authorisation | Not met | TC-013 (High) is Blocked and TC-018 (High) is Not Run; TC-016 (High) failed with an unresolved defect (DEF-003) affecting booking state when the SMS provider errors | Coverage and defect gaps remain in high-risk scope; residual risk must be explicitly carried forward |
| Any Blocked/Not Run test explicitly listed with residual risk and owner | Met | TC-013, TC-018 and TC-020 are listed with residual risk and ownership in Section 5 of this report | Transparency requirement satisfied; decision-makers have visibility of what remains unverified |
| Confirmation and regression evidence exists for any defect fixed during the cycle | Met | DEF-001 (TC-007 confirmed, TC-017 regression corrected), DEF-002 (TC-011 confirmed) and TEST-001 (TC-010 confirmed) all have documented confirmation/regression evidence in FixedBuildResults.csv and Week6ConfirmationRegression.md | The three defects that were fixed during this cycle can be trusted as genuinely resolved |

## 5. Remaining defects and residual risks

|---|---|---|---|---|
| TC-008: unhandled exception under concurrent final-slot booking (ANO-02, Critical) | A patient can receive an internal-server error when two people book the last slot simultaneously; the database is never over-booked, but the user-facing experience fails ungracefully. Low expected frequency in a small supervised pilot | Monitor booking-service error logs closely during the pilot; treat any occurrence as a priority fix trigger | Development team | Fix and confirm before any release beyond the supervised pilot |
| TC-016: SMS provider error rolls back an otherwise valid booking (DEF-003, High) | A patient could lose a legitimately completed booking simply because the reminder failed to send - a direct, visible harm to the pilot's clinic users | Disable automatic SMS reminders for the duration of the supervised pilot; have clinic staff follow up manually where reminders are expected | Development team (fix); Clinic operations (manual workaround) | Fix the resilience defect so a reminder failure no longer rolls back the booking transaction, then re-enable reminders |
| TC-013: database restart reliability untested (Blocked) | Unknown whether a booking survives a service restart; if it does not, this is a data-loss risk in production operation | None available this cycle; restart window was never provided | Environment specialist | Schedule and execute a restart test before any release beyond the supervised pilot |
| TC-018: end-to-end cancellation untested (Not Run) | The cancellation flow has been verified at the service level (TC-009, TC-010, TC-011) but not through the full UI/API/database path | Rely on service-level cancellation evidence as a proxy; monitor pilot cancellations manually | Test team | Execute TC-018 before scaling beyond the supervised pilot |
| TC-020: audit-log test untested (Not Run) | Auditability is an explicit requirement (REQ-AUD-01) for this system; without this test, there is no verified evidence that actions are being logged correctly | None available this cycle | Test team | Execute TC-020 as a priority in the next cycle, given audit trail integrity matters for a healthcare booking system |

## 6. Release recommendation

Recommendation: Restricted release

Evidence-based rationale: The pilot's core booking, cancellation, authorisation, persistence and audit-of-intent behaviours are well-supported by evidence - all Critical/High defects found earlier in the cycle (duplicate booking, authorisation bypass, the test fixture problem) have been fixed and confirmed, with regression evidence showing no related behaviour broke. However, two issues remain open that directly affect patient-facing reliability: an unresolved Critical concurrency defect (TC-008) that can produce an ungraceful error under simultaneous final-slot bookings, and an unresolved High defect (TC-016) where an SMS provider failure incorrectly destroys a valid booking rather than just failing the reminder. Additionally, three tests (TC-013, TC-018, TC-020) remain unverified. Given this is a supervised pilot with clinic staff able to monitor activity closely and low expected concurrent load, these risks are manageable if the SMS-reminder exposure is removed and the concurrency issue is actively monitored - but they are not acceptable for an unrestricted release.

Restrictions or conditions, if any:
1. Automatic SMS reminders are disabled for the duration of the pilot; clinic staff manually follow up on appointment reminders instead, removing the DEF-003 exposure entirely.
2. The pilot proceeds at low/supervised booking volume only, with booking-service error logs actively monitored for the TC-008 concurrency symptom; any occurrence triggers an immediate priority fix.
3. TC-013 (restart reliability), TC-018 (end-to-end cancellation) and TC-020 (audit logging) must be executed and passed before any release beyond this supervised pilot.
4. Clinic operations lead accepts the residual risk on TC-008 and TC-016 for the duration of the pilot, informed by this report.

## 7. Copilot challenge and human judgement

Prompt used: "Act as a sceptical release manager. Challenge the recommendation below. Identify missing evidence, unsupported assumptions and residual risks. Do not replace the final human decision."

Useful challenge accepted: Copilot pointed out that assuming "low expected frequency" for the TC-008 concurrency defect is itself an unverified assumption - the defect occurred in 3 of 10 intentional concurrency-trigger runs, and I had not stated what real-world booking volume the pilot would actually see. I accepted this and tightened the recommendation to require active log monitoring with a defined trigger (any occurrence) rather than just asserting the risk was low.

Suggestion rejected or modified: Copilot suggested delaying the release entirely until TC-008 is fixed, treating any unresolved Critical-risk defect as an automatic blocker. I rejected this because the database was never shown to be corrupted or over-booked in any concurrency test run - the defect produces a failed request and an ungraceful error, not silent data loss - and the pilot's supervised, low-volume nature meaningfully reduces the chance of the defect triggering. A blanket "any Critical defect blocks release" rule does not account for the difference between a data-integrity failure and a recoverable, user-facing error.

Why human judgement was required: Deciding how much residual risk a specific clinic and pilot context can tolerate is a business and stakeholder decision, not a purely technical one. Copilot can surface the technical facts and challenge unsupported assumptions, but weighing "is this risk acceptable for this pilot, at this volume, with this monitoring in place" requires judgement about real-world consequences that sits with the human release manager, not the AI reviewer.