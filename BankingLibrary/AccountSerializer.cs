using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public enum AccountFileFormat
    {
        TXT = 1,
        XML,
        JSON,
        SQLITE
    }

    public abstract class AccountSerializer
    {
        /// <summary>
        /// The list of accounts to be serialized
        /// </summary>
        protected List<Account> _accountList;

        /// <summary>
        /// The path to the directory where account files will be saved
        /// </summary>
        protected string _dataDirPath;

        /// <summary>
        /// The name of the folder containing all the account files. 
        /// </summary>
        private const string ACCOUNT_DATA_FOLDER = "BankingData";

        /// <summary>
        /// Prefix used for account files storing regular account data
        /// </summary>
        protected const string ACCT_FILE_PREFIX = "acct";

        public AccountSerializer()
        {
            //the account list must be initialized through the write-only property
            _accountList = null;

            //determine the root path where account files will be stored. Depending
            //on the actual serialization method used the files will be stored in 
            //a subfolder of this root path
            _dataDirPath = Path.Combine(AccountSerializer.RootDirectory, ACCOUNT_DATA_FOLDER);

            //make the directory if it does not exist
            if (Directory.Exists(_dataDirPath) == false) // The ! operator could be used but it is not very visible
            {
                Directory.CreateDirectory(_dataDirPath);
            }
        }

        public List<Account> AccountList
        {
            set
            {
                _accountList = value;
            }
        }

        public string DataDirectoryPath
        {
            get { return _dataDirPath; }
        }

        public static string RootDirectory { get; set; }

        public abstract void Load();

        public abstract void Save();
    }
}
