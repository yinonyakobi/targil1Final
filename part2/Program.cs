using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Michael Boxenhorn 212309041
Yinon Yakobi 324160910
*/

namespace part2
{
    class Program
    {
        public const int NumOfMonthsInAYear = 12;
        public const int NumOfDaysInAMonth = 31;
        static void Main(string[] args)
        {
            bool[,] occupied = new bool[NumOfMonthsInAYear, NumOfDaysInAMonth];
            occupied.Initialize();

            bool exit = false;
            int decision;
            while (!exit)
            {
                printMenu();
                Int32.TryParse(Console.ReadLine(), out decision);
                switch (decision)
                {
                    case 1: // books a number of days in succession.
                        int month, day, length;

                        // I/O
                        Console.WriteLine("\nenter the start date and length of the stay.");
                        Console.Write(" - enter start date month number: ");
                        Int32.TryParse(Console.ReadLine(), out month);
                        Console.Write(" - enter start date day number: ");
                        Int32.TryParse(Console.ReadLine(), out day);
                        Console.Write(" - enter the number of days: ");
                        Int32.TryParse(Console.ReadLine(), out length);

                        bookNights(occupied, month - 1, day - 1, length - 1);
                        break;
                    case 2: // prints the occupied days.
                        Console.WriteLine("\noccupied:");
                        printList(occupied);
                        break;
                    case 3: // prints the number of occupied days in the year, and the percentage.
                        int counter = countBookedDays(occupied);

                        Console.WriteLine("\nnumber of occupied days in year: " + counter);
                        Console.WriteLine("percentage of occupied days in year: " +
                            ((float)counter / (NumOfMonthsInAYear * NumOfDaysInAMonth)) * 100);
                        break;
                    case 4: // exits the program.
                        exit = true;
                        break;
                }


            }

        }

        // books a number of following ('len') days starting at a date ('i', 'j').
        static void bookNights(bool[,] calender, int i, int j, int len)
        {
            if (!isAvailable(calender, i, j, len))
            {
                Console.WriteLine("\nthe request was denied.");
                return;
            }

            for (int k = 0; k < len; ++k)
                calender[i + (j + k) / NumOfDaysInAMonth, (j + k) % NumOfDaysInAMonth] = true;

            Console.WriteLine("\nthe request was accepted.");
        }

        // returns 'true' if the dates are available and 'false' if they are not
        static bool isAvailable(bool[,] calender, int i, int j, int len)
        {
            if (i < 0 || j < 0 || len < 1 || i >= NumOfMonthsInAYear || j >= NumOfDaysInAMonth) return false;
            if ((i * NumOfDaysInAMonth + j + len) > NumOfDaysInAMonth * NumOfMonthsInAYear) return false; // if staying time exceeds the limit of days in the year.
            return len <= continuityLength(calender, i, j, false);
        }

        // (case 2:) prints a list of occupied dates.
        static void printList(bool[,] calender)
        {
            int i = 0, numOfContinuousDays;

            while (i < NumOfMonthsInAYear * NumOfDaysInAMonth)
            {
                numOfContinuousDays = continuityLength(calender, i / NumOfDaysInAMonth, i % NumOfDaysInAMonth, true);

                if (numOfContinuousDays > 0) // if there are occupied days, print them!
                {
                    Console.Write(/*"from: " +*/ (i % NumOfDaysInAMonth + 1) + "/" + (i / NumOfDaysInAMonth + 1));
                    Console.WriteLine(/*" to: "*/" - " + ((i + numOfContinuousDays) % NumOfDaysInAMonth + 1) + "/" + ((i + numOfContinuousDays) / NumOfDaysInAMonth + 1));
                }

                else
                    numOfContinuousDays += continuityLength(calender, i / NumOfDaysInAMonth, i % NumOfDaysInAMonth, false);

                i += numOfContinuousDays;
            }
        }

        // returns the length from an index ('i', 'j') to the closest falowing index hows value is not 'continuityType' (including the index).
        static int continuityLength(bool[,] calender, int i, int j, bool continuityValue)
        {
            int counter = 0;

            // starts at calender[i, j], and goes on till calender[i, j]'s value is difrent from continuityValue, or until end of array.
            while ((i * NumOfDaysInAMonth + j) < (NumOfMonthsInAYear * NumOfDaysInAMonth))
            {
                i += j / NumOfDaysInAMonth;
                j %= NumOfDaysInAMonth;

                if (calender[i, j] == continuityValue) // counts the number of values in the continuity.
                    ++counter;

                else
                    return counter;

                ++j;
            }

            return counter;
        }

        // prints the menu
        static void printMenu()
        {
            Console.WriteLine("\nTo book a date enter: 1");
            Console.WriteLine("To show occupied dates enter: 2");
            Console.WriteLine("To show the number of booked days and percentage of days that are booked enter: 3");
            Console.WriteLine("To exit enter: 4");
        }

        // returns the number of booked days
        static int countBookedDays(bool[,] calender)
        {
            int counter = 0;
            int index = 0;
            int help;

            // goes over the array
            while (index < NumOfDaysInAMonth * NumOfMonthsInAYear)
            {
                help = continuityLength(calender, index / NumOfDaysInAMonth, index % NumOfDaysInAMonth, true);

                if (help != 0)
                    counter += 1 + help;

                index += help;
                index += continuityLength(calender, index / NumOfDaysInAMonth, index % NumOfDaysInAMonth, false);
            }

            return counter;
        }

    }
}
