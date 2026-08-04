using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrescriptionRefill;

namespace PrescriptionRefill.Tests
{
    [TestClass]
    public class RefillServiceTests
    {
        [TestMethod]
        public void SubmitRequest_ValidPatientAndMedicine_ReturnsSuccess()
        {
            var patient = new Patient("P001", "Aroha Smith", 10);
            var service = new RefillService();

            RefillResult result = service.SubmitRequest(patient, "Amoxicillin");

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void Patient_EmptyPatientId_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Patient("", "Aroha Smith", 10));
        }

        [TestMethod]
        public void SubmitRequest_EmptyMedicineName_ReturnsFailure()
        {
            var patient = new Patient("P001", "Aroha Smith", 10);
            var service = new RefillService();

            RefillResult result = service.SubmitRequest(patient, "");

            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "medicine name is required");
        }

        [TestMethod]
        public void SubmitRequest_TwoOrFewerDaysRemaining_MarksRequestAsUrgent()
        {
            var patient = new Patient("P001", "Aroha Smith", 2);
            var service = new RefillService();

            RefillResult result = service.SubmitRequest(patient, "Amoxicillin");

            Assert.IsTrue(result.IsUrgent);
        }

        [TestMethod]
        public void SubmitRequest_ResultMessage_IsClear()
        {
            var patient = new Patient("P001", "Aroha Smith", 10);
            var service = new RefillService();

            RefillResult result = service.SubmitRequest(patient, "Amoxicillin");

            StringAssert.Contains(result.Message, "Refill request successful");
            StringAssert.Contains(result.Message, "Amoxicillin");
        }

        [TestMethod]
        public void SubmitRequest_MoreThanTwoDaysRemaining_NotUrgent()
        {
            var patient = new Patient("P001", "Aroha Smith", 5);
            var service = new RefillService();

            RefillResult result = service.SubmitRequest(patient, "Amoxicillin");

            Assert.IsFalse(result.IsUrgent);
        }

        [TestMethod]
        public void SubmitRequest_NullPatient_ReturnsFailure()
        {
            var service = new RefillService();

            RefillResult result = service.SubmitRequest(null, "Amoxicillin");

            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "patient details are required");
        }
    }
}