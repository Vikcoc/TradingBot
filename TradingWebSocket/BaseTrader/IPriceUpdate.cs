namespace TradingWebSocket.BaseTrader
{
    public interface IPriceUpdate
    {
        public string? InstrumentName { get; set; }
        public double? BestBid { get; set; }
        public double? BestAsk { get; set; }
        public double? Actual { get; set; }
        public double? Low { get; set; }
        public double? High { get; set; }
        public double Volume { get; set; }
        public double? Change { get; set; }
        public long Timestamp { get; set; }
        public double BigVolume { get; set; }
        public double? PartChange { get; set; }
    }
}
