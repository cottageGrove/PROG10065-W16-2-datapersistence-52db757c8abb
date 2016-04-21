using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public class AccountSQLiteSerializer : AccountSerializer
    {
        public AccountSQLiteSerializer()
        {
            //update the path to match the path for account text files
            _dataDirPath = Path.Combine(_dataDirPath, AccountFileFormat.SQLITE.ToString());

            //make the directory if it does not exist
            if (Directory.Exists(_dataDirPath) == false) // The ! operator could be used but it is not very visible
            {
                Directory.CreateDirectory(_dataDirPath);
            }
        }

        public override void Load()
        {

        }

        public override void Save()
        {

        }
    }
}
