namespace BilbolStack.BlockChainScraper.Repository
{
    public class BlockNumberFileRepository : IBlockNumberRepository
    {
        private const string _file = "lastblock.txt";
        private static object _lockObj = new object();

        public long LastBlock()
        {
            lock (_lockObj)
            {
                return File.Exists(_file) ? long.Parse(File.ReadAllText(_file)) : 0;
            }
        }

        public void UpdateLastBlock(long newBlock)
        {
            lock (_lockObj)
            {
                File.WriteAllText(_file, newBlock.ToString());
            }
        }
    }
}
