using FinancialTracker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Models
{
   public class TransactionDataModel
    {
        public TransactionType Type { get; set; }   
        public decimal Amount { get; set; }
        public Enum? Category { get ; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }


    }
}
