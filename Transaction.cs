using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;
using EllipticCurve;

namespace ThtimuliCoin
{
    class Transaction
    {
        public PublicKey FromAddress { get; set; }
        public PublicKey ToAddress { get; set; }
        public decimal Amount { get; set; }

        public Signature Signature { get; set; }
        public Transaction(PublicKey fromAddress, PublicKey toAddress, decimal amount)
        {
            this.FromAddress = fromAddress;
            this.ToAddress = toAddress;
            this.Amount = amount;
           
        }
        public void SignTransaction(PrivateKey privateKey)
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string signDER = BitConverter.ToString(privateKey.publicKey().toDer()).Replace("-", "");

            if (fromAddressDER != signDER)
            {
                throw new Exception("Error: cannot sign for other wallets");
            }
            string tdHash = this.CalcHash();
            this.Signature = Ecdsa.sign(tdHash, privateKey);
        }
        
        public string CalcHash()
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string toAddressDER = BitConverter.ToString(ToAddress.toDer()).Replace("-", "");
            string transactionData = fromAddressDER + toAddressDER + this.Amount;
            byte[] tdBytes = Encoding.ASCII.GetBytes(transactionData);

            return BitConverter.ToString(SHA256.Create().ComputeHash(tdBytes)).Replace("-", "");
        }

        public bool IsValid()
        {
            if (this.FromAddress is null) return true;

            if (this.Signature is null)
            {
                throw new Exception("No signature for this transaction");
            }

            return Ecdsa.verify(this.CalcHash(), this.Signature, this.FromAddress);
        }
    }
}
