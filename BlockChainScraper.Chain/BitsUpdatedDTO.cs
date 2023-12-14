using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace BilbolStack.BlockChainScraper.Chain
{
    [Event("BitsUpdated")]
    public class BitsUpdatedDTO : IEventDTO
    {
        [Parameter("address", "by", 1, true)]
        public string By { get; set; }

        [Parameter("uint256", "id", 2, true)]
        public BigInteger Id { get; set; }
        [Parameter("uint256", "oldBits", 3, false)]
        public BigInteger OldBits { get; set; }
        [Parameter("uint256", "newBits", 4, false)]
        public BigInteger NewBits { get; set; }

    }
}
