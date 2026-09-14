using System;
using System.Collections.Generic;
using System.Text;

using FoodDeliveryCheckout.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorTotalTests
{
    [TestMethod]
    public void CalculateTotal_RegularCustomerLocalDelivery_ReturnsExpectedTotal()
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Burger", 20.00m, 1),
            new OrderItem("Drink", 5.00m, 1)
        };

        // Act
        decimal result = calculator.CalculateTotal(
            items,
            DeliveryZone.Local,
            CustomerType.Regular);

        // Assert
        Assert.AreEqual(34.78m, result);
    }

    [TestMethod]
    public void CalculateTotal_PremiumCustomerSuburbanDelivery_ReturnsExpectedTotal()
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Pizza", 30.00m, 2)
        };

        // Act
        decimal result = calculator.CalculateTotal(
            items,
            DeliveryZone.Suburban,
            CustomerType.Premium);

        // Assert
        Assert.AreEqual(73.59m, result);
    }

    [TestMethod]
    public void CalculateTotal_StudentCustomerRuralDelivery_ReturnsExpectedTotal()
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Sushi Pack", 15.00m, 2),
            new OrderItem("Juice", 4.00m, 1)
        };

        // Act
        decimal result = calculator.CalculateTotal(
            items,
            DeliveryZone.Rural,
            CustomerType.Student);

        // Assert
        Assert.AreEqual(50.13m, result);
    }

    [TestMethod]
    public void CalculateTotal_SubtotalBelowMinimum_ThrowsInvalidOperationException()
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Small Cookie", 5.00m, 1)
        };

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(
            () => calculator.CalculateTotal(
                items,
                DeliveryZone.Local,
                CustomerType.Regular));
    }
}
