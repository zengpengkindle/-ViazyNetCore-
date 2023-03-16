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

 Date: 16/03/2023 22:02:08
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
INSERT INTO `BmsMenus` VALUES ('1E6D9ZAXH6O5687', '', '权限管理', 1, '/permission', '', 22, 1, '', '2023-02-17 23:30:54', '', 0, b'1', 'lollipop', '');
INSERT INTO `BmsMenus` VALUES ('1E6E9QYV9DS5688', '1E6D9ZAXH6O5687', '按钮权限', 0, '/permission/button/index', '', 99, 0, 'PermissionButton', '2023-02-17 23:50:42', '', 0, b'1', 'flUser', '');
INSERT INTO `BmsMenus` VALUES ('1E6FAU9BDQ84143', '1E6D9ZAXH6O5687', '页面权限', 0, '/permission/page/index', '', 99, 0, 'PermissionPage', '2023-02-18 00:11:14', '', 0, b'1', 'setting', '');
INSERT INTO `BmsMenus` VALUES ('1E7PIUJAZ402224', '1E6E9QYV9DS5688', '新增', 2, '', '', 0, 1, 'btn_add', '2023-02-18 15:32:24', '', 0, b'1', 'ep:add-location', '');
INSERT INTO `BmsMenus` VALUES ('1E7QRR9H5J45149', '1E6E9QYV9DS5688', '编辑', 2, '', '', 0, 1, 'btn_edit', '2023-02-18 15:57:16', '', 0, b'1', 'ep:', '');
INSERT INTO `BmsMenus` VALUES ('1EMAP43YWCG2265', 'BROB0AXXBNCW6012', '字典管理', 0, '/system/dictionary/index', '', 0, 1, 'dictionary', '2023-02-25 21:58:37', '', 0, b'1', 'ep:calendar', '');
INSERT INTO `BmsMenus` VALUES ('1EMBMFIFVI85474', 'BROB0AXXBNCW6012', '操作日志', 0, '/system/op/index', '', 0, 1, 'op', '2023-02-25 22:17:04', '', 0, b'1', 'ep:cellphone', '');
INSERT INTO `BmsMenus` VALUES ('1EVPTW437402546', '1E6D9ZAXH6O5687', '部门管理', 0, '/permission/org/index', '', 5, 1, 'Org', '2023-03-02 14:37:13', '', 0, b'1', 'ep:stamp', '');
INSERT INTO `BmsMenus` VALUES ('1FMOJ4TLSU86544', 'BROCCJIF2BY84464', '退货单管理', 0, '/shopmall/refund/index', '', 10, 1, 'RefundTrade', '2023-03-16 01:02:21', '', 0, b'1', 'fa:cart-arrow-down', '');
INSERT INTO `BmsMenus` VALUES ('BROB0AXXBNCW6012', '', '系统管理', 1, '/system', NULL, 11, 1, '', '2022-07-18 21:29:20', NULL, 0, b'1', 'setting', '');
INSERT INTO `BmsMenus` VALUES ('BROB0B0P7L6O6013', '1E6D9ZAXH6O5687', '用户管理', 0, '/permission/user/index', NULL, 0, 1, 'User', '2022-07-18 21:29:20', NULL, 0, b'1', 'flUser', '');
INSERT INTO `BmsMenus` VALUES ('BROB0B2D55HC6014', '1E6D9ZAXH6O5687', '角色管理', 0, '/permission/role/index', NULL, 10, 1, 'Role', '2022-07-18 21:29:20', NULL, 0, b'1', 'role', '');
INSERT INTO `BmsMenus` VALUES ('BROB0B5U02RK6015', '1E6D9ZAXH6O5687', '菜单管理', 0, '/permission/menu/index', NULL, 20, 1, 'Menu', '2022-07-18 21:29:20', NULL, 0, b'1', 'menu', '');
INSERT INTO `BmsMenus` VALUES ('BROB0B9AV01S6016', '1E6D9ZAXH6O5687', '权限管理', 0, '/permission/permission/index', NULL, 30, 1, 'Permission', '2022-07-18 21:29:20', NULL, 0, b'1', 'lollipop', '');
INSERT INTO `BmsMenus` VALUES ('BROC2QSJ85C04459', NULL, '商品管理', 1, '/shopmall', NULL, 0, 1, '', '2022-07-18 21:41:18', NULL, 0, b'1', 'ep:goods', NULL);
INSERT INTO `BmsMenus` VALUES ('BROC6JJ64GSG4460', 'BROC2QSJ85C04459', '商品管理', 0, '/shopmall/product/index', NULL, 0, 1, 'product', '2022-07-18 21:42:29', NULL, 0, b'1', 'ep:goods-filled', NULL);
INSERT INTO `BmsMenus` VALUES ('BROC98R8UJ284462', 'BROC2QSJ85C04459', '商品类别管理', 0, '/shopmall/productOuter/index', NULL, 10, 1, 'productOuter', '2022-07-18 21:43:20', NULL, 0, b'1', 'ep:briefcase', NULL);
INSERT INTO `BmsMenus` VALUES ('BROCANIWJZ0G4463', 'BROC2QSJ85C04459', '商品类别定价管理', 0, '/shopmall/productOuterSpecialCredit/index', NULL, 20, 1, 'productOuterSpecialCredit', '2022-07-18 21:43:46', NULL, 0, b'1', 'ep:price-tag', NULL);
INSERT INTO `BmsMenus` VALUES ('BROCCJIF2BY84464', NULL, '订单管理', 1, '/trade', NULL, 0, 1, '', '2022-07-18 21:44:21', NULL, 0, b'1', 'fa:shopping-cart', NULL);
INSERT INTO `BmsMenus` VALUES ('BROCDVRHGCG04465', 'BROCCJIF2BY84464', '订单管理', 0, '/shopmall/Trade/Index', NULL, 0, 1, 'Trade', '2022-07-18 21:44:46', NULL, 0, b'1', 'ep:document', NULL);
INSERT INTO `BmsMenus` VALUES ('BROCH9FGWRNK4466', NULL, '系统管理', 1, '/systemsettings', NULL, 6, 1, '', '2022-07-18 21:45:50', NULL, 0, b'1', 'ep:add-location', NULL);
INSERT INTO `BmsMenus` VALUES ('BROCI4Z28YRK4467', 'BROB0AXXBNCW6012', '交易货币管理', 0, '/shopmall/creditType/index', NULL, 0, 1, 'credit', '2022-07-18 21:46:06', NULL, 0, b'1', 'ep:baseball', NULL);
INSERT INTO `BmsMenus` VALUES ('BRPZ9FB3GF7K6895', NULL, '商品库存管理', 1, '/stock', NULL, 0, 0, '', '2022-07-19 08:44:47', NULL, 0, b'1', 'ep:present', NULL);

-- ----------------------------
-- Table structure for BmsOrg
-- ----------------------------
DROP TABLE IF EXISTS `BmsOrg`;
CREATE TABLE `BmsOrg`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `TenantId` bigint(20) NULL DEFAULT NULL,
  `ParentId` bigint(20) NULL DEFAULT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Value` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `MemberCount` int(11) NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `Sort` int(11) NULL DEFAULT NULL,
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsOrg
-- ----------------------------
INSERT INTO `BmsOrg` VALUES (1, NULL, 0, '部门1', '', '', 0, 1, 1, '', '2023-03-02 15:04:22');
INSERT INTO `BmsOrg` VALUES (2, NULL, 1, '子部门1', '', '', 0, 1, 1, '', '2023-03-02 15:06:05');
INSERT INTO `BmsOrg` VALUES (3, NULL, 0, '部门2', '', '', 0, 1, 2, '', '2023-03-09 00:04:11');

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
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `uk_ownId_perkey_type`(`PermissionItemKey`, `OwnerId`, `OwnerType`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsOwnerPermission
-- ----------------------------
INSERT INTO `BmsOwnerPermission` VALUES ('1E4BAVBBQKG9953', 'User', 1, NULL, '1E1SQFF5MYO1802', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG2RPDS2265', '3', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG3V1A82266', '4', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG4VK1S2267', '2', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG5W2TC2268', '1', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG6WLKW2269', 'User', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG7X4CG2270', 'Product', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQG90G8W2271', 'Trade', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQGA0Z0G2272', 'Stock', 1, NULL, '1', 11, b'0');
INSERT INTO `BmsOwnerPermission` VALUES ('1EMAQGB1HS02273', 'Settings', 1, NULL, '1', 11, b'0');
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
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `uk_premissionkey`(`PermissionId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsPermission
-- ----------------------------
INSERT INTO `BmsPermission` VALUES (1, 'User', '用户管理', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (2, 'Product', '商品管理', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (3, 'Trade', '订单管理', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (4, 'Stock', '库存管理', NULL, NULL);
INSERT INTO `BmsPermission` VALUES (5, 'Settings', '系统设置', NULL, NULL);

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
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAGZUUYO0757', 'User', '1E6D9ZAXH6O5687');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAH37NSW0758', 'User', '1E6E9QYV9DS5688');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAH4NMTC0759', 'User', '1E7PIUJAZ402224');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAH650E80760', 'User', 'BROB0AXXBNCW6012');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAH7KZEO0761', 'User', 'BROB0B0P7L6O6013');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAH92CZK0762', 'User', 'BROB0B2D55HC6014');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAHAIC000763', 'User', 'BROB0B5U02RK6015');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAHBYB0G0764', 'User', 'BROB0B9AV01S6016');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAHDFOLC0765', 'User', 'BROCH9FGWRNK4466');
INSERT INTO `BmsPermissionMenu` VALUES ('1E8AAHEX2680766', 'User', 'BROCI4Z28YRK4467');
INSERT INTO `BmsPermissionMenu` VALUES ('1EMBMRN4VGG1893', 'Settings', 'BROB0AXXBNCW6012');
INSERT INTO `BmsPermissionMenu` VALUES ('1EMBMRO5E801894', 'Settings', '1EMAP43YWCG2265');
INSERT INTO `BmsPermissionMenu` VALUES ('1EMBMRP5WZK1895', 'Settings', '1EMBMFIFVI85474');
INSERT INTO `BmsPermissionMenu` VALUES ('1FFHKDM2DMO2960', 'Product', 'BROB0AXXBNCW6012');
INSERT INTO `BmsPermissionMenu` VALUES ('1FFHKDNE4XS2961', 'Product', 'BROCI4Z28YRK4467');
INSERT INTO `BmsPermissionMenu` VALUES ('1FFHKDOHGU82962', 'Product', 'BROC2QSJ85C04459');
INSERT INTO `BmsPermissionMenu` VALUES ('1FFHKDPM7B42963', 'Product', 'BROC6JJ64GSG4460');
INSERT INTO `BmsPermissionMenu` VALUES ('1FFHKDQO4N42964', 'Product', 'BROC98R8UJ284462');
INSERT INTO `BmsPermissionMenu` VALUES ('1FFHKDRSV402965', 'Product', 'BROCANIWJZ0G4463');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX68HI28ZK6901', 'Trade', 'BROCCJIF2BY84464');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPX68HI28ZL6902', 'Trade', 'BROCDVRHGCG04465');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPZFGL3HVCW6903', 'Stock', 'BRPZ9FB3GF7K6895');
INSERT INTO `BmsPermissionMenu` VALUES ('BRPZFGL3HVCX6904', 'Stock', 'BRPZCO9OSQ9S6896');

-- ----------------------------
-- Table structure for BmsRole
-- ----------------------------
DROP TABLE IF EXISTS `BmsRole`;
CREATE TABLE `BmsRole`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  `ExtraData` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsRole
-- ----------------------------
INSERT INTO `BmsRole` VALUES ('1', '超级管理员', 1, '2022-10-12 15:29:38', '2022-10-12 15:42:31', NULL);
INSERT INTO `BmsRole` VALUES ('1E1SQFF5MYO1802', '用户角色', 1, '2023-02-15 16:51:43', '2023-02-18 22:18:09', '');
INSERT INTO `BmsRole` VALUES ('1E252CAFWXS1471', '测试角色', -1, '2023-02-15 20:57:28', '2023-02-17 14:31:15', '测试');

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
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `uk_username`(`Username`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of BmsUser
-- ----------------------------
INSERT INTO `BmsUser` VALUES ('1', 'admin', 'N8DP41WT+VRlD4bkpr6Jj/VRrEMOmlrxu2SYeU0U5VE=', 'a42bbec9-99ef-49e0-805f-35841b3b4d24', 'Administrator', 1, '2022-10-12 10:20:26', '2023-02-15 20:00:13', 'init', NULL);
INSERT INTO `BmsUser` VALUES ('1E22D8OQWE82829', 'User01', '1KGcVCdNbwZLqMoxoK2FzLJ15BAyY/nJB1QLUGATSYA=', '023813bc-41c4-4c33-b3a7-3544e3045be7', '用户01', 1, '2023-02-15 20:03:43', '2023-02-21 19:24:17', '', NULL);
INSERT INTO `BmsUser` VALUES ('1E9C179WIM88630', 'User02', 'WgNXHU6chD4J1Nc9YzHkZUNt54JQfjmcFxa0G5CKUUs=', '0f9bd45a-22d1-4d6e-aea9-db8f026e182e', '用户02', 0, '2023-02-19 10:58:27', '2023-02-25 22:04:41', '', NULL);

-- ----------------------------
-- Table structure for BmsUserOrg
-- ----------------------------
DROP TABLE IF EXISTS `BmsUserOrg`;
CREATE TABLE `BmsUserOrg`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `OrgId` bigint(20) NULL DEFAULT NULL,
  `UserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsManager` bit(1) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  `ModifyTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsUserOrg
-- ----------------------------

-- ----------------------------
-- Table structure for BmsUserRole
-- ----------------------------
DROP TABLE IF EXISTS `BmsUserRole`;
CREATE TABLE `BmsUserRole`  (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `RoleId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of BmsUserRole
-- ----------------------------
INSERT INTO `BmsUserRole` VALUES ('1E48UWKZWAO9387', '1', '1E22D8OQWE82829');
INSERT INTO `BmsUserRole` VALUES ('1E48Y7O2JB49388', '1', '1');
INSERT INTO `BmsUserRole` VALUES ('1E48Y7PIIBK9389', '1E1SQFF5MYO1802', '1');

-- ----------------------------
-- Table structure for DictionaryType
-- ----------------------------
DROP TABLE IF EXISTS `DictionaryType`;
CREATE TABLE `DictionaryType`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` smallint(6) NULL DEFAULT NULL,
  `Sort` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of DictionaryType
-- ----------------------------
INSERT INTO `DictionaryType` VALUES (1, '性别', 'sex', '', 1, 0);
INSERT INTO `DictionaryType` VALUES (2, '状态', 'status', '', 1, 0);

-- ----------------------------
-- Table structure for DictionaryValue
-- ----------------------------
DROP TABLE IF EXISTS `DictionaryValue`;
CREATE TABLE `DictionaryValue`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DictionaryTypeId` bigint(20) NULL DEFAULT NULL,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Status` smallint(6) NULL DEFAULT NULL,
  `Sort` int(11) NULL DEFAULT NULL,
  `CreateTime` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `idx_dictionarytypeId`(`DictionaryTypeId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of DictionaryValue
-- ----------------------------
INSERT INTO `DictionaryValue` VALUES (1, '性别', NULL, 'sex', '', 1, 0, NULL);
INSERT INTO `DictionaryValue` VALUES (2, '男', 1, 'male', '', 1, 0, '2023-02-28 22:16:25');
INSERT INTO `DictionaryValue` VALUES (3, '女', 1, 'female', '', 1, 0, '2023-02-28 22:17:29');
INSERT INTO `DictionaryValue` VALUES (4, '启用', 2, 'enable', '', 1, 0, '2023-02-28 22:20:24');
INSERT INTO `DictionaryValue` VALUES (5, '禁用', 2, 'disable', '', 1, 0, '2023-02-28 22:21:58');

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
) ENGINE = InnoDB AUTO_INCREMENT = 155 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

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
INSERT INTO `OperationLog` VALUES (39, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 15:06:20', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (40, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 19:57:43', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (41, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 21:56:30', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (42, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 22:04:08', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (43, '127.0.0.1', '1', NULL, 1, '重置密码', '1E9C179WIM88630', '重置密码', '2023-02-25 22:04:42', '编码:1E9C179WIM88630', NULL, 2);
INSERT INTO `OperationLog` VALUES (44, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 22:04:53', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (45, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 22:08:14', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (46, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-25 22:15:57', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (47, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-26 12:23:54', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (48, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-26 12:48:28', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (49, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-26 13:04:02', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (50, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-27 19:39:04', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (51, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-27 19:41:10', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (52, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-27 22:37:59', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (53, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-28 20:37:13', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (54, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-28 20:44:04', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (55, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-02-28 22:15:47', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (56, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-01 10:03:35', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (57, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-01 10:36:40', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (58, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-01 13:20:38', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (59, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-01 16:40:33', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (60, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-01 22:31:05', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (61, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 14:06:21', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (62, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 14:26:17', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (63, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 14:29:10', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (64, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 14:32:01', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (65, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 14:39:17', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (66, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 14:48:59', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (67, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 15:00:31', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (68, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 15:03:16', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (69, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 15:11:30', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (70, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 15:22:42', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (71, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 19:27:13', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (72, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 22:15:57', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (73, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-02 23:04:05', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (74, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-03 09:53:44', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (75, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-03 11:41:37', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (76, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-03 12:01:28', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (77, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-03 13:01:59', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (78, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 12:19:51', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (79, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 12:57:50', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (80, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 14:21:43', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (81, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 14:50:57', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (82, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 22:18:44', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (83, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 22:32:06', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (84, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-08 23:18:50', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (85, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-09 00:00:53', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (86, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-09 00:13:11', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (87, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-09 16:15:11', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (88, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-09 20:30:50', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (89, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-10 12:49:44', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (90, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-11 15:47:24', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (91, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 10:19:37', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (92, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 10:26:44', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (93, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 10:53:34', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (94, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 10:58:38', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (95, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 11:26:05', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (96, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 13:06:37', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (97, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 18:57:38', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (98, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 19:54:37', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (99, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 20:50:03', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (100, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 21:08:30', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (101, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 22:39:37', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (102, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 22:59:11', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (103, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 23:14:40', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (104, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-12 23:30:11', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (105, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-13 12:16:02', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (106, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-13 19:30:28', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (107, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-13 20:59:16', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (108, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-13 23:19:51', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (109, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-13 23:33:19', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (110, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-14 00:00:11', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (111, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-14 10:36:01', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (112, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-14 19:09:28', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (113, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-14 22:04:38', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (114, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-14 22:50:10', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (115, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-14 22:53:02', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (116, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 09:11:34', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (117, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 09:58:28', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (118, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:09:05', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (119, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:09:40', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (120, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:11:41', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (121, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:13:59', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (122, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:14:39', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (123, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:15:28', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (124, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:27:33', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (125, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:30:47', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (126, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:34:23', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (127, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:36:08', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (128, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:37:59', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (129, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 10:41:06', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (130, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 12:41:34', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (131, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 13:10:22', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (132, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 15:53:26', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (133, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 18:05:51', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (134, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 21:37:26', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (135, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 21:38:51', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (136, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-15 23:20:49', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (137, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 00:03:07', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (138, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 00:11:24', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (139, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 00:45:18', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (140, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 00:50:00', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (141, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 10:54:14', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (142, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 10:55:30', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (143, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 11:34:43', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (144, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 13:05:55', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (145, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 14:31:30', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (146, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 16:17:09', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (147, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 17:01:24', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (148, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 17:19:22', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (149, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 17:54:25', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (150, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 18:00:51', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (151, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 19:25:48', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (152, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 19:28:57', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (153, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 20:31:02', '登录用户：admin,登陆成功', NULL, 0);
INSERT INTO `OperationLog` VALUES (154, '127.0.0.1', '1', NULL, 1, '登录', '1', 'Administrator', '2023-03-16 21:13:27', '登录用户：admin,登陆成功', NULL, 0);

SET FOREIGN_KEY_CHECKS = 1;
