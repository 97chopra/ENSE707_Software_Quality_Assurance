using FoodDeliveryCheckout.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorSubtotalTests
{
    [TestMethod]
    public void GetLineTotal_ValidItem_ReturnsUnitPriceTimesQuantity()
    {
        // Arrange
        OrderItem item = new OrderItem("Chicken Burger", 12.50m, 2);

        // Act
        decimal result = item.GetLineTotal();

        // Assert
        Assert.AreEqual(25.00m, result);
    }

    [TestMethod]
    public void CalculateSubtotal_MultipleItems_ReturnsCorrectSubtotal()
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Chicken Burger", 12.50m, 2),
            new OrderItem("Fries", 4.00m, 1),
            new OrderItem("Drink", 3.50m, 2)
        };

        // Act
        decimal result = calculator.CalculateSubtotal(items);

        // Assert
        Assert.AreEqual(36.00m, result);
    }

    [TestMethod]
    public void CalculateSubtotal_EmptyItemList_ThrowsArgumentException()
    {
        // Arrange
        var calculator = new CheckoutCalculator();
        var items = new List<OrderItem>();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(
            () => calculator.CalculateSubtotal(items));
    }

    [TestMethod]
    public void CalculateSubtotal_NullItemList_ThrowsArgumentException()
    {
        // Arrange
        var calculator = new CheckoutCalculator();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(
            () => calculator.CalculateSubtotal(null!));
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Constructor_InvalidItemName_ThrowsArgumentException(string? name)
    {
        Assert.ThrowsExactly<ArgumentException>(
            () => new OrderItem(name!, 10.00m, 1));
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Constructor_InvalidUnitPrice_ThrowsArgumentOutOfRangeException(double unitPrice)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new OrderItem("Burger", (decimal)unitPrice, 1));
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Constructor_InvalidQuantity_ThrowsArgumentOutOfRangeException(int quantity)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => new OrderItem("Burger", 10.00m, quantity));
    }
}