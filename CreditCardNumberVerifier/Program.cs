using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardNumberVerifier
{
    internal class Program
    {
        static void Main(string[] args)
        {

            long cardNumber = RequestValidInput();
            string cardNumberString = cardNumber.ToString();

            bool validLength = VerifyNumberLength(cardNumberString);

            if (validLength)
            {
                VerifyCardNumber(cardNumber);
            }



            Console.ReadLine();
        }

        static long RequestValidInput()
        {
            long number = 0;
            bool validNumber = false;

            while (!validNumber)
            {
                Console.Write("Enter a credit card number (without spaces) to be verified: ");
                string input = Console.ReadLine();

                validNumber = long.TryParse(input, out number);

                if (!validNumber)
                {
                    Console.WriteLine("Invalid entry.");
                }
            }

            return number;
        }

        static bool VerifyNumberLength(string input)
        {
            if (input.Length != 13 && input.Length != 15 && input.Length != 16)
            {
                Console.WriteLine("NOT A CREDIT CARD NUMBER.");
                return false;
            }

            return true;
        }

        static void VerifyCardNumber(long number)
        {
            string numberString = number.ToString();

            if (numberString.Length == 13)
            {
                VerifyIfVisa(number);
            }
            else if (numberString.Length == 15)
            {
                VerifyIfAmex(number);
            }
            else if (numberString.Length == 16)
            {
                VerifyIfMastercardOrVisa(number);
            }
        }

        static void VerifyIfVisa(long number)
        {
            int firstDigit = GetFirstDigit(number);

            if (firstDigit != 4)
            {
                Console.WriteLine("INVALID CARD NUMBER.");
            }
            else
            {
                bool valid = LuhnAlgorithmOddLength(number);
                if (valid)
                {
                    Console.WriteLine("VALID VISA");
                }
                else
                {
                    Console.WriteLine("INVALID CARD NUMBER.");
                }
            }
        }

        static void VerifyIfAmex(long number)
        {
            int firstTwoDigits = GetFirstTwoDigits(number);

            if (firstTwoDigits != 34 && firstTwoDigits != 37)
            {
                Console.WriteLine("INVALID CARD NUMBER.");
            }
            else
            {
                bool valid = LuhnAlgorithmOddLength(number);
                if (valid)
                {
                    Console.WriteLine("VALID AMEX");
                }
                else
                {
                    Console.WriteLine("INVALID CARD NUMBER.");
                }
            }
        }

        static void VerifyIfMastercardOrVisa(long number)
        {
            int firstTwoDigits = GetFirstTwoDigits(number);
            int firstDigit = (firstTwoDigits - firstTwoDigits % 10) / 10;

            if (firstTwoDigits != 51 && firstTwoDigits != 52 && firstTwoDigits != 53 && firstTwoDigits != 54 && firstTwoDigits != 55 && firstDigit != 4)
            {
                Console.WriteLine("INVALID CARD NUMBER.");
            }
            else
            {
                bool valid = LuhnAlgorithmEvenLength(number);
                if (valid && firstDigit == 4)
                {
                    Console.WriteLine("VALID VISA");
                }
                else if (valid && (firstTwoDigits == 51 || firstTwoDigits == 52 || firstTwoDigits == 53 || firstTwoDigits == 54 || firstTwoDigits == 55))
                {
                    Console.WriteLine("VALID MASTERCARD");
                }
                else
                {
                    Console.WriteLine("INVALID CARD NUMBER.");
                }
            }
        }

        static int GetFirstDigit(long number)
        {
            while (number >= 10)
            {
                number = (number - number % 10) / 10;
            }

            return (int)number;
        }

        static int GetFirstTwoDigits(long number)
        {
            while (number >= 100)
            {
                number = (number - number % 10) / 10;
            }

            return (int)number;
        }

        static bool LuhnAlgorithmOddLength(long number)
        {
            long n1 = (number - number % 10) / 10;
            int i;
            int j = 0;
            while (n1 >= 10)
            {
                i = (int)((n1 % 10) * 2);
                while (i > 0)
                {
                    j += i % 10;
                    i = (i - i % 10) / 10;
                }
                n1 = (n1 - n1 % 100) / 100;
            }

            long n2 = number;
            while (n2 > 0)
            {
                j = (int)(j + (n2 % 10));
                n2 = (n2 - n2 % 100) / 100;
            }

            if (j % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool LuhnAlgorithmEvenLength(long number)
        {
            long n1 = (number - number % 10) / 10;
            int i;
            int j = 0;
            while (n1 > 0)
            {
                i = (int)((n1 % 10) * 2);
                while (i > 0)
                {
                    j += i % 10;
                    i = (i - i % 10) / 10;
                }
                n1 = (n1 - n1 % 100) / 100;
            }

            long n2 = number;
            while (n2 >= 10)
            {
                j = (int)(j + (n2 % 10));
                n2 = (n2 - n2 % 100) / 100;
            }

            if (j % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
