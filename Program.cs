using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace BankManagementSystem
{
    class Program
    {
        private const int indexFname = 0, indexLname = 1, indexAddress = 2, indexPhone = 3, indexEmail = 4;
        private const int indexAccount = 0, indexBalance = 1;

        static void Main(string[] args)
        {
            Login user = new Login();
            user.LoginScreen();

            bool exit = false;
            while (!exit)
            {
                string optionStr = Screen.DisplayMenu();
                int option = 0;
                if (Check.IsInteger(optionStr, ref option))
                {
                    if (option < 1 || option > 7) continue;
                    switch (option)
                    {
                        case 1: CreateAccount(); break;
                        case 2: SearchAccount(); break;
                        case 3: MakeDeposit(); break;
                        case 4: MakeWithdrawal(); break;
                        case 5: GenerateStatement(); break;
                        case 6: DeleteAccount(); break;
                        case 7: exit = true; break;
                    }
                }
            }
        }

        static void CreateAccount()
        {
            List<string> fieldNames = new List<string>()
                {"First Name:","Last Name:","Address:","Phone:","Email:"};
            List<string> fields;
            do
            {
                fields = Screen.TakeUserInput("***CREATE A NEW ACCOUNT***", "Enter details below.", fieldNames);
            }
            while (!Check.ValidateCreateAccount(fields));

            Console.Write("\n\n Is the information correct (y/n)? ");
            string confirm = Console.ReadLine();
            if (confirm == "y")
            {
                Account account = new Account(Check.GenerateAccountNumber(), 0, fields[indexFname], fields[indexLname], fields[indexAddress], int.Parse(fields[indexPhone]), fields[indexEmail]);
                account.SaveAccount();

                Console.WriteLine("\n\n Account successfully created! Details will be provided via email.");
                Console.WriteLine(" Account number is: {0}", account.AccountNumber);
                Console.WriteLine("\n\nPlease wait...");

                // Send account details to Email
                try
                {
                    var path = account.AccountNumber + ".txt";
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("syfsofiena@gmail.com");
                    mail.To.Add(fields[indexEmail]);
                    mail.Subject = "SimpleBankingApp - Account Details";
                    mail.Body = "Dear Customer, please find attached your new account details. Thank You";

                    using (Attachment attachment = new Attachment(path))
                    {
                        mail.Attachments.Add(attachment);
                        SmtpServer.Port = 587;
                        SmtpServer.UseDefaultCredentials = true;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("syfsofiena@gmail.com", "ewezenccobhzjohb");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                    }
                    Console.WriteLine("Your details were sent. Press any key to return to menu");
                    Console.ReadKey();
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, something went wrong. Press any key to return to menu");
                    Console.ReadKey();
                }
            }
            else return;
        }

        static void SearchAccount()
        {
            List<string> fieldNames = new List<string>();
            List<string> fields = null;
            while (true)
            {
                fieldNames.Clear();
                fieldNames.Add("Account Number:");
                bool found = false;
                string confirm;
                while (!found)
                {
                    do
                    {
                        fields = Screen.TakeUserInput("***SEARCH AN ACCOUNT***", "Enter details below.", fieldNames);
                    }
                    while (!Check.ValidateSearchAccount(fields));

                    found = File.Exists(string.Format("{0}.txt", fields[indexAccount]));

                    if (!found)
                    {
                        Console.WriteLine("\n\n Account not found!");
                        Console.Write(" Check another account (y/n)? ");
                        confirm = Console.ReadLine();
                        if (confirm == "y") continue;
                        else return;
                    }
                }
                Console.WriteLine("\n\n Account found!");
                int accountNumber = int.Parse(fields[indexAccount]);
                Account account = new Account(accountNumber);
                account.ReadAccountInfo();
                fieldNames.Clear();
                fieldNames.Add("First Name: " + account.FirstName);
                fieldNames.Add("Last Name: " + account.LastName);
                fieldNames.Add("Address: " + account.Address);
                fieldNames.Add("Phone: " + account.Phone);
                fieldNames.Add("Email: " + account.Email);
                fieldNames.Add("Account No: " + account.AccountNumber);
                fieldNames.Add("Account Balance: $" + account.Balance);

                Screen.PrintInfo("***ACCOUNT DETAILS***", "", fieldNames);
                Console.Write("\n\n Check another account (y/n)? ");
                confirm = Console.ReadLine();
                if (confirm == "y") continue;
                else return;
            }
        }

        static void MakeDeposit()
        {
            List<string> fieldNames = new List<string>();
            List<string> fields = null;

            fieldNames.Add("Account Number:");
            fieldNames.Add("Amount: $");
            bool found = false;
            string confirm;
            while (!found)
            {
                do
                {
                    fields = Screen.TakeUserInput("***MAKE A DEPOSIT***", "Enter details below.", fieldNames);
                }
                while (!Check.ValidateTransactionInput(fields));

                found = File.Exists(string.Format("{0}.txt", fields[indexAccount]));

                if (!found)
                {
                    Console.WriteLine("\n\n Account not found!");
                    Console.Write(" Retry (y/n)? ");
                    confirm = Console.ReadLine();
                    if (confirm == "y") continue;
                    else return;
                }
            }
            Console.WriteLine("\n\n Account found!");
            int accountNumber = int.Parse(fields[indexAccount]);
            Account account = new Account(accountNumber);
            account.ReadAccountInfo();
            account.Deposit(double.Parse(fields[indexBalance]));
            account.SaveAccount();

            Console.WriteLine(" Amount succesfully deposited!");
            Console.WriteLine(" New Balance is {0:C}", account.Balance);
        }

        static void MakeWithdrawal()
        {
            List<string> fieldNames = new List<string>();
            List<string> fields = null;

            fieldNames.Add("Account Number:");
            fieldNames.Add("Amount: $");
            bool found = false;
            string confirm;
            Account account = null;
            bool overdraft = true;
            double amount = 0;

            while (overdraft)
            {
                while (!found)
                {
                    do
                    {
                        fields = Screen.TakeUserInput("***WITHDRAW***", "Enter details below.", fieldNames);
                    }
                    while (!Check.ValidateTransactionInput(fields));

                    found = File.Exists(string.Format("{0}.txt", fields[indexAccount])); ; ;

                    if (!found)
                    {
                        Console.WriteLine("\n\n Account not found!");
                        Console.Write(" Retry (y/n)? ");
                        confirm = Console.ReadLine();
                        if (confirm == "y") continue;
                        else return;
                    }
                }
                Console.WriteLine("\n\n Account found!");
                int accountNumber = int.Parse(fields[indexAccount]);
                account = new Account(accountNumber);
                account.ReadAccountInfo();
                amount = double.Parse(fields[indexBalance]);
                if (amount > account.Balance)
                {
                    Console.WriteLine(" Withdrawal unsuccessful! Not enough money in account. Press any key to try again");
                    Console.ReadKey();
                    found = false;
                }
                else overdraft = false;
            }
            account.Withdraw(amount);
            account.SaveAccount();

            Console.WriteLine(" Withdrawal successful!");
            Console.WriteLine(" New Balance is: {0:C}", account.Balance);
        }

        static void GenerateStatement()
        {
            List<string> fieldNames = new List<string>();
            List<string> fields = null;
            while (true)
            {
                fieldNames.Clear();
                fieldNames.Add("Account Number:");
                bool found = false;
                string confirm;
                while (!found)
                {
                    do
                    {
                        fields = Screen.TakeUserInput("***GENERATE STATEMENT***", "Enter details below.", fieldNames);
                    }
                    while (!Check.ValidateSearchAccount(fields));

                    found = File.Exists(string.Format("{0}.txt", fields[indexAccount]));

                    if (!found)
                    {
                        Console.WriteLine("\n\n Account not found!");
                        Console.Write(" Retry (y/n)? ");
                        confirm = Console.ReadLine();
                        if (confirm == "y") continue;
                        else return;
                    }
                }
                Console.WriteLine("\n\n Account found!");
                Console.WriteLine(" Displaying statement...");
                int accountNumber = int.Parse(fields[indexAccount]);
                Account account = new Account(accountNumber);
                account.ReadAccountInfo();
                fieldNames.Clear();
                fieldNames.Add("Account No: " + account.AccountNumber);
                fieldNames.Add("Account Balance: $" + account.Balance);
                fieldNames.Add("First Name: " + account.FirstName);
                fieldNames.Add("Last Name: " + account.LastName);
                fieldNames.Add("Address: " + account.Address);
                fieldNames.Add("Phone: " + account.Phone);
                fieldNames.Add("Email: " + account.Email);

                Screen.PrintInfo("***ACCOUNT STATEMENT***", "", fieldNames);

                Console.Write("\n\n Email statement (y/n)? ");
                string emailConfirm = Console.ReadLine();
                if (emailConfirm == "y")
                {
                    Console.WriteLine("\n\nPlease wait...");

                    // Send transaction details to Email
                    try
                    {
                        var path = account.AccountNumber + ".txt";
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress("syfsofiena@gmail.com");
                        mail.To.Add(account.Email);
                        mail.Subject = "SimpleBankingApp - Your Account Statement";
                        mail.Body = "Dear Customer, please find attached your a/c statement. Thank You";

                        using (Attachment attachment = new Attachment(path))
                        {
                            mail.Attachments.Add(attachment);
                            SmtpServer.Port = 587;
                            SmtpServer.UseDefaultCredentials = true;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("syfsofiena@gmail.com", "ewezenccobhzjohb");
                            SmtpServer.EnableSsl = true;
                            SmtpServer.Send(mail);
                        }
                        Console.WriteLine("Your statement was  successfully sent!");
                        return;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Sorry, something went wrong. Press any key to return to menu");
                        Console.ReadKey();
                    }
                } else return;
            }
        }

        static void DeleteAccount()
        {
            List<string> fieldNames = new List<string>();
            List<string> fields = null;

            fieldNames.Add("Account Number:");
            bool found = false;
            string confirm;

            while (!found)
            {
                do
                {
                    fields = Screen.TakeUserInput("***DELETE AN ACCOUNT***", "Enter details below.", fieldNames);
                }
                while (!Check.ValidateSearchAccount(fields));

                found = File.Exists(string.Format("{0}.txt", fields[indexAccount]));

                if (!found)
                {
                    Console.WriteLine("\n\n Account not found!");
                    Console.Write(" Retry (y/n)? ");
                    confirm = Console.ReadLine();
                    if (confirm == "y") continue;
                    else return;
                }
            }
            Console.WriteLine("\n\n Account found!");
            Console.WriteLine(" Displaying account details...");
            int accountNumber = int.Parse(fields[indexAccount]);
            Account account = new Account(accountNumber);
            account.ReadAccountInfo();

            fieldNames.Clear();
            fieldNames.Add("Account No: " + account.AccountNumber);
            fieldNames.Add("Account Balance: $" + account.Balance);
            fieldNames.Add("First Name: " + account.FirstName);
            fieldNames.Add("Last Name: " + account.LastName);
            fieldNames.Add("Address: " + account.Address);
            fieldNames.Add("Phone: " + account.Phone);
            fieldNames.Add("Email: " + account.Email);

            Screen.PrintInfo("***ACOUNT DETAILS***", "", fieldNames);
            Console.Write("\n\n Delete (y/n)? ");
            confirm = Console.ReadLine();
            if (confirm == "y")
            {
                File.Delete(string.Format("{0}.txt", accountNumber));
                Console.WriteLine("\n\n Account successfully deleted!");
            }
            else return;
        }
    }
}
