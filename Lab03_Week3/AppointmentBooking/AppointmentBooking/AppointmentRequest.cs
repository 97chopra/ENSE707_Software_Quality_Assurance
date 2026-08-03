namespace ENSE707_AppointmentBooking
{
    public class AppointmentRequest
    {
        public Patient Patient { get; }
        public Doctor Doctor { get; }
        public DateTime RequestedDate { get; }

        public AppointmentRequest(Patient patient, Doctor doctor, DateTime requestedDate)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));

            // Business rule: clinic requires at least one day's notice.
            // Same-day bookings are no longer allowed.
            if (requestedDate.Date <= DateTime.Today)
                throw new ArgumentException("Appointments require at least one day's notice.");

            RequestedDate = requestedDate;
        }
    }
}