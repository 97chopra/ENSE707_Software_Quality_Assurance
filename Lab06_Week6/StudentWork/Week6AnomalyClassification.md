# Week 6 - Anomaly Classification (Activity 3, Part A)

## ANO-01 - Duplicate booking after a retry (TC-007)
**Classification:** Application defect
**Evidence:** The same request key, resubmitted 600ms later, produced two separate bookings (B-4318 and B-4319) for the same patient/doctor/time, reproduced in 3/3 controlled retries.
**Unknown:** Whether the idempotency mechanism is missing entirely or just has too short a dedup window.

## ANO-02 - Concurrent final-slot request (TC-008)
**Classification:** Application defect
**Evidence:** In 3/10 runs, a concurrent request on the last slot threw an unhandled InvalidOperationException; the database was never over-booked, but the API returned an unhandled server error.
**Unknown:** Whether this is a race condition in a check-then-act pattern or a missing exception handler around a legitimate conflict case.

## ANO-03 - Repeat cancellation fails only in the suite (TC-010)
**Classification:** Test asset or fixture problem
**Evidence:** Isolated rerun passed 5/5 times; full suite run failed. Fixture inspection shows all cancellation tests share a static appointment object with method-level parallel execution enabled.
**Unknown:** Evidence does not yet fully rule out a production concurrency issue; should be reconfirmed once the fixture is fixed.

## ANO-04 - Another user's appointment can be cancelled (TC-011)
**Classification:** Application defect
**Evidence:** Authenticated user patient-b successfully cancelled patient-a's appointment (204 No Content instead of expected 403 Forbidden), reproduced with two different user pairs.
**Unknown:** Whether the authorisation check is missing entirely on this endpoint or just has faulty ownership logic.

## ANO-05 - SMS tests blocked (TC-015, TC-016)
**Classification:** Environment/dependency incident
**Evidence:** DNS lookup for sms-sandbox.test fails from both staging and the environment specialist's independent diagnostic container; provider's status notice confirms a sandbox outage.
**Unknown:** Recovery time is not yet confirmed.