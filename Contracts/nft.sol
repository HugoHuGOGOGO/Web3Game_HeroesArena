// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/utils/Strings.sol";

/**
 * @title NFTManager
 * @dev 管理 12 种 NFT 的智能合约（基于 ERC1155 标准）
 */
contract NFTManager is ERC1155 {
    using Strings for uint256;

    // 定义 NFT 的种类数量为 12
    uint256 public constant NFT_TYPE_COUNT = 12;

    /**
     * @dev 构造函数：初始化基础 URI
     */
    constructor() ERC1155("https://example.com/api/nft/{id}.json") {}

    /**
     * @dev distributeAllNFTs
     * 给指定玩家发放所有 12 种 NFT 各一个
     * @param player 接收 NFT 的玩家地址
     */
    function distributeAllNFTs(address player) public {
        for (uint256 i = 1; i <= NFT_TYPE_COUNT; i++) {
            _mint(player, i, 1, "");
        }
    }

    /**
     * @dev queryPlayerNFTs
     * 查询指定玩家每种 NFT 的数量，并返回字符串数组
     * @param player 玩家地址
     * @return nftCounts 字符串数组，数组下标 0 对应 NFT 类型 1 的数量（已转换为字符串）
     */
    function queryPlayerNFTs(address player) public view returns (string[] memory) {
        string[] memory nftCounts = new string[](NFT_TYPE_COUNT);
        for (uint256 i = 1; i <= NFT_TYPE_COUNT; i++) {
            nftCounts[i - 1] = balanceOf(player, i).toString();
        }
        return nftCounts;
    }

    /**
     * @dev distributeNFTsByTypes
     * 根据传入的 4 个 NFT 类型编号，给指定玩家发放这 4 种 NFT 各一个
     * 传入编号必须在 1 到 12 范围内
     * @param player 接收 NFT 的玩家地址
     * @param t1 NFT 类型编号 1
     * @param t2 NFT 类型编号 2
     * @param t3 NFT 类型编号 3
     * @param t4 NFT 类型编号 4
     */
    function distributeNFTsByTypes(
        address player,
        uint256 t1,
        uint256 t2,
        uint256 t3,
        uint256 t4
    ) public {
        // 参数合法性检查：确保 NFT 类型编号在合法范围内
        require(t1 >= 1 && t1 <= NFT_TYPE_COUNT, unicode"t1: 无效的NFT类型");
        require(t2 >= 1 && t2 <= NFT_TYPE_COUNT, unicode"t2: 无效的NFT类型");
        require(t3 >= 1 && t3 <= NFT_TYPE_COUNT, unicode"t3: 无效的NFT类型");
        require(t4 >= 1 && t4 <= NFT_TYPE_COUNT, unicode"t4: 无效的NFT类型");

        // 为玩家发放每种 NFT 各 1 个
        _mint(player, t1, 1, "");
        _mint(player, t2, 1, "");
        _mint(player, t3, 1, "");
        _mint(player, t4, 1, "");
    }
}