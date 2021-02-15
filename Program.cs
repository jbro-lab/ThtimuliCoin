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
            PrivateKey key1 = new PrivateKey();
            PublicKey wallet1 = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey wallet2 = key2.publicKey();

            Blockchain ThtimuliCoin = new Blockchain(2, 100);

            Console.WriteLine("Start the Miner: ");
            ThtimuliCoin.MinePendingTransaction(wallet1);

            Console.WriteLine("\nBalance of wallet1 is $" + ThtimuliCoin.GetWalletBalance(wallet1).ToString());

            //ThtimuliCoin.AddBlock(new Block(1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amount: 500"));
            //ThtimuliCoin.AddBlock(new Block(2, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amount: 40"));

            Transaction tx1 = new Transaction(wallet1, wallet2, 50);
            tx1.SignTransaction(key1);
            ThtimuliCoin.AddPendingTransaction(tx1);


            Console.WriteLine("Start the Miner: ");
            ThtimuliCoin.MinePendingTransaction(wallet2);
            Console.WriteLine("\nBalance of wallet1 is $" + ThtimuliCoin.GetWalletBalance(wallet1).ToString());
            Console.WriteLine("\nBalance of wallet2 is $" + ThtimuliCoin.GetWalletBalance(wallet2).ToString());

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
    
   

    
