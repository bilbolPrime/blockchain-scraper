namespace BilbolStack.BlockChainScraper.Chain
{
    public class ChainSettings
    {
        public const string ConfigKey = "ChainInfo";
        public string AccountPrivateKey { get; set; }
        public long ChainId { get; set; }
        public string RpcUrl { get; set; }
        public string NFTContractAddress { get; set; }
    }
}
