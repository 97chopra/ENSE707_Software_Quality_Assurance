using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrescriptionRefill
{
    public class RefillService
    {
        private const int UrgentThresholdDays = 2;

        public RefillResult SubmitRequest(Patient patient, string medicineName)
        {
            if (patient == null)
                return new RefillResult(false, "Refill request failed: patient details are required.");

            if (string.IsNullOrWhiteSpace(medicineName))
                return new RefillResult(false, "Refill request failed: medicine name is required.");

            bool isUrgent = patient.DaysOfMedicineRemaining <= UrgentThresholdDays;

            var request = new RefillRequest(patient.Id, medicineName, isUrgent);

            string urgencyNote = isUrgent ? " This request is marked as URGENT." : "";
            string message = $"Refill request successful: '{request.MedicineName}' has been submitted for {patient.FullName}.{urgencyNote}";

            return new RefillResult(true, message, isUrgent);
        }
    }
}