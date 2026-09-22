using System;

namespace Week09QualityLab.Core
{
    // Contains business logic for calculating a customer's final price after discount.
    public class DiscountCalculator
    {
        // Calculates the final price for a customer, based on customer type.
        // Regular: no discount. Premium: 10% discount. VIP: 20% discount.
        // Throws an exception if the original price is negative (invalid input).
        public decimal CalculateFinalPrice(decimal originalPrice, string customerType)
        {
            // Guard clause: reject negative prices before any discount logic runs.
            if (originalPrice < 0)
            {
                throw new ArgumentException("Original price cannot be negative.");
            }

            // Determine the discount rate based on customer type.
            // Refactored from separate if-statements to a single switch expression —
            // same behaviour, cleaner and easier to extend.
            decimal discountRate = customerType switch
            {
                "Premium" => 0.10m,
                "VIP" => 0.20m,
                _ => 0.00m
            };

            return originalPrice * (1 - discountRate);
        }
    }
}