# Continuous Improvement

## What Worked Well
Writing the five cancellation tests directly from the REQ-CAN requirements
made it straightforward to confirm each requirement was actually covered.
Rebuilding and re-running the full test suite after every code change
(23 existing + 5 new) caught any regression immediately rather than at the
end of the lab. Committing after each discrete step (docs structure, each
`.md` file, the code, the tests) kept the Git history readable and
traceable back to individual lab steps.

## What Did Not Work Well
The initial plan (per the lab handout) assumed `BookAppointment` could
simply be changed to return an `Appointment` directly. Reviewing the
actual Week 2 code showed this would have broken all 23 existing tests,
since they assert against `BookingResult.Success`. This was only caught
because the real `BookingResult.cs` and `AppointmentBookingService.cs`
were checked before writing new code, rather than following the handout's
example literally.

## Root Cause of One Issue
The lab handout's example code assumes a simpler return type than the
actual Week 2 implementation uses. The root cause was a mismatch between
a generic instructional example and the specific, already-evolved codebase
it was being applied to — a reminder that starter-code examples in
documentation should always be checked against the real, current state of
the code rather than applied blindly.

## Improvement Action
Before implementing any new feature from a lab handout or specification,
first review the actual current signatures of the classes/methods being
extended, rather than assuming the handout's simplified example matches
the real code exactly.

## How We Will Check the Improvement
Future feature work will start with a short review step: open and read
the relevant existing class(es) before writing any new code, and confirm
assumptions about return types, constructors, and property names before
implementation begins.

## Quality Culture Reflection
Early requirement review and test-first thinking in Weeks 2 and 3 helped
prevent defects before they were introduced — the potential breaking
change to `BookAppointment`'s return type was caught during design, before
any test ever failed. Regular, small commits improved visibility into
progress and made it possible to see exactly which step introduced which
change. Test results, rather than assumed confidence, provided the
evidence needed to support the "Recommended for demonstration" release
decision in the Test Summary Report — specifically, the 28/28 passing
result after the full suite was re-run. Although this project currently
has a single contributor, the same discipline (documented requirements,
test evidence, defect logs, backward-compatible changes) is exactly what
allows quality responsibility to be shared and verified across a real
team, rather than resting on any one person's memory of how the code
works. Going forward, the improvement action above — reviewing real
existing code before extending it — will be applied to every feature in
the remaining labs.

## Agile and DevOps Quality Practices for This Project

| Practice | How It Could Be Used in This Project |
|---|---|
| Sprint planning | Select a small set of features and quality tasks for the week (e.g. Week 3's cancellation feature plus its docs) |
| Daily stand-up | Discuss progress, blockers, and testing issues — even solo, a short daily check-in against the test plan keeps scope honest |
| Definition of Done | A feature is complete only when coded, reviewed, tested (all suite tests passing), and documented in `docs/` |
| Continuous Integration | Automatically run the MSTest suite whenever code is pushed to GitHub, catching regressions like the one avoided in this lab |
| Regression testing | Re-run the full 28-test suite after every change, as was done throughout this lab |
| Retrospective | This Continuous Improvement section itself — reviewing what worked, what didn't, and one concrete improvement action |