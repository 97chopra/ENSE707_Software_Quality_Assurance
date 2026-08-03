using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSE707_AppointmentBooking
{
    public class Appointment
    {
        public string Id { get; }
        public Doctor Doctor { get; }
        public Patient Patient { get; }
        public DateTime AppointmentDate { get; }

        // Exposed with a private setter so cancellation can only occur
        // through the controlled Cancel() method below — this protects
        // the invariant that IsCancelled cannot be set arbitrarily from
        // outside the class.
        public bool IsCancelled { get; private set; }

        public Appointment(string id, Doctor doctor, Patient patient, DateTime appointmentDate)
        {
            // Guard clause: an appointment must have a valid identifier.
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Appointment ID is required.");

            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            AppointmentDate = appointmentDate;
            IsCancelled = false;
        }

        // Cancellation rule: an appointment can only be cancelled once.
        // Attempting to cancel an already-cancelled appointment indicates
        // a logic error upstream, so it is treated as an invalid operation
        // rather than being silently ignored.
        public void Cancel()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Appointment has already been cancelled.");

            IsCancelled = true;
        }
    }
}
