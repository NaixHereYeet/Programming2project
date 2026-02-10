csharp PROGRAMMINGPROJECT 2\SpecialOffer.cs
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class SpecialOffer
    {
        public string RestaurantName { get; set; }
        public string OfferCode { get; set; }
        public string Description { get; set; }
        public double DiscountAmount { get; set; }

        public SpecialOffer() { }

        public SpecialOffer(string resName, string code, string desc, double discount)
        {
            RestaurantName = resName ?? string.Empty;
            OfferCode = code ?? string.Empty;
            Description = desc ?? string.Empty;
            DiscountAmount = discount;
        }

        public override string ToString() => $"{OfferCode} @ {RestaurantName}: {Description} ({DiscountAmount:F2})";
    }
}