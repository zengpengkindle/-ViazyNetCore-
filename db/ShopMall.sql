/*
 Navicat Premium Data Transfer

 Source Server         : tencent
 Source Server Type    : MySQL
 Source Server Version : 50718
 Source Host           : sh-cynosdbmysql-grp-88q6q6gm.sql.tencentcdb.com:22370
 Source Schema         : ShopMall

 Target Server Type    : MySQL
 Target Server Version : 50718
 File Encoding         : 65001

 Date: 12/04/2023 19:47:57
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Areas
-- ----------------------------
DROP TABLE IF EXISTS `Areas`;
CREATE TABLE `Areas`  (
  `AreaCode` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ParentCode` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name` varchar(31) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PostCode` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DisplayOrder` int(11) NULL DEFAULT NULL,
  `Depth` int(11) NULL DEFAULT NULL,
  `ChildCount` int(11) NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Areas
-- ----------------------------

-- ----------------------------
-- Table structure for Credits
-- ----------------------------
DROP TABLE IF EXISTS `Credits`;
CREATE TABLE `Credits`  (
  `Id` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreditType` int(11) NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `Name` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreditKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Credits
-- ----------------------------
INSERT INTO `Credits` VALUES ('BRPYX25UVF286895', 1, 1, 'RMB', 'RMB', NULL, '2022-07-19 08:40:56');
INSERT INTO `Credits` VALUES ('BRPYXKE4P1J46896', 2, 1, 'Cion', 'Cion', NULL, '2022-07-19 08:41:05');
INSERT INTO `Credits` VALUES ('1FFGFI7BX4W4320', 2, 1, '积分', 'Cion2', NULL, '2023-03-12 10:38:47');

-- ----------------------------
-- Table structure for Finance
-- ----------------------------
DROP TABLE IF EXISTS `Finance`;
CREATE TABLE `Finance`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OutId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CreditName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Type` int(11) NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Amount` decimal(18, 4) NOT NULL,
  `Balance` decimal(18, 4) NOT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Finance
-- ----------------------------

-- ----------------------------
-- Table structure for LogisticsCompany
-- ----------------------------
DROP TABLE IF EXISTS `LogisticsCompany`;
CREATE TABLE `LogisticsCompany`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `RegMailNo` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of LogisticsCompany
-- ----------------------------

-- ----------------------------
-- Table structure for Member
-- ----------------------------
DROP TABLE IF EXISTS `Member`;
CREATE TABLE `Member`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Password` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Salt` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CreateTime` datetime NOT NULL,
  `LastLoginTime` datetime NULL DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `Exdata` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AvatarUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Member
-- ----------------------------
INSERT INTO `Member` VALUES ('a1234', 'a1234', 'member1', 'JC3fFEhKp0LqyJ7gBBqLFbt99/Z8AG7t1mvFnEbOInA=', '04166047-a076-489f-a804-f4a7d2e87b82', '2022-07-19 20:53:43', '2022-07-19 20:53:46', 1, NULL, NULL);

-- ----------------------------
-- Table structure for MemberAddress
-- ----------------------------
DROP TABLE IF EXISTS `MemberAddress`;
CREATE TABLE `MemberAddress`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsDefault` bit(1) NOT NULL,
  `ReceiverProvince` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverCity` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDistrict` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDetail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverMobile` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AreaCode` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PostalCode` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of MemberAddress
-- ----------------------------
INSERT INTO `MemberAddress` VALUES ('1GY89JU83285146', 'test_member', b'0', '天津市', '天津市', '和平区', '', '林XX', '15705938302', '', '');
INSERT INTO `MemberAddress` VALUES ('1GYEE6BV7NK5147', 'test_member', b'1', '福建省', '福州市', '鼓楼区', '', '卓XX', '18850020000', '', '');
INSERT INTO `MemberAddress` VALUES ('BRRRVAIYYG3K0366', 'test_member', b'0', '福建省', '福州市', '鼓楼区', '测试地址', '林测试', '15980771111', '350102', '');

-- ----------------------------
-- Table structure for MemberCapital
-- ----------------------------
DROP TABLE IF EXISTS `MemberCapital`;
CREATE TABLE `MemberCapital`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Credit1` decimal(18, 4) NOT NULL,
  `Credit2` decimal(18, 4) NOT NULL,
  `Credit3` decimal(18, 4) NOT NULL,
  `Credit4` decimal(18, 4) NOT NULL,
  `Credit5` decimal(18, 4) NOT NULL,
  `Exdata` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of MemberCapital
-- ----------------------------

-- ----------------------------
-- Table structure for MemberCar
-- ----------------------------
DROP TABLE IF EXISTS `MemberCar`;
CREATE TABLE `MemberCar`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ItemNum` int(11) NOT NULL,
  `TotalMoney` decimal(18, 4) NOT NULL,
  `TotalCredit1` decimal(18, 4) NOT NULL,
  `TotalCredit2` decimal(18, 4) NOT NULL,
  `TotalCredit3` decimal(18, 4) NOT NULL,
  `TotalCredit4` decimal(18, 4) NOT NULL,
  `TotalCredit5` decimal(18, 4) NOT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of MemberCar
-- ----------------------------

-- ----------------------------
-- Table structure for MemberCarItem
-- ----------------------------
DROP TABLE IF EXISTS `MemberCarItem`;
CREATE TABLE `MemberCarItem`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CarId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ProductId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `SkuId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OrderId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `SkuText` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Price` decimal(18, 4) NULL DEFAULT NULL,
  `IsFreeFreight` bit(1) NOT NULL,
  `Freight` decimal(18, 4) NOT NULL,
  `FreightStep` int(11) NOT NULL,
  `Credit1` decimal(18, 4) NOT NULL,
  `Credit2` decimal(18, 4) NOT NULL,
  `Credit3` decimal(18, 4) NOT NULL,
  `Credit4` decimal(18, 4) NOT NULL,
  `Credit5` decimal(18, 4) NOT NULL,
  `Num` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  `AddTime` datetime NOT NULL,
  `ChangedTime` datetime NULL DEFAULT NULL,
  `OrderTime` datetime NULL DEFAULT NULL,
  `RemoveTime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of MemberCarItem
-- ----------------------------
INSERT INTO `MemberCarItem` VALUES ('BRQRG6PBOL4W3800', 'test_member', '1FKHSRFF51C1848', NULL, '10000', NULL, '小羊咩咩', '', '/upload/public/image/2023/03/14/6381443119233469888795835.png', 12.0000, b'0', 0.0000, 0, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 4, 0, '2022-07-19 14:00:46', '2022-07-19 14:00:46', NULL, NULL);
INSERT INTO `MemberCarItem` VALUES ('BRRRP7RCLGQO7015', NULL, '1FKIBOLF3281851', NULL, '10000', NULL, '【懿琪宝贝】TZ宝宝牛仔裤2022春季新款男女童裤子YB21KZ013', '', '/upload/public/image/2023/03/14/6381443119233469888795835.png', 30.0000, b'0', 0.0000, 0, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 1, 0, '2022-07-19 20:47:09', '2022-07-19 20:47:09', NULL, NULL);
INSERT INTO `MemberCarItem` VALUES ('BRRRS0JTLMV40366', NULL, 'BRQITVPA39C05149', NULL, '10000', NULL, 'Test1', '', '/upload/public/image/2023/03/14/6381442979251715415542528.png', 30.0000, b'0', 0.0000, 0, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 2, 0, '2022-07-19 20:48:01', '2022-07-19 20:48:01', NULL, NULL);

-- ----------------------------
-- Table structure for MemberPayment
-- ----------------------------
DROP TABLE IF EXISTS `MemberPayment`;
CREATE TABLE `MemberPayment`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `TradeType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PayType` int(11) NOT NULL,
  `PayChannel` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Amount` decimal(18, 4) NOT NULL,
  `Status` int(11) NOT NULL,
  `StatusChangeTime` datetime NOT NULL,
  `CreateTime` datetime NOT NULL,
  `ResponseResult` blob NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of MemberPayment
-- ----------------------------

-- ----------------------------
-- Table structure for Product
-- ----------------------------
DROP TABLE IF EXISTS `Product`;
CREATE TABLE `Product`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `BrandId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BrandName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CatId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CatName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CatPath` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SubTitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Keywords` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Cost` decimal(10, 2) NOT NULL,
  `Price` decimal(10, 2) NOT NULL,
  `IsFreeFreight` bit(1) NOT NULL,
  `Freight` decimal(10, 2) NOT NULL,
  `FreightStep` int(11) NOT NULL,
  `FreightValue` decimal(10, 2) NOT NULL,
  `IsHidden` bit(1) NOT NULL,
  `Status` smallint(6) NOT NULL,
  `StatusChangeTime` datetime(3) NOT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SubImage` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OpenSpec` bit(1) NOT NULL,
  `SkuTree` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Detail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `CreateTime` datetime NOT NULL,
  `ModifyTime` datetime NOT NULL,
  `SearchContent` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `HasOuter` bit(1) NOT NULL,
  `OuterType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RefundType` int(11) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Product
-- ----------------------------
INSERT INTO `Product` VALUES ('1FKHSRFF51C1848', NULL, NULL, '12001', '童鞋/婴儿鞋', '10000', '测试', NULL, '小羊咩咩', NULL, NULL, NULL, 10.00, 12.00, b'0', 0.00, 0, 0.00, b'0', 2, '2023-03-14 22:53:17.000', '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', b'0', '[]', '<p>测试</p>', '2023-03-14 22:53:17', '2023-04-01 13:59:30', '小羊咩咩____', b'0', NULL, 4, NULL);
INSERT INTO `Product` VALUES ('1FKIBOLF3281851', NULL, NULL, '11001', '奶茶/茶饮', '10000', '测试', NULL, '小羊咩咩没~~咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩', NULL, NULL, NULL, 96.00, 12.90, b'0', 0.00, 0, 0.00, b'0', 2, '2023-03-14 23:03:45.000', '/upload/public/image/2023/03/14/6381443164371038354913416.png', '', b'1', '[{\"K\":\"颜色\",\"V\":[{\"Id\":\"s10001\",\"Name\":\"红色\",\"ImgUrl\":\"/upload/public/image/2023/03/14/6381443183453456688369200.png\"},{\"Id\":\"s10002\",\"Name\":\"白色\",\"ImgUrl\":\"/upload/public/image/2023/03/14/6381443183707506325539155.png\"},{\"Id\":\"s10003\",\"Name\":\"绿色\",\"ImgUrl\":\"\"},{\"Id\":\"s10004\",\"Name\":\"粉色\",\"ImgUrl\":\"\"}],\"k_s\":\"s1\"}]', '<p><img src=\"/upload/public/image/2023/03/15/6381449259326860566568908.jpg\" alt=\"\" data-href=\"\" style=\"\"/><img src=\"/upload/public/image/2023/04/06/6381639192399020791926411.webp\" alt=\"\" data-href=\"\" style=\"\"/><img src=\"/upload/public/image/2023/04/06/6381639192405933975806809.webp\" alt=\"\" data-href=\"\" style=\"\"/><img src=\"/upload/public/image/2023/04/06/6381639192408384257950677.webp\" alt=\"\" data-href=\"\" style=\"\"/></p>', '2023-03-14 23:03:46', '2023-04-06 15:52:58', '小羊咩咩没~~咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩____', b'0', NULL, 4, NULL);
INSERT INTO `Product` VALUES ('1H4OIPMG75S7641', NULL, NULL, '1GLWD6WZXC01548', '手机通讯', '10000', '测试', NULL, '迷你便携高颜值蓝牙无线耳机立体声只能触控式操作简约立体声耳机', NULL, NULL, NULL, 300.00, 290.00, b'0', 0.00, 0, 0.00, b'0', 2, '2023-04-11 22:44:09.578', '/upload/public/image/2023/04/11/6381684984115551305246234.png', '', b'0', '[]', '<p><img src=\"/upload/public/image/2023/04/11/6381684984786700333699273.webp\" alt=\"\" data-href=\"\" style=\"\"/></p>', '2023-04-11 22:44:10', '2023-04-11 22:44:10', '迷你便携高颜值蓝牙无线耳机立体声只能触控式操作简约立体声耳机____', b'0', NULL, 4, NULL);
INSERT INTO `Product` VALUES ('1H4OZC2R7SW2942', NULL, NULL, '1GLWG4RKT4W1554', '烹饪锅具', '10000', '测试', NULL, '简约餐盘耐热家用盘子菜盘套装多颜色简约餐盘耐热家用盘子', NULL, NULL, NULL, 200.00, 129.00, b'0', 0.00, 0, 0.00, b'0', 2, '2023-04-11 22:53:21.820', '/upload/public/image/2023/04/11/6381685038582121902066009.png', '/upload/public/image/2023/04/11/6381685038809388107425954.png,/upload/public/image/2023/04/11/6381685039134285997473193.png', b'1', '[{\"K\":\"颜色\",\"V\":[{\"Id\":\"s10001\",\"Name\":\"灰色\",\"ImgUrl\":\"/upload/public/image/2023/04/11/6381685037797091102348210.png\"},{\"Id\":\"s10002\",\"Name\":\"白色\",\"ImgUrl\":\"/upload/public/image/2023/04/11/6381685038102550322545719.png\"}],\"k_s\":\"s1\"}]', '<p><img src=\"/upload/public/image/2023/04/11/6381685039904593071489102.webp\" alt=\"\" data-href=\"\" style=\"\"/></p>', '2023-04-11 22:53:22', '2023-04-11 22:53:22', '简约餐盘耐热家用盘子菜盘套装多颜色简约餐盘耐热家用盘子____', b'0', NULL, 4, NULL);
INSERT INTO `Product` VALUES ('BRQITVPA39C05149', NULL, NULL, '3', '学习用品', '10000', '测试', '学习用品', 'Test1', NULL, NULL, NULL, 100.00, 30.00, b'0', 0.00, 0, 0.00, b'0', 1, '2022-07-19 12:24:09.000', '/upload/public/image/2023/03/14/6381442979251715415542528.png', '', b'0', '[]', '<p>测试<br></p>', '2022-07-19 12:24:09', '2023-03-25 12:17:42', 'Test1___学习用品_', b'0', '', 1, NULL);

-- ----------------------------
-- Table structure for ProductBrand
-- ----------------------------
DROP TABLE IF EXISTS `ProductBrand`;
CREATE TABLE `ProductBrand`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `IsHidden` bit(1) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `Sort` int(11) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductBrand
-- ----------------------------

-- ----------------------------
-- Table structure for ProductCat
-- ----------------------------
DROP TABLE IF EXISTS `ProductCat`;
CREATE TABLE `ProductCat`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `IsHidden` bit(1) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsParent` bit(1) NOT NULL,
  `Path` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Sort` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductCat
-- ----------------------------
INSERT INTO `ProductCat` VALUES ('1', b'0', '酒水饮料', NULL, b'0', NULL, '/upload/public/image/2023/04/02/6381604066709757586487626.webp', 10, 1, NULL, '2023-04-02 14:02:04');
INSERT INTO `ProductCat` VALUES ('11001', b'0', '奶茶/茶饮', '1', b'0', NULL, NULL, 1, 1, NULL, '2023-04-02 14:02:26');
INSERT INTO `ProductCat` VALUES ('11002', b'0', '果汁/果醋', '1', b'0', NULL, NULL, 2, 1, NULL, '2023-04-02 14:02:41');
INSERT INTO `ProductCat` VALUES ('11003', b'0', '啤酒', '1', b'0', NULL, NULL, 3, 1, NULL, '2023-04-02 14:02:52');
INSERT INTO `ProductCat` VALUES ('11004', b'0', '冲调即饮咖啡', '1', b'0', NULL, NULL, 4, 1, NULL, '2023-04-02 14:03:12');
INSERT INTO `ProductCat` VALUES ('12001', b'0', '童鞋/婴儿鞋', '2', b'0', NULL, NULL, 1, 1, NULL, NULL);
INSERT INTO `ProductCat` VALUES ('12002', b'0', '童装/婴儿装', '2', b'0', NULL, NULL, 2, 1, NULL, NULL);
INSERT INTO `ProductCat` VALUES ('12003', b'0', '孕妇装', '2', b'0', NULL, NULL, 3, 1, NULL, NULL);
INSERT INTO `ProductCat` VALUES ('12004', b'0', '婴童用品', '2', b'0', NULL, NULL, 4, 1, NULL, NULL);
INSERT INTO `ProductCat` VALUES ('12005', b'0', '婴童尿裤', '2', b'0', NULL, NULL, 5, 1, NULL, NULL);
INSERT INTO `ProductCat` VALUES ('12006', b'0', '奶粉/辅食', '2', b'0', NULL, NULL, 6, 1, NULL, NULL);
INSERT INTO `ProductCat` VALUES ('1GLLKE9Y16O9989', b'0', '卫生巾', '3', b'0', NULL, '', 0, 1, '', '2023-04-02 14:04:27');
INSERT INTO `ProductCat` VALUES ('1GLVTLESUIO1536', b'0', '休闲食品', '', b'0', NULL, '/upload/public/image/2023/04/02/6381604069150375836821239.webp', 0, 1, '', '2023-04-02 13:58:14');
INSERT INTO `ProductCat` VALUES ('1GLVULB64JK1537', b'0', '家电数码', '', b'0', NULL, '/upload/public/image/2023/04/02/6381604072611302879084898.jpg', 0, 1, '', '2023-04-02 13:58:47');
INSERT INTO `ProductCat` VALUES ('1GLVW6CSEBK1538', b'0', '家居百货', '', b'0', NULL, '/upload/public/image/2023/04/02/6381604077867752601831433.webp', 0, 1, '', '2023-04-02 13:59:40');
INSERT INTO `ProductCat` VALUES ('1GLVWS3QDTS1539', b'0', '粮油调味', '', b'0', NULL, '/upload/public/image/2023/04/02/6381604079882016843557567.jpg', 0, 1, '', '2023-04-02 14:00:00');
INSERT INTO `ProductCat` VALUES ('1GLW33QFB0G1540', b'0', '功能/运动饮料', '1', b'0', NULL, '', 0, 1, '', '2023-04-02 14:03:30');
INSERT INTO `ProductCat` VALUES ('1GLW5CXDHMO1541', b'0', '面部护理', '3', b'0', NULL, '', 0, 1, '', '2023-04-02 14:04:45');
INSERT INTO `ProductCat` VALUES ('1GLW5QEOP6O1542', b'0', '沐浴清洁', '3', b'0', NULL, '', 0, 1, '', '2023-04-02 14:04:57');
INSERT INTO `ProductCat` VALUES ('1GLW6XKLFUO1543', b'0', '纸品湿巾', '4', b'0', NULL, '', 0, 1, '', '2023-04-02 14:05:37');
INSERT INTO `ProductCat` VALUES ('1GLW7ABSXHC1544', b'0', '清洁工具', '4', b'0', NULL, '', 0, 1, '', '2023-04-02 14:05:49');
INSERT INTO `ProductCat` VALUES ('1GLWBVU9R401545', b'0', '饼干', '1GLVTLESUIO1536', b'0', NULL, '', 0, 1, '', '2023-04-02 14:08:21');
INSERT INTO `ProductCat` VALUES ('1GLWC84FEKG1546', b'0', '糕点', '1GLVTLESUIO1536', b'0', NULL, '', 0, 1, '', '2023-04-02 14:08:33');
INSERT INTO `ProductCat` VALUES ('1GLWCJCUQV41547', b'0', '果冻糕点', '1GLVTLESUIO1536', b'0', NULL, '', 0, 1, '', '2023-04-02 14:08:43');
INSERT INTO `ProductCat` VALUES ('1GLWD6WZXC01548', b'0', '手机通讯', '1GLVULB64JK1537', b'0', NULL, '', 0, 1, '', '2023-04-02 14:09:05');
INSERT INTO `ProductCat` VALUES ('1GLWDOHWZ9C1549', b'0', '电脑电教', '1GLVULB64JK1537', b'0', NULL, '', 0, 1, '', '2023-04-02 14:09:21');
INSERT INTO `ProductCat` VALUES ('1GLWDZ3ZG5C1550', b'0', '大家电', '1GLVULB64JK1537', b'0', NULL, '', 0, 1, '', '2023-04-02 14:09:31');
INSERT INTO `ProductCat` VALUES ('1GLWEC3URUO1551', b'0', '电饭煲', '1GLVULB64JK1537', b'0', NULL, '', 0, 1, '', '2023-04-02 14:09:43');
INSERT INTO `ProductCat` VALUES ('1GLWF1AOKG01552', b'0', '厨用小电器', '1GLVULB64JK1537', b'0', NULL, '', 0, 1, '', '2023-04-02 14:10:06');
INSERT INTO `ProductCat` VALUES ('1GLWFRZ5RWG1553', b'0', '户外踏青', '1GLVW6CSEBK1538', b'0', NULL, '', 0, 1, '', '2023-04-02 14:10:31');
INSERT INTO `ProductCat` VALUES ('1GLWG4RKT4W1554', b'0', '烹饪锅具', '1GLVW6CSEBK1538', b'0', NULL, '', 0, 1, '', '2023-04-02 14:10:42');
INSERT INTO `ProductCat` VALUES ('1GLWGFMKY2O1555', b'0', '厨房配件', '1GLVW6CSEBK1538', b'0', NULL, '', 0, 1, '', '2023-04-02 14:10:52');
INSERT INTO `ProductCat` VALUES ('1GLWGWCMWSG1556', b'0', '水具酒具', '1GLVW6CSEBK1538', b'0', NULL, '', 0, 1, '', '2023-04-02 14:11:08');
INSERT INTO `ProductCat` VALUES ('1GLWHK33N9C1557', b'0', '调味汁', '1GLVWS3QDTS1539', b'0', NULL, '', 0, 1, '', '2023-04-02 14:11:30');
INSERT INTO `ProductCat` VALUES ('1GLWI8G8HOG1558', b'0', '面条意面', '1GLVWS3QDTS1539', b'0', NULL, '', 0, 1, '', '2023-04-02 14:11:52');
INSERT INTO `ProductCat` VALUES ('1GLWII7LHMO1559', b'0', '大米', '1GLVWS3QDTS1539', b'0', NULL, '', 0, 1, '', '2023-04-02 14:12:01');
INSERT INTO `ProductCat` VALUES ('1GLWIQAR3B41560', b'0', '食用油', '1GLVWS3QDTS1539', b'0', NULL, '', 0, 1, '', '2023-04-02 14:12:09');
INSERT INTO `ProductCat` VALUES ('2', b'0', '母婴玩具', NULL, b'0', NULL, '/upload/public/image/2023/04/02/6381604063939833675241483.webp', 1, 1, NULL, '2023-04-02 13:57:38');
INSERT INTO `ProductCat` VALUES ('3', b'0', '个护美妆', NULL, b'0', NULL, '/upload/public/image/2023/04/02/6381604082137500894595001.webp', 2, 1, NULL, '2023-04-02 14:00:34');
INSERT INTO `ProductCat` VALUES ('4', b'0', '纸品家清', NULL, b'0', NULL, '/upload/public/image/2023/04/02/6381604075845773944169860.webp', 3, 1, NULL, '2023-04-02 13:59:31');
INSERT INTO `ProductCat` VALUES ('5', b'0', '生鲜', NULL, b'1', NULL, NULL, 4, 1, NULL, NULL);

-- ----------------------------
-- Table structure for ProductLogistics
-- ----------------------------
DROP TABLE IF EXISTS `ProductLogistics`;
CREATE TABLE `ProductLogistics`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `TradeId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ExpressNo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverProvince` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverCity` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDistrict` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDetail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverMobile` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `LogisticsCompany` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `LogisticsFee` decimal(18, 4) NULL DEFAULT NULL,
  `AgencyFee` decimal(18, 4) NULL DEFAULT NULL,
  `DeliveryAmount` decimal(18, 4) NULL DEFAULT NULL,
  `LogisticsStatus` int(11) NOT NULL,
  `LogisticsCreateTime` datetime NOT NULL,
  `LogisticsUpdateTime` datetime NOT NULL,
  `LogisticsSettlementTime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductLogistics
-- ----------------------------

-- ----------------------------
-- Table structure for ProductOuter
-- ----------------------------
DROP TABLE IF EXISTS `ProductOuter`;
CREATE TABLE `ProductOuter`  (
  `Id` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OuterName` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterType` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BeginTime` datetime NULL DEFAULT NULL,
  `EndTime` datetime NULL DEFAULT NULL,
  `CreateTime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductOuter
-- ----------------------------
INSERT INTO `ProductOuter` VALUES ('1FG6RJE1W007965', '积分活动', 'Cion', 1, '积分活动', '2023-02-28 16:00:00', '2023-03-30 16:00:00', '2023-03-12 19:23:36');
INSERT INTO `ProductOuter` VALUES ('BRQ02QMKMHVK6895', '营销商品', 'Marketing', 1, 'description', '2023-02-28 16:00:00', '2023-03-30 16:00:00', '2022-07-19 08:53:55');

-- ----------------------------
-- Table structure for ProductOuterSpecialCredit
-- ----------------------------
DROP TABLE IF EXISTS `ProductOuterSpecialCredit`;
CREATE TABLE `ProductOuterSpecialCredit`  (
  `Id` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OuterType` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreditKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ObjectKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ObjectName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ComputeType` int(11) NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `FeeMoney` decimal(18, 4) NULL DEFAULT NULL,
  `FeePercent` decimal(18, 4) NULL DEFAULT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductOuterSpecialCredit
-- ----------------------------
INSERT INTO `ProductOuterSpecialCredit` VALUES ('BRQ0RTRZZ18G6895', 'Marketing', 'Cion', '3折', '3折', 4, 1, NULL, NULL, NULL, '2022-07-19 09:01:43');

-- ----------------------------
-- Table structure for ProductOuterSpecialPrice
-- ----------------------------
DROP TABLE IF EXISTS `ProductOuterSpecialPrice`;
CREATE TABLE `ProductOuterSpecialPrice`  (
  `Id` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductId` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `SkuId` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OuterType` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CreditKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ObjectKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Price` decimal(18, 4) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductOuterSpecialPrice
-- ----------------------------

-- ----------------------------
-- Table structure for ProductSku
-- ----------------------------
DROP TABLE IF EXISTS `ProductSku`;
CREATE TABLE `ProductSku`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OuterSkuId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `S1` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Key1` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name1` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `S2` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Key2` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name2` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `S3` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Key3` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name3` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Cost` decimal(18, 4) NOT NULL,
  `Price` decimal(18, 4) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SpecDetailIds` varchar(511) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SpecDetailText` varchar(511) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `UpdateTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductSku
-- ----------------------------
INSERT INTO `ProductSku` VALUES ('1FKIBOLF3281850', '1FKIBOLF3281851', NULL, 's10001', '颜色', '红色', '0', '', '', '0', '', '', 10.0000, 12.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `ProductSku` VALUES ('1FKIBOQ99HC1851', '1FKIBOLF3281851', NULL, 's10002', '颜色', '白色', '0', '', '', '0', '', '', 10.0000, 13.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `ProductSku` VALUES ('1FKIBOUWF281852', '1FKIBOLF3281851', NULL, 's10003', '颜色', '绿色', '0', '', '', '0', '', '', 10.0000, 15.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `ProductSku` VALUES ('1FM3W4Z64CG1183', '1FKIBOLF3281851', NULL, 's10004', '颜色', '粉色', '0', '', '', '0', '', '', 10.0000, 15.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `ProductSku` VALUES ('1H4OZC2R7SW2942', '1H4OZC2R7SW2942', NULL, 's10001', '颜色', '灰色', '0', '', '', '0', '', '', 200.0000, 129.0000, NULL, NULL, NULL, NULL, 0, '2023-04-11 22:53:22', '2023-04-11 22:53:22');
INSERT INTO `ProductSku` VALUES ('1H4OZC74JEO2943', '1H4OZC2R7SW2942', NULL, 's10002', '颜色', '白色', '0', '', '', '0', '', '', 200.0000, 129.0000, NULL, NULL, NULL, NULL, 0, '2023-04-11 22:53:22', '2023-04-11 22:53:22');

-- ----------------------------
-- Table structure for ProductStock
-- ----------------------------
DROP TABLE IF EXISTS `ProductStock`;
CREATE TABLE `ProductStock`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductSkuId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `InStock` int(11) NOT NULL,
  `Lock` int(11) NOT NULL,
  `OutStock` int(11) NOT NULL,
  `SellNum` int(11) NOT NULL,
  `Refund` int(11) NOT NULL,
  `Exchange` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductStock
-- ----------------------------
INSERT INTO `ProductStock` VALUES ('1FKHSRFHY681848', '1FKHSRFF51C1848', '', 100, 0, 0, 0, 0, 0, '2023-03-14 22:53:17', '2023-03-14 22:53:17');
INSERT INTO `ProductStock` VALUES ('1FKI69K8U2O1849', '1FKI69K60XS1849', '1FKI69K7FI81848', 100, 0, 0, 0, 0, 0, '2023-03-14 23:00:46', '2023-03-14 23:00:46');
INSERT INTO `ProductStock` VALUES ('1FKI6FKZQRK1850', '1FKI6FKZQRK1850', '1FKI6FKZQRK1849', 100, 0, 0, 0, 0, 0, '2023-03-14 23:00:51', '2023-03-14 23:00:51');
INSERT INTO `ProductStock` VALUES ('1FKIBOLF3281851', '1FKIBOLF3281851', '1FKIBOLF3281850', 100, 0, 0, 0, 0, 0, '2023-03-14 23:03:46', '2023-03-14 23:03:46');
INSERT INTO `ProductStock` VALUES ('1FKIBOQ99HC1852', '1FKIBOLF3281851', '1FKIBOQ99HC1851', 100, 0, 0, 0, 0, 0, '2023-03-14 23:03:46', '2023-03-14 23:03:46');
INSERT INTO `ProductStock` VALUES ('1FKIBOUWF281853', '1FKIBOLF3281851', '1FKIBOUWF281852', 0, 0, 0, 0, 0, 0, '2023-03-14 23:03:46', '2023-03-14 23:03:46');
INSERT INTO `ProductStock` VALUES ('1FM3W4Z7IWW1183', '1FKIBOLF3281851', '1FM3W4Z64CG1183', 1, 0, 0, 0, 0, 0, '2023-03-15 18:11:02', '2023-03-15 18:11:02');
INSERT INTO `ProductStock` VALUES ('1H4OIPMJ0AO7641', '1H4OIPMG75S7641', '', 100, 0, 0, 0, 0, 0, '2023-04-11 22:44:10', '2023-04-11 22:44:10');
INSERT INTO `ProductStock` VALUES ('1H4OPUB3Y2O7642', '1H4OPUB14XS7642', '1H4OPUB2JI87641', 100, 0, 0, 0, 0, 0, '2023-04-11 22:48:07', '2023-04-11 22:48:07');
INSERT INTO `ProductStock` VALUES ('1H4OPYTDXZ47643', '1H4OPYTDXZ47643', '1H4OPYTDXZ47642', 100, 0, 0, 0, 0, 0, '2023-04-11 22:48:11', '2023-04-11 22:48:11');
INSERT INTO `ProductStock` VALUES ('1H4OQ71M5HC7644', '1H4OQ71M5HC7644', '1H4OQ71M5HC7643', 100, 0, 0, 0, 0, 0, '2023-04-11 22:48:18', '2023-04-11 22:48:18');
INSERT INTO `ProductStock` VALUES ('1H4OS0HS6RK7645', '1H4OS0HS6RK7645', '1H4OS0HS6RK7644', 100, 0, 0, 0, 0, 0, '2023-04-11 22:49:19', '2023-04-11 22:49:19');
INSERT INTO `ProductStock` VALUES ('1H4OT9ZGQPS7646', '1H4OSXZYKAO7646', '1H4OT3UFBK07645', 100, 0, 0, 0, 0, 0, '2023-04-11 22:50:01', '2023-04-11 22:50:01');
INSERT INTO `ProductStock` VALUES ('1H4OZC2U0XS2942', '1H4OZC2R7SW2942', '1H4OZC2R7SW2942', 100, 0, 0, 0, 0, 0, '2023-04-11 22:53:22', '2023-04-11 22:53:22');
INSERT INTO `ProductStock` VALUES ('1H4OZC74JEO2943', '1H4OZC2R7SW2942', '1H4OZC74JEO2943', 100, 0, 0, 0, 0, 0, '2023-04-11 22:53:22', '2023-04-11 22:53:22');
INSERT INTO `ProductStock` VALUES ('BRQITVPA39C05149', 'BRQITVPA39C05149', '', 99, 1, 0, 0, 0, 0, '2022-07-19 12:24:09', '2022-07-19 12:24:33');

-- ----------------------------
-- Table structure for ProductStockUpdateLog
-- ----------------------------
DROP TABLE IF EXISTS `ProductStockUpdateLog`;
CREATE TABLE `ProductStockUpdateLog`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `StockId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OldInStock` int(11) NOT NULL,
  `NewInStock` int(11) NOT NULL,
  `Amount` int(11) NOT NULL,
  `UserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductStockUpdateLog
-- ----------------------------
INSERT INTO `ProductStockUpdateLog` VALUES ('BRQIV6YYX1J45148', 'BRQITVPA39C05149', 0, 100, 100, 'BROAN5W22SQO1028', '初始', '2022-07-19 12:24:33');

-- ----------------------------
-- Table structure for ProductTrade
-- ----------------------------
DROP TABLE IF EXISTS `ProductTrade`;
CREATE TABLE `ProductTrade`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `MemberName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PaymentId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `HasOuter` bit(1) NOT NULL,
  `OuterId` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ObjectKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreditKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BusinessType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `TotalFreight` decimal(10, 2) NOT NULL,
  `ProductMoney` decimal(10, 2) NOT NULL,
  `TotalMoney` decimal(10, 2) NOT NULL,
  `ReceiverProvince` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverCity` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDistrict` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDetail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverMobile` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsCode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsCompany` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsCost` decimal(10, 2) NULL DEFAULT NULL,
  `PayMode` smallint(6) NOT NULL,
  `Message` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime(3) NOT NULL,
  `PayTime` datetime(3) NULL DEFAULT NULL,
  `ConsignTime` datetime(3) NULL DEFAULT NULL,
  `CompleteTime` datetime(3) NULL DEFAULT NULL,
  `Status` smallint(6) NOT NULL,
  `StatusChangedTime` datetime(3) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductTrade
-- ----------------------------
INSERT INTO `ProductTrade` VALUES ('1GZNK8LTJ7K4836', 'test_member', NULL, '10000', '直营', '', b'0', NULL, NULL, NULL, NULL, NULL, 0.00, 25.00, 25.00, '福建省', '福州市', '鼓楼区', '测试地址', '林测试', '15980771111', NULL, NULL, NULL, NULL, 0, NULL, '2023-04-09 10:37:50.919', NULL, NULL, NULL, -1, '2023-04-09 10:37:50.919', NULL);
INSERT INTO `ProductTrade` VALUES ('1GZXREIBF5S4837', 'test_member', NULL, '10000', '直营', '', b'0', NULL, NULL, NULL, NULL, NULL, 0.00, 12.00, 12.00, '福建省', '福州市', '鼓楼区', '', '卓XX', '18850020000', NULL, NULL, NULL, NULL, 0, NULL, '2023-04-09 14:01:06.585', NULL, NULL, NULL, -1, '2023-04-09 14:01:06.585', NULL);
INSERT INTO `ProductTrade` VALUES ('1H27NRVJ8SG6510', 'test_member', NULL, '10000', '直营', '', b'0', NULL, NULL, NULL, NULL, NULL, 0.00, 12.00, 12.00, '福建省', '福州市', '鼓楼区', '', '卓XX', '18850020000', NULL, NULL, NULL, NULL, 0, NULL, '2023-04-10 17:13:16.708', NULL, NULL, NULL, -1, '2023-04-10 17:13:16.708', NULL);
INSERT INTO `ProductTrade` VALUES ('1H27NSBQ6GW6511', 'test_member', NULL, '10000', '直营', '', b'0', NULL, NULL, NULL, NULL, NULL, 0.00, 12.00, 12.00, '福建省', '福州市', '鼓楼区', '', '卓XX', '18850020000', NULL, NULL, NULL, NULL, 0, NULL, '2023-04-10 17:13:17.354', NULL, NULL, NULL, -1, '2023-04-10 17:13:17.354', NULL);
INSERT INTO `ProductTrade` VALUES ('BRRU2JLSTL342314', 'a1234', 'member1', '10000', NULL, '', b'0', NULL, NULL, NULL, NULL, NULL, 0.00, 30.00, 30.00, '福建省', '福州市', '鼓楼区', '测试地址', '林测试', '15980771111', NULL, NULL, NULL, NULL, 0, NULL, '2022-07-19 21:13:43.000', NULL, NULL, NULL, -1, '2022-07-19 21:13:43.000', NULL);
INSERT INTO `ProductTrade` VALUES ('BRRU2JLSTL342315', 'a1234', 'member1', '10000', NULL, '', b'0', NULL, NULL, NULL, NULL, NULL, 0.00, 30.00, 30.00, '福建省', '福州市', '闽侯县', '测试地址', '林测试', '15980771111', NULL, NULL, NULL, NULL, 0, NULL, '2022-07-19 21:13:43.000', NULL, NULL, NULL, 1, '2022-07-19 21:13:43.000', NULL);

-- ----------------------------
-- Table structure for ProductTradeOrder
-- ----------------------------
DROP TABLE IF EXISTS `ProductTradeOrder`;
CREATE TABLE `ProductTradeOrder`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `HasOuter` bit(1) NOT NULL,
  `OuterType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ProductTradeId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ProductId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BrandId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BrandName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SkuId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterSkuId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SkuText` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Num` int(11) NOT NULL,
  `Cost` decimal(10, 2) NOT NULL,
  `RefundType` smallint(6) NOT NULL,
  `Price` decimal(10, 2) NOT NULL,
  `TotalMoney` decimal(10, 2) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductTradeOrder
-- ----------------------------
INSERT INTO `ProductTradeOrder` VALUES ('1GZNK8SSYV44836', b'0', NULL, '1GZNK8LTJ7K4836', '1FKIBOLF3281851', NULL, NULL, '小羊咩咩没~~咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩', NULL, '/upload/public/image/2023/03/14/6381443183707506325539155.png', '1FKIBOQ99HC1851', NULL, NULL, '颜色:白色;', 1, 10.00, 4, 13.00, 13.00, NULL);
INSERT INTO `ProductTradeOrder` VALUES ('1GZNK8VVXQ84837', b'0', NULL, '1GZNK8LTJ7K4836', '1FKHSRFF51C1848', NULL, NULL, '小羊咩咩', NULL, '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', NULL, NULL, '', 1, 10.00, 4, 12.00, 12.00, NULL);
INSERT INTO `ProductTradeOrder` VALUES ('1GZXREM6HDS4838', b'0', NULL, '1GZXREIBF5S4837', '1FKHSRFF51C1848', NULL, NULL, '小羊咩咩', NULL, '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', NULL, NULL, '', 1, 10.00, 4, 12.00, 12.00, NULL);
INSERT INTO `ProductTradeOrder` VALUES ('1H27NRYHZY86510', b'0', NULL, '1H27NRVJ8SG6510', '1FKHSRFF51C1848', NULL, NULL, '小羊咩咩', NULL, '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', NULL, NULL, '', 1, 10.00, 4, 12.00, 12.00, NULL);
INSERT INTO `ProductTradeOrder` VALUES ('1H27NSECAIO6511', b'0', NULL, '1H27NSBQ6GW6511', '1FKHSRFF51C1848', NULL, NULL, '小羊咩咩', NULL, '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', NULL, NULL, '', 1, 10.00, 4, 12.00, 12.00, NULL);
INSERT INTO `ProductTradeOrder` VALUES ('BRRU2JM0BA4G2314', b'0', NULL, 'BRRU2JLSTL342314', 'BRQITVPA39C05149', NULL, NULL, 'Test1', NULL, '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', NULL, NULL, '', 1, 100.00, 1, 30.00, 30.00, NULL);
INSERT INTO `ProductTradeOrder` VALUES ('BRRU2JM0BA4G2315', b'0', NULL, 'BRRU2JLSTL342315', 'BRQITVPA39C05149', NULL, NULL, 'Test1', NULL, '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', NULL, NULL, '', 1, 100.00, 1, 30.00, 30.00, NULL);

-- ----------------------------
-- Table structure for ProductTradeSet
-- ----------------------------
DROP TABLE IF EXISTS `ProductTradeSet`;
CREATE TABLE `ProductTradeSet`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `TradeCount` int(11) NULL DEFAULT NULL,
  `BusinessType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `TotalFreight` decimal(18, 4) NOT NULL,
  `ProductMoney` decimal(18, 4) NOT NULL,
  `TotalMoney` decimal(18, 4) NOT NULL,
  `PayMode` int(11) NOT NULL,
  `PayMoney` decimal(18, 4) NOT NULL,
  `RealPayMoney` decimal(18, 4) NULL DEFAULT NULL,
  `RealPayFreight` decimal(18, 4) NULL DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `PayTime` datetime NULL DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `StatusChangedTime` datetime NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductTradeSet
-- ----------------------------

-- ----------------------------
-- Table structure for Shop
-- ----------------------------
DROP TABLE IF EXISTS `Shop`;
CREATE TABLE `Shop`  (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ComponyName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Contact` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Phone` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AddressFH` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AddressTH` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OpenMode` smallint(6) NOT NULL,
  `Intro` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Logo` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Banner` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OfficeTime` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `MinDistribution` decimal(10, 2) NOT NULL,
  `Province` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `City` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Area` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Shop
-- ----------------------------
INSERT INTO `Shop` VALUES ('10000', '直营', NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 0.00, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for ShopPage
-- ----------------------------
DROP TABLE IF EXISTS `ShopPage`;
CREATE TABLE `ShopPage`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Layout` enum('Mobile','Pc') CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Type` enum('Mobile','Pc') CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` smallint(6) NOT NULL,
  `CreateTime` datetime(3) NOT NULL,
  `ModifyTime` datetime(3) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ShopPage
-- ----------------------------
INSERT INTO `ShopPage` VALUES (1, 'mobile_home', '首页', '', 'Mobile', 'Mobile', 1, '2023-03-18 14:32:11.218', '2023-03-18 14:32:11.218');

-- ----------------------------
-- Table structure for ShopPageItem
-- ----------------------------
DROP TABLE IF EXISTS `ShopPageItem`;
CREATE TABLE `ShopPageItem`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `WidgetCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PageCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PositionId` int(11) NOT NULL,
  `Sort` int(11) NOT NULL,
  `Parameters` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 69 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ShopPageItem
-- ----------------------------
INSERT INTO `ShopPageItem` VALUES (64, 'search', 'mobile_home', 0, 1, '{\r\n  \"keywords\": \"请输入关键字搜索\",\r\n  \"style\": \"round\"\r\n}');
INSERT INTO `ShopPageItem` VALUES (65, 'imgSlide', 'mobile_home', 1, 2, '{\r\n  \"duration\": 2500,\r\n  \"list\": [\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"linkType\": \"\",\r\n      \"linkValue\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"linkType\": \"\",\r\n      \"linkValue\": \"\"\r\n    }\r\n  ]\r\n}');
INSERT INTO `ShopPageItem` VALUES (66, 'navBar', 'mobile_home', 2, 3, '{\r\n  \"limit\": 4,\r\n  \"list\": [\r\n    {\r\n      \"image\": \"/src/assets/images/empty.png\",\r\n      \"text\": \"新品推荐\",\r\n      \"linkType\": 1,\r\n      \"linkValue\": \"/pages/selection/result/index\",\r\n      \"url\": \"/upload/public/image/2023/04/10/6381676761945089684314620.png\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty.png\",\r\n      \"text\": \"限时抢购\",\r\n      \"linkType\": 1,\r\n      \"linkValue\": \"/pages/selection/result/index\",\r\n      \"url\": \"/upload/public/image/2023/04/10/6381676763408593864006283.png\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty.png\",\r\n      \"text\": \"爆品推荐\",\r\n      \"linkType\": 1,\r\n      \"linkValue\": \"/pages/selection/result/index\",\r\n      \"url\": \"/upload/public/image/2023/04/10/6381676765294845261090523.png\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty.png\",\r\n      \"text\": \"推荐好物\",\r\n      \"linkType\": 1,\r\n      \"linkValue\": \"/pages/selection/result/index\",\r\n      \"url\": \"/upload/public/image/2023/04/10/6381676766308265789439821.png\"\r\n    }\r\n  ]\r\n}');
INSERT INTO `ShopPageItem` VALUES (67, 'imgWindow', 'mobile_home', 3, 4, '{\r\n  \"style\": 0,\r\n  \"margin\": 0,\r\n  \"list\": [\r\n    {\r\n      \"image\": \"/upload/public/image/2023/03/19/6381486097760473737756926.png\",\r\n      \"linkType\": \"\",\r\n      \"linkValue\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"linkType\": \"\",\r\n      \"linkValue\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"linkType\": \"\",\r\n      \"linkValue\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"linkType\": \"\",\r\n      \"linkValue\": \"\"\r\n    }\r\n  ]\r\n}');
INSERT INTO `ShopPageItem` VALUES (68, 'goods', 'mobile_home', 4, 5, '{\r\n  \"title\": \"商品组名称\",\r\n  \"lookMore\": \"true\",\r\n  \"type\": \"auto\",\r\n  \"classifyId\": \"\",\r\n  \"brandId\": \"\",\r\n  \"limit\": 10,\r\n  \"display\": \"list\",\r\n  \"column\": 2,\r\n  \"list\": [\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"name\": \"\",\r\n      \"price\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"name\": \"\",\r\n      \"price\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"name\": \"\",\r\n      \"price\": \"\"\r\n    },\r\n    {\r\n      \"image\": \"/src/assets/images/empty-banner.png\",\r\n      \"name\": \"\",\r\n      \"price\": \"\"\r\n    }\r\n  ]\r\n}');

-- ----------------------------
-- Table structure for comarchive
-- ----------------------------
DROP TABLE IF EXISTS `comarchive`;
CREATE TABLE `comarchive`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Type` int(11) NOT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Keywords` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Content` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `ExpireTime` datetime NULL DEFAULT NULL,
  `UserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `ExData` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of comarchive
-- ----------------------------

-- ----------------------------
-- Table structure for comsettings
-- ----------------------------
DROP TABLE IF EXISTS `comsettings`;
CREATE TABLE `comsettings`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Varchar` blob NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of comsettings
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
