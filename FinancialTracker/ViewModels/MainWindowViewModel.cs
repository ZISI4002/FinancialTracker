using FinancialTracker.Models;
using FinancialTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancialTracker.ViewModels
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public MainWindowViewModel(Window window) : base(window)
        {


            Transactions = TransactionStorage.Load();
        }

        public List<TransactionDataModel> Transactions { get; set; }


    }




}
