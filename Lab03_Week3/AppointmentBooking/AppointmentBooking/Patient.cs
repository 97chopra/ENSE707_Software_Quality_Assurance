namespace ENSE707_AppointmentBooking
{
    public class Patient
    {
        public string Id { get; }
        public string LegalName { get; }
        public string PreferredName { get; }

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

            // Business rule: a patient ID must follow a minimal valid format
            // (at least 2 characters) to be considered a real identifier,
            // preventing accidental single-character or placeholder IDs.
            if (id.Trim().Length < 2)
                throw new ArgumentException("Patient ID is not valid.");

            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Legal name is required.");

            Id = id;
            LegalName = legalName;
            PreferredName = preferredName;
        }
    }
}