# Exploratory Testing

## Charter
Explore the appointment-booking workflow for ways that a booking could be duplicated, an available-slot count could become incorrect, or a receptionist could receive misleading feedback.

## Observations

| Observation | Expected? | Evidence | Requirement clear? | Possible risk / next question |
|---|---|---|---|---|
| Booking the same patient/doctor/date combination is allowed with no duplicate check | Unexpected | Console run: P010, Dr Dup, 2 slots, booked successfully, remaining slots 1 (no error or warning about duplicates) | Unclear — no requirement states duplicate bookings should be blocked | Should the system prevent double-booking the same patient with the same doctor on the same day? Needs stakeholder clarification. |
| Entering non-numeric text for "Available slots" crashes the whole console app with a raw stack trace | Unexpected | Console run: Doctor "Dr Bad", slots "abc" → unhandled `System.FormatException`, full .NET stack trace shown, process exited with code -532462766 | Requirement gap — nothing specifies input validation for the console layer | This is a confirmed defect: input validation is missing at the UI/console boundary, even though the domain classes below are well validated. |
| Entering an extremely large slot count (beyond `int` range) also crashes the app | Unexpected | Console run: Doctor "Dr Big", slots "999999999999999" → unhandled `System.OverflowException`, same style of crash, process exited with code -532462766 | Requirement gap — same root cause as above | Same defect class as above; both stem from unguarded `int.Parse()` calls in `Program.cs`. |
| Leaving Patient ID blank is handled gracefully with a clear message | Expected | Console run: blank Patient ID → "Validation error: Patient ID is required." Process exited normally with code 0 | Yes — matches `Patient` constructor's validation | None — this is working as intended, a positive finding. |

## Product risk assessment

| Risk | Likelihood | Impact | Priority | Testing response |
|---|---|---|---|---|
| Receptionist types non-numeric text into "Available slots" or "Days from today" and crashes the app mid-booking | High (very easy to mistype in a live console) | High (application crashes, no booking completed, unclear to a non-technical user what went wrong) | High | Add input validation around `int.Parse()` in `Program.cs` (e.g. `int.TryParse`), with a friendly re-prompt instead of letting the exception propagate. Add tests that simulate invalid console input. |
| Extremely large numeric input (accidental extra digit) crashes the app the same way | Medium (less likely than a typo, but plausible on a numeric pad) | High (same crash behaviour as above) | Medium-High | Same fix as above — `TryParse` plus a sensible upper bound check on slot counts. |
| No duplicate-booking prevention exists | Low-Medium (depends on real-world usage pattern) | Medium (could lead to a doctor being double-booked if the same request is accidentally submitted twice) | Medium | Requires clarification from stakeholders on whether duplicate detection is in scope; if so, add a check (e.g. against existing bookings) before confirming. |

## Reflection questions

**If you could add only one further test before release, which risk would you test first and why?**
I would prioritise the non-numeric/invalid-number input crash. It is the highest-likelihood, highest-impact risk: any receptionist can trigger it by a simple typo, and the result is a hard crash with a raw stack trace rather than a usable error message — this would be highly disruptive in a live clinic setting compared to the lower-likelihood duplicate-booking scenario.

**Did exploratory testing reveal a confirmed defect, a requirement gap, a possible change request, or something that needs stakeholder clarification?**
It revealed a mix of all four. The crash on invalid numeric input is a confirmed defect (the code does not defend against bad user input at the console boundary). The lack of duplicate-booking prevention is a requirement gap that needs stakeholder clarification, since no rule set currently addresses it. If stakeholders decide duplicate prevention should exist, that would become a change request.

**Why is an unexpected behaviour not automatically a software defect?**
An unexpected behaviour only becomes a defect once it is checked against what the system was actually required to do. Some unexpected behaviours (like allowing repeated identical bookings) may simply reflect an unstated or ambiguous requirement rather than a coding error — the correct fix might be a requirements discussion, not a code change. A defect specifically means the software fails to meet an established, agreed specification; until that specification is confirmed, an unexpected result is just an open question.