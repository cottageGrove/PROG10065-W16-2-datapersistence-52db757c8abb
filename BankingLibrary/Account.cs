using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace BankingLibrary
{
    /// <summary>
    /// Defines the valid account types that can be created by the user. Note that the 
    /// application creates default general accounts as well but the user can only create
    /// accounts designated through the enum values below.
    /// </summary>
    public enum AccountType
    {
        Chequing = 1,
        Savings
    }

    /// <summary>
    /// Defines a bank account its associated attributes and operations.
    /// </summary>
    [DataContract]
    public class Account
    {
        /// <summary>
        /// the account number, read-only attribute
        /// </summary>
        [DataMember(Name="AccountNumber")]
        private int _acctNo;

        /// <summary>
        /// the name of the account holder, read-only attribute
        /// </summary>
        [DataMember(Name ="AccountHolder")]
        private string _acctHolderName;

        /// <summary>
        /// the account balance that gets affected by withdrawls and deposits. Note the visibility
        /// of the field variable is set to "protected" to allow it to be accessed by derived classes.
        /// NOTE: the type is used as a double while the annual interest rate is float just to showcase
        /// the two floating point types. In practice financial application actually would use "decimals"
        /// which provide most precision.
        /// </summary>
        [DataMember(Name="Balance")]
        protected double _balance;

        /// <summary>
        /// the annual interest rate applicable on the balance.Note the visibility
        /// of the field variable is set to "protected" to allow it to be accessed by derived classes.
        /// </summary>
        [DataMember(Name="AnnualInterestRate")]
        protected float _annualIntrRate;

        /// <summary>
        /// The history of transactions performed on this account. Note the visibility
        /// of the field variable is set to "protected" to allow it to be accessed by derived classes.
        /// </summary>
        [DataMember(Name="Transactions")]
        protected List<Transaction> _transactionList;

        /// <summary>
        /// Initialize the account object with its attributes.
        /// The account constructor requires the caller to supply an account number and
        /// the name of the account holder in order to create an account. 
        /// 
        /// NOTE: the constructor assigns default values to each parameter allowing the code
        /// not to supply them (i.e. acct = Account()). If the calling code does not supply
        /// values for the two parameters they will receive these default values. This is used
        /// when the accounts are created from data files 
        /// </summary>
        /// <param name="acctNo">the account number</param>
        /// <param name="acctHolderName">the name of the account holder</param>
        public Account(int acctNo = -1, string acctHolderName = "")
        {
            _acctNo = acctNo;
            _acctHolderName = acctHolderName;
            _balance = 0.0f;
            _annualIntrRate = 0.0f;
            _transactionList = new List<Transaction>();
        }

        #region Properties
        
        /// <summary>
        /// The account number that uniquely identifies the account in the bank.
        /// </summary>
        public int AccountNumber
        { 
            get { return _acctNo; }
            set { _acctNo = value; } 
        }

        /// <summary>
        /// The name of the account holder
        /// </summary>
        public string AcctHolderName 
        {
            get { return _acctHolderName; }
            set { _acctHolderName = value; }
        }

        /// <summary>
        /// The annual interest rate. To set provide the whole percentage (e.g. 3 = 3% resulting in an interest rate of 0.03).
        /// </summary>
        public virtual float AnnualIntrRate 
        {
            get { return _annualIntrRate * 100; }
            set 
            {
                //the value is expected to be a percentage. Not the "f" suffix which makes the 100 literal a "float"
                _annualIntrRate = value / 100f; 
            }
        }

        /// <summary>
        /// The monthly interest rate as a read-only property. Valued is derived from the annual interest rate
        /// </summary>
        public float MonthlyIntrRate
        {
            get { return _annualIntrRate / 12; }
        }

        /// <summary>
        /// The balance of the account. Read-only property. The balance can be changed only via a banking tansaction
        /// such as a deposit or withdrawal
        /// </summary>
        public double Balance 
        { 
            get { return _balance; }
        }

        /// <summary>
        /// Returns the list of transaction. This is not good practice but hiding the list of transaction requires
        /// more advanced knowledge of C# and OOP learned later in the course
        /// </summary>
        public IEnumerable<Transaction> TransactionList
        { 
            get { return _transactionList; }
        }

        public int TransactionCount
        {
            get { return _transactionList.Count; }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Deposit the given amount in the account and return the new balance. This method is polymorphic (defined as virtual)
        /// too allow specific (derived) account classes to override this base functionality
        /// </summary>
        /// <param name="initDepositAmount">the amount to be deposited</param>
        /// <returns>the new account balance AFTER the amount was deposited to avoid a call to the Balance.get if needed</returns>
        public virtual double Deposit(double amount)
        {
            //change the balance
            double oldBalance = _balance;
            _balance += amount;

            //record the transaction
            _transactionList.Add(new Transaction(TransactionType.Deposit, amount, oldBalance, _balance));

            //provide the new balance to the caller to avoid a getBalance() call
            return _balance;
        }

        /// <summary>
        /// Withdraw the given amount from the account and return the new balance. This method is polymorphic (defined as virtual)
        /// too allow specific (derived) account classes to override this base functionality
        /// </summary>
        /// <param name="amount">the amount to be withdrawn, cannot be negative or greater than available balance</param>
        /// <returns>the new account balance AFTER the amount was deposited to avoid a call to getBalance() if needed</returns>
        public virtual double Withdraw(double amount)
        {
            //change the balance
            double oldBalance = _balance;
            _balance -= amount;
        
            //record the transaction
            _transactionList.Add(new Transaction(TransactionType.Withdrawal, amount, oldBalance, _balance));

            //provide the new balance to the caller to avoid a getBalance() call
            return _balance;
        }

        /// <summary>
        /// Load the account information using the given stream reader object
        /// </summary>
        /// <param name="acctFileReader">file reader object to read the account file data</param>
        public void Load(StreamReader acctFileReader)
        {
            //read the account properties in the same order they were saved
            _acctNo = int.Parse(acctFileReader.ReadLine());
            _acctHolderName = acctFileReader.ReadLine();
            _balance = double.Parse(acctFileReader.ReadLine());
            _annualIntrRate = float.Parse(acctFileReader.ReadLine());

            //read the transaction list
            int transCount = int.Parse(acctFileReader.ReadLine());
            for(int iTrans = 0; iTrans < transCount; iTrans++)
            {
                Transaction trans = new Transaction();
                trans.Load(acctFileReader);
                _transactionList.Add(trans);
            }
        }

        /// <summary>
        /// Save the account information using the given stream writer object
        /// </summary>
        /// <param name="fileWriter">file writer object to write the account file data</param>
        public void Save(StreamWriter acctFileWriter)
        {
            //write the account properties, one per line
            acctFileWriter.WriteLine(_acctNo);
            acctFileWriter.WriteLine(_acctHolderName);
            acctFileWriter.WriteLine(_balance);
            acctFileWriter.WriteLine(_annualIntrRate);            

            //write the number of transactions and the list of transactions
            acctFileWriter.WriteLine(_transactionList.Count);
            foreach (Transaction trans in _transactionList)
            {
                trans.Save(acctFileWriter);
            }
        }

        #endregion
    }
}
