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

Decision: start fully / start partially / suspend

Rationale:

| Entry criterion | Met / Partly / Not met | Evidence | Consequence |
|---|---|---|---|
| | | | |
| | | | |
| | | | |

## Exit criteria

1.
2.
3.
4.

## Suspension and resumption

Suspension condition:
Resumption evidence required:

## Work-breakdown estimate

| Work item | Effort | Dependency | Can run in parallel? | Assumption |
|---|---:|---|---|---|
| | | | | |

Total estimated person-hours:
Estimated calendar duration:
Main uncertainty:
