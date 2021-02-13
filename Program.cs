using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;

namespace ThtimuliCoin
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain ThtimuliCoin = new Blockchain();

            ThtimuliCoin.AddBlock(new Block(1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amount: 500"));
            ThtimuliCoin.AddBlock(new Block(2, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amount: 40"));
            string blockJSON = JsonConvert.SerializeObject(ThtimuliCoin, Formatting.Indented);
            Console.WriteLine(blockJSON);

            if (ThtimuliCoin.ChainIsValid())
            {
                Console.WriteLine("Chain is Valid");
            }
            else
            {
                Console.WriteLine("Error: Chain not valid");
            }
        }
    }
    
    class Blockchain
    {
        public List<Block> Chain { get; set; }

        public Blockchain()
        {
            this.Chain = new List<Block>();
            this.Chain.Add(CreateGenesis());
        }

        public Block CreateGenesis()
        {
            return new Block(0, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "Welcome to Thtimuli Coin");
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
    }

    class Block
    {
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public string TimeStamp { get; set; }
        public string data { get; set; }
        public string Hash { get; set; }

        public Block(int index, string timeStamp, string data, string previousHash = "")
        {
            this.Index = index;
            this.TimeStamp = timeStamp;
            this.data = data;
            this.PreviousHash = previousHash;
            this.Hash = CalcHash();

        }

        public string CalcHash()
        {
            string blockData = this.Index + this.PreviousHash + this.TimeStamp + this.data;
            byte[] blockBytes = Encoding.ASCII.GetBytes(blockData);
            byte[] hashBytes = SHA256.Create().ComputeHash(blockBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

    }
}
