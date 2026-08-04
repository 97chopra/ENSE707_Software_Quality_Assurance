# Requirements Traceability Matrix — Library Book Reservation System

| Requirement ID | Requirement Summary | Acceptance Criteria | Test Case | Status |
|---|---|---|---|---|
| REQ-LIB-01 | Reserve only available book | AC-01 | ReserveBook_AvailableBookAndValidMember_ReturnsSuccess; ReserveBook_AvailableBook_MarksBookAsReserved | Passed |
| REQ-LIB-02 | Reject empty member ID | AC-02 | Member_EmptyMemberId_ThrowsException | Passed |
| REQ-LIB-03 | Reject already reserved book | AC-03 | ReserveBook_AlreadyReservedBook_ReturnsFailure | Passed |
| REQ-LIB-04 | Return clear success or failure message | AC-04 | ReserveBook_NullBook_ReturnsClearFailureMessage; ReserveBook_NullMember_ReturnsClearFailureMessage | Passed |

Traceability helps the team check whether each requirement has test evidence. It also supports change management because if a requirement changes, the related test cases can be identified, reviewed, and updated.
## Step 10: Requirement Change Discussion — REQ-LIB-05

**New rule:** A member cannot reserve more than one book at the same time.

**Which class may need to change?**
`ReservationService` needs to change — it currently has no concept of a member's 
existing reservations, so it can't check "does this member already hold a book?" 
This would require tracking active reservations per member, e.g. by adding a 
`Dictionary<string, Book>` (member ID → reserved book) to `ReservationService`.

**Which test cases need to be added?**
- `ReserveBook_MemberAlreadyHasActiveReservation_ReturnsFailure` — verifies a 
  second `ReserveBook` call for the same member (on a different book) fails 
  with a clear message.
- A regression test confirming the existing one-member-one-book behaviour 
  still passes.

**What should be added to the RTM?**

| Requirement ID | Requirement Summary | Acceptance Criteria | Test Case | Status |
|---|---|---|---|---|
| REQ-LIB-05 | Prevent multiple concurrent reservations per member | AC-05 | ReserveBook_MemberAlreadyHasActiveReservation_ReturnsFailure | Not yet implemented |