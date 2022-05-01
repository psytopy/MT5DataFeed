using System;

namespace MT5DealFeed
{
    class DealData
    {
        public long Login { get; set; }
        public long DealNo { get; set; }
        public string Time { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double ContractSize { get; set; }
        public double Commission { get; set; }
        public double Profit { get; set; }
        public string Comment { get; set; }
    }

    enum DealType
    {
        Buy,
        Sell
    }
}
