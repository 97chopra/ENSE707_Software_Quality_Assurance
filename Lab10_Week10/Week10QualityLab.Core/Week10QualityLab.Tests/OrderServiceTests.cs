using Microsoft.VisualStudio.TestTools.UnitTesting;
using QualityLab;
using Week10QualityLab.Core;

namespace QualityLab.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        // ---------------------------------------------------------------
        // Part 4 - Weak test.
        // 100 - 20 and 100 - 20% both give 80, so this passes even
        // with the buggy code and does NOT expose the defect.
        // ---------------------------------------------------------------
        [TestMethod]
        public void ApplyDiscount_WhenPriceIs100AndDiscountIs20_Returns80()
        {
            // Arrange
            var service = new OrderService();

            // Act
            var result = service.ApplyDiscount(100m, 20m);

            // Assert
            Assert.AreEqual(80m, result);
        }

        // ---------------------------------------------------------------
        // Part 5.1 - Stronger test that exposes the discount defect.
        // 20% of 200 = 40, so the correct result is 160.
        // Buggy code does 200 - 20 = 180, so this test should FAIL.
        // ---------------------------------------------------------------
        [TestMethod]
        public void ApplyDiscount_WhenPriceIs200AndDiscountIs20_Returns160()
        {
            // Arrange
            var service = new OrderService();

            // Act
            var result = service.ApplyDiscount(200m, 20m);

            // Assert
            Assert.AreEqual(160m, result);
        }

        // ---------------------------------------------------------------
        // Part 5.2 - Boundary test: 0% discount keeps the price unchanged.
        // May pass with buggy code too (100 - 0 = 100).
        // ---------------------------------------------------------------
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsZero_ReturnsOriginalPrice()
        {
            var service = new OrderService();

            var result = service.ApplyDiscount(100m, 0m);

            Assert.AreEqual(100m, result);
        }

        // ---------------------------------------------------------------
        // Part 5.2 - Boundary test: 100% discount makes the price 0.
        // May pass with buggy code too (100 - 100 = 0).
        // ---------------------------------------------------------------
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsOneHundred_ReturnsZero()
        {
            var service = new OrderService();

            var result = service.ApplyDiscount(100m, 100m);

            Assert.AreEqual(0m, result);
        }

        // ---------------------------------------------------------------
        // Part 5.3 - Invalid input tests.
        // ThrowsExactly passes only if an ArgumentException is thrown.
        // (Older MSTest used Assert.ThrowsException, removed in MSTest 4.)
        // The buggy code has no validation, so these should FAIL for now.
        // ---------------------------------------------------------------

        // A negative price makes no business sense.
        [TestMethod]
        public void ApplyDiscount_WhenPriceIsNegative_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<System.ArgumentException>(() =>
                service.ApplyDiscount(-1m, 10m));
        }

        // A negative discount percentage is not allowed.
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsNegative_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<System.ArgumentException>(() =>
                service.ApplyDiscount(100m, -5m));
        }

        // A discount above 100% would give a negative price.
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsGreaterThan100_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<System.ArgumentException>(() =>
                service.ApplyDiscount(100m, 101m));
        }

        // ---------------------------------------------------------------
        // Part 5.4 - Free shipping boundary tests.
        // Business rule: free shipping applies at $100 OR MORE.
        // ---------------------------------------------------------------

        // Exactly $100 is the boundary value and should get free shipping.
        // The buggy code uses "orderTotal > 100", so this test should FAIL.
        [TestMethod]
        public void IsEligibleForFreeShipping_WhenOrderTotalIs100_ReturnsTrue()
        {
            var service = new OrderService();

            var result = service.IsEligibleForFreeShipping(100m);

            Assert.IsTrue(result);
        }

        // Just below the boundary ($99) should NOT get free shipping.
        // This should PASS even with the buggy code (99 > 100 is false).
        [TestMethod]
        public void IsEligibleForFreeShipping_WhenOrderTotalIs99_ReturnsFalse()
        {
            var service = new OrderService();

            var result = service.IsEligibleForFreeShipping(99m);

            Assert.IsFalse(result);
        }

        // ---------------------------------------------------------------
        // Part 5.5 - Final total tests (discount + shipping together).
        // ---------------------------------------------------------------

        // 10% of 200 = 20, discounted price = 180 (>= 100, so free shipping).
        // Shipping (15) is waived, so the final total should be 180.
        // With the buggy code: 200 - 10 = 190 (also >= 100), total = 190,
        // so this test should FAIL.
        [TestMethod]
        public void CalculateFinalTotal_WhenDiscountedPriceIsEligibleForFreeShipping_ReturnsTotalWithoutShipping()
        {
            var service = new OrderService();

            var result = service.CalculateFinalTotal(200m, 10m, 15m);

            Assert.AreEqual(180m, result);
        }

        // 10% of 80 = 8, discounted price = 72 (< 100, so shipping applies).
        // Final total = 72 + 15 = 87.
        // With the buggy code: 80 - 10 = 70, total = 70 + 15 = 85,
        // so this test should also FAIL.
        [TestMethod]
        public void CalculateFinalTotal_WhenDiscountedPriceIsNotEligibleForFreeShipping_AddsShipping()
        {
            var service = new OrderService();

            var result = service.CalculateFinalTotal(80m, 10m, 15m);

            Assert.AreEqual(87m, result);
        }
    }
}