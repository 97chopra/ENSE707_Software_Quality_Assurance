using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Week09QualityLab.Core;

namespace Week09QualityLab.Tests
{
    // Test class for the ShippingCalculator business logic.
    // Following TDD: this test is written BEFORE the ShippingCalculator class exists.
    [TestClass]
    public class ShippingCalculatorTests
    {
        // Test 1: Standard shipping on an order under $100 should cost $10.
        [TestMethod]
        public void CalculateShippingCost_ShouldReturnTenDollars_ForStandardShipping_WhenOrderIsUnderHundred()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new ShippingCalculator();

            // Act: call the method under test with Standard shipping on a $50 order
            decimal result = calculator.CalculateShippingCost(50m, "Standard");

            // Assert: standard shipping should cost $10
            Assert.AreEqual(10m, result);
        }

        // Test 2: Express shipping on an order under $100 should cost $20.
        [TestMethod]
        public void CalculateShippingCost_ShouldReturnTwentyDollars_ForExpressShipping_WhenOrderIsUnderHundred()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new ShippingCalculator();

            // Act: call the method under test with Express shipping on a $50 order
            decimal result = calculator.CalculateShippingCost(50m, "Express");

            // Assert: express shipping should cost $20
            Assert.AreEqual(20m, result);
        }

        // Test 3: Orders over $100 get free standard shipping.
        [TestMethod]
        public void CalculateShippingCost_ShouldReturnZero_ForStandardShipping_WhenOrderIsOverHundred()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new ShippingCalculator();

            // Act: call the method under test with Standard shipping on a $150 order
            decimal result = calculator.CalculateShippingCost(150m, "Standard");

            // Assert: orders over $100 get free standard shipping
            Assert.AreEqual(0m, result);
        }

        // Test 4: An unrecognised shipping type is invalid and should throw an exception.
        [TestMethod]
        public void CalculateShippingCost_ShouldThrowException_WhenShippingTypeIsInvalid()
        {
            // Arrange: set up the calculator instance we're testing
            var calculator = new ShippingCalculator();

            // Act and Assert: an unknown shipping type should throw an ArgumentException
            Assert.ThrowsExactly<ArgumentException>(() =>
                calculator.CalculateShippingCost(50m, "Overnight"));
        }
    }
}