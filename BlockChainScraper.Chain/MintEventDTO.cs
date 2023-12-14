using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace BilbolStack.BlockChainScraper.Chain
{
    [Event("NFTMinted")]
    public class MintEventDTO : IEventDTO
    {
        [Parameter("address", "minter", 1, true)]
        public string Minter { get; set; }

        [Parameter("uint256", "id", 2, true)]
        public BigInteger Id { get; set; }

        [Parameter("address", "owner", 3, true)]
        public string Owner { get; set; }
        [Parameter("uint256", "bits", 4, false)]
        public BigInteger Bits { get; set; }
    }
}
