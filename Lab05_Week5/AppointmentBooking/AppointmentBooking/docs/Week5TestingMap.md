# Week 5 Testing Map

## Activity 1: Baseline Diagnosis

### Test run summary
- Tests discovered: 12
- Tests passed: 12
- Tests failed: 0

### Classes exercised by the current suite
- `Doctor` (slot validation, negative slots)
- `Patient` (empty ID, preferred vs legal name display)
- `AppointmentRequest` (past date validation)
- `AppointmentBookingService` (booking success/failure, slot decrement, result messages)

### Missing confidence despite a passing suite

**Question: We have a passing automated test suite. What important kinds of confidence are still missing?**

Even though all 12 tests pass, a green suite only proves the specific scenarios it covers are correct — it does not mean the whole product is ready for release. Several important kinds of confidence are still missing:

1. **Concurrency safety.** No test checks what happens if two booking requests try to claim the last available slot at the same time. `Doctor.ReserveSlot()` reads and decrements `AvailableSlots` with no locking, so a race condition could allow overbooking, but this is never exercised.

2. **Extreme or unusual input values.** No test checks very large slot counts, unusually long patient/doctor names, or special characters (symbols, emojis) in names. The system's behaviour at these edges is unknown.

3. **System-level (end-to-end) behaviour.** All 12 tests call the C# classes directly (`Doctor`, `Patient`, `AppointmentBookingService`), bypassing any real entry point a user would interact with. There is no evidence the system behaves correctly through an actual interface, such as a console or UI, including how invalid input is parsed and displayed.

4. **Data persistence and reliability.** Nothing tests whether a successful booking is actually saved anywhere or would survive an application restart or crash. The current suite only proves in-memory state changes correctly during a single test run.

5. **Non-functional quality attributes.** There is no evidence about performance under load (many simultaneous booking requests), nor about how the system behaves under partial failures (e.g. an exception thrown mid-booking).

## Activity 2: Testing Map

| Existing test/evidence | Level | Type/focus | Technique/perspective | What it provides evidence for | Important gap |
|---|---|---|---|---|---|
| Doctor: negative slots (`Doctor_WhenAvailableSlotsIsNegative_ThrowsException`) | Unit | Validation / constructor guard | Black-box, boundary value (below 0) | The `Doctor` constructor correctly rejects invalid state at creation | Does not test slot count changing to negative after creation via any other path |
| Booking: no slots (`BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure`) | Unit / small component (exercises `Doctor` + `AppointmentBookingService` together) | Negative functional test | Black-box, equivalence partitioning (zero-slot partition) | The service correctly refuses a booking when no capacity exists | Does not test concurrent requests competing for the same zero-remaining state |
| Patient: preferred display name (`Patient_WhenPreferredNameExists_DisplayNameUsesPreferredName`) | Unit | Functional / business rule | Black-box | `DisplayName` correctly prefers the preferred name when set | Does not test edge cases like whitespace-only preferred name or very long names |
| Request: past appointment date (`AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException`) | Unit | Validation | Black-box, boundary value (past vs today) | Past dates are rejected at construction | Does not explicitly test "today" as a boundary value (only implied by other passing tests) |
| Booking: helpful success message (`BookAppointment_WhenSuccessful_ReturnsHelpfulMessage`) | Unit / small component | Functional, message content | Black-box | Confirms the result message contains expected text fragments | Does not verify the full/exact message format, or localisation/formatting concerns |
| Doctor: empty ID (`Doctor_WhenIdIsEmpty_ThrowsException`) | Unit | Validation | Black-box, equivalence partitioning (invalid ID partition) | Constructor rejects an empty doctor ID | Does not test null ID, whitespace-only ID beyond empty string, or duplicate IDs |
| **Integration** | — | — | — | The project provides **no** integration-level evidence; all 12 tests exercise in-memory objects only, with no real I/O, persistence, or cross-component boundary. | Add small component/integration tests with real file or database persistence (see Activity 7). |
| **System** | — | — | — | The project provides **no** system-level evidence; nothing is tested through an actual external interface such as the console app. | Add manual system tests through the console harness (see Activity 5). |
| **Acceptance** | — | — | — | The project provides **no** acceptance-level evidence; no scenarios have been validated from a business/user-acceptance perspective. | Would require UAT scenarios and stakeholder sign-off (see Activity 8). |
| **Non-functional** | — | — | — | The project provides **no** non-functional evidence; nothing addresses performance, concurrency safety, reliability, or usability. | Add concurrency and load-focused tests; document NFR expectations explicitly. |

**Note on unit vs. small component test boundary:** Some `AppointmentBookingService` tests (e.g. `BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess`) could arguably be called "small component tests" rather than pure unit tests, since they exercise `Doctor`, `Patient`, `AppointmentRequest`, and `AppointmentBookingService` collaborating together rather than one class in isolation with mocks. I classify these as unit/small-component tests because everything still runs in-memory with no external dependencies (no I/O, no database, no network) — the test boundary is the whole in-process object graph, not a single class.