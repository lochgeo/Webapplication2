using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System.Threading.Tasks;
using System.Threading;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;

namespace AccountRegistry.Services
{
    public class Ethereum
    {
        private readonly Web3 web3;
        private Contract contract;
        string senderAddress = "";
        static string contractAddress = "0x28ea3b854008249cf823e517f7ea5f08cbea820e";
        static string contractAbi = @"[{""constant"":false,""inputs"":[{""name"":""seller"",""type"":""address""},{""name"":""buyer_email"",""type"":""string""},{""name"":""account_number"",""type"":""string""}],""name"":""Authorize"",""outputs"":[{""name"":""result"",""type"":""string""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""seller"",""type"":""address""},{""name"":""account_number"",""type"":""string""},{""name"":""account_name"",""type"":""string""}],""name"":""StoreAccount"",""outputs"":[{""name"":""result"",""type"":""string""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""seller"",""type"":""address""},{""name"":""buyer_email"",""type"":""string""},{""name"":""account_number"",""type"":""string""}],""name"":""ViewAccount"",""outputs"":[{""name"":""result"",""type"":""string""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""seller"",""type"":""address""},{""name"":""buyer_email"",""type"":""string""},{""name"":""account_number"",""type"":""string""}],""name"":""Revoke"",""outputs"":[{""name"":""result"",""type"":""string""}],""payable"":false,""type"":""function""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""seller"",""type"":""address""},{""indexed"":false,""name"":""result"",""type"":""string""}],""name"":""NotifyResult"",""type"":""event""}]";

        public Ethereum()
        {
            this.web3 = new Web3("http://45.79.188.21:8081");
            this.contract = web3.Eth.GetContract(contractAbi, contractAddress);
        }

        public Ethereum(string _address)
        {
            this.web3 = new Web3("http://45.79.188.21:8081");
            this.contract = web3.Eth.GetContract(contractAbi, contractAddress);
            this.senderAddress = _address;
        }

        public async Task<HexBigInteger> GetBlockNum()
        {
            return await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        }

        public async Task<string> NewAccount(string passPhrase)
        {
            return await web3.Personal.NewAccount.SendRequestAsync(passPhrase);
        }

        public async Task<string> ExecuteContractStore(string passPhrase, string accountNumber, string accountName)
        {
            var gas = new HexBigInteger(1000000);
            var res = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, passPhrase, new HexBigInteger(120));

            Function storeFn = contract.GetFunction("StoreAccount");
            Event notifyResult = contract.GetEvent("NotifyResult");
            var filterAll = await notifyResult.CreateFilterAsync();
            var storeTxHash = await storeFn.SendTransactionAsync(senderAddress, gas, null, senderAddress, accountNumber, accountName);
            var receipt = await GetReceiptAsync(storeTxHash);
            var log = await notifyResult.GetFilterChanges<NotifyEvent>(filterAll);

            return log[0].Event.Result;
        }

        public async Task<string> ExecuteContractView(string passPhrase, string buyerEmail, string accountNumber)
        {
            var gas = new HexBigInteger(1000000);
            var res = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, passPhrase, new HexBigInteger(120));

            Function viewFn = contract.GetFunction("ViewAccount");
            Event notifyResult = contract.GetEvent("NotifyResult");
            var filterAll = await notifyResult.CreateFilterAsync();
            var viewTxHash = await viewFn.SendTransactionAsync(senderAddress, gas, null, senderAddress, buyerEmail, accountNumber);
            var receipt = await GetReceiptAsync(viewTxHash);
            var log = await notifyResult.GetFilterChanges<NotifyEvent>(filterAll);

            return log[0].Event.Result;
        }

        public async Task<string> ExecuteContractAuth(string passPhrase, string buyerEmail, string accountNumber)
        {
            var gas = new HexBigInteger(1000000);
            var res = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, passPhrase, new HexBigInteger(120));

            Function authFn = contract.GetFunction("StoreAccount");
            Event notifyResult = contract.GetEvent("NotifyResult");
            var filterAll = await notifyResult.CreateFilterAsync();
            var authTxHash = await authFn.SendTransactionAsync(senderAddress, gas, null, senderAddress, buyerEmail, accountNumber);
            var receipt = await GetReceiptAsync(authTxHash);
            var log = await notifyResult.GetFilterChanges<NotifyEvent>(filterAll);

            return log[0].Event.Result;
        }

        public async Task<string> ExecuteContractRevoke(string passPhrase, string buyerEmail, string accountNumber)
        {
            var gas = new HexBigInteger(1000000);
            var res = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, passPhrase, new HexBigInteger(120));

            Function revokeFn = contract.GetFunction("StoreAccount");
            Event notifyResult = contract.GetEvent("NotifyResult");
            var filterAll = await notifyResult.CreateFilterAsync();
            var revokeTxHash = await revokeFn.SendTransactionAsync(senderAddress, gas, null, senderAddress, buyerEmail, accountNumber);
            var receipt = await GetReceiptAsync(revokeTxHash);
            var log = await notifyResult.GetFilterChanges<NotifyEvent>(filterAll);

            return log[0].Event.Result;
        }

        public async Task<TransactionReceipt> GetReceiptAsync(string transactionHash)
        {
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while (receipt == null)
            {
                Console.WriteLine("Sleeping for 1s");
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            return receipt;
        }

        public class NotifyEvent
        {
            [Parameter("address", "seller", 1, true)]
            public string Seller { get; set; }

            [Parameter("string", "result", 2, false)]
            public string Result { get; set; }

        }

    }
}