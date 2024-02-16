using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementSystem
{
    class Screen
    {
        private const int width = 50;

        public static void DisplayTop(string heading)
        {
            Console.WriteLine("");
            Console.WriteLine("╔══SimpleBankingApp══════════════════════════════╗");
            DisplayBody("");
            Console.Write("║");
            int left = (width - heading.Length - 2) / 2;
            for (int i = 0; i < left; i++) Console.Write(" ");
            Console.Write(heading);
            for (int i = 0; i < width - heading.Length - 2 - left; i++) Console.Write(" ");
            Console.WriteLine("║");
            DisplayBody("");
        }

        public static void DisplayBody(string text)
        {
            Console.Write("║");
            for (int i = 0; i < 5; i++) Console.Write(" ");
            Console.Write(text);
            for (int i = 0; i < width - text.Length - 2 - 5; i++) Console.Write(" ");
            Console.WriteLine("║");
        }

        public static void DisplayBottom()
        {
            DisplayBody("");
            DisplayBody("");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
        }

        public static string DisplayMenu()
        {
            Console.Clear();
            DisplayTop("***MENU***");
            DisplayBody("1. Create a new account");
            DisplayBody("2. Search for an account");
            DisplayBody("3. Deposit");
            DisplayBody("4. Withdraw");
            DisplayBody("5. A/C statement");
            DisplayBody("6. Delete account");
            DisplayBody("7. Exit");
            DisplayBody("");
            Console.Write("║     Enter your choice (1-7): ");
            int ChoiceCursorLeft = Console.CursorLeft;
            int ChoiceCursorTop = Console.CursorTop;
            Console.Write("                  ║\n");
            DisplayBody("");
            DisplayBottom();

            Console.SetCursorPosition(ChoiceCursorLeft, ChoiceCursorTop);
            string optionStr = Console.ReadLine();

            return optionStr;
        }

        public static List<string> TakeUserInput(string heading, string subheading, List<string> fieldNames)
        {
            Console.Clear();
            DisplayTop(heading);
            DisplayBody(subheading);
            DisplayBody("");
            foreach (string field in fieldNames) DisplayBody(field);
            DisplayBottom();
            List<string> fields = new List<string>();
            Console.SetCursorPosition(0, Console.CursorTop - fieldNames.Count - 3);
            for (int i = 0; i < fieldNames.Count; i++)
            {
                Console.SetCursorPosition(7 + fieldNames[i].Length, Console.CursorTop);
                fields.Add(Console.ReadLine());
            }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            return fields;
        }

        public static void PrintInfo(string heading, string subheading, List<string> fieldNames)
        {
            DisplayTop(heading);
            DisplayBody(subheading);
            foreach (string field in fieldNames) DisplayBody(field);
            DisplayBottom();
            Console.WriteLine();
        }
    }
}
