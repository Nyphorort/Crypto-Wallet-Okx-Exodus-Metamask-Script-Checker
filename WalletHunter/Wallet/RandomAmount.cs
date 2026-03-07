using log4net;

namespace CreatWallet
{
    public class RandomAmount
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        public void CreatRandomAmount()
        {

            for (int i = 1; i <= 100; i++)
            {
                Random random = new Random();

                // 生成 50 到 100 之间的随机浮点数
                double randomNumber = random.NextDouble() * (100 - 50) + 50;

                // 生成小数点后 2 到 6 位的随机数
                int decimalPlaces = random.Next(2, 7);
                double roundedNumber = Math.Round(randomNumber, decimalPlaces);
                log.Info("第" + i + "个随机金额为:" + roundedNumber);
            }
        }
    }
}

