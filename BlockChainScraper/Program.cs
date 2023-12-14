using BilbolStack.BlockChainScraper.Chain;
using BilbolStack.BlockChainScraper.Repository;
using BlockChainScraper;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<INFTContractScraper, NFTContractScraper>();
builder.Services.AddSingleton<IBlockNumberRepository, BlockNumberFileRepository>();
builder.Services.AddSingleton<INFTRepository, NFTFileRepository>();
builder.Services.AddOptions<ChainSettings>().BindConfiguration(ChainSettings.ConfigKey);

var host = builder.Build();
host.Run();
