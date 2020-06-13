using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySizer
{
    public static class Extensions
    {
        static readonly string[] Units = { "B", "KiB", "MiB", "GiB", "TiB", "PiB" };

        public static string ToUnits(this long bytes)
        {
            var counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }

            return $"{number:n1}{Units[counter]}";
        }
    }
}
