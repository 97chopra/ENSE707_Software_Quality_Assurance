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