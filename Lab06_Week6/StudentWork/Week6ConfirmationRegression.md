# Week 6 - Confirmation, Regression and Workflow Status (Activity 5)

## Classification of FixedBuildResults.csv (AB-1.4-RC2)

| Result | Test | Purpose | Outcome |
|---|---|---|---|
| FR-01 | TC-007 | Confirmation | Passed |
| FR-02 | TC-001 | Regression | Passed |
| FR-03 | TC-017 | Regression | Failed |
| FR-04 | TC-011 | Confirmation | Passed |
| FR-05 | TC-015 | Previously blocked | Passed |
| FR-06 | TC-016 | Previously blocked | Failed |
| FR-07 | TC-010 | Test-fixture verification | Passed |

## DEF-001 (duplicate booking) - what FR-01 proves and what FR-03 reveals

FR-01 proves the originally reported symptom is fixed: retrying the exact same request no longer creates a second booking, it safely returns the original one. FR-03 reveals a regression introduced by the fix: a genuinely different patient booking the same doctor and time slot is now incorrectly rejected with a 409 "duplicate" response, instead of being processed as a distinct booking attempt. The fix appears to key its duplicate check too broadly (on doctor and time) rather than on the original request/idempotency key, blocking a legitimate scenario that TC-017 exercises end-to-end.

## DEF-001 workflow decision

Decision: Reopen DEF-001 and raise a new linked regression issue for the TC-017 symptom.
Justification: The original retry scenario is genuinely fixed (FR-01), so closing DEF-001 outright would misrepresent the evidence. The regression in FR-03 is a distinct symptom on a different code path (a different-patient booking, not a retry), caused by the same fix, so it deserves its own linked issue rather than being folded silently back into DEF-001. This preserves clean traceability between what broke and what change caused it.

## ENV-001 (SMS sandbox) - restoration vs proof of resilience

FR-05 shows the sandbox is back and the happy-path reminder scenario (TC-015) now works, but this only proves connectivity is restored. TC-016 tests a different scenario: provider failure mid-transaction. FR-06 shows this fails - a provider error now causes the entire booking transaction to roll back, when REQ-SMS-02 (Resilience) expects the booking to remain confirmed even if the reminder fails. Restoring the sandbox only made this defect (DEF-003) visible; it says nothing about whether the product handles provider failure correctly, which it currently does not.

## TEST-001 (fixture fix) - why this changes evidence reliability

FR-07 shows that switching to a fresh appointment object per test removed the suite-only failure; TC-010 now passes reliably even inside the full parallel suite run. This confirms the original ANO-03 hypothesis: the failure was caused by shared static test state, not a real product defect. Because there is now positive evidence the fixture was the cause, future TC-010 results can be trusted as reflecting actual product behaviour, whereas before this fix a Failed result there could not be trusted either way.

## Targeted regression scope for the duplicate-booking fix

- TC-001 (normal first booking) - already regression-tested (FR-02, passed); confirms the basic happy path still works.
- TC-006 (state integrity on a failed booking) - shares the booking-creation code path the fix modified; must confirm slot-count integrity was not broken.
- TC-008 (concurrent final-slot booking, Critical) - concurrency handling and the dedup fix both sit in the booking-creation path; important to confirm no new interaction defect.
- TC-017 (end-to-end booking) - already failing (FR-03); must be retested once the dedup-key logic is corrected.

These four all exercise the booking-creation logic that the duplicate-prevention fix touched, so they give the most direct evidence of whether the fix is now safe.