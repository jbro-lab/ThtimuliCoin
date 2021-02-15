using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using EllipticCurve;

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
}
    
   

    
