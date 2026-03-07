
using Nethereum.HdWallet;
using NBitcoin;
using log4net;

namespace CreatWallet
{
    public class CreatWallet
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        public List<WalletInfo> CreateWallets(int numWallets, string filePath)
        {

            List<WalletInfo> wallets = new List<WalletInfo>();
            bool foundFlag = false;
            // 为每个钱包生成助记词和私钥
            for (int i = 0; i < numWallets; i++)
            {
                string mnemonics = new Mnemonic(Wordlist.English, WordCount.Twelve).ToString();

                var wallet = new Wallet(mnemonics, "");

                string publicKey = wallet.GetAccount(0).Address;
                string privateKey = wallet.GetAccount(0).PrivateKey;

                // 将助记词，公钥和私钥写入文件
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    //i++;
                    writer.WriteLine($"Wallet {i + 1}:");
                    writer.WriteLine("助记词: " + mnemonics);
                    writer.WriteLine("公钥: " + publicKey);
                    writer.WriteLine("私钥: " + privateKey);
                    writer.WriteLine();
                    log.Info("创建钱包" + publicKey + "成功。");
                }

                wallets.Add(new WalletInfo { Mnemonics = mnemonics, PublicKey = publicKey, PrivateKey = privateKey });

            }
            return wallets;

        }

        public List<WalletInfo> CreateWallets(int numWallets, string filePath, string targetString)
        {
            List<WalletInfo> wallets = new List<WalletInfo>();
            bool foundFlag = false;
            for (int i = 0; i < numWallets; i++)
            {
                string mnemonics = new Mnemonic(Wordlist.English, WordCount.Twelve).ToString();

                //使用助记词生成钱包
                var wallet = new Wallet(mnemonics, "");

                // 获取公钥和私钥
                string publicKey = wallet.GetAccount(0).Address;
                string privateKey = wallet.GetAccount(0).PrivateKey;

                if (publicKey.EndsWith(targetString))
                {
                    // 将助记词，公钥和私钥写入文件
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Wallet {i + 1}:");
                        writer.WriteLine("助记词: " + mnemonics);
                        writer.WriteLine("公钥: " + publicKey);
                        writer.WriteLine("私钥: " + privateKey);
                        writer.WriteLine();
                        log.Info("创建钱包" + publicKey + "成功。");
                    }
                    wallets.Add(new WalletInfo { Mnemonics = mnemonics, PublicKey = publicKey, PrivateKey = privateKey });
                    foundFlag = true;
                }
                else
                {
                    //没找到，就重新执行一次
                    i--;
                }
            }
            return wallets;

        }

    }
}


