namespace FoodDeliveryCheckout.Core;

public class CheckoutCalculator
{
    public decimal CalculateSubtotal(List<OrderItem> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.", nameof(items));

        return items.Sum(item => item.GetLineTotal());
    }

    public decimal CalculateDeliveryFee(DeliveryZone zone)
    {
        return zone switch
        {
            DeliveryZone.Local => 3.99m,
            DeliveryZone.Suburban => 6.99m,
            DeliveryZone.Rural => 12.99m,
            _ => throw new ArgumentOutOfRangeException(nameof(zone))
        };
    }

    public decimal CalculateDiscount(decimal subtotal, CustomerType customerType)
    {
        if (subtotal < 0)
            throw new ArgumentOutOfRangeException(nameof(subtotal));

        decimal rate = customerType switch
        {
            CustomerType.Regular => 0.00m,
            CustomerType.Premium => 0.10m,
            CustomerType.Student => 0.15m,
            _ => throw new ArgumentOutOfRangeException(nameof(customerType))
        };

        return subtotal * rate;
    }

    public decimal CalculateServiceFee(decimal subtotal)
    {
        if (subtotal < 0)
            throw new ArgumentOutOfRangeException(nameof(subtotal));

        return subtotal * 0.05m;
    }

    public decimal CalculateTotal(
        List<OrderItem> items,
        DeliveryZone zone,
        CustomerType customerType)
    {
        decimal subtotal = CalculateSubtotal(items);

        if (subtotal < 10)
            throw new InvalidOperationException("Minimum order subtotal is $10.");

        decimal discount = CalculateDiscount(subtotal, customerType);
        decimal serviceFee = CalculateServiceFee(subtotal);
        decimal deliveryFee = CalculateDeliveryFee(zone);

        decimal taxableAmount = subtotal - discount + serviceFee + deliveryFee;
        decimal gst = taxableAmount * 0.15m;

        decimal total = taxableAmount + gst;

        return Math.Round(total, 2);
    }
}