using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankManagementSystem
{
    class Login
    {
        string userName, userPass;

        public void LoginScreen()
        {
            Console.Clear();
            Screen.DisplayTop("Welcome! Please log in to start.");                            
            Console.WriteLine("║       ╔═Login══════════════════════════╗       ║");
            Console.Write("║       ║ Username: ");
            int UserCursorLeft = Console.CursorLeft;
            int UserCursorTop = Console.CursorTop;
            Console.Write("                     ║       ║");
            Console.Write("\n║       ║ Password: ");
            int PwdCursorLeft = Console.CursorLeft;
            int PwdCursorTop = Console.CursorTop;
            Console.WriteLine("                     ║       ║");
            Console.WriteLine("║       ╚════════════════════════════════╝       ║");
            Screen.DisplayBottom();
            
            Console.SetCursorPosition(UserCursorLeft, UserCursorTop);
            userName = Console.ReadLine();
            Console.SetCursorPosition(PwdCursorLeft, PwdCursorTop);
            userPass = "";

            // Hides password with stars
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    userPass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && userPass.Length > 0)
                    {
                        userPass = userPass.Substring(0, (userPass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            Console.SetCursorPosition(1, Console.CursorTop + 6);

            // Calling method to validate login credentials

            var isValid = ValidateCredentials(userName, userPass);

            if (isValid)
            {
                Console.WriteLine("Login successful! ...");
            }

            else
            {
                Console.WriteLine("Invalid credentials! Press any key to try again.");
                Console.ReadKey();
                LoginScreen();
            }

        }

        static bool ValidateCredentials(string userName, string userPass)
        {
            // Check against "login.txt" file
            string[] lines = File.ReadAllLines("login.txt");
            foreach (string set in lines)
            {
                // Split each line with "|" as delimiter
                string[] splits = set.Split('|');
                // Check if username and password match
                if (splits[0] == userName && splits[1] == userPass)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
