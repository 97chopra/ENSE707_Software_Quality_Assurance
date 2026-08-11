using System;
using System.Collections.Generic;
using System.Text;

using ENSE707_AppointmentBooking;

namespace ENSE707_AppointmentBooking.Tests
{
    [TestClass]
    public class AppointmentBookingServiceTests
    {
        [TestMethod]
        public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(request);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(request);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            service.BookAppointment(request);

            Assert.AreEqual(1, doctor.AvailableSlots);
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            service.BookAppointment(request);

            Assert.AreEqual(0, doctor.AvailableSlots);
        }

        [TestMethod]
        public void Doctor_WhenIdIsEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Doctor("", "Dr Mark", 2));
        }

        [TestMethod]
        public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Doctor("D001", "Dr Mark", -1));
        }

        [TestMethod]
        public void Doctor_WhenFullNameIsEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Doctor("D001", "", 2));
        }

        [TestMethod]
        public void Patient_WhenLegalNameIsEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Patient("P001", ""));
        }

        [TestMethod]
        public void AppointmentRequest_WhenPatientIsNull_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);

            Assert.Throws<ArgumentNullException>(() =>
                new AppointmentRequest(null, doctor, DateTime.Today.AddDays(1)));
        }

        [TestMethod]
        public void AppointmentRequest_WhenDoctorIsNull_ThrowsException()
        {
            var patient = new Patient("P001", "Diana William");

            Assert.Throws<ArgumentNullException>(() =>
                new AppointmentRequest(patient, null, DateTime.Today.AddDays(1)));
        }

        [TestMethod]
        public void Doctor_ReserveSlot_WhenNoAvailableSlots_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);

            Assert.Throws<InvalidOperationException>(() =>
                doctor.ReserveSlot());
        }

        [TestMethod]
        public void Patient_WhenIdIsEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Patient("", "Diana William"));
        }

        [TestMethod]
        public void Patient_WhenPreferredNameExists_DisplayNameUsesPreferredName()
        {
            var patient = new Patient("P001", "Diana William", "Aroha");

            Assert.AreEqual("Aroha", patient.DisplayName);
        }

        [TestMethod]
        public void Patient_WhenPreferredNameMissing_DisplayNameUsesLegalName()
        {
            var patient = new Patient("P001", "Diana William");

            Assert.AreEqual("Diana William", patient.DisplayName);
        }

        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");

            Assert.Throws<ArgumentException>(() =>
                new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(-1)));

        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William", "Aroha");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(request);

            StringAssert.Contains(result.Message, "Appointment booked successfully");
            StringAssert.Contains(result.Message, "Aroha");
        }

        [TestMethod]
        public void BookAppointment_WhenNoSlots_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(request);

            StringAssert.Contains(result.Message, "no available slots");
        }

        [TestMethod]
        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(2, true)]
        public void HasAvailableSlot_BoundaryCases(int availableSlots, bool expected)
        {
            var doctor = new Doctor("D001", "Dr Mark", availableSlots);
            Assert.AreEqual(expected, doctor.HasAvailableSlot());
        }

        [TestMethod]
        [DataRow(-1, true)]   // yesterday -> should throw
        [DataRow(0, false)]   // today -> should NOT throw
        [DataRow(1, false)]   // tomorrow -> should NOT throw
        public void AppointmentRequest_DateBoundaryCases(int daysFromToday, bool expectThrow)
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var date = DateTime.Today.AddDays(daysFromToday);

            if (expectThrow)
            {
                Assert.Throws<ArgumentException>(() =>
                    new AppointmentRequest(patient, doctor, date));
            }
            else
            {
                var request = new AppointmentRequest(patient, doctor, date);
                Assert.AreEqual(date, request.RequestedDate);
            }
        }
    }
}