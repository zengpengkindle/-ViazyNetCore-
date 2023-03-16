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

 Date: 16/03/2023 22:01:34
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
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
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
INSERT INTO `MemberAddress` VALUES ('BRRRVAIYYG3K0366', 'a1234', b'1', '福建省', '福州市', '鼓楼区', '测试地址', '林测试', '15980771111', '350102', '');

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
INSERT INTO `MemberCarItem` VALUES ('BRQRG6PBOL4W3800', NULL, 'BRQITVPA39C05149', NULL, '10000', NULL, 'Test1', '', '/upload/public/image/20220719/6379383013820600209908540.jpg', 30.0000, b'0', 0.0000, 0, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 1, 0, '2022-07-19 14:00:46', '2022-07-19 14:00:46', NULL, NULL);
INSERT INTO `MemberCarItem` VALUES ('BRRRP7RCLGQO7015', '0b45eda4ad284710a8b430db80cffa0b', 'BRQITVPA39C05149', NULL, '10000', NULL, 'Test1', '', '/upload/public/image/20220719/6379383013820600209908540.jpg', 30.0000, b'0', 0.0000, 0, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 1, 0, '2022-07-19 20:47:09', '2022-07-19 20:47:09', NULL, NULL);
INSERT INTO `MemberCarItem` VALUES ('BRRRS0JTLMV40366', 'a1234', 'BRQITVPA39C05149', NULL, '10000', NULL, 'Test1', '', '/upload/public/image/20220719/6379383013820600209908540.jpg', 30.0000, b'0', 0.0000, 0, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 1, 0, '2022-07-19 20:48:01', '2022-07-19 20:48:01', NULL, NULL);

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
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `BrandId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BrandName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CatId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `CatName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ShopId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CatPath` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `SubTitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Keywords` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` blob NULL,
  `Cost` decimal(18, 4) NOT NULL,
  `Price` decimal(18, 4) NOT NULL,
  `IsFreeFreight` bit(1) NOT NULL,
  `Freight` decimal(18, 4) NOT NULL,
  `FreightStep` int(11) NOT NULL,
  `FreightValue` decimal(18, 4) NOT NULL,
  `IsHidden` bit(1) NOT NULL,
  `Status` int(11) NOT NULL,
  `StatusChangeTime` datetime NOT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `SubImage` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OpenSpec` bit(1) NULL DEFAULT NULL,
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
INSERT INTO `Product` VALUES ('1FKHSRFF51C1848', NULL, NULL, 'A1', '学习用品', '10000', '测试', NULL, '小羊咩咩', NULL, NULL, NULL, 10.0000, 12.0000, b'0', 0.0000, 0, 0.0000, b'0', 2, '2023-03-14 22:53:17', '/upload/public/image/2023/03/14/6381443119233469888795835.png', '', b'0', '[]', '<p>测试</p>', '2023-03-14 22:53:17', '2023-03-15 15:59:05', '小羊咩咩____', b'0', NULL, 4, NULL);
INSERT INTO `Product` VALUES ('1FKIBOLF3281851', NULL, NULL, 'A1', '学习用品', '10000', '测试', NULL, '小羊咩咩没~~咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩', NULL, NULL, NULL, 96.0000, 12.0000, b'0', 0.0000, 0, 0.0000, b'0', 2, '2023-03-14 23:03:45', '/upload/public/image/2023/03/14/6381443164371038354913416.png', '', b'1', '[{\"K\":\"颜色\",\"V\":[{\"Id\":\"s10001\",\"Name\":\"红色\",\"ImgUrl\":\"/upload/public/image/2023/03/14/6381443183453456688369200.png\"},{\"Id\":\"s10002\",\"Name\":\"白色\",\"ImgUrl\":\"/upload/public/image/2023/03/14/6381443183707506325539155.png\"},{\"Id\":\"s10003\",\"Name\":\"绿色\",\"ImgUrl\":\"\"},{\"Id\":\"s10004\",\"Name\":\"粉色\",\"ImgUrl\":\"\"}],\"k_s\":\"s1\"}]', '<p><img src=\"/upload/public/image/2023/03/15/6381449259326860566568908.jpg\" alt=\"\" data-href=\"\" style=\"\"/></p>', '2023-03-14 23:03:46', '2023-03-15 22:00:45', '小羊咩咩没~~咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩____', b'0', NULL, 4, NULL);
INSERT INTO `Product` VALUES ('BRQITVPA39C05149', NULL, NULL, 'A1', '学习用品', '10000', '测试', '学习用品', 'Test1', NULL, NULL, NULL, 100.0000, 30.0000, b'0', 0.0000, 0, 0.0000, b'0', 1, '2022-07-19 12:24:09', '/upload/public/image/2023/03/14/6381442979251715415542528.png', '', b'0', '[]', '<p>测试<br/></p>', '2022-07-19 12:24:09', '2023-03-14 22:30:00', 'Test1___学习用品_', b'0', '', 1, NULL);

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
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductCat
-- ----------------------------
INSERT INTO `ProductCat` VALUES ('A1', b'0', '学习用品', NULL, b'0', NULL, NULL, 0, 1, NULL);

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
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductSku
-- ----------------------------
INSERT INTO `ProductSku` VALUES ('1FKIBOLF3281850', '1FKIBOLF3281851', NULL, 's10001', '颜色', '红色', '0', '', '', '0', '', '', 10.0000, 12.0000, NULL);
INSERT INTO `ProductSku` VALUES ('1FKIBOQ99HC1851', '1FKIBOLF3281851', NULL, 's10002', '颜色', '白色', '0', '', '', '0', '', '', 10.0000, 13.0000, NULL);
INSERT INTO `ProductSku` VALUES ('1FKIBOUWF281852', '1FKIBOLF3281851', NULL, 's10003', '颜色', '绿色', '0', '', '', '0', '', '', 10.0000, 15.0000, NULL);
INSERT INTO `ProductSku` VALUES ('1FM3W4Z64CG1183', '1FKIBOLF3281851', NULL, 's10004', '颜色', '粉色', '0', '', '', '0', '', '', 10.0000, 15.0000, NULL);

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
INSERT INTO `ProductStock` VALUES ('1FKHSRFHY681848', '1FKHSRFF51C1848', '', 0, 0, 0, 0, 0, 0, '2023-03-14 22:53:17', '2023-03-14 22:53:17');
INSERT INTO `ProductStock` VALUES ('1FKI69K8U2O1849', '1FKI69K60XS1849', '1FKI69K7FI81848', 100, 0, 0, 0, 0, 0, '2023-03-14 23:00:46', '2023-03-14 23:00:46');
INSERT INTO `ProductStock` VALUES ('1FKI6FKZQRK1850', '1FKI6FKZQRK1850', '1FKI6FKZQRK1849', 100, 0, 0, 0, 0, 0, '2023-03-14 23:00:51', '2023-03-14 23:00:51');
INSERT INTO `ProductStock` VALUES ('1FKIBOLF3281851', '1FKIBOLF3281851', '1FKIBOLF3281850', 100, 0, 0, 0, 0, 0, '2023-03-14 23:03:46', '2023-03-14 23:03:46');
INSERT INTO `ProductStock` VALUES ('1FKIBOQ99HC1852', '1FKIBOLF3281851', '1FKIBOQ99HC1851', 0, 0, 0, 0, 0, 0, '2023-03-14 23:03:46', '2023-03-14 23:03:46');
INSERT INTO `ProductStock` VALUES ('1FKIBOUWF281853', '1FKIBOLF3281851', '1FKIBOUWF281852', 0, 0, 0, 0, 0, 0, '2023-03-14 23:03:46', '2023-03-14 23:03:46');
INSERT INTO `ProductStock` VALUES ('1FM3W4Z7IWW1183', '1FKIBOLF3281851', '1FM3W4Z64CG1183', 1, 0, 0, 0, 0, 0, '2023-03-15 18:11:02', '2023-03-15 18:11:02');
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
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MemberName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ShopId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ShopName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PaymentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `HasOuter` bit(1) NOT NULL,
  `OuterId` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ObjectKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreditKey` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BusinessType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `TotalFreight` decimal(18, 4) NOT NULL,
  `ProductMoney` decimal(18, 4) NOT NULL,
  `TotalMoney` decimal(18, 4) NOT NULL,
  `ReceiverProvince` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverCity` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDistrict` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverDetail` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReceiverMobile` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsCompany` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogisticsCost` decimal(18, 4) NULL DEFAULT NULL,
  `PayMode` int(11) NULL DEFAULT NULL,
  `Message` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime NULL DEFAULT NULL,
  `PayTime` datetime NULL DEFAULT NULL,
  `ConsignTime` datetime NULL DEFAULT NULL,
  `CompleteTime` datetime NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `StatusChangedTime` datetime NULL DEFAULT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductTrade
-- ----------------------------
INSERT INTO `ProductTrade` VALUES ('BRRU2JLSTL342314', 'a1234', 'member1', '10000', NULL, '', b'0', NULL, NULL, NULL, NULL, NULL, 0.0000, 30.0000, 30.0000, '福建省', '福州市', '鼓楼区', '测试地址', '林测试', '15980771111', NULL, NULL, NULL, NULL, 0, NULL, '2022-07-19 21:13:43', NULL, NULL, NULL, -1, '2022-07-19 21:13:43', NULL);

-- ----------------------------
-- Table structure for ProductTradeOrder
-- ----------------------------
DROP TABLE IF EXISTS `ProductTradeOrder`;
CREATE TABLE `ProductTradeOrder`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `HasOuter` bit(1) NOT NULL,
  `OuterType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ProductTradeId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `BrandId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BrandName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SkuId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OuterSkuId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SkuText` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Num` int(11) NOT NULL,
  `Cost` decimal(18, 4) NOT NULL,
  `RefundType` int(11) NOT NULL,
  `Price` decimal(18, 4) NOT NULL,
  `TotalMoney` decimal(18, 4) NOT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ProductTradeOrder
-- ----------------------------
INSERT INTO `ProductTradeOrder` VALUES ('BRRU2JM0BA4G2314', b'0', NULL, 'BRRU2JLSTL342314', 'BRQITVPA39C05149', NULL, NULL, 'Test1', NULL, '/upload/public/image/20220719/6379383013820600209908540.jpg', '', NULL, NULL, '', 1, 100.0000, 1, 30.0000, 30.0000, NULL);

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
  `Id` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ComponyName` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Contact` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Phone` varchar(31) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AddressFH` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AddressTH` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OpenMode` int(11) NULL DEFAULT NULL,
  `Intro` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Logo` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Banner` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OfficeTime` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `MinDistribution` decimal(18, 4) NOT NULL,
  `Province` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `City` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Area` varchar(63) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Shop
-- ----------------------------
INSERT INTO `Shop` VALUES ('10000', '直营', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, NULL, NULL, NULL);

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
