namespace ENSE707_AppointmentBooking
{
    public class BookingResult
    {
        public bool Success { get; }
        public string Message { get; }

        // Populated only on a successful booking, so the caller can
        // later cancel this specific appointment. Defaults to null to
        // keep existing calls to this constructor unaffected.
        public Appointment Appointment { get; }

        public BookingResult(bool success, string message, Appointment appointment = null)
        {
            Success = success;
            Message = message;
            Appointment = appointment;
        }
    }
}