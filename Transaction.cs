using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementSystem
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public double PostBalance { get; set; }

        public Transaction(DateTime date, TransactionType type, double amount, double postBalance)
        {
            this.Date = date;
            this.Type = type;
            this.Amount = amount;
            this.PostBalance = postBalance;

        }

        public Transaction()
        {

        }

        public enum TransactionType
        {
            Deposit,
            Withdrawal
        }
    }

}
