using BankingLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataPersistenceUniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Bank _newBank;

        private Bank _loadedBank;

        private AccountSerializer _serializer;

        public MainPage()
        {
            InitializeComponent();

            _newBank = null;
            _loadedBank = null;
            _serializer = null;
           
            //determine a default root folder where the application is allowed to write
            AccountSerializer.RootDirectory = ApplicationData.Current.LocalFolder.Path;

            //create the text serializer by default
            _serializer = new AccountTextSerializer();
            _txtAcctFileDataPath.Text = _serializer.DataDirectoryPath;

            //setup the radio buttons to change the serializer when checked
            _rbtnTextSerializer.Checked += OnChangeSerializer;
            _rbtnObjectSerializer.Checked += OnChangeSerializer;
            _rbtnJSONSerializer.Checked += OnChangeSerializer;
            _rbtnSQLiteSerializer.Checked += OnChangeSerializer;
        }


        private async void OnCreateBank(object sender, RoutedEventArgs e)
        {
            _newBank = new Bank();

            MessageDialog msgDlg = new MessageDialog("A new bank object was created with no accounts.", "Data Persistence");
            await msgDlg.ShowAsync();
        }

        private async void OnCreateBankToLoad(object sender, RoutedEventArgs e)
        {
            _loadedBank = new Bank();

            MessageDialog msgDlg = new MessageDialog("A new bank object was created with no accounts. Read to load accounts", "Data Persistence");
            await msgDlg.ShowAsync();
        }

        private async void OnCreateAccounts(object sender, RoutedEventArgs e)
        {
            _newBank.CreateAccounts();

            //display acccounts
            foreach (Account acct in _newBank)
            {
                _lvNewBank.Items.Add(acct);
            }

            MessageDialog msgDlg = new MessageDialog("Random accounts created in the bank.", "Data Persistence");
            await msgDlg.ShowAsync();
        }

        private void OnSaveAccounts(object sender, RoutedEventArgs e)
        {
            _newBank.Save(_serializer);
        }

        private void OnLoadAccounts(object sender, RoutedEventArgs e)
        {
            _loadedBank.Load(_serializer);

            //display acccounts
            foreach (Account acct in _loadedBank)
            {
                _lvLoadedBank.Items.Add(acct);
            }
        }

        private void OnClearNewBankData(object sender, RoutedEventArgs e)
        {
            _newBank = null;
            _lvNewBank.Items.Clear();

        }
        private void OnClearLoadedBankData(object sender, RoutedEventArgs e)
        {
            _loadedBank = null;
            _lvLoadedBank.Items.Clear();

        }

        private async void OnChangeSerializer(object sender, RoutedEventArgs e)
        {
            if (sender == _rbtnTextSerializer)
            {
                _serializer = new AccountTextSerializer();
            }
            else if (sender == _rbtnObjectSerializer)
            {
                _serializer = new AccountObjectSerializer();
            }
            else if (sender == _rbtnJSONSerializer)
            {
                _serializer = new AccountJsonSerializer();
            }
            else if (sender == _rbtnSQLiteSerializer)
            {
                _serializer = new AccountSQLiteSerializer();
                MessageDialog noSqliteMsg = new MessageDialog("The SQLite serializer is not implemented. Please choose a different serializer", "Data Persistence");
                await noSqliteMsg.ShowAsync();
                return;
            }


            _txtAcctFileDataPath.Text = _serializer.DataDirectoryPath; 

            MessageDialog msgDlg = new MessageDialog("Serializer type has been changed. The storage location was updated.", "Data Persistence");
            await msgDlg.ShowAsync();
        }

        private async void OnAccountSelected(object sender, SelectionChangedEventArgs e)
        {
            Account selectedAccount = e.AddedItems[0] as Account;

            MessageDialog msgDlg = new MessageDialog(
                String.Format("You have selected the account {0}", selectedAccount.AccountNumber),
                selectedAccount.AcctHolderName);
            await msgDlg.ShowAsync();
        }
    }
}
