## Baseline context (Activity 1)

- **Build/commit identifier:** a231a607fe946cba086fd6e2ae79dd320f044d16
- **Operating system:** Windows 11
- **.NET SDK version:** 10.0.302
- **Test command/runner:** Visual Studio Test Explorer (equivalent to `dotnet test Starter/AppointmentBooking.slnx`)
- **Execution date:** 18 August 2026
- **Results:** 12 tests discovered, 12 passed, 0 failed

**Checkpoint question:** If every supplied domain-level test passes, what important confidence is still missing before an integrated release decision can be made?

1. *Example 1 — Integration with real infrastructure
The 12 tests exercise AppointmentBookingService in isolation, using in-memory objects. They give no confidence that the system works against a real database, a real SMS/email reminder provider, or any other external dependency — those integration points could still fail even though the domain logic passes.*
2. *Example 2 — End-to-end user and authorisation flows
The tests don't exercise the full request pipeline: how a request reaches the service (via a web UI or API), whether authorisation/access control correctly restricts who can book, cancel, or view appointments, or whether the audit trail is actually recorded end-to-end. Domain logic passing says nothing about whether the wired-up system enforces these correctly.*

---



# Week 6 Test-Cycle Addendum

## Release and objective

Release/build:
Decision this cycle must support:

## Readiness decision

Decision: Start partially

Rationale: All four entry criteria are met, so the technical and data foundation for testing is sound. However, the SMS sandbox outage blocks the two SMS-dependent test cases (TC-015, TC-016). Since this is a scoped infrastructure issue affecting only reminder-related functionality rather than the whole release, it does not justify suspending the entire cycle. Testing can proceed fully across the other 18 test cases while the SMS-dependent scope remains blocked until sandbox recovery is confirmed.

Permitted scope: All test cases except TC-015 and TC-016 (18 of 20).
Blocked scope: TC-015 (send reminder) and TC-016 (SMS provider failure mode) — pending SMS sandbox recovery.

| Entry criterion | Met / Partly / Not met | Evidence | Consequence |
|---|---|---|---|
| RC deployed to staging and domain-level MSTest baseline passes | Met | AB-1.4-RC1 deployed to staging; baseline run shows 12/12 tests passing | Testing can begin against a stable technical foundation |
| Staging environment and database available, pass smoke check | Met | Release brief confirms the test database responds to the environment smoke check | Environment-dependent test cases (e.g. TC-012–TC-014, TC-017, TC-018) can be executed with trustworthy evidence |
| Test users, doctors, appointments and synthetic data prepared | Met | Release brief confirms test users, doctors and appointments are prepared; synthetic-data-only policy in force | Execution can proceed without needing additional data setup time |
| No open Severity 1 / Critical product defect known | Met | Release brief states no open Severity 1 defect is known before execution begins | Evidence gathered this cycle won't be immediately invalidated by a known blocking issue |
## Exit criteria

1. All Critical-risk test cases (TC-008, TC-011, TC-014) have been executed, with no unresolved Critical or Severity 1 product defect remaining.
2. All High-risk test cases within the permitted scope (18 of 20, excluding SMS-dependent TC-015/TC-016) have been executed, with a documented pass rate and no unresolved High-severity defect affecting state integrity or authorisation.
3. Any test case that remains Blocked or Not Run (including TC-015/TC-016 if the SMS sandbox has not recovered) is explicitly listed with its residual risk and owner in the completion report.
4. Confirmation and targeted regression evidence exists for any defect that was fixed during the cycle, showing the original failure no longer occurs and no related passing test has regressed.

## Suspension and resumption

Suspension condition: If the staging environment or database becomes unavailable during execution (e.g. an outage preventing reliable evidence collection), testing of environment-dependent test cases will be suspended.

Resumption evidence required: The environment specialist confirms connectivity is restored and a fresh environment smoke check passes before execution resumes.

## Work-breakdown estimate

| Work item | Effort | Dependency | Can run in parallel? | Assumption |
|---|---:|---|---|---|
| Environment smoke checks (×3) | 0.5h | Staging build and accounts | Partly (app and database checks can be split) | Environment specialist available from 1:00 pm, so this cannot start earlier |
| Prepare and verify test-data sets (×4) | 1.0h | Environment smoke checks | Yes | Synthetic data only; assumes data templates already exist and just need instantiating |
| Execute planned test cases (×20) | 4.0h | Environment and data ready | Yes | Uses the flat 12-min/test planning rate, not the individual per-test estimates in TestPortfolio.csv; assumes 2 testers split the load |
| Investigate and triage anomalies (×4) | 1.67h | Initial results and evidence | Partly (testers classify first, developer joins after) | Developer only available after initial triage, for 2h total |
| Confirmation and targeted regression tests (×6) | 1.5h | Resolved build | Yes | Assumes at least one anomaly is resolved during the cycle; scope may shrink if none are |
| Prepare progress report | 0.5h | Checkpoint A results | No | Can begin while investigation continues, per the estimation notes |
| Prepare completion report | 0.75h | Final evidence and decisions | No | Must be last; cannot start until all other evidence is finalised |

Total estimated person-hours: 9.92 hours (9h 55m)

Estimated calendar duration: Roughly 1.5 working days — starting 1:00 pm Day 1 (environment checks → data prep → test execution → initial triage) and finishing Day 2 morning (developer fixes, confirmation/regression, completion report) ahead of the 4:00 pm decision deadline. Available staff hours (Tester A 4h + Tester B 3h + Developer 2h = 9h) are a close match to the 9.92h estimate, leaving little slack.

Main uncertainty: The SMS sandbox recovery time is unconfirmed. This estimate excludes TC-015 and TC-016 (SMS-dependent) entirely. If the sandbox recovers mid-cycle, additional unbudgeted testing and triage time would be needed to cover that scope before the decision deadline.