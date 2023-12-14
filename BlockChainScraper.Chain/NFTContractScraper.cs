using BilbolStack.BlockChainScraper.Chain;
using BilbolStack.BlockChainScraper.Repository;
using Microsoft.Extensions.Options;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace BilbolStack.BlockChainScraper.Chain
{
    public class NFTContractScraper : INFTContractScraper
    {
        protected Web3 _web3;
        protected string _contractAddress;
        protected Account _account;

        private INFTRepository _nftRepository;
        private IBlockNumberRepository _blockNumberRepository;

        public NFTContractScraper(IOptions<ChainSettings> chainSettings, INFTRepository nftRepository, IBlockNumberRepository blockNumberRepository)
        {
            _account = new Account(chainSettings.Value.AccountPrivateKey, chainSettings.Value.ChainId);
            _web3 = new Web3(_account, chainSettings.Value.RpcUrl);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _contractAddress = chainSettings.Value.NFTContractAddress;
            _nftRepository = nftRepository;
            _blockNumberRepository = blockNumberRepository;
        }

        public async Task CheckChange()
        {
            var startBlock = (int) _blockNumberRepository.LastBlock();
            var latestBlockNumber = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var endBlock = Math.Min(startBlock  + 255, latestBlockNumber.ToLong());

            if(startBlock > latestBlockNumber.Value.ToHexBigInteger().ToLong())
            {
                return;
            }

            {
                var mintEvent = _web3.Eth.GetEvent<MintEventDTO>(_contractAddress);
                var filterInput = mintEvent.CreateFilterInput(new BlockParameter(startBlock.ToHexBigInteger()), new BlockParameter(((int)endBlock).ToHexBigInteger()));
                var mints = await mintEvent.GetAllChangesAsync(filterInput);
                foreach(var mint in mints)
                {
                    _nftRepository.AddNFT(mint.Event.Owner, mint.Event.Id.ToHexBigInteger().ToLong(), mint.Event.Bits.ToHexBigInteger().ToLong(), mint.Event.Minter);
                }
            }

            {
                var transferEvent = _web3.Eth.GetEvent<TransferEventDTO>(_contractAddress);
                var filterInput = transferEvent.CreateFilterInput(new BlockParameter(startBlock.ToHexBigInteger()), new BlockParameter(((int)endBlock).ToHexBigInteger()));
                var transfers = await transferEvent.GetAllChangesAsync(filterInput);
                foreach(var transfer in transfers.Where(i => i.Event.From != "0x0000000000000000000000000000000000000000"))
                {
                    _nftRepository.TransferNFT(transfer.Event.Value.ToHexBigInteger().ToLong(), transfer.Event.To);
                }                
            }

            {
                var bitsUpdatedEvent = _web3.Eth.GetEvent<BitsUpdatedDTO>(_contractAddress);
                var filterInput = bitsUpdatedEvent.CreateFilterInput(new BlockParameter(startBlock.ToHexBigInteger()), new BlockParameter(((int)endBlock).ToHexBigInteger()));
                var bitupdates = await bitsUpdatedEvent.GetAllChangesAsync(filterInput);
                foreach(var bitUpdate in bitupdates)
                {
                    _nftRepository.UpdateBits(bitUpdate.Event.By, bitUpdate.Event.Id.ToHexBigInteger().ToLong(), bitUpdate.Event.OldBits.ToHexBigInteger().ToLong(), bitUpdate.Event.NewBits.ToHexBigInteger().ToLong());
                }
            }

            _blockNumberRepository.UpdateLastBlock(endBlock + 1);
        }
    }
}
