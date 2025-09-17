using System;

namespace RefactoringExample
{
    public class DiscountCalculator
    {
        public static double CalculateDiscount(bool isVIP, int orders)
        {
            const double BigDiscount = 0.15;
            const double NormalDiscount = 0.1;
            const double NoDiscount = 0;

            const int OrderForDiscount = 6;

            double discount;

            if (isVIP)
            {
                discount = BigDiscount;
            }
            else if (orders >= OrderForDiscount)
            {
                discount = NormalDiscount;
            }
            else
            {
                discount = NoDiscount;
            }

            return discount;
        }
    }
}