# Heroes Arena

Heroes Arena is a decentralized card battle game built with Unity and blockchain technology.  
It combines real-time strategic gameplay with true digital asset ownership, using NFTs and smart contracts on the Ethereum Sepolia testnet.

## Features

- 🎮 **Real-Time Card Battle System**: Inspired by Clash Royale, players deploy NFT units in real-time battles with strategic energy management.
- 🧩 **NFT-Based Cards**: All battle units are represented as ERC-1155 NFTs. Players can own, collect, and manage their cards permanently.
- 🔗 **Blockchain Integration**: Key gameplay data — including card ownership, resources (gold, diamonds), and chest rewards — is recorded fully on-chain.
- 📦 **Chest Reward System**: After victories, players receive chests that unlock new NFTs after a countdown, with timers managed via smart contracts.
- 💎 **Diamond Purchase with ETH**: Players can buy premium resources (diamonds) directly through ETH transactions on the blockchain.
- 🌐 **WebGL Support**: The game is built to run in browsers, offering seamless wallet connection and blockchain interaction through the Thirdweb SDK.

## Technologies Used

- **Unity Engine** (WebGL Target Platform)
- **Ethereum Sepolia Testnet** (for all smart contract deployments)
- **Solidity** (ERC-1155 based NFT contracts and custom game logic contracts)
- **Thirdweb SDK** (Wallet connection and smart contract interaction)
- **C#** (Unity scripting)
- **OpenZeppelin** (Smart contract standards)

## Smart Contracts

The project includes four main smart contracts:

| Contract | Purpose |
|:---|:---|
| `UserManager.sol` | Manages player resources (gold, diamonds, ladder ratings) |
| `NFTManager.sol` | Issues and tracks card NFTs (ERC-1155 standard) |
| `DiamondPurchase.sol` | Handles diamond purchases via ETH |
| `Timer.sol` | Manages chest cooldown timers for players |

All contracts are deployed on the Sepolia testnet and follow Solidity best practices.

## Project Structure

```
HeroesArena/
├── Assets/             # Unity assets and scripts
├── Contracts/          # Solidity smart contracts
├── Packages/           # Unity packages
├── ProjectSettings/    # Unity project settings
├── README.md           # Project description
└── .gitignore          # Git tracking configuration
```

## How to Run

1. Clone this repository.
2. Open the project with Unity (recommended version: 2022.3+).
3. Build for WebGL or run in Unity editor.
4. Connect your Web3 wallet (e.g., MetaMask) via the login interface.
5. Start battling and managing your NFT cards!

> Note: To interact with the blockchain, you will need Sepolia test ETH for gas fees.

## Future Improvements

- Full NFT trading, staking, and rental marketplace integration
- Layer 2 blockchain migration for faster transactions
- Account Abstraction (EIP-4337) for better wallet experience
- Enhanced battle modes and PvP matchmaking

## About the Developer

This project was developed as part of a master's thesis, focusing on applying decentralized technologies to modern gaming.  
If you have any questions, feel free to reach out!

---