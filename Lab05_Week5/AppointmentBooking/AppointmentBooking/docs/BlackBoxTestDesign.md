# Black-Box Test Design

## Rule set A – Doctor availability

### Equivalence partitions (doctor slots)
| Partition | Representative value | Expected |
|---|---|---|
| Invalid (negative) | -1 | Constructor throws `ArgumentException` |
| Valid, unavailable (zero) | 0 | Doctor created; `HasAvailableSlot()` = false |
| Valid, available | 1 or more (e.g. 2) | Doctor created; `HasAvailableSlot()` = true |

### Boundary values (invalid/unavailable/available transition)
| Value | Boundary | Expected |
|---|---|---|
| -1 | Just below 0 | Throws `ArgumentException` |
| 0 | Exact boundary (valid but no capacity) | No exception; `HasAvailableSlot()` = false |
| 1 | Just above 0 | No exception; `HasAvailableSlot()` = true |

## Rule set B – Appointment date

### Equivalence partitions (appointment dates)
| Partition | Representative value | Expected |
|---|---|---|
| Invalid (past) | Yesterday | Constructor throws `ArgumentException` |
| Valid (today) | Today | Constructor succeeds |
| Valid (future) | Tomorrow or later | Constructor succeeds |

### Boundary values (past/today transition)
| Value | Boundary | Expected |
|---|---|---|
| Today - 1 | Just before today | Throws `ArgumentException` |
| Today | Exact boundary | No exception |
| Today + 1 | Just after today | No exception |

## Rule set C – Booking result decision table

| Case | Doctor has available slot? | Request valid (date not past)? | Result |
|---|---|---|---|
| 1 | No | Yes | Booking fails; slots unchanged; message explains no availability |
| 2 | Yes | Yes | Booking succeeds; slots decrease by 1; helpful success message returned |
| 3 | Yes / No | No (past date) | Exception thrown at `AppointmentRequest` construction; `BookAppointment` never called |
| 4 | N/A | N/A | Every call to `BookAppointment` returns a non-null `BookingResult` with a message, whether it succeeds or fails |

## Cases already covered vs. missing from baseline suite

| Designed case | Already in baseline suite? |
|---|---|
| Negative slots throws exception | Yes (`Doctor_WhenAvailableSlotsIsNegative_ThrowsException`) |
| Zero slots → booking fails | Yes (`BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure`) |
| One+ slots → booking succeeds |  Yes (`BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess`) |
| Past date throws exception |  Yes (`AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException`) |
| **Today is a valid boundary date** | Missing — not explicitly tested |
| Future date is valid |  Implied by several tests using `AddDays(1)` |
| Successful booking decreases slots by exactly 1 | Yes (`BookAppointment_WhenSuccessful_DecreasesAvailableSlots`) |
| Every booking attempt returns a non-null message |  Yes (partially, via the two "helpful message" tests) |