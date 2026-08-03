# Test Strategy — Appointment Booking System

## 1. Purpose
This Test Strategy defines the overall approach to testing the Appointment
Booking System. It establishes the principles, levels, and types of
testing used to provide confidence that the system meets the clinic's
functional and quality requirements before each release.

## 2. Scope of Testing
- Appointment booking logic (validation, slot allocation)
- Appointment cancellation logic (state change, slot release)
- Domain model validation (Doctor, Patient, Appointment, AppointmentRequest)
- Regression testing of existing functionality after each change

## 3. Out of Scope
- User interface testing (no UI exists in this phase of the project)
- Performance/load testing (not required at the current scale)
- Third-party integrations (e.g. payment, SMS notifications — not yet implemented)

## 4. Test Levels
- **Unit testing** — individual classes and methods (Appointment, Doctor, Patient, BookingService)
- **Integration testing** — interaction between BookingService and the domain classes it coordinates
- **System testing** — end-to-end booking and cancellation workflows

## 5. Test Types
- Unit testing
- Integration testing
- System testing
- Regression testing
- Usability testing (informal, deferred until a UI exists)
- Validation testing (confirming business rules such as slot counts and ID format)

## 6. Test Environment
Tests are executed locally in Visual Studio using the MSTest framework
against the AppointmentBooking.Tests project. No external environment or
database is required at this stage; all test data is created in-memory
within each test.

## 7. Tools
- Visual Studio 2022
- MSTest test framework
- Git / GitHub for version control and traceability
- GitHub Copilot (used critically, with all suggestions reviewed and tested before acceptance)

## 8. Defect Management Approach
Defects identified during testing are recorded in the defect log in
`docs/QualityGovernance.md`, with a unique ID, description, severity,
status, and the point at which the defect was found and fixed. No defect
is closed without a passing regression test that demonstrates the fix.

## 9. Entry Criteria
- Requirements for the feature under test are documented and reviewed
- Code compiles without errors
- Unit tests have been written to cover the new functionality

## 10. Exit Criteria
- All planned test cases have been executed
- All high-severity defects are resolved and retested
- All existing (regression) tests continue to pass

## 11. Risks and Mitigation
| Risk | Mitigation |
|---|---|
| Incomplete requirements lead to missed test cases | Requirements reviewed before test design; traceability to REQ IDs |
| Manual testing misses edge cases | Automated MSTest suite covering boundary and negative cases |
| Regression introduced by new features | Full test suite re-run before every commit and release |
| Over-reliance on Copilot-generated tests | All AI-suggested tests reviewed, understood, and validated before acceptance |