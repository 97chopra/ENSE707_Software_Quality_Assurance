namespace ENSE707_AppointmentBooking
{
    public class AppointmentBookingService
    {
        public BookingResult BookAppointment(AppointmentRequest request)
        {
            if (request == null)
                return new BookingResult(false, "Appointment request is missing.");

            if (!request.Doctor.HasAvailableSlot())
            {
                return new BookingResult(
                    false,
                    $"Appointment cannot be booked because {request.Doctor.FullName} has no available slots.");
            }

            request.Doctor.ReserveSlot();

            var appointment = new Appointment(
                Guid.NewGuid().ToString(),
                request.Doctor,
                request.Patient,
                request.RequestedDate);

            return new BookingResult(
                true,
                $"Appointment booked successfully for {request.Patient.DisplayName} with {request.Doctor.FullName}.",
                appointment);
        }

        // Cancels an existing appointment and releases the doctor's slot.
        // Delegates the "already cancelled" business rule to Appointment.Cancel(),
        // keeping that rule in one place rather than duplicating it here.
        public void CancelAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            appointment.Cancel();
            appointment.Doctor.ReleaseSlot();
        }
    }
}