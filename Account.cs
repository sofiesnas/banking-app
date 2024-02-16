using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankManagementSystem
{
    public class Account
    {
        List<Transaction> listOfTransactions = new List<Transaction>();
        public int AccountNumber { get; set; }
        public double Balance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }

        public Account(int accountNumber, double balance, string firstName, string lastName, string address, int phone, string email)
        {
            this.AccountNumber = accountNumber;
            this.Balance = balance;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
        }

        public Account(int accountNumber)
        {
            this.AccountNumber = accountNumber;
        }

        public void SaveAccount()
        {
            TextWriter sw = new StreamWriter(string.Format("{0}.txt", AccountNumber));
            sw.WriteLine("First Name|" + FirstName);
            sw.WriteLine("Last Name|" + LastName);
            sw.WriteLine("Address|" + Address);
            sw.WriteLine("Phone|" + Phone);
            sw.WriteLine("Email|" + Email);
            sw.WriteLine("AccountNo|" + AccountNumber);
            sw.WriteLine("Balance|" + Balance);
            foreach (var tran in listOfTransactions)
            {
                sw.WriteLine(tran.Date + "|" + tran.Type + "|" + tran.Amount);
            }
            sw.Close();
        }

        public void ReadAccountInfo()
        {
            foreach (string line in File.ReadLines(string.Format("{0}.txt", AccountNumber)))
            {
                string[] spilttedString = line.Split('|');
                if (spilttedString.Length < 2)
                {
                    Console.WriteLine("File is corrupted, cannot split string with '|'");
                }
                var key = spilttedString[0];
                var value = spilttedString[1];
                switch (key)
                {
                    case "AccountNo":
                        AccountNumber = Convert.ToInt32(value);
                        break;
                    case "Balance":
                        Balance = Convert.ToDouble(value);
                        break;
                    case "First Name":
                        FirstName = value;
                        break;
                    case "Last Name":
                        LastName = value;
                        break;
                    case "Address":
                        Address = value;
                        break;
                    case "Phone":
                        Phone = Convert.ToInt32(value);
                        break;
                    case "Email":
                        Email = value;
                        break;                  
                }
            }
        }

        public void Deposit(double amount)
        {
            var deposit = new Transaction()
            {
                Date = DateTime.Now,
                Type = Transaction.TransactionType.Deposit,
                Amount = amount
            };
            listOfTransactions.Add(deposit);
            Balance += amount;
        }

        public void Withdraw(double amount)
        {
            var withdrawal = new Transaction()
            {
                Date = DateTime.Now,
                Type = Transaction.TransactionType.Withdrawal,
                Amount = amount
            };
            listOfTransactions.Add(withdrawal);
            Balance -= amount;
        }

    }
}
