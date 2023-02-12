/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50737
Source Host           : localhost:3306
Source Database       : shopmall

Target Server Type    : MYSQL
Target Server Version : 50737
File Encoding         : 65001

Date: 2023-02-12 16:04:03
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for bmsmenus
-- ----------------------------
DROP TABLE IF EXISTS `bmsmenus`;
CREATE TABLE `bmsmenus` (
  `Id` varchar(50) NOT NULL,
  `ParentId` varchar(50) DEFAULT NULL,
  `Name` varchar(50) NOT NULL,
  `Type` int(11) DEFAULT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `SysId` varchar(50) DEFAULT NULL,
  `OrderId` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `ProjectId` varchar(50) DEFAULT NULL,
  `OpenType` int(11) NOT NULL,
  `IsHomeShow` bit(1) DEFAULT NULL,
  `Exdata` varchar(1000) DEFAULT NULL,
  `Icon` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmsmenus
-- ----------------------------
INSERT INTO `bmsmenus` VALUES ('BROB0AXXBNCW6012', '10000', '我的工作台', '1', null, null, '99', '1', '2022-07-18 21:29:20', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROB0B0P7L6O6013', 'BROB0AXXBNCW6012', '用户管理', '0', '/system/user/index', null, '0', '1', '2022-07-18 21:29:20', null, null, '0', '', null, 'el-icon-share');
INSERT INTO `bmsmenus` VALUES ('BROB0B2D55HC6014', 'BROB0AXXBNCW6012', '角色管理', '0', '/system/role/index', null, '1', '1', '2022-07-18 21:29:20', null, null, '0', '', null, 'el-icon-location');
INSERT INTO `bmsmenus` VALUES ('BROB0B5U02RK6015', 'BROB0AXXBNCW6012', '菜单管理', '0', '/system/menu/index', null, '2', '1', '2022-07-18 21:29:20', null, null, '0', '', null, 'el-icon-menu');
INSERT INTO `bmsmenus` VALUES ('BROB0B9AV01S6016', 'BROB0AXXBNCW6012', '权限管理', '0', '/system/permission/index', null, '3', '1', '2022-07-18 21:29:20', null, null, '0', '', null, 'el-icon-view');
INSERT INTO `bmsmenus` VALUES ('BROC2QSJ85C04459', '10000', '商品管理', '1', null, null, '0', '1', '2022-07-18 21:41:18', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROC6JJ64GSG4460', 'BROC2QSJ85C04459', '商品管理', '0', '/Product/Index', null, '0', '1', '2022-07-18 21:42:29', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROC98R8UJ284462', 'BROC2QSJ85C04459', '商品类别管理', '0', '/ProductOuter/Index', null, '0', '1', '2022-07-18 21:43:20', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROCANIWJZ0G4463', 'BROC2QSJ85C04459', '商品类别定价管理', '0', '/ProductOuterSpecialCredit/Index', null, '0', '1', '2022-07-18 21:43:46', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROCCJIF2BY84464', '10000', '订单管理', '1', null, null, '0', '1', '2022-07-18 21:44:21', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROCDVRHGCG04465', 'BROCCJIF2BY84464', '订单管理', '0', '/Trade/Index', null, '0', '1', '2022-07-18 21:44:46', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROCH9FGWRNK4466', '10000', '系统管理', '1', null, null, '6', '1', '2022-07-18 21:45:50', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BROCI4Z28YRK4467', 'BROCH9FGWRNK4466', '交易货币管理', '0', '/CreditType/Index', null, '0', '1', '2022-07-18 21:46:06', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BRPZ9FB3GF7K6895', '10000', '商品库存管理', '1', null, null, '0', '1', '2022-07-19 08:44:47', null, null, '0', '', null, null);
INSERT INTO `bmsmenus` VALUES ('BRPZCO9OSQ9S6896', 'BRPZ9FB3GF7K6895', '商品库存列表', '0', '/stock/index', null, '0', '1', '2022-07-19 08:45:48', null, null, '0', '', null, null);

-- ----------------------------
-- Table structure for bmsownerpermission
-- ----------------------------
DROP TABLE IF EXISTS `bmsownerpermission`;
CREATE TABLE `bmsownerpermission` (
  `Id` varchar(50) NOT NULL,
  `PermissionItemKey` varchar(255) NOT NULL,
  `Status` int(11) NOT NULL,
  `Exdata` varchar(2000) DEFAULT NULL,
  `OwnerId` varchar(255) NOT NULL,
  `OwnerType` int(11) NOT NULL,
  `IsLock` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmsownerpermission
-- ----------------------------
INSERT INTO `bmsownerpermission` VALUES ('BRPZXULF85C06910', 'User', '1', null, 'BRPX86X6HLA86895', '11', '\0');
INSERT INTO `bmsownerpermission` VALUES ('BRPZXULF85C16911', 'Product', '1', null, 'BRPX86X6HLA86895', '11', '\0');
INSERT INTO `bmsownerpermission` VALUES ('BRPZXULF85C26912', 'Trade', '1', null, 'BRPX86X6HLA86895', '11', '\0');
INSERT INTO `bmsownerpermission` VALUES ('BRPZXULF85C36913', 'Stock', '1', null, 'BRPX86X6HLA86895', '11', '\0');
INSERT INTO `bmsownerpermission` VALUES ('CCALO86QO1KW5954', 'Product', '1', null, 'CCALO82DCFSW5954', '11', '\0');
INSERT INTO `bmsownerpermission` VALUES ('CCALO86T5XXC5955', 'Stock', '1', null, 'CCALO82DCFSW5954', '11', '\0');

-- ----------------------------
-- Table structure for bmspermission
-- ----------------------------
DROP TABLE IF EXISTS `bmspermission`;
CREATE TABLE `bmspermission` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `PermissionId` varchar(50) NOT NULL,
  `Handler` varchar(255) DEFAULT NULL,
  `Exdata` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmspermission
-- ----------------------------
INSERT INTO `bmspermission` VALUES ('1', '用户管理', 'User', null, null);
INSERT INTO `bmspermission` VALUES ('2', '商品管理', 'Product', null, null);
INSERT INTO `bmspermission` VALUES ('3', '订单管理', 'Trade', null, null);
INSERT INTO `bmspermission` VALUES ('4', '库存管理', 'Stock', null, null);

-- ----------------------------
-- Table structure for bmspermissionmenu
-- ----------------------------
DROP TABLE IF EXISTS `bmspermissionmenu`;
CREATE TABLE `bmspermissionmenu` (
  `Id` varchar(50) NOT NULL,
  `PermissionItemKey` varchar(255) NOT NULL,
  `MenuId` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmspermissionmenu
-- ----------------------------
INSERT INTO `bmspermissionmenu` VALUES ('BROB0BE85SLC6012', 'User', 'BROB0AXXBNCW6012');
INSERT INTO `bmspermissionmenu` VALUES ('BROB0BEANOXS6013', 'User', 'BROB0B0P7L6O6013');
INSERT INTO `bmspermissionmenu` VALUES ('BROB0BEANOXT6014', 'User', 'BROB0B2D55HC6014');
INSERT INTO `bmspermissionmenu` VALUES ('BROB0BEANOXU6015', 'User', 'BROB0B5U02RK6015');
INSERT INTO `bmspermissionmenu` VALUES ('BROB0BEANOXV6016', 'User', 'BROB0B9AV01S6016');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX57MXZI806895', 'Product', 'BROC2QSJ85C04459');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX57N0HEKG6896', 'Product', 'BROC6JJ64GSG4460');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX57N0HEKH6897', 'Product', 'BROC98R8UJ284462');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX57N0HEKI6898', 'Product', 'BROCANIWJZ0G4463');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX57N0HEKJ6899', 'Product', 'BROCH9FGWRNK4466');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX57N2ZAWW6900', 'Product', 'BROCI4Z28YRK4467');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX68HI28ZK6901', 'Trade', 'BROCCJIF2BY84464');
INSERT INTO `bmspermissionmenu` VALUES ('BRPX68HI28ZL6902', 'Trade', 'BROCDVRHGCG04465');
INSERT INTO `bmspermissionmenu` VALUES ('BRPZFGL3HVCW6903', 'Stock', 'BRPZ9FB3GF7K6895');
INSERT INTO `bmspermissionmenu` VALUES ('BRPZFGL3HVCX6904', 'Stock', 'BRPZCO9OSQ9S6896');

-- ----------------------------
-- Table structure for bmsrole
-- ----------------------------
DROP TABLE IF EXISTS `bmsrole`;
CREATE TABLE `bmsrole` (
  `Id` varchar(50) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `Exdata` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmsrole
-- ----------------------------
INSERT INTO `bmsrole` VALUES ('BRPX86X6HLA86895', '超级管理员', '1', null);
INSERT INTO `bmsrole` VALUES ('CCALO82DCFSW5954', '商品管理员', '1', null);

-- ----------------------------
-- Table structure for bmsuser
-- ----------------------------
DROP TABLE IF EXISTS `bmsuser`;
CREATE TABLE `bmsuser` (
  `Id` varchar(50) NOT NULL,
  `Username` varchar(255) NOT NULL,
  `Password` varchar(50) NOT NULL,
  `Salt` varchar(50) NOT NULL,
  `Nickname` varchar(50) DEFAULT NULL,
  `CreateTime` datetime NOT NULL,
  `Status` int(11) DEFAULT NULL,
  `Exdata` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmsuser
-- ----------------------------
INSERT INTO `bmsuser` VALUES ('BROAN5W22SQO1028', 'admin', 'JC3fFEhKp0LqyJ7gBBqLFbt99/Z8AG7t1mvFnEbOInA=', '04166047-a076-489f-a804-f4a7d2e87b82', '管理员', '2022-07-18 21:25:15', '1', null);

-- ----------------------------
-- Table structure for bmsuserrole
-- ----------------------------
DROP TABLE IF EXISTS `bmsuserrole`;
CREATE TABLE `bmsuserrole` (
  `Id` varchar(50) NOT NULL,
  `RoleId` varchar(50) NOT NULL,
  `UserId` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of bmsuserrole
-- ----------------------------
INSERT INTO `bmsuserrole` VALUES ('BRPX8S44Z3EO6895', 'BRPX86X6HLA86895', 'BROAN5W22SQO1028');
