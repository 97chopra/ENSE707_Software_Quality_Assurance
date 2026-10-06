using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Week11DefectLifeCycle.Core;

namespace Week11DefectLifeCycle.Tests
{
    [TestClass]
    public class OrderServiceDefectTests
    {
        // Retest for defect D-001: 20% off $200 must give $160
        [TestMethod]
        public void D001_ApplyDiscount_WhenPriceIs200AndDiscountIs20_Returns160()
        {
            // Arrange
            var service = new OrderService();

            // Act
            var result = service.ApplyDiscount(200m, 20m);

            // Assert
            Assert.AreEqual(160m, result);
        }

        // Retest for defect D-002: exactly $100 must be eligible for free shipping
        [TestMethod]
        public void D002_IsEligibleForFreeShipping_WhenOrderTotalIs100_ReturnsTrue()
        {
            // Arrange
            var service = new OrderService();

            // Act
            var result = service.IsEligibleForFreeShipping(100m);

            // Assert
            Assert.IsTrue(result);
        }

        // Regression: 0% discount must leave the price unchanged
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsZero_ReturnsOriginalPrice()
        {
            var service = new OrderService();

            var result = service.ApplyDiscount(100m, 0m);

            Assert.AreEqual(100m, result);
        }

        // Regression: 100% discount must make the price zero
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsOneHundred_ReturnsZero()
        {
            var service = new OrderService();

            var result = service.ApplyDiscount(100m, 100m);

            Assert.AreEqual(0m, result);
        }

        // Regression: 200 - 10% = 180, which is eligible, so shipping is waived
        [TestMethod]
        public void CalculateFinalTotal_WhenDiscountedPriceIsEligibleForFreeShipping_ReturnsTotalWithoutShipping()
        {
            var service = new OrderService();

            var result = service.CalculateFinalTotal(200m, 10m, 15m);

            Assert.AreEqual(180m, result);
        }

        // Regression: 80 - 10% = 72, not eligible, so 15 shipping is added (72 + 15 = 87)
        [TestMethod]
        public void CalculateFinalTotal_WhenDiscountedPriceIsNotEligibleForFreeShipping_AddsShipping()
        {
            var service = new OrderService();

            var result = service.CalculateFinalTotal(80m, 10m, 15m);

            Assert.AreEqual(87m, result);
        }

        // Rule 4: negative price is invalid
        // Note: ThrowsExactly replaces ThrowsException in MSTest 4.x
        [TestMethod]
        public void ApplyDiscount_WhenPriceIsNegative_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<ArgumentException>(() =>
                service.ApplyDiscount(-1m, 10m));
        }

        // Rule 4: negative discount is invalid
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsNegative_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<ArgumentException>(() =>
                service.ApplyDiscount(100m, -5m));
        }

        // Rule 4: discount over 100% is invalid
        [TestMethod]
        public void ApplyDiscount_WhenDiscountIsGreaterThan100_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<ArgumentException>(() =>
                service.ApplyDiscount(100m, 101m));
        }

        // Rule 4: negative shipping cost is invalid
        [TestMethod]
        public void CalculateFinalTotal_WhenShippingCostIsNegative_ThrowsArgumentException()
        {
            var service = new OrderService();

            Assert.ThrowsExactly<ArgumentException>(() =>
                service.CalculateFinalTotal(100m, 10m, -5m));
        }
    }
}