using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Week09QualityLab.Core;

namespace Week09QualityLab.Tests
{
    // Test class for the DiscountCalculator business logic.
    // Following TDD: each test below was written BEFORE the corresponding
    // production code, driving the implementation one requirement at a time.
    [TestClass]
    public class DiscountCalculatorTests
    {
        // Test 1: A "Regular" customer should get no discount at all.
        [TestMethod]
        public void CalculateFinalPrice_ShouldReturnOriginalPrice_ForRegularCustomer()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new DiscountCalculator();

            // Act: call the method under test with a Regular customer
            decimal result = calculator.CalculateFinalPrice(100m, "Regular");

            // Assert: final price should equal the original price (no discount)
            Assert.AreEqual(100m, result);
        }

        // Test 2: A "Premium" customer should get a 10% discount.
        [TestMethod]
        public void CalculateFinalPrice_ShouldApplyTenPercentDiscount_ForPremiumCustomer()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new DiscountCalculator();

            // Act: call the method under test with a Premium customer
            decimal result = calculator.CalculateFinalPrice(100m, "Premium");

            // Assert: 10% discount means final price should be 90
            Assert.AreEqual(90m, result);
        }

        // Test 3: A "VIP" customer should get a 20% discount.
        [TestMethod]
        public void CalculateFinalPrice_ShouldApplyTwentyPercentDiscount_ForVipCustomer()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new DiscountCalculator();

            // Act: call the method under test with a VIP customer
            decimal result = calculator.CalculateFinalPrice(100m, "VIP");

            // Assert: 20% discount means final price should be 80
            Assert.AreEqual(80m, result);
        }

        // Test 4: A negative original price is invalid and should throw an exception.
        [TestMethod]
        public void CalculateFinalPrice_ShouldThrowException_WhenOriginalPriceIsNegative()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new DiscountCalculator();

            // Act and Assert: calling with a negative price should throw an ArgumentException
            // Note: using ThrowsExactly instead of the older ThrowsException,
            // since ThrowsException was removed in newer MSTest versions.
            Assert.ThrowsExactly<ArgumentException>(() =>
                calculator.CalculateFinalPrice(-50m, "Regular"));
        }
    }
}