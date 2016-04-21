using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public class AccountJsonSerializer : AccountSerializer
    {
        public AccountJsonSerializer()
        {
            //update the path to match the path for account text files
            _dataDirPath = Path.Combine(_dataDirPath, AccountFileFormat.JSON.ToString());

            //make the directory if it does not exist
            if (Directory.Exists(_dataDirPath) == false) // The ! operator could be used but it is not very visible
            {
                Directory.CreateDirectory(_dataDirPath);
            }
        }

        public override void Load()
        {
            //get the list of files in the directory
            string[] acctFileList = Directory.GetFiles(_dataDirPath);

            //go through the list of files, create the appropriate accounts and load the file
            foreach (string acctFileName in acctFileList)
            {
                using (FileStream acctStream = new FileStream(acctFileName, FileMode.Open))
                {                    
                    //deserialize the account object
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Account));
                    Account acct = serializer.ReadObject(acctStream) as Account;

                    //add the account to the list of accounts
                    _accountList.Add(acct);
                }
            }
        }

        public override void Save()
        {
            //go through each account in the list of accounts and ask it to save itself into a corresponding file
            foreach (Account acct in _accountList)
            {
                //determine the account file name
                string acctFileName = String.Format("{0}{1}.json", ACCT_FILE_PREFIX, acct.AccountNumber);
                string acctFilePath = Path.Combine(_dataDirPath, acctFileName);

                //write the account data to the file
                using (FileStream acctStream = new FileStream(acctFilePath, FileMode.Create))
                {
                    //serialize the account object
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Account));

                    serializer.WriteObject(acctStream, acct);
                }
            }

        }
    }
}
