using BilbolStack.BlockChainScraper.Chain;

namespace BlockChainScraper
{
    public class Worker : BackgroundService
    {
        private INFTContractScraper _nftContractScraper;
        public Worker(INFTContractScraper nftContractScraper)
        {
            _nftContractScraper = nftContractScraper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(3000, stoppingToken);
                await _nftContractScraper.CheckChange();
            }
        }
    }
}
