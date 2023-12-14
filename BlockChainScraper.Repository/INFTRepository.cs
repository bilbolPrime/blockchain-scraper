namespace BilbolStack.BlockChainScraper.Repository
{
    public interface INFTRepository
    {
        void AddNFT(string owner, long id, long bits, string minter);
        void TransferNFT(long id, string newOwner);
        void UpdateBits(string by, long id, long oldBits, long bits);
    }
}
