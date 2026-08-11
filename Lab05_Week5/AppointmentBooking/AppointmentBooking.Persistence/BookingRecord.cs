using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentBooking.Persistence;

public record BookingRecord(
    string PatientId,
    string DoctorId,
    DateTime RequestedDate,
    string Message);