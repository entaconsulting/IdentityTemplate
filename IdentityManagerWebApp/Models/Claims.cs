using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityManagerWebApp.Models
{
    public class Claims
    {
        public const string UO = "UO";
        public const string DIVISION = "DIVISION";
        public const string COUNTRY = "COUNTRY";
        public const string BU = "BUSINESS_UNIT";

        public static IEnumerable<string> GetClaimss()
        {
            return new List<string>()
            {
                UO,
                DIVISION,
                COUNTRY,
                BU,

            };
        }
    }
}