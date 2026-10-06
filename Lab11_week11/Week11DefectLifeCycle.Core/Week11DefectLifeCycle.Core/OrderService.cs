using System;
using System.Collections.Generic;
using System.Text;

namespace Week11DefectLifeCycle.Core
{
    public class OrderService
    {
        // DEFECT D-001: subtracts discountPercent directly instead of calculating the percentage
        public decimal ApplyDiscount(decimal price, decimal discountPercent)
        {
            return price - discountPercent;
        }

        // DEFECT D-002: uses > instead of >=, so exactly $100 is not eligible
        public bool IsEligibleForFreeShipping(decimal orderTotal)
        {
            return orderTotal > 100;
        }

        // Combines discount + shipping rule to give the final amount payable
        public decimal CalculateFinalTotal(decimal price, decimal discountPercent, decimal shippingCost)
        {
            // Apply the discount first
            var discountedPrice = ApplyDiscount(price, discountPercent);

            // Free shipping check is done on the discounted total
            if (IsEligibleForFreeShipping(discountedPrice))
            {
                shippingCost = 0;
            }

            return discountedPrice + shippingCost;
        }
    }
}