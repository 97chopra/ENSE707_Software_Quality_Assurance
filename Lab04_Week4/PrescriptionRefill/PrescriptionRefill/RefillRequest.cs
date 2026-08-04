using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace PrescriptionRefill
{
    public class RefillRequest
    {
        public string PatientId { get; }
        public string MedicineName { get; }
        public bool IsUrgent { get; }

        public RefillRequest(string patientId, string medicineName, bool isUrgent)
        {
            if (string.IsNullOrWhiteSpace(patientId))
                throw new ArgumentException("Patient ID is required.");

            if (string.IsNullOrWhiteSpace(medicineName))
                throw new ArgumentException("Medicine name is required.");

            PatientId = patientId;
            MedicineName = medicineName;
            IsUrgent = isUrgent;
        }
    }
}
