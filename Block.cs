using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using EllipticCurve;

namespace ThtimuliCoin
{
    class Block
    {
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public string TimeStamp { get; set; }
        //public string data { get; set; }
        public string Hash { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Nonce { get; set; }

        public Block(int index, string timeStamp, List<Transaction> transactions, string previousHash = "")
        {
            this.Index = index;
            this.TimeStamp = timeStamp;
            this.Transactions = transactions;
            this.PreviousHash = previousHash;
            this.Hash = CalcHash();
            this.Nonce = 0;

        }

        public string CalcHash()
        {
            string blockData = this.Index + this.PreviousHash + this.TimeStamp + this.Transactions.ToString() + this.Nonce;
            byte[] blockBytes = Encoding.ASCII.GetBytes(blockData);
            byte[] hashBytes = SHA256.Create().ComputeHash(blockBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public void Mine(int difficulty)
        {
            while (this.Hash.Substring(0, difficulty) != new string('0', difficulty))
            {
                this.Nonce++;
                this.Hash = CalcHash();
                Console.WriteLine("Mining: " + this.Hash);
            }

            Console.WriteLine("Block has been mined: " + this.Hash);
        }

    }
}

