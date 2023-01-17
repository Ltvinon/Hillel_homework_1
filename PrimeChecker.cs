using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hillel_homework_1
{
    public static class PrimeChecker
    {
        static PrimeChecker() { }

        /// <summary>
        /// Проверка на простые числа. 
        /// https://en.wikipedia.org/wiki/Primality_test
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true если число простое.</returns>
        public static bool PrimeCheck(double value)
        {
            if (value == 2 || value == 3)
                return true;

            if (value <= 1 || value % 2 == 0 || value % 3 == 0)
                return false;

            for (int i = 5; i * i <= value; i += 6)
            {
                if (value % i == 0 || value % (i + 2) == 0)
                    return false;
            }
            return true;
        }
    }
}
