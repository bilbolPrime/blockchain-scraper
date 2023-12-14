using Newtonsoft.Json;

namespace BilbolStack.BlockChainScraper.Repository
{
    public class NFTFileRepository : INFTRepository
    {
        private const string _nftFile = "nfts.txt";
        private const string _mintsFile = "mints.txt";
        private const string _transferLogFile = "transfers.txt";
        private const string _bitUpdatesFile = "bitUpdates.txt";

        private static object _lockObj = new object();
        public void AddNFT(string owner, long id, long bits, string minter)
        {
            lock (_lockObj)
            {
                var nfts = File.Exists(_nftFile) ? JsonConvert.DeserializeObject<List<NFTData>>(File.ReadAllText(_nftFile)) : new List<NFTData>();
                nfts.Add(new NFTData() { Id = id, Owner = owner, Bits = bits });
                File.WriteAllText(_nftFile, JsonConvert.SerializeObject(nfts));

                var mints = File.Exists(_mintsFile) ? JsonConvert.DeserializeObject<List<NFTMints>>(File.ReadAllText(_mintsFile)) : new List<NFTMints>();
                mints.Add(new NFTMints() { Id = id, Minter = minter });
                File.WriteAllText(_mintsFile, JsonConvert.SerializeObject(mints));
            }
        }

        public void TransferNFT(long id, string newOwner)
        {
            lock (_lockObj)
            {
                var nfts = File.Exists(_nftFile) ? JsonConvert.DeserializeObject<List<NFTData>>(File.ReadAllText(_nftFile)) : new List<NFTData>();
                var existingNFT = nfts.FirstOrDefault(i => i.Id == id);
                if (existingNFT == null)
                {
                    // oops
                    return;
                }

                var transfers = File.Exists(_transferLogFile) ? JsonConvert.DeserializeObject<List<NFTTransfers>>(File.ReadAllText(_transferLogFile)) : new List<NFTTransfers>();
                transfers.Add(new NFTTransfers() { Id = id, Owner = existingNFT.Owner, NewOwner = newOwner});
                File.WriteAllText(_transferLogFile, JsonConvert.SerializeObject(transfers));


                existingNFT.Owner = newOwner;
                File.WriteAllText(_nftFile, JsonConvert.SerializeObject(nfts));
            }
        }

        public void UpdateBits(string by, long id, long oldBits, long bits)
        {
            lock (_lockObj)
            {
                var nfts = File.Exists(_nftFile) ? JsonConvert.DeserializeObject<List<NFTData>>(File.ReadAllText(_nftFile)) : new List<NFTData>();
                var existingNFT = nfts.FirstOrDefault(i => i.Id == id);
                if (existingNFT == null)
                {
                    // oops
                    return;
                }

                var bitUpdates = File.Exists(_bitUpdatesFile) ? JsonConvert.DeserializeObject<List<NFTBitsUpdate>>(File.ReadAllText(_bitUpdatesFile)) : new List<NFTBitsUpdate>();
                bitUpdates.Add(new NFTBitsUpdate() { By = by, Id = id, Bits = oldBits, NewBits = bits });
                File.WriteAllText(_bitUpdatesFile, JsonConvert.SerializeObject(bitUpdates));


                existingNFT.Bits = bits;
                File.WriteAllText(_nftFile, JsonConvert.SerializeObject(nfts));
            }
        }


        class NFTData
        {
            public long Id { get; set; }
            public string Owner { get; set; }
            public long Bits { get; set; }
        }

        class NFTMints
        {
            public long Id { get; set; }
            public string Minter { get; set; }
        }

        class NFTTransfers
        {
            public long Id { get; set; }
            public string Owner { get; set; }
            public string NewOwner { get; set; }
        }

        class NFTBitsUpdate
        {
            public string By { get; set; }
            public long Id { get; set; }
            public long Bits { get; set; }
            public long NewBits { get; set; }
        }
    }
}
