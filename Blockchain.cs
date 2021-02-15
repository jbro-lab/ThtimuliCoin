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
}
