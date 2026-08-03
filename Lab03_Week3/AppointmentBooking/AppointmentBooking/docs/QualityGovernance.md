# Quality Governance

## Process Assurance vs Product Assurance

| Area | Process Assurance | Product Assurance |
|---|---|---|
| Main focus | How the work is performed | Quality of the software product |
| Example in this project | Requirements review, coding standards, Git commits, test process | Validation logic, working booking feature, passing tests |
| Evidence | Review checklist, commits, test plan, CI results | Test results, defect reports, working prototype |
| Goal | Prevent quality problems | Detect and confirm product quality |

Process assurance and product assurance are complementary rather than
interchangeable. Process assurance provides confidence *before* delivery
by ensuring requirements are reviewed, coding standards are followed, and
changes are tracked consistently through version control — this reduces
the likelihood that defects are introduced in the first place. Product
assurance provides confidence *at* delivery by directly evaluating the
artefact itself through testing and validation. A disciplined process does
not guarantee a defect-free product, and a product that passes today's
tests does not guarantee the process behind it is reliable going forward.
For the Appointment Booking System, both are needed: process assurance
(reviewed requirements, a defined test strategy, consistent commits)
reduces the risk of introducing defects as the cancellation feature is
built, while product assurance (the MSTest suite, defect log, and test
summary report) confirms the feature actually behaves correctly before
release to the clinic.
## Quality Governance Rules

| Governance Area | Rule | Evidence |
|---|---|---|
| Requirements | Each new feature must have at least one requirement ID | Requirements list (e.g. REQ-CAN-01 to REQ-CAN-03) |
| Testing | Each requirement must have at least one test case | Traceability between REQ IDs and MSTest methods |
| Code quality | Code must pass all unit tests before commit | Test Explorer results (28/28 passing) |
| GitHub | Each student must commit meaningful work regularly | Git commit history |
| AI use | Copilot suggestions must be reviewed and tested | AI reflection notes (see below) |
| Defects | Defects must be recorded with status and severity | Defect log (below) |
| Release | A feature can only be released if exit criteria are met | Test summary report |

These rules support quality governance by making expectations explicit and
verifiable rather than assumed. Requiring a requirement ID for every
feature prevents undocumented scope creep; requiring a test case per
requirement enforces traceability between what was asked for and what was
actually verified — for example, REQ-CAN-01 to REQ-CAN-03 map directly to
the five cancellation tests added in Step 8. The commit-history rule
provides evidence that work was performed incrementally with a visible
process, rather than delivered as a single unexplained change. The release
rule ties every deployment decision back to documented exit criteria
(all tests passing, no unresolved high-severity defects) rather than
informal confidence.

## Defect Log

| Defect ID | Description | Severity | Status | Found In | Fixed In |
|---|---|---|---|---|---|
| DEF-001 (sample) | If `BookAppointment`'s return type had been changed directly to `Appointment` instead of extending `BookingResult`, all 23 existing Week 2 tests would have failed to compile, since they assert against `BookingResult.Success` | High | N/A — avoided by design | Design review before implementation | Extended `BookingResult` with an optional `Appointment` property instead |

No defects were found during execution of the Week 3 test suite (28/28
passed on first run). The entry above is included as a sample per the lab
instructions, documenting a design risk that was identified and avoided
during implementation rather than discovered through test failure.