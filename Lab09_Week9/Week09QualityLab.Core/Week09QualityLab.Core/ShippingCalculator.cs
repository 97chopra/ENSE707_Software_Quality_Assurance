using System;
using System.Collections.Generic;
using System.Text;
using System;

using System;

namespace Week09QualityLab.Core
{
    // Calculates the shipping cost for an order.
    public class ShippingCalculator
    {
        // Standard shipping costs $10, Express shipping costs $20.
        // Orders over $100 get free standard shipping.
        // Throws an exception if the shipping type is not recognised.
        public decimal CalculateShippingCost(decimal orderAmount, string shippingType)
        {
            // Guard clause: reject any shipping type we don't recognise.
            if (shippingType != "Standard" && shippingType != "Express")
            {
                throw new ArgumentException("Shipping type must be Standard or Express.");
            }

            // Free standard shipping applies before the normal rate lookup.
            if (shippingType == "Standard" && orderAmount > 100m)
            {
                return 0m;
            }

            // Refactored from separate if-statements to a single switch expression —
            // same behaviour, cleaner and easier to extend.
            return shippingType switch
            {
                "Express" => 20m,
                _ => 10m // Standard
            };
        }
    }
}