Patient Records
 ^
 │
Reception UI -> Appointment System -> Doctor Schedule
 │
 v
 SMS Service
 # Industry Testing Phases: SIT, UAT, PVT

## Context

The Week 2 problem statement notes that the clinic may later integrate the booking system with Patient Records, Doctor Schedules, an SMS reminder service, and online booking. The scenarios below assume that future integrated architecture:
Patient Records
^
│
Reception UI -> Appointment System -> Doctor Schedule
│
v
SMS Service
## System Integration Testing (SIT)

**Purpose:** Verify that the Appointment System correctly interacts with the other complete systems/services it depends on, once they are connected.

**Scenario 1 — Doctor Schedule synchronisation:**
When a booking is confirmed in the Appointment System, verify that the Doctor Schedule service receives and correctly records the updated availability via its API, including handling of authentication tokens and a realistic network timeout (e.g. the Doctor Schedule service takes 10+ seconds to respond).

**Scenario 2 — SMS Service failure handling:**
When a booking succeeds but the SMS Service is unavailable or returns an error, verify that the Appointment System still confirms the booking to the receptionist, logs the SMS failure, and does not silently lose the appointment record — testing retry logic and graceful degradation of a downstream dependency.

**Scenario 3 — Patient Records data mapping:**
Verify that patient data fields (ID, legal name, preferred name) map correctly between the Appointment System's internal model and the Patient Records system's schema, including edge cases like differing field length limits or character encoding between the two systems.

## User Acceptance Testing (UAT)

**Purpose:** Determine whether real clinic receptionists and staff can use the system to accomplish their actual work, and whether the business rules match real-world need.

**Scenario 1 — Receptionist daily workflow:**
A receptionist uses the system to book, view, and manage a full day's worth of appointments across multiple doctors, evaluating whether the workflow is efficient, whether error messages are clear enough for a non-technical user, and whether the "Remaining slots" display gives them the information they need to make decisions.

**Scenario 2 — Handling a walk-in patient with an unusual name:**
A receptionist books an appointment for a walk-in patient whose name includes a preferred/cultural name different from their legal name, evaluating whether the system's preferred-name handling matches how the clinic actually wants to greet and address patients in confirmations and SMS reminders.

## Production Validation Testing (PVT)

**Purpose:** Safely validate that the deployed production system, running against real production configuration, is healthy — without risking real patient data or real bookings.

**Scenario 1 — Production smoke test with a controlled test account:**
After deployment, use a dedicated test patient/doctor account (clearly marked as non-production data) to perform one full booking end-to-end against the live production Doctor Schedule and SMS endpoints, confirming connectivity, authentication, and that the booking appears correctly — then immediately clean up the test data.

**Scenario 2 — Monitoring and logging verification:**
Confirm that production logs and monitoring dashboards correctly capture a real booking event (success and failure paths), verifying that alerts would fire appropriately if the Doctor Schedule or SMS integration failed in production, without needing to wait for an actual real-world failure to find out.

## Why can't the existing 12 MSTest tests simply be renamed "UAT" or "PVT"?

The existing 12 MSTest tests operate entirely in memory, calling C# classes directly with no real user, no real production configuration, and no real external systems involved. UAT specifically requires real business users evaluating whether the system meets their actual working needs — a judgement an automated test cannot make, since it only checks a program's logic against what the test author expected, not whether the correct thing was expected in the first place. PVT specifically requires validating the actual deployed production environment (real endpoints, real configuration, real monitoring) — something in-memory unit tests inherently do not touch, since they never leave the developer's local build. Renaming these tests wouldn't change what they actually exercise; the label needs to match the reality of what's being tested, not just describe an aspiration for it.