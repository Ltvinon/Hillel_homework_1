﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hillel_homework_1
{
    public class PrimeChecker : INumberChecker
    {
        public PrimeChecker() { }

        /// <summary>
        /// Проверка на простые числа. 
        /// https://en.wikipedia.org/wiki/Primality_test
        /// </summary>
        /// <param name="value">Число для проверки.</param>
        /// <param name="infoString">"- is prime number" если число простое.</param>
        /// <returns>true если число простое.</returns>
        public bool Check(double value, out string infoString)
        {
            infoString = "";
            //Проверка на целочисельное
            if (value != (int)value)
                return false;

            if (value == 2 || value == 3)
            {
                infoString = " - is prime number";
                return true;
            }

            if (value <= 1 || value % 2 == 0 || value % 3 == 0)
                return false;

            for (int i = 5; i * i <= value; i += 6)
            {
                if (value % i == 0 || value % (i + 2) == 0)
                    return false;
            }
            {
                infoString = " - is prime number";
                return true;
            }
        }
    }
}
