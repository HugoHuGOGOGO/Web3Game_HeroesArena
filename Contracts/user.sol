// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/utils/Strings.sol";

contract UserManager  {    

    struct User {
        uint256 gold;
        uint256 diamond;
        uint256 ladderRating;
        bool exists;
    }

    mapping(address => User) private users;

    uint256 public constant DEFAULT_GOLD = 10000;
    uint256 public constant DEFAULT_DIAMOND = 0;
    uint256 public constant DEFAULT_LADDER_RATING = 1000;


    function ifNew(address user) public view returns (uint256) {
    return users[user].exists ? 1 : 0;
    }

    // 内部注册新用户（如果该地址不存在）
    function isNew(address user) public {
        
            users[user] = User({
                gold: DEFAULT_GOLD,
                diamond: DEFAULT_DIAMOND,
                ladderRating: DEFAULT_LADDER_RATING,
                exists: true
            });
            
        
    }


    function setGold(address user, uint256 amount) external returns(uint256) {
        users[user].gold += amount;
        return users[user].gold;
    }

    function setDiamond(address user, uint256 amount) external  returns(uint256){
        users[user].diamond += amount;
        return users[user].diamond;
    }

    function setLadderRating(address user, uint256 rating) external returns(uint256) {
        users[user].ladderRating += rating;
        return users[user].ladderRating;
    }

    function useDiamond(address user, uint256 amount)external {
        require(users[user].diamond >= amount, "Insufficient diamonds");
        users[user].diamond -= amount;
    }


    

    function getUserData(address user) external view returns (string memory gold, string memory diamond, string memory ladderRating) {
    User storage u = users[user];
    gold = Strings.toString(u.gold);
    diamond = Strings.toString(u.diamond);
    ladderRating = Strings.toString(u.ladderRating);
    }

    
}