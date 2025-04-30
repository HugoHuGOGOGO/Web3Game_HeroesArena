// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract Timer {
    // 每个用户地址对应一个长度为4的数组，默认值均为0
    mapping(address => uint256[4]) private userLists;

    /// @notice 查询指定用户的列表（返回字符串格式）
    /// @param user 要查询的用户地址
    /// @return 返回该账户的4个字符串形式的数字
    function queryList(address user) public view returns (string[4] memory) {
        uint256[4] memory list = userLists[user];
        string[4] memory result;

        for (uint i = 0; i < 4; i++) {
            result[i] = _uintToString(list[i]);
        }

        return result;
    }

    /// @notice 如果列表中存在0，则在最左边的空位置填入当前时间戳+180
    /// @param user 要操作的用户地址
    function addNumber(address user) public {
        uint256[4] storage list = userLists[user];
        bool added = false;
        for (uint i = 0; i < 4; i++) {
            if (list[i] == 0) {
                list[i] = block.timestamp + 180;
                added = true;
                break;
            }
        }
        require(added, "No empty slot available");
    }

    /// @notice 删除用户列表中指定位置的数字（该位置的数字必须非0）
    /// @param user 要操作的用户地址
    /// @param index 要删除数字的位置，范围0~3
    function deleteNumber(address user, uint256 index) public {
        require(index < 4, "Index out of range");
        uint256[4] storage list = userLists[user];
        require(list[index] != 0, "Slot is already empty");
        list[index] = 0;
    }

    /// @dev 将 uint256 转换为 string
    function _uintToString(uint256 value) internal pure returns (string memory) {
        if (value == 0) {
            return "0";
        }
        uint256 temp = value;
        uint256 digits;
        while (temp != 0) {
            digits++;
            temp /= 10;
        }
        bytes memory buffer = new bytes(digits);
        while (value != 0) {
            digits -= 1;
            buffer[digits] = bytes1(uint8(48 + uint256(value % 10)));
            value /= 10;
        }
        return string(buffer);
    }
}