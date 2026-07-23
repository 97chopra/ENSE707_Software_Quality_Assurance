namespace ENSE707_AppointmentBooking
{
    public class Patient
    {
        // Legal name is required and immutable after creation.
        public string Id { get; }
        public string LegalName { get; }

        // Preferred name is optional — supports cultural naming
        // preferences without losing the official legal identity.
        public string PreferredName { get; }

        // DisplayName decides what the system actually shows to staff,
        // falling back to LegalName only when no preferred name exists.
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PreferredName))
                    return LegalName;

                return PreferredName;
            }
        }

        public Patient(string id, string legalName, string preferredName = "")
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Patient ID is required.");

            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Legal name is required.");

            Id = id;
            LegalName = legalName;
            PreferredName = preferredName;
        }
    }
}