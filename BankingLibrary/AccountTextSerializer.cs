using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public class AccountTextSerializer : AccountSerializer
    {
        public AccountTextSerializer()
        {
            //update the path to match the path for account text files
            _dataDirPath = Path.Combine(_dataDirPath, AccountFileFormat.TXT.ToString());

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
                using (StreamReader acctFileReader = new StreamReader(new FileStream(acctFileName, FileMode.Open)))
                {
                    //read the account type and create the correct account
                    string acctType = acctFileReader.ReadLine();
                    Account acct = new Account();

                    //load the data into the account object
                    acct.Load(acctFileReader);

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
                string acctFileName = String.Format("{0}{1}.dat", ACCT_FILE_PREFIX, acct.AccountNumber);
                string acctFilePath = Path.Combine(_dataDirPath, acctFileName);

                //write the account data to the file
                using (StreamWriter acctFileWriter = new StreamWriter(new FileStream(acctFilePath, FileMode.Create)))
                {
                    //write the type of account
                    acctFileWriter.WriteLine(acct.GetType().Name);

                    acct.Save(acctFileWriter);
                }
            }
        }
    }
}
