// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

interface IUserManager {
    function setDiamond(address user, uint256 amount) external;
}

contract DiamondPurchase {
    IUserManager public userManager;
    address public owner;
    // 单价：每颗钻石 0.0007 ether
    uint256 public constant UNIT_PRICE = 0.0007 ether;

    constructor(address _userManager) {
        userManager = IUserManager(_userManager);
        owner = msg.sender;
    }
    
    /**
     * @notice 用户通过选择档位购买钻石
     * @param tier 档位编号：1 => 1个，2 => 6个，3 => 30个，4 => 128个，5 => 328个，6 => 648个
     * @return 成功则返回 true
     */
    function purchaseDiamonds(uint256 tier) external payable returns (bool) {
        uint256 diamondCount;
        if (tier == 1) {
            diamondCount = 1;
        } else if (tier == 2) {
            diamondCount = 6;
        } else if (tier == 3) {
            diamondCount = 30;
        } else if (tier == 4) {
            diamondCount = 128;
        } else if (tier == 5) {
            diamondCount = 328;
        } else if (tier == 6) {
            diamondCount = 648;
        } else {
            revert("Invalid tier selection");
        }
        
        // 计算所需支付的金额
        uint256 requiredValue = diamondCount * UNIT_PRICE;
        require(msg.value == requiredValue, "Incorrect ETH value sent");
        
        // 调用 UserManager 合约的 setDiamond 方法更新用户的钻石数量
        userManager.setDiamond(msg.sender, diamondCount);
        
        return true;
    }
    
    // 提现方法，允许合约拥有者提取合约中的累计 ETH
    function withdraw() external {
        require(msg.sender == owner, "Only owner can withdraw");
        payable(owner).transfer(address(this).balance);
    }
}