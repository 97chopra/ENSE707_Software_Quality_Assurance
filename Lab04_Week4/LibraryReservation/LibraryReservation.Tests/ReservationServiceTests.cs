using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryReservation;

namespace LibraryReservation.Tests
{
    [TestClass]
    public class ReservationServiceTests
    {
        [TestMethod]
        public void ReserveBook_AvailableBookAndValidMember_ReturnsSuccess()
        {
            var book = new Book("B001", "Software Testing Basics");
            var member = new Member("M001", "Aroha Smith");
            var service = new ReservationService();

            ReservationResult result = service.ReserveBook(book, member);

            Assert.IsTrue(result.Success);
            StringAssert.Contains(result.Message, "Reservation successful");
        }

        [TestMethod]
        public void ReserveBook_AvailableBook_MarksBookAsReserved()
        {
            var book = new Book("B001", "Software Testing Basics");
            var member = new Member("M001", "Aroha Smith");
            var service = new ReservationService();

            service.ReserveBook(book, member);

            Assert.IsTrue(book.IsReserved);
        }

        [TestMethod]
        public void Member_EmptyMemberId_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new Member("", "Aroha Smith"));
        }

        [TestMethod]
        public void ReserveBook_AlreadyReservedBook_ReturnsFailure()
        {
            var book = new Book("B001", "Software Testing Basics");
            var member1 = new Member("M001", "Aroha Smith");
            var member2 = new Member("M002", "John Chen");
            var service = new ReservationService();

            service.ReserveBook(book, member1);
            ReservationResult result = service.ReserveBook(book, member2);

            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "already reserved");
        }

        [TestMethod]
        public void ReserveBook_NullBook_ReturnsClearFailureMessage()
        {
            var member = new Member("M001", "Aroha Smith");
            var service = new ReservationService();

            ReservationResult result = service.ReserveBook(null, member);

            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "book details are required");
        }

        [TestMethod]
        public void ReserveBook_NullMember_ReturnsClearFailureMessage()
        {
            var book = new Book("B001", "Software Testing Basics");
            var service = new ReservationService();

            ReservationResult result = service.ReserveBook(book, null);

            Assert.IsFalse(result.Success);
            StringAssert.Contains(result.Message, "member details are required");
        }
    }
}
