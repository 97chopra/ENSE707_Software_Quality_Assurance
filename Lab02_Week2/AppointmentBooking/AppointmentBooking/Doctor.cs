namespace ENSE707_AppointmentBooking
{
    public class Doctor
    {
        // Read-only after construction — prevents external code from
        // reassigning identity fields after the object is created.
        public string Id { get; }
        public string FullName { get; }

        // Private setter ensures slot count can only change through
        // ReserveSlot(), not by direct external assignment.
        public int AvailableSlots { get; private set; }

        public Doctor(string id, string fullName, int availableSlots)
        {
            // Guard clauses validate the object at the point of creation,
            // so an invalid Doctor can never exist in the system.
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Doctor ID is required.");

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Doctor name is required.");

            if (availableSlots < 0)
                throw new ArgumentException("Available slots cannot be negative.");

            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
        }

        // Encapsulates the "is a booking possible?" business rule.
        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0;
        }

        // Encapsulates the "reserve a slot" business rule, guarding
        // against reserving when no slots remain.
        public void ReserveSlot()
        {
            if (!HasAvailableSlot())
                throw new InvalidOperationException("No appointment slots are available.");

            AvailableSlots--;
        }
    }
}