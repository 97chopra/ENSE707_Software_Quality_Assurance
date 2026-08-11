# System Tests (via Console Interface)

All tests executed through `AppointmentBooking.Console`, using Ctrl+F5 (Start Without Debugging).

## SYS-01: Valid future booking with final available slot

- **Preconditions:** Fresh console run, no prior state.
- **Inputs:** Patient ID `P001`, Legal name `Diana William`, Preferred name (blank), Doctor `Dr Mark`, Available slots `1`, Days from today `1`.
- **Steps:** Run console app, enter inputs above in order.
- **Expected Result:** Success message using legal name (no preferred name given); remaining slots = 0.
- **Actual Result:** "Appointment booked successfully for Diana William with Dr Mark." Remaining slots: 0.
- **Pass/Fail:** Pass
- **Evidence:** Console output screenshot, process exited with code 0.

## SYS-02: No availability

- **Preconditions:** Fresh console run.
- **Inputs:** Patient ID `P002`, Legal name `John Smith`, Preferred name (blank), Doctor `Dr Lee`, Available slots `0`, Days from today `1`.
- **Steps:** Run console app, enter inputs above in order.
- **Expected Result:** Failure message explaining no available slots; remaining slots stays 0.
- **Actual Result:** "Appointment cannot be booked because Dr Lee has no available slots." Remaining slots: 0.
- **Pass/Fail:** Pass
- **Evidence:** Console output screenshot, process exited with code 0.

## SYS-03: Past appointment date

- **Preconditions:** Fresh console run.
- **Inputs:** Patient ID `P003`, Legal name `Amelia Ray`, Preferred name (blank), Doctor `Dr Singh`, Available slots `1`, Days from today `-1`.
- **Steps:** Run console app, enter inputs above in order.
- **Expected Result:** Validation error; booking is not performed.
- **Actual Result:** "Validation error: Requested appointment date cannot be in the past." (No booking confirmation or remaining-slots line printed, since the exception is thrown before the service is called.)
- **Pass/Fail:** Pass
- **Evidence:** Console output screenshot, process exited with code 0.

## SYS-04: Preferred name shown

- **Preconditions:** Fresh console run.
- **Inputs:** Patient ID `P004`, Legal name `Robert Chen`, Preferred name `Aroha`, Doctor `Dr Patel`, Available slots `1`, Days from today `1`.
- **Steps:** Run console app, enter inputs above in order.
- **Expected Result:** Success message uses "Aroha" instead of the legal name.
- **Actual Result:** "Appointment booked successfully for Aroha with Patel." Remaining slots: 0.
- **Pass/Fail:** Pass
- **Evidence:** Console output screenshot, process exited with code 0.

## SYS-05 (own scenario): Booking exactly today

- **Preconditions:** Fresh console run.
- **Inputs:** Patient ID `P005`, Legal name `Test Today`, Preferred name (blank), Doctor `Dr Today`, Available slots `2`, Days from today `0`.
- **Steps:** Run console app, enter inputs above in order.
- **Expected Result:** Today is a valid boundary date (per Rule set B), so booking should succeed; remaining slots should become `1`.
- **Actual Result:** *(To be filled in after running this scenario.)*
- **Pass/Fail:** *(To be filled in.)*
- **Evidence:** *(Screenshot once run.)*

## Reflection

Comparing the system tests here with the existing MSTest service tests: the underlying logic being exercised is identical (the same `Doctor`, `Patient`, `AppointmentRequest`, and `AppointmentBookingService` classes), but the test boundary and entry point differ. The MSTest suite calls these classes directly in-process, with no user interface involved, so it gives fast, precise feedback about individual class behaviour. The system tests here go through the console application's actual entry point, exercising string-to-type parsing (`int.Parse`), console I/O, and the full request/response flow a real user would experience. This matters because a unit test can pass perfectly while the console layer around it still has bugs (e.g. a crash on non-numeric input, or a misleading prompt) — evidence a human pressing "run" on the console app can catch that an in-process test cannot.