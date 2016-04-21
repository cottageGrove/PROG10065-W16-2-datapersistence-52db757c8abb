using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BankingLibrary
{
    /// <summary>
    /// Represents a bank composed of a list of accounts.
    /// </summary>
    public class Bank : IEnumerable<Account>
    {
        /// <summary>
        /// the list of accounts managed by the bank
        /// </summary>
        private List<Account> _accountList;

        /// <summary>
        /// The first account number. New account numbers are generated using an incremental process
        /// starting with this number.
        /// </summary>
        private const int DEFAULT_ACCT_NO_START = 100;

        private static string[] s_lastNameSet = { "Abraham", "Rahid", "Peltier", "Leclair", "Lai", "Olivarez", "Reyes", "Sadler", "Wester", "Yamamoto" };
        private static string[] s_firstNameSet = { "Arlie", "Cortez", "Gino", "Joleen", "Tim", "Antonia", "Brenda", "Nikita", "Olga", "Yu"};
        private static Random s_randomizer = new Random();
        public Bank()
        {
            _accountList = new List<Account>();
        }

        /// <summary>
        /// Load the account data for all the accounts. The account data files are stored in a directory
        /// named BankingData located in the current directory, the directory used to run the application from        
        /// </summary>
        public void Load(AccountSerializer serializer)
        {
            //provide the serializer with access to the list
            serializer.AccountList = _accountList;
            serializer.Load();
        }

        /// <summary>
        /// Saves the data for all accounts in the data directory of the application. Each account is
        /// saved in a separate file which contains all the information and list of transactions performed
        /// in the account. The account data files are stored in a directory named BankingData located in the 
        /// current directory, the directory used to run the application from
        /// </summary>
        public void Save(AccountSerializer serializer)
        {
            //provide the serializer with access to the list
            serializer.AccountList = _accountList;
            serializer.Save();
        }

        /// <summary>
        /// Create 10 accounts with predefined IDs and balances. The default accounts are created only
        /// if no account data files exist
        /// </summary>
        public void CreateAccounts()
        {
            _accountList.Clear();

            for(byte iAccount = 0; iAccount < 10; iAccount++)
            {
                //generate a random name
                int randFirstNameIndex = s_randomizer.Next(s_firstNameSet.Length);
                int randLastNameIndex = s_randomizer.Next(s_lastNameSet.Length);
                string randomName = $"{s_firstNameSet[randFirstNameIndex]} {s_lastNameSet[randLastNameIndex]}";
                
                //create the account with required default properties
                Account randAcct =  new Account(DEFAULT_ACCT_NO_START + iAccount, randomName);                
                randAcct.AnnualIntrRate = 2.5f;

                //generate random transactions
                int transCount = s_randomizer.Next(10);
                for (int iTrans = 0; iTrans < transCount; iTrans++)
                {
                    //generate  random transation type
                    TransactionType transType = (TransactionType)s_randomizer.Next(1,3);
                    switch (transType)
                    {
                        case TransactionType.Deposit:
                            randAcct.Deposit(s_randomizer.Next(100, 1000));
                            break;

                        case TransactionType.Withdrawal:
                            randAcct.Withdraw(s_randomizer.Next(10, 200));
                            break;
                    }
                }

                //add the account to the list
                _accountList.Add(randAcct);
            }
        }

        public IEnumerator<Account> GetEnumerator()
        {
            return ((IEnumerable<Account>)_accountList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Account>)_accountList).GetEnumerator();
        }
    }
}
