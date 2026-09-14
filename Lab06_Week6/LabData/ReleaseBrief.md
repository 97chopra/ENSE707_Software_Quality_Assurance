# Release Brief - AB-1.4-RC1

## Decision to be made

The clinic wants to decide whether release candidate `AB-1.4-RC1` is suitable for a supervised pilot. A decision is required tomorrow at 4:00 pm. The decision can be full release, restricted release with mitigation, or delay.

## Release scope

The integrated candidate includes:

- Creating an appointment when a doctor has capacity.
- Preventing invalid, duplicate and conflicting bookings.
- Cancelling an appointment without corrupting the slot count.
- Preventing one user from cancelling another user's appointment.
- Saving and reloading bookings from the staging database.
- Sending reminders through an SMS sandbox without allowing reminder failure to undo a valid booking.
- Recording important actions in an audit log.

The supplied C# solution is the familiar domain-level baseline. The integrated release evidence is supplied separately because students may have completed different optional components in earlier labs.

## Resources and constraints

- Tester A is available for four hours.
- Tester B is available for three hours.
- One developer is available for two hours after initial triage.
- The environment specialist is available from 1:00 pm.
- The staging application and database are available at the start of the cycle.
- The SMS sandbox has a known service outage. Recovery time is not yet confirmed.
- Only synthetic test data may be used. Production patient data is prohibited.
- The 20 test cases have already been reviewed and prioritised. Students should not redesign the whole portfolio.

## Readiness information

- `AB-1.4-RC1` is deployed to staging.
- The domain-level MSTest baseline passes on the supplied solution.
- Test users, doctors and appointments have been prepared.
- The test database responds to the environment smoke check.
- The SMS sandbox does not currently resolve from staging.
- The requirements and 20 test cases have stable identifiers.
- No open Severity 1 defect is known before execution begins.

## Suggested release-policy constraints

Tailor these rather than copying them mechanically:

- All Critical tests should be executed.
- No unresolved Critical or Severity 1 product defect should remain.
- Important state-integrity and authorisation scenarios should pass.
- Any untested or blocked scope must be visible in the completion report.
- A restricted release must identify the restriction, evidence, risk owner and follow-up action.
