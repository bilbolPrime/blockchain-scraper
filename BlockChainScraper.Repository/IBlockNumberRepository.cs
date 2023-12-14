namespace BilbolStack.BlockChainScraper.Repository
{
    public interface IBlockNumberRepository
    {
        long LastBlock();
        void UpdateLastBlock(long newBlock);
    }
}
