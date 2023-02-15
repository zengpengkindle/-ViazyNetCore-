/*
 Navicat Premium Data Transfer

 Source Server         : tencent
 Source Server Type    : MySQL
 Source Server Version : 50718
 Source Host           : sh-cynosdbmysql-grp-88q6q6gm.sql.tencentcdb.com:22370
 Source Schema         : ViazyNetCore

 Target Server Type    : MySQL
 Target Server Version : 50718
 File Encoding         : 65001

 Date: 15/02/2023 13:11:35
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for BmsArticle
-- ----------------------------
DROP TABLE IF EXISTS `BmsArticle`;
CREATE TABLE `BmsArticle`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Type` int(11) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Style` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  `EventName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `EventArguments` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ExtraData` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsArticle
-- ----------------------------

-- ----------------------------
-- Table structure for BmsMenus
-- ----------------------------
DROP TABLE IF EXISTS `BmsMenus`;
CREATE TABLE `BmsMenus`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Type` int(11) NULL DEFAULT NULL,
  `Url` varchar(511) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `SysId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OrderId` int(11) NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ProjectId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OpenType` int(11) NULL DEFAULT NULL,
  `IsHomeShow` bit(1) NULL DEFAULT NULL,
  `Icon` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsMenus
-- ----------------------------
INSERT INTO `BmsMenus` VALUES ('BROB0AXXBNCW6012', '10000', '我的工作台', 1, NULL, NULL, 99, 1, '2022-07-18 21:29:20', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROB0B0P7L6O6013', 'BROB0AXXBNCW6012', '用户管理', 0, '/system/user/index', NULL, 0, 1, '2022-07-18 21:29:20', NULL, NULL, 0, b'1', NULL, 'el-icon-share');
INSERT INTO `BmsMenus` VALUES ('BROB0B2D55HC6014', 'BROB0AXXBNCW6012', '角色管理', 0, '/system/role/index', NULL, 1, 1, '2022-07-18 21:29:20', NULL, NULL, 0, b'1', NULL, 'el-icon-location');
INSERT INTO `BmsMenus` VALUES ('BROB0B5U02RK6015', 'BROB0AXXBNCW6012', '菜单管理', 0, '/system/menu/index', NULL, 2, 1, '2022-07-18 21:29:20', NULL, NULL, 0, b'1', NULL, 'el-icon-menu');
INSERT INTO `BmsMenus` VALUES ('BROB0B9AV01S6016', 'BROB0AXXBNCW6012', '权限管理', 0, '/system/permission/index', NULL, 3, 1, '2022-07-18 21:29:20', NULL, NULL, 0, b'1', NULL, 'el-icon-view');
INSERT INTO `BmsMenus` VALUES ('BROC2QSJ85C04459', '10000', '商品管理', 1, NULL, NULL, 0, 1, '2022-07-18 21:41:18', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROC6JJ64GSG4460', 'BROC2QSJ85C04459', '商品管理', 0, '/Product/Index', NULL, 0, 1, '2022-07-18 21:42:29', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROC98R8UJ284462', 'BROC2QSJ85C04459', '商品类别管理', 0, '/ProductOuter/Index', NULL, 0, 1, '2022-07-18 21:43:20', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROCANIWJZ0G4463', 'BROC2QSJ85C04459', '商品类别定价管理', 0, '/ProductOuterSpecialCredit/Index', NULL, 0, 1, '2022-07-18 21:43:46', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROCCJIF2BY84464', '10000', '订单管理', 1, NULL, NULL, 0, 1, '2022-07-18 21:44:21', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROCDVRHGCG04465', 'BROCCJIF2BY84464', '订单管理', 0, '/Trade/Index', NULL, 0, 1, '2022-07-18 21:44:46', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROCH9FGWRNK4466', '10000', '系统管理', 1, NULL, NULL, 6, 1, '2022-07-18 21:45:50', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BROCI4Z28YRK4467', 'BROCH9FGWRNK4466', '交易货币管理', 0, '/CreditType/Index', NULL, 0, 1, '2022-07-18 21:46:06', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BRPZ9FB3GF7K6895', '10000', '商品库存管理', 1, NULL, NULL, 0, 1, '2022-07-19 08:44:47', NULL, NULL, 0, b'1', NULL, NULL);
INSERT INTO `BmsMenus` VALUES ('BRPZCO9OSQ9S6896', 'BRPZ9FB3GF7K6895', '商品库存列表', 0, '/stock/index', NULL, 0, 1, '2022-07-19 08:45:48', NULL, NULL, 0, b'1', NULL, NULL);

-- ----------------------------
-- Table structure for BmsOwnerPermission
-- ----------------------------
DROP TABLE IF EXISTS `BmsOwnerPermission`;
CREATE TABLE `BmsOwnerPermission`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PermissionItemKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Status` int(11) NOT NULL,
  `Exdata` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OwnerId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `OwnerType` int(11) NOT NULL,
  `IsLock` bit(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsOwnerPermission
-- ----------------------------
INSERT INTO `BmsOwnerPermission` VALUES ('BRPZXULF85C06910', 'User', 1, NULL, 'BRPX86X6HLA86895', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('BRPZXULF85C16911', 'Product', 1, NULL, 'BRPX86X6HLA86895', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('BRPZXULF85C26912', 'Trade', 1, NULL, 'BRPX86X6HLA86895', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('BRPZXULF85C36913', 'Stock', 1, NULL, 'BRPX86X6HLA86895', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('CCALO86QO1KW5954', 'Product', 1, NULL, 'CCALO82DCFSW5954', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('CCALO86T5XXC5955', 'Stock', 1, NULL, 'CCALO82DCFSW5954', 11, b'0');

-- ----------------------------
-- Table structure for BmsPage
-- ----------------------------
DROP TABLE IF EXISTS `BmsPage`;
CREATE TABLE `BmsPage`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `GroupId` bigint(20) NULL DEFAULT NULL,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Icon` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Sort` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsPage
-- ----------------------------
INSERT INTO `BmsPage` VALUES (11, 4, '用户列表', 'user-filled', '/system/user/list', 0, 1, '2022-10-12 14:29:05', '2022-10-12 14:29:05');
INSERT INTO `BmsPage` VALUES (12, 4, '角色列表', 'guide', '/system/role/list', 0, 1, '2022-10-12 14:29:05', '2022-10-12 14:29:05');
INSERT INTO `BmsPage` VALUES (13, 4, '菜单列表', 'menu', '/system/menu/list', 0, 1, '2022-10-12 14:29:05', '2022-10-12 14:29:05');

-- ----------------------------
-- Table structure for BmsPageGroup
-- ----------------------------
DROP TABLE IF EXISTS `BmsPageGroup`;
CREATE TABLE `BmsPageGroup`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ParentId` bigint(20) NOT NULL DEFAULT 0,
  `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Icon` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Sort` int(11) NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsPageGroup
-- ----------------------------
INSERT INTO `BmsPageGroup` VALUES (3, 0, '系统管理', 'setting', 0, 1, '2022-10-12 14:29:05', '2022-10-12 14:29:05');
INSERT INTO `BmsPageGroup` VALUES (4, 3, '用户管理', 'user', 0, 1, '2022-10-12 14:29:05', '2022-10-12 14:29:05');

-- ----------------------------
-- Table structure for BmsPermission
-- ----------------------------
DROP TABLE IF EXISTS `BmsPermission`;
CREATE TABLE `BmsPermission`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PermissionId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Handler` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Exdata` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsPermission
-- ----------------------------
INSERT INTO `BmsPermission` VALUES (1, '用户管理', 'User', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (2, '商品管理', 'Product', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (3, '订单管理', 'Trade', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (4, '库存管理', 'Stock', NULL, NULL);

-- ----------------------------
-- Table structure for BmsPermissionMenu
-- ----------------------------
DROP TABLE IF EXISTS `BmsPermissionMenu`;
CREATE TABLE `BmsPermissionMenu`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `PermissionItemKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `MenuId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsPermissionMenu
-- ----------------------------
INSERT INTO `BmsPermissionMenu` VALUES ('BROB0BE85SLC6012', 'User', 'BROB0AXXBNCW6012');
INSERT INTO `BmsPermissionMenu` VALUES ('BROB0BEANOXS6013', 'User', 'BROB0B0P7L6O6013');
INSERT INTO `BmsPermissionMenu` VALUES ('BROB0BEANOXT6014', 'User', 'BROB0B2D55HC6014');
INSERT INTO `BmsPermissionMenu` VALUES ('BROB0BEANOXU6015', 'User', 'BROB0B5U02RK6015');
INSERT INTO `BmsPermissionMenu` VALUES ('BROB0BEANOXV6016', 'User', 'BROB0B9AV01S6016');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX57MXZI806895', 'Product', 'BROC2QSJ85C04459');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX57N0HEKG6896', 'Product', 'BROC6JJ64GSG4460');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX57N0HEKH6897', 'Product', 'BROC98R8UJ284462');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX57N0HEKI6898', 'Product', 'BROCANIWJZ0G4463');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX57N0HEKJ6899', 'Product', 'BROCH9FGWRNK4466');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX57N2ZAWW6900', 'Product', 'BROCI4Z28YRK4467');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX68HI28ZK6901', 'Trade', 'BROCCJIF2BY84464');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX68HI28ZL6902', 'Trade', 'BROCDVRHGCG04465');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPZFGL3HVCW6903', 'Stock', 'BRPZ9FB3GF7K6895');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPZFGL3HVCX6904', 'Stock', 'BRPZCO9OSQ9S6896');

-- ----------------------------
-- Table structure for BmsRole
-- ----------------------------
DROP TABLE IF EXISTS `BmsRole`;
CREATE TABLE `BmsRole`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  `ExtraData` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsRole
-- ----------------------------
INSERT INTO `BmsRole` VALUES (1, '超级管理员', 1, '2022-10-12 15:29:38', '2022-10-12 15:42:31', NULL);

-- ----------------------------
-- Table structure for BmsRolePage
-- ----------------------------
DROP TABLE IF EXISTS `BmsRolePage`;
CREATE TABLE `BmsRolePage`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `RoleId` bigint(20) NULL DEFAULT NULL,
  `PageId` bigint(20) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsRolePage
-- ----------------------------
INSERT INTO `BmsRolePage` VALUES (1, 1, 11, '2022-10-12 22:39:05');
INSERT INTO `BmsRolePage` VALUES (2, 1, 12, '2022-10-12 22:39:05');
INSERT INTO `BmsRolePage` VALUES (3, 1, 13, '2022-10-12 22:39:05');

-- ----------------------------
-- Table structure for BmsRolePermission
-- ----------------------------
DROP TABLE IF EXISTS `BmsRolePermission`;
CREATE TABLE `BmsRolePermission`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `RoleId` bigint(20) NOT NULL,
  `ItemId` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsRolePermission
-- ----------------------------
INSERT INTO `BmsRolePermission` VALUES (1, 1, 'Page.FindAll', '2022-10-12 21:20:41');
INSERT INTO `BmsRolePermission` VALUES (2, 1, 'Page.Manage', '2022-10-12 21:20:41');
INSERT INTO `BmsRolePermission` VALUES (3, 1, 'Page.Remove', '2022-10-12 21:20:41');
INSERT INTO `BmsRolePermission` VALUES (4, 1, 'PageGroup.FindAll', '2022-10-12 21:20:41');
INSERT INTO `BmsRolePermission` VALUES (5, 1, 'PageGroup.Manage', '2022-10-12 21:20:41');
INSERT INTO `BmsRolePermission` VALUES (6, 1, 'PageGroup.Remove', '2022-10-12 21:20:41');

-- ----------------------------
-- Table structure for BmsUser
-- ----------------------------
DROP TABLE IF EXISTS `BmsUser`;
CREATE TABLE `BmsUser`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PasswordSalt` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Nickname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  `ExtraData` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `GoogleKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsUser
-- ----------------------------
INSERT INTO `BmsUser` VALUES ('1', 'admin', 'N8DP41WT+VRlD4bkpr6Jj/VRrEMOmlrxu2SYeU0U5VE=', 'a42bbec9-99ef-49e0-805f-35841b3b4d24', 'Administrator', 1, '2022-10-12 10:20:26', '2022-10-12 10:20:26', 'init', NULL);
INSERT INTO `BmsUser` VALUES ('1DZ8DXUCG3K3089', 'User01', 'Da8/PKJG578Ao2ksPmJ5INtmt8lAJrYyMoqaE2MGlOo=', 'c4facc1b-2abe-4f37-aa6a-19cd0af35163', '用户01', 1, '2023-02-14 10:11:20', '2023-02-14 10:11:20', '', NULL);

-- ----------------------------
-- Table structure for OperationLog
-- ----------------------------
DROP TABLE IF EXISTS `OperationLog`;
CREATE TABLE `OperationLog`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `OperatorIP` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OperateUserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Operator` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `OperatorType` int(11) NULL DEFAULT NULL,
  `OperationType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ObjectId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ObjectName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `MerchantId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `LogLevel` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 39 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of OperationLog
-- ----------------------------
INSERT INTO `OperationLog` VALUES (1, '127.0.0.1', '11111111111', NULL, 1, '登录', NULL, NULL, '2022-10-12 13:05:05', '登录用户：11111111111,登陆失败!The account or password what you enter is error,you can only try it for 3 times', NULL, 0);
INSERT INTO `OperationLog` VALUES (2, '127.0.0.1', 'admin', NULL, 1, '登录', NULL, NULL, '2022-10-12 13:06:18', '登录用户：admin,登陆失败!The account or password what you enter is error,you can only try it for 4 times', NULL, 0);
INSERT INTO `OperationLog` VALUES (3, '127.0.0.1', 'admin', NULL, 1, '登录', NULL, NULL, '2022-10-12 13:07:23', '登录用户：admin,登陆失败!The account or password what you enter is error,you can only try it for 3 times', NULL, 0);
INSERT INTO `OperationLog` VALUES (4, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 13:23:31', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (5, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 14:20:11', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (6, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 14:30:14', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (7, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 14:49:20', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (8, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 14:59:39', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (9, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 15:26:59', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (10, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 15:29:27', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (11, '127.0.0.1', '1', 'admin', 1, '添加角色', '1', '角色添加', '2022-10-12 15:29:38', '角色名：test', NULL, 3);
INSERT INTO `OperationLog` VALUES (12, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 15:35:04', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (13, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 15:36:41', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (14, '127.0.0.1', 'admin', NULL, 1, '登录', NULL, NULL, '2022-10-12 15:41:03', '登录用户：admin,登陆失败!The account or password what you enter is error,you can only try it for 4 times', NULL, 0);
INSERT INTO `OperationLog` VALUES (15, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 15:41:12', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (16, '127.0.0.1', '1', 'admin', 1, '添加用户', '0', '用户添加', '2022-10-12 15:41:35', '用户名：test1', NULL, 3);
INSERT INTO `OperationLog` VALUES (17, '127.0.0.1', '1', 'admin', 1, '删除用户', '2', '用户删除', '2022-10-12 15:41:49', '用户编码:2', NULL, 3);
INSERT INTO `OperationLog` VALUES (18, '127.0.0.1', '1', 'admin', 1, '修改角色', '1', '角色修改', '2022-10-12 15:42:31', '角色名：超级管理员', NULL, 3);
INSERT INTO `OperationLog` VALUES (19, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 21:05:07', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (20, '127.0.0.1', '1', 'admin', 1, '修改接口权限', '1', '角色接口', '2022-10-12 21:20:42', '角色编号：1', NULL, 3);
INSERT INTO `OperationLog` VALUES (21, '127.0.0.1', 'admin', NULL, 1, '登录', NULL, NULL, '2022-10-12 22:01:14', '登录用户：admin,登陆失败!The account or password what you enter is error,you can only try it for 4 times', NULL, 0);
INSERT INTO `OperationLog` VALUES (22, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 22:01:20', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (23, '127.0.0.1', 'admin', NULL, 1, '登录', NULL, NULL, '2022-10-12 22:30:24', '登录用户：admin,登陆失败!The account or password what you enter is error,you can only try it for 4 times', NULL, 0);
INSERT INTO `OperationLog` VALUES (24, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 22:30:32', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (25, '127.0.0.1', '1', 'admin', 1, '修改页面权限', '1', '角色页面', '2022-10-12 22:39:05', '角色编号：1', NULL, 3);
INSERT INTO `OperationLog` VALUES (26, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 22:48:45', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (27, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-12 22:50:37', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (28, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 00:08:36', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (29, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 00:17:49', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (30, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 00:20:23', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (31, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 11:51:02', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (32, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 12:03:13', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (33, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 12:24:43', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (34, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 14:24:13', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (35, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 14:42:27', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (36, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-13 20:58:17', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (37, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-16 17:09:52', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (38, '127.0.0.1', '1', 'admin', 1, '登录', '1', 'Administrator', '2022-10-17 20:02:44', '登录用户：admin,登陆成功', NULL, 0);

SET FOREIGN_KEY_CHECKS = 1;
