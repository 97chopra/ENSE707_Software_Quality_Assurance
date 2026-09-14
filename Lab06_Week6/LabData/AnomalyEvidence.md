# Anomaly Evidence Pack

A failed or blocked test is an observation. Do not assume every anomaly is an application-code defect. Use the evidence below and identify what is known, what is inferred and what remains unknown.

## ANO-01 - Duplicate booking after a retry

- Test: `TC-007`
- Build: `AB-1.4-RC1`
- Environment: staging
- Request 1: `POST /appointments`, request key `retry-8841`, response `201`, booking `B-4318`
- Request 2: identical body and request key sent 600 ms later, response `201`, booking `B-4319`
- Database query: both booking IDs exist for the same patient, doctor and start time.
- Slot count decreased twice.
- Reproduced in three of three controlled retries.

## ANO-02 - Concurrent final-slot request

- Test: `TC-008`
- Build: `AB-1.4-RC1`
- Initial data: doctor `D004` has one remaining slot.
- Two requests were released from a test barrier at the same time.
- Result across ten runs: seven runs produced one success and one controlled failure; three runs produced one success and one unhandled `InvalidOperationException`.
- The database never showed more than one booking, but an API consumer received an internal-server error.
- No application log correlation ID was returned to the client.

## ANO-03 - Repeat cancellation fails only in the suite

- Test: `TC-010`
- Build: `AB-1.4-RC1`
- Full-suite result: slot count increased from 1 to 2 after the repeated cancellation step.
- Isolated rerun: passed five times.
- Inspection of the test fixture shows that all cancellation tests use the same static appointment object.
- Method-level parallel execution is enabled.
- The production service has not yet been ruled in or ruled out as the cause.

## ANO-04 - Another user's appointment can be cancelled

- Test: `TC-011`
- Build: `AB-1.4-RC1`
- Precondition: appointment `B-4402` belongs to test user `patient-a`.
- Action: authenticated test user `patient-b` sends `DELETE /appointments/B-4402`.
- Expected: `403 Forbidden`; appointment remains active.
- Actual: `204 No Content`; appointment status becomes cancelled.
- Reproduced with two different user pairs.

## ANO-05 - SMS tests blocked

- Tests: `TC-015` and `TC-016`
- Staging application and database checks pass.
- DNS lookup for `sms-sandbox.test` fails from staging and from the environment specialist's diagnostic container.
- The provider status notice reports a sandbox outage.
- No product request reached the SMS adapter during these attempts.
- Recovery time is unknown at Checkpoint A.
