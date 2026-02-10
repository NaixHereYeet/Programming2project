using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class SpecialOffer
    {
        public string RestaurantName { get; set; }
        public string OfferCode { get; set; }
        public string Description { get; set; }
        public double DiscountAmount { get; set; }
        public SpecialOffer(string r, string c, string d, double disc) { RestaurantName = r; OfferCode = c; Description = d; DiscountAmount = disc; }
    }
}