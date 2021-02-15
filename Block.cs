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
        public string data { get; set; }
        public string Hash { get; set; }

        public int Nonce { get; set; }

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

