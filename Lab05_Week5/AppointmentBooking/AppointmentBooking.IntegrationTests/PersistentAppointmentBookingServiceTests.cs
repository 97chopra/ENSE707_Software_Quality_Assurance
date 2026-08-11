using ENSE707_AppointmentBooking;
using AppointmentBooking.Persistence;

namespace AppointmentBooking.IntegrationTests
{
    [TestClass]
    public class PersistentAppointmentBookingServiceTests
    {
        private string _testFilePath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            // Unique file per test to keep parallel test runs isolated
            _testFilePath = Path.Combine(
                Path.GetTempPath(),
                $"bookings_{Guid.NewGuid()}.json");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_IsPersistedAndReadableByFreshRepository()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var repository = new JsonBookingRepository(_testFilePath);
            var service = new PersistentAppointmentBookingService(
                new AppointmentBookingService(), repository);

            BookingResult result = service.BookAppointment(request);

            Assert.IsTrue(result.Success);

            // Fresh repository instance pointing at the same file
            var freshRepository = new JsonBookingRepository(_testFilePath);
            var records = freshRepository.GetAll();

            Assert.AreEqual(1, records.Count);
            Assert.AreEqual("P001", records[0].PatientId);
            Assert.AreEqual("D001", records[0].DoctorId);
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_IsNotPersisted()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            var repository = new JsonBookingRepository(_testFilePath);
            var service = new PersistentAppointmentBookingService(
                new AppointmentBookingService(), repository);

            BookingResult result = service.BookAppointment(request);

            Assert.IsFalse(result.Success);

            var records = repository.GetAll();
            Assert.AreEqual(0, records.Count);
        }
    }
}