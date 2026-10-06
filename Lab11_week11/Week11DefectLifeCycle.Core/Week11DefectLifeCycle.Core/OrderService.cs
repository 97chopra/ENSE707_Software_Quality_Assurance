using System;
using System.Collections.Generic;
using System.Text;

namespace Week11DefectLifeCycle.Core
{
    public class OrderService
    {
        // FIX D-001: calculates the real percentage discount, and validates inputs (Rule 4)
        public decimal ApplyDiscount(decimal price, decimal discountPercent)
        {
            // Negative prices are invalid
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            // Discount must be a percentage from 0 to 100
            if (discountPercent < 0 || discountPercent > 100)
            {
                throw new ArgumentException("Discount must be between 0 and 100.");
            }

            // e.g. 200 - (200 * 20 / 100) = 160
            return price - (price * discountPercent / 100);
        }

        // FIX D-002 (restored): >= so that exactly $100 is eligible for free shipping (Rule 3)
        public bool IsEligibleForFreeShipping(decimal orderTotal)
        {
            return orderTotal >= 100;
        }

        // Applies discount first, then waives shipping if the discounted total is eligible
        public decimal CalculateFinalTotal(decimal price, decimal discountPercent, decimal shippingCost)
        {
            // Negative shipping cost is invalid
            if (shippingCost < 0)
            {
                throw new ArgumentException("Shipping cost cannot be negative.");
            }

            var discountedPrice = ApplyDiscount(price, discountPercent);

            // Free shipping is decided on the discounted total
            if (IsEligibleForFreeShipping(discountedPrice))
            {
                shippingCost = 0;
            }

            return discountedPrice + shippingCost;
        }
    }
}