# Test Plan — Appointment Cancellation Feature

## Feature Under Test
Cancellation of an existing appointment, including release of the doctor's
booked slot.

## Test Objective
To verify that reception staff can cancel an existing appointment, that
the doctor's available slot count is correctly restored, and that invalid
cancellation attempts are correctly rejected.

## Requirements to be Tested
- REQ-CAN-01: The system shall allow an existing appointment to be cancelled.
- REQ-CAN-02: When an appointment is cancelled, the doctor's available slot count shall increase by one.
- REQ-CAN-03: The system shall not allow cancellation of an appointment that does not exist.

## Test Items
- `Appointment.Cancel()`
- `BookingService.CancelAppointment(Appointment appointment)`
- `Doctor.ReleaseSlot()`

## Test Approach
Unit testing using MSTest, covering the happy path, slot-release
behaviour, and negative/exception cases (null appointment, already-
cancelled appointment). Tests are written alongside the implementation and
re-run against the full existing suite to confirm no regression in the
booking functionality carried over from Week 2.

## Test Data
- A valid `Doctor` with a known `AvailableSlots` count
- A valid `Patient`
- A booked `Appointment` linking the two, with a known `AppointmentDate`

## Responsibilities
| Role | Responsibility |
|---|---|
| Developer/Tester (Aarti) | Implement feature, write and execute tests, record results |

## Schedule
Testing is completed within the Week 3 lab session, alongside implementation.

## Pass and Fail Criteria
- **Pass:** All test cases execute with the expected result and no regression occurs in existing tests.
- **Fail:** Any test case produces an unexpected result, or a previously passing test now fails.

## Risks
| Risk | Mitigation |
|---|---|
| Slot count not restored correctly | Dedicated test: `CancelAppointment_ExistingAppointment_ReleasesDoctorSlot` |
| Double cancellation allowed | Dedicated test: `CancelAppointment_AlreadyCancelledAppointment_ThrowsException` |
| Null reference not handled | Dedicated test: `CancelAppointment_NullAppointment_ThrowsException` |