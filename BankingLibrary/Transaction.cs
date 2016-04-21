using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BankingLibrary
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal
    }

    /// <summary>
    /// Represents a banking transaction performed on a bank account. Two transaction types are supported: deposit and withdrawal
    /// </summary>
    [DataContract]
    public class Transaction
    {
        /// <summary>
        /// The type of transaction. In normal operation this should not be "None". 
        /// It can be None while the object is being loaded from an account file
        /// </summary>
        [DataMember(Name = "TransactionType")]
        private TransactionType _type;

        /// <summary>
        /// The amount of money that was part of the transaction
        /// </summary>
        [DataMember(Name = "TransactionAmount")]
        private double _amount;

        /// <summary>
        /// The original balance before the transaction took place
        /// </summary>
        [DataMember(Name = "BalanceBeforeTransaction")]
        private double _originalBalance;

        /// <summary>
        /// The new balance after the transaction took place
        /// </summary>
        [DataMember(Name ="BalanceAfterTransaction")]
        private double _newBalance;

        /// <summary>
        /// Default constructor used by XML Serialization
        /// </summary>
        public Transaction()
        {
            _type = TransactionType.Deposit;
            _amount = 0.0;
            _originalBalance = 0.0;
            _newBalance = 0.0;
        }

        /// <summary>
        /// Constructor that allows the initialization of the transaction field variables
        /// NOTE: the constructor assigns default values to each parameter allowing the code
        /// not to supply them (i.e. trans = Transaction()). If the calling code does not supply
        /// values for the two parameters they will receive these default values. This is used
        /// when the accounts are created from data files 
        /// </summary>
        /// <param name="transactionType">type of transaction. None by default</param>
        /// <param name="amount">amount that was deposited. Zero by default.</param>
        /// <param name="oldBalance">the balance before the transaction took place. Zero by default</param>
        /// <param name="newBalance">the balance after the transaction took place. Zero by default</param>
        public Transaction(TransactionType type = 0, 
                           double amount = 0.0, double oldBalance = 0.0, 
                           double newBalance = 0.0)
        {
            _type = type;
            _amount = amount;
            _originalBalance = oldBalance;
            _newBalance = newBalance;
        }

        #region Properties

        /// <summary>
        /// Read-only property to obtain the transaction type
        /// </summary>
        public TransactionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Read-only property to obtain the transaction amount
        /// </summary>
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Read-only property to obtain the original balance before the transaction took place
        /// </summary>
        public double OriginalBalance
        {
            get { return _originalBalance; }
            set { _originalBalance = value; }
        }

        /// <summary>
        /// Read-only property to obtain the new balance after the transaction took place
        /// </summary>
        public double NewBalance
        {
            get { return _newBalance; }
            set { _newBalance = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the the transaction information from an account file using the given account file reader object
        /// </summary>
        /// <param name="acctFileReader">the account file reader object used to read from the account file</param>
        public void Load(StreamReader acctFileReader)
        {
            //load the encoded information as the entire transaction information is stored on one line
            string[] transData = acctFileReader.ReadLine().Split('~');

            //decode the information and assign it to the respective transaction properties
            _type = (TransactionType)int.Parse(transData[0]);
            _amount = double.Parse(transData[1]);
            _originalBalance = double.Parse(transData[2]);
            _newBalance = double.Parse(transData[3]);
        }

        /// <summary>
        /// Save the transaction information into an account file using the given account file writer object
        /// </summary>
        /// <param name="acctFileWriter">the account file writer object used to write to the account file</param>
        public void Save(StreamWriter acctFileWriter)
        {
            //encode the transaction information into one line of data
            string transData = String.Format("{0}~{1}~{2}~{3}", (int)_type, _amount, _originalBalance, _newBalance);
            acctFileWriter.WriteLine(transData);
        }

        /// <summary>
        /// Default "built-in" method that is called automatically when a transaction object is printed
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string transType = (_type == TransactionType.Withdrawal) ? "Withdrawal" : "Deposit";
            return String.Format("{0}: Amount = {1}, Original Balance = {2}, New Balance = {3}",
                                transType, _amount, _originalBalance, _newBalance);
            
        }
        #endregion
    }
}
