using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ENSE707_AppointmentBooking.Tests
{
    [TestClass]
    public class AppointmentCancellationTests
    {
        [TestMethod]
        public void CancelAppointment_ExistingAppointment_MarksAppointmentAsCancelled()
        {
            var doctor = new Doctor("D01", "Dr Smith", 5);
            var patient = new Patient("P01", "Jane Doe");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            var booking = service.BookAppointment(request);

            service.CancelAppointment(booking.Appointment);

            Assert.IsTrue(booking.Appointment.IsCancelled);
        }

        [TestMethod]
        public void CancelAppointment_ExistingAppointment_ReleasesDoctorSlot()
        {
            var doctor = new Doctor("D02", "Dr Lee", 3);
            var patient = new Patient("P02", "John Roe");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            var booking = service.BookAppointment(request);

            service.CancelAppointment(booking.Appointment);

            Assert.AreEqual(3, doctor.AvailableSlots);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CancelAppointment_NullAppointment_ThrowsException()
        {
            var service = new AppointmentBookingService();

            service.CancelAppointment(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CancelAppointment_AlreadyCancelledAppointment_ThrowsException()
        {
            var doctor = new Doctor("D03", "Dr Patel", 2);
            var patient = new Patient("P03", "Amy Chen");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            var booking = service.BookAppointment(request);
            service.CancelAppointment(booking.Appointment);

            service.CancelAppointment(booking.Appointment);
        }

        [TestMethod]
        public void BookAppointment_Success_ReturnsAppointmentWithCorrectDetails()
        {
            var doctor = new Doctor("D04", "Dr Nguyen", 4);
            var patient = new Patient("P04", "Sam Wells");
            var date = DateTime.Today.AddDays(2);
            var request = new AppointmentRequest(patient, doctor, date);
            var service = new AppointmentBookingService();

            var booking = service.BookAppointment(request);

            Assert.IsTrue(booking.Success);
            Assert.IsNotNull(booking.Appointment);
            Assert.AreEqual(doctor, booking.Appointment.Doctor);
            Assert.AreEqual(patient, booking.Appointment.Patient);
            Assert.AreEqual(date, booking.Appointment.AppointmentDate);
            Assert.IsFalse(booking.Appointment.IsCancelled);
        }
    }
}
