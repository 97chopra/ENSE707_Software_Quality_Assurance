# Requirements Traceability Matrix — Clinic Prescription Refill Request System

| Requirement ID | Requirement Summary | Acceptance Criteria | Test Case | Status |
|---|---|---|---|---|
| REQ-RX-01 | Allow valid patient to submit a refill request | AC-RX-01: Given a valid patient and medicine name, when the patient submits a request, then it succeeds. | SubmitRequest_ValidPatientAndMedicine_ReturnsSuccess | Passed |
| REQ-RX-02 | Reject empty patient ID | AC-RX-02: Given an empty patient ID, when a Patient is created, then an ArgumentException is thrown. | Patient_EmptyPatientId_ThrowsException | Passed |
| REQ-RX-03 | Reject empty medicine name | AC-RX-03: Given an empty medicine name, when a request is submitted, then it fails with a clear message. | SubmitRequest_EmptyMedicineName_ReturnsFailure | Passed |
| REQ-RX-04 | Mark request urgent if ≤2 days of medicine remaining | AC-RX-04: Given a patient with two or fewer days remaining, when a request is submitted, then it is marked urgent. | SubmitRequest_TwoOrFewerDaysRemaining_MarksRequestAsUrgent; SubmitRequest_MoreThanTwoDaysRemaining_NotUrgent | Passed |
| REQ-RX-05 | Return clear success/failure message for every request | AC-RX-05: Every refill request attempt returns a message that explains the result. | SubmitRequest_ResultMessage_IsClear; SubmitRequest_NullPatient_ReturnsFailure | Passed |

Traceability helps the team check whether each requirement has test evidence. It also supports change management because if a requirement changes, the related test cases can be identified, reviewed, and updated.