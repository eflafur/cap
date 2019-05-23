using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap
{
    public static class NumberUtils
    {
        public static String ToEuropeanCurrency(this Decimal d)
        {
            //return d.ToString("N2", CultureInfo.GetCultureInfo("it-IT"));

            return d.ToString("0.00", CultureInfo.GetCultureInfo("it-IT"));
        }

       
    }
}
