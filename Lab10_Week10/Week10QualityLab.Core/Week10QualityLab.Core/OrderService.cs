using System;
using System.Collections.Generic;
using System.Text;

namespace Week10QualityLab.Core
{


    public class OrderService
    {
        // Fix 1: discountPercent is now treated as a PERCENTAGE, not a flat amount.
        // Fix 2: added input validation for invalid price and discount values.
        public decimal ApplyDiscount(decimal price, decimal discountPercent)
        {
            // A negative price makes no business sense
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            // A percentage must be between 0 and 100
            if (discountPercent < 0 || discountPercent > 100)
            {
                throw new ArgumentException("Discount must be between 0 and 100.");
            }

            // Restored correct logic: discountPercent is a PERCENTAGE.
            // e.g. 200 with 20% => 200 - (200 * 20 / 100) = 160
            return price - (price * discountPercent / 100);
        }

        // Fix 3: boundary corrected from ">" to ">=".
        // Business rule: free shipping applies at $100 OR MORE.
        public bool IsEligibleForFreeShipping(decimal orderTotal)
        {
            return orderTotal >= 100;
        }

        public decimal CalculateFinalTotal(decimal price, decimal discountPercent, decimal shippingCost)
        {
            // Fix 4: shipping cost cannot be negative
            if (shippingCost < 0)
            {
                throw new ArgumentException("Shipping cost cannot be negative.");
            }

            // Apply the (now correct and validated) discount first
            var discountedPrice = ApplyDiscount(price, discountPercent);

            // Free shipping is decided on the DISCOUNTED total
            if (IsEligibleForFreeShipping(discountedPrice))
            {
                shippingCost = 0;
            }

            return discountedPrice + shippingCost;
        }
    }

}
