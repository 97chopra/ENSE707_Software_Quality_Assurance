using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace PrescriptionRefill
{
    public class Patient
    {
        public string Id { get; }
        public string FullName { get; }
        public int DaysOfMedicineRemaining { get; }

        public Patient(string id, string fullName, int daysOfMedicineRemaining)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Patient ID is required.");

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Patient name is required.");

            if (daysOfMedicineRemaining < 0)
                throw new ArgumentException("Days of medicine remaining cannot be negative.");

            Id = id;
            FullName = fullName;
            DaysOfMedicineRemaining = daysOfMedicineRemaining;
        }
    }
}