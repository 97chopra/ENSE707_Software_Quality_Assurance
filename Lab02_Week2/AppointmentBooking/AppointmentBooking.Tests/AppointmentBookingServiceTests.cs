using ENSE707_AppointmentBooking;

namespace AppointmentBooking.Tests
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
            Assert.ThrowsException<ArgumentException>(() =>
                new Doctor("", "Dr Mark", 2));
        }

        [TestMethod]
        public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Doctor("D001", "Dr Mark", -1));
        }

        [TestMethod]
        public void Patient_WhenIdIsEmpty_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
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

            Assert.ThrowsException<ArgumentException>(() =>
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

        // ===== Copilot-suggested tests (Step 15) =====

        // Copilot-suggested test: Reliability - null request handling
        [TestMethod]
        public void BookAppointment_NullRequest_ReturnsFailureAndHelpfulMessage()
        {
            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(null);

            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "missing");
        }

        // Copilot-suggested test: Reliability - null patient guard
        [TestMethod]
        public void AppointmentRequest_NullPatient_ThrowsArgumentNullException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 1);

            Assert.ThrowsException<ArgumentNullException>(() =>
                new AppointmentRequest(null, doctor, DateTime.Today.AddDays(1)));
        }

        // Copilot-suggested test: Reliability - null doctor guard
        [TestMethod]
        public void AppointmentRequest_NullDoctor_ThrowsArgumentNullException()
        {
            var patient = new Patient("P001", "Diana William");

            Assert.ThrowsException<ArgumentNullException>(() =>
                new AppointmentRequest(patient, null, DateTime.Today.AddDays(1)));
        }

        // Copilot-suggested test: Usability + Cultural quality - international names
        [TestMethod]
        public void BookAppointment_WithInternationalNames_IncludesUnicodeNamesInMessage()
        {
            var doctor = new Doctor("D002", "Dr Leila", 1);
            var patient = new Patient("P002", "Elodie Gerard", "Elo");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            var result = service.BookAppointment(request);

            Assert.IsTrue(result.Success);
            StringAssert.Contains(result.Message, "Elo");
        }

        // Copilot-suggested test: Cultural quality - whitespace-only preferred name
        [TestMethod]
        public void Patient_WhitespacePreferredName_FallsBackToLegalName()
        {
            var patient = new Patient("P004", "Legal Name", "   ");

            Assert.AreEqual("Legal Name", patient.DisplayName);
        }

        // ===== Final Task 1/2: Business rule tests =====

        // Updated test: same-day booking is no longer allowed (was previously permitted)
        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsToday_ThrowsException()
        {
            var doctor = new Doctor("D004", "Dr Now", 1);
            var patient = new Patient("P005", "Today Test");

            Assert.ThrowsException<ArgumentException>(() =>
                new AppointmentRequest(patient, doctor, DateTime.Today));
        }

        // New business rule test: booking for tomorrow is accepted
        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsTomorrow_IsAccepted()
        {
            var doctor = new Doctor("D004", "Dr Now", 1);
            var patient = new Patient("P005", "Tomorrow Test");

            var req = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            Assert.AreEqual(DateTime.Today.AddDays(1), req.RequestedDate.Date);
        }

        // New business rule test: doctor cannot be created with more than max daily appointments
        [TestMethod]
        public void Doctor_WhenAvailableSlotsExceedsMaximum_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Doctor("D006", "Dr Overloaded", 9));
        }

        // New business rule test: doctor can be created at exactly the maximum limit
        [TestMethod]
        public void Doctor_WhenAvailableSlotsEqualsMaximum_IsAccepted()
        {
            var doctor = new Doctor("D007", "Dr AtLimit", 8);

            Assert.AreEqual(8, doctor.AvailableSlots);
        }

        // New business rule test: a single-character patient ID is rejected as invalid
        [TestMethod]
        public void Patient_WhenIdIsTooShort_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Patient("P", "Diana William"));
        }

        // New business rule test: booking message clearly names the doctor (actionable/clear)
        [TestMethod]
        public void BookAppointment_FailureMessage_MentionsDoctorName()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(request);

            StringAssert.Contains(result.Message, "Dr Mark");
        }
    }
}