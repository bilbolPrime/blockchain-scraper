# BlockChain Scraper

A simple blockchain scraper for  the ERC721 standard.

# Local Chain

1. Install [Ganache](https://www.trufflesuite.com/ganache)

## Local Chain Setup

1. `npm install` (inside ./Chain)
1. `truffle compile --all` (inside ./Chain)
1. `truffle deploy` (inside ./Chain)
1. `mint` to mint NFTs to any address
1. `setBits` to update an NFT's bits


```
let nftContract = await NFTContract.deployed()
await nftContract.mint('some address', some bit value)
await nftContract.setBits(nft id, new bits value)
```

# App Settings Setup

1. Update ChainInfo data to match local chain or remote chain
1. Assign any private key

# Functionality

There are no exposed APIs. This is a worker service that will run once three seconds to check for up to 255 blocks for `Transfer`, `NFTMinted` and `BitsUpdated` events. Existing implementation will create text files that hold a serialized list of NFT data (owners and bits), history of mints, history of transfers and history of bit updates.

# Version History

1. 2023-12-14: Initial release v1.0.0 

# Disclaimer

This implementation was made for educational / training purposes only.

# License

License is [MIT](https://en.wikipedia.org/wiki/MIT_License)

# MISC

Birbia is coming
