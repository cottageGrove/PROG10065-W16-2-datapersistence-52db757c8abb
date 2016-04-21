using BankingLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPersistence
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();

            //create a general account serializer - NOT POSSIBLE because it is an abstract class
            //AccountSerializer genAcctSerializer = new AccountSerializer();

            //set the root of the data files
            AccountSerializer.RootDirectory = Directory.GetCurrentDirectory();

            prog.SerializeWithText();
            prog.SerializeWithObjects();
            prog.SerializeWithJSON();

            Console.ReadKey();
        }

       

        private void SerializeWithText()
        {
            Console.WriteLine("Creating bank with random accounts...");
            Bank bank = new Bank();
            bank.CreateAccounts();

            AccountTextSerializer txtSerializer = new AccountTextSerializer();
            bank.Save(txtSerializer);
            Console.WriteLine($"Saving accounts using a text serializer in \n\t{txtSerializer.DataDirectoryPath}");

            Console.WriteLine("Creating a new bank object...");
            Bank newBank = new Bank();

            Console.WriteLine("Loading accounts using the text serializer...");
            newBank.Load(new AccountTextSerializer());
        }

        private void SerializeWithObjects()
        {
            Console.WriteLine("Creating bank with random accounts...");
            Bank bank = new Bank();
            bank.CreateAccounts();

            AccountObjectSerializer objSerializer = new AccountObjectSerializer();
            bank.Save(objSerializer);
            Console.WriteLine($"Saving accounts using a object XML serializer in \n\t{objSerializer.DataDirectoryPath}");

            Console.WriteLine("Creating a new bank object...");
            Bank newBank = new Bank();

            Console.WriteLine("Loading accounts using the object XML serializer...");
            newBank.Load(new AccountObjectSerializer());
        }

        private void SerializeWithJSON()
        {
            Console.WriteLine("Creating bank with random accounts...");
            Bank bank = new Bank();
            bank.CreateAccounts();

            AccountJsonSerializer objSerializer = new AccountJsonSerializer();
            bank.Save(objSerializer);
            Console.WriteLine($"Saving accounts using a JSON serializer in \n\t{objSerializer.DataDirectoryPath}");

            Console.WriteLine("Creating a new bank object...");
            Bank newBank = new Bank();

            Console.WriteLine("Loading accounts using the JSON serializer...");
            newBank.Load(new AccountJsonSerializer());
        }
    }
}
