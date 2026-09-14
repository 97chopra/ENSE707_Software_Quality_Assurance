using System;
using System.Collections.Generic;
using System.Text;

using FoodDeliveryCheckout.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorDiscountAndFeeTests
{
    [DataTestMethod]
    [DataRow(DeliveryZone.Local, 3.99)]
    [DataRow(DeliveryZone.Suburban, 6.99)]
    [DataRow(DeliveryZone.Rural, 12.99)]
    public void CalculateDeliveryFee_ValidZone_ReturnsExpectedFee(
        DeliveryZone zone,
        double expectedFee)
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        // Act
        decimal result = calculator.CalculateDeliveryFee(zone);

        // Assert
        Assert.AreEqual((decimal)expectedFee, result);
    }

    [DataTestMethod]
    [DataRow(100, CustomerType.Regular, 0)]
    [DataRow(100, CustomerType.Premium, 10)]
    [DataRow(100, CustomerType.Student, 15)]
    [DataRow(50, CustomerType.Premium, 5)]
    [DataRow(80, CustomerType.Student, 12)]
    public void CalculateDiscount_ValidSubtotalAndCustomerType_ReturnsExpectedDiscount(
        double subtotal,
        CustomerType customerType,
        double expectedDiscount)
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        // Act
        decimal result = calculator.CalculateDiscount((decimal)subtotal, customerType);

        // Assert
        Assert.AreEqual((decimal)expectedDiscount, result);
    }

    [DataTestMethod]
    [DataRow(100, 5)]
    [DataRow(50, 2.5)]
    [DataRow(20, 1)]
    public void CalculateServiceFee_ValidSubtotal_ReturnsFivePercent(
        double subtotal,
        double expectedFee)
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        // Act
        decimal result = calculator.CalculateServiceFee((decimal)subtotal);

        // Assert
        Assert.AreEqual((decimal)expectedFee, result);
    }

    [TestMethod]
    public void CalculateDiscount_NegativeSubtotal_ThrowsArgumentOutOfRangeException()
    {
        var calculator = new CheckoutCalculator();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => calculator.CalculateDiscount(-1, CustomerType.Regular));
    }

    [TestMethod]
    public void CalculateServiceFee_NegativeSubtotal_ThrowsArgumentOutOfRangeException()
    {
        var calculator = new CheckoutCalculator();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => calculator.CalculateServiceFee(-1));
    }
}
