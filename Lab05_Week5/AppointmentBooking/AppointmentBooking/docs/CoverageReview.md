# Coverage Review

## Which branch or path was initially missing?

Before adding new tests, code coverage showed several under-tested paths:
- The `AppointmentRequest` constructor's null-checks for `patient` and `doctor` (71.4% blocks / 75.0% lines).
- `Doctor.ReserveSlot()`'s guard against reserving a slot when none are available (71.4% blocks / 80.0% lines).
- The `Doctor` constructor's validation of an empty/whitespace `fullName` (85.7% blocks / 91.7% lines).
- The `Patient` constructor's validation of an empty/whitespace `legalName` (80.0% blocks / 90.0% lines).
- `AppointmentBookingService.BookAppointment`'s null-request guard remains only partially covered (92.6% blocks / 92.3% lines) even after the other additions.

## Which test did you add?

- `Doctor_WhenFullNameIsEmpty_ThrowsException`
- `Patient_WhenLegalNameIsEmpty_ThrowsException`
- `AppointmentRequest_WhenPatientIsNull_ThrowsException`
- `AppointmentRequest_WhenDoctorIsNull_ThrowsException`
- `Doctor_ReserveSlot_WhenNoAvailableSlots_ThrowsException`
- (Optional) `BookAppointment_WhenRequestIsNull_ReturnsFailureMessage`

## What code became covered?

After adding these tests, `AppointmentRequest`, `Doctor`, and `Patient` all reached 100% block and line coverage. Overall project coverage rose from 91.7%/93.1% to 95.0%/94.1% (blocks/lines).

## What important quality risks are still not addressed even after coverage increases?

Coverage only proves these lines were *executed*, not that they behave correctly under all real-world conditions. Risks still unaddressed include:
- **Concurrency**: no test proves `Doctor.ReserveSlot()` is safe when called from multiple threads simultaneously.
- **Boundary and extreme values**: very large slot counts, unusually long or malformed names, and internationalised text are untested.
- **System-level behaviour**: nothing has been tested through an actual external interface (e.g. the console app) — see Activity 5.
- **Non-functional attributes**: performance, reliability under partial failure, and persistence are not addressed by any of these tests.

## Why would "100% line coverage" still be an insufficient release argument?

Line coverage only shows that a statement executed at least once during testing — it says nothing about whether the assertions are strong enough, whether all meaningful input combinations were tried, whether the code is thread-safe, whether real integrations (databases, files, external services) work correctly, or whether the system meets actual user needs. A test suite can reach 100% line coverage while still missing critical defects in logic, concurrency, or usability.