using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankManagementSystem
{
    class Check
    {
        private static Random rnd = new Random();
        private const int minDigit = 100000;
        private const int maxDigit = 10000000;

        public static bool IsInteger(string s, ref int number)
        {
            try
            {
                number = int.Parse(s);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool IsDouble(string s, ref double number)
        {
            try
            {
                number = double.Parse(s);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool ValidateCreateAccount(List<string> fields)
        {
            int phone = 0;
            if (!IsInteger(fields[3], ref phone) || fields[3].Length > 10)
            {
                Console.WriteLine("\n\n Invalid phone number! Press any key to try again");
                Console.ReadKey();
                return false;
            }
            if (!fields[4].Contains("@"))
            {
                Console.WriteLine("\n\n Invalid email address! Press any key to try again");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        public static int GenerateAccountNumber()
        {
            int id = rnd.Next(minDigit, maxDigit);
            while (File.Exists(String.Format("{0}.txt", id)))
                id = rnd.Next(minDigit, maxDigit);
            return id;
        }

        public static bool ValidateSearchAccount(List<string> fields)
        {
            int account = 0;
            if (!IsInteger(fields[0], ref account) || fields[0].Length > 10)
            {
                Console.WriteLine("\n\n Invalid account! Press any key to try again");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        public static bool ValidateTransactionInput(List<string> fields)
        {
            int account = 0;
            if (!IsInteger(fields[0], ref account) || fields[0].Length > 10)
            {
                Console.WriteLine("\n\n Invalid account! Press any key to try again");
                Console.ReadKey();
                return false;
            }
            double amount = 0;
            if (!IsDouble(fields[1], ref amount))
            {
                Console.WriteLine("\n\n Invalid amount! Press any key to try again");
                Console.ReadKey();
                return false;
            }
            if (amount <= 0)
            {
                Console.WriteLine("\n\n Invalid amount! Press any key to try again");
                Console.ReadKey();
                return false;
            }
            return true;
        }
    }
}
