using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using EllipticCurve;

namespace ThtimuliCoin
{
    class Blockchain
    {
        public List<Block> Chain { get; set; }

        public int Difficulty { get; set; }
        
        public decimal MiningReward { get; set; }

        public List<Transaction> PendingTransactions { get; set; }

        public Blockchain(int difficulty, decimal miningReward)
        {
            this.Chain = new List<Block>();
            this.Chain.Add(CreateGenesis());
            this.Difficulty = difficulty;
            this.MiningReward = miningReward;
            this.PendingTransactions = new List<Transaction>();
        }

        public Block CreateGenesis()
        {
            return new Block(0, DateTime.Now.ToString("yyyyMMddHHmmssffff"), new List<Transaction>());
        }

        public Block GetLatest()
        {
            return this.Chain.Last();
        }

        public void AddBlock(Block newBlock)
        {
            newBlock.PreviousHash = this.GetLatest().Hash;
            newBlock.Hash = newBlock.CalcHash();
            this.Chain.Add(newBlock);
        }

        public bool ChainIsValid()
        {

            for (int i = 1; i < this.Chain.Count; i++)
            {
                if (Chain[i].Hash != Chain[i].CalcHash())
                {
                    return false;
                }

                if (Chain[i].PreviousHash != Chain[i - 1].Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public decimal GetWalletBalance(PublicKey address)
        {
            decimal balance = 0;

            string addressDER = BitConverter.ToString(address.toDer()).Replace("-", "");
            

            foreach (Block block in this.Chain)
            {
                foreach(Transaction transaction in block.Transactions)
                {
                    if (!(transaction.FromAddress is null)) 
                    {
                        string fromDER = BitConverter.ToString(transaction.FromAddress.toDer()).Replace("-", "");
                        if (fromDER == addressDER)
                            {
                                balance -= transaction.Amount;
                            }
                    }
                    string toDER = BitConverter.ToString(transaction.ToAddress.toDer()).Replace("-", "");
                    if (toDER == addressDER)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }

        public void MinePendingTransaction(PublicKey miningRewardWallet)
        {
            Transaction rewardTx = new Transaction(null, miningRewardWallet, MiningReward);
            this.PendingTransactions.Add(rewardTx);

            Block newBlock = new Block(GetLatest().Index + 1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), this.PendingTransactions, GetLatest().Hash);
            newBlock.Mine(Difficulty);

            Console.WriteLine("Block successfully mined");
            this.Chain.Add(newBlock);
            this.PendingTransactions = new List<Transaction>();
        }

        public void AddPendingTransaction(Transaction transaction)
        {
            if (transaction.ToAddress is null || transaction.FromAddress is null)
            {
                throw new Exception("Transaction must have to and from address to be valid");
            }

            if (transaction.Amount > this.GetWalletBalance(transaction.FromAddress))
            {
                throw new Exception("Insufficient funds");
            }

            if (transaction.IsValid() == false)
            {
                throw new Exception("transaction must be valid to be added to the Block");
            }
            this.PendingTransactions.Add(transaction);
        }
    }
}
