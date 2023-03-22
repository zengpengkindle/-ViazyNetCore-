<script lang="ts" setup>
import { ref, shallowRef, triggerRef, watch } from "vue";
import { useToken } from "@/hooks";
import { useUserStore } from "@/store/user";

import type { MenuGroup } from "./memberconfig";
import { onShow } from "@dcloudio/uni-app";

const { token } = useToken();
const user = useUserStore();

const toLoginOrEditInfo = () => {
  if (token.value) {
    uni.navigateTo({ url: "/pages/accountSetting/index" });
  } else {
    uni.navigateTo({ url: "/pages/authorization/index" });
  }
};

const sampleOrderStatusList = ref([
  {
    title: "待付款",
    status: 0,
    count: 0
  },
  {
    title: "待发货",
    status: 100,
    count: 0
  },
  {
    title: "待收货",
    status: 200,
    count: 0
  },
  {
    title: "待评价",
    status: 300,
    count: 0
  },
  {
    title: "退款/售后",
    status: 400,
    count: 0
  }
]);
const toSampleList = (status: number) => {
  uni.navigateTo({ url: `/pages/sampleOrder/index?status=${status}` });
};

const toSampleOrder = () => {
  uni.navigateTo({ url: "/pages/sampleOrder/index" });
};

const menuList = shallowRef<MenuGroup[]>([
  [
    {
      title: "我的钱包",
      path: "/pages/myWallet/index",
      icon: "wallet",
      desc: "",
      descColor: "#869198"
    },
    { title: "抖客收益", path: "/pages/cpsBenefit/index", icon: "folder" },
    { title: "推送管理", path: "/pages/pushManage/index", icon: "userPush" },
    { title: "赏金记录", path: "/pages/rewardRecord/index", icon: "record" }
  ],
  [
    {
      title: "我的关注",
      path: "/pages/myCollection/index",
      icon: "userCollect"
    },
    { title: "抖音号管理", path: "/pages/dyhManage/index", icon: "userDy" },
    { title: "我的橱窗", path: "/pages/myShopwindow/index", icon: "userStore" },
    { title: "货源管理", path: "/pages/channelManage/index", icon: "userTz" }
  ],
  [
    {
      title: "联系我们",
      path: "/pages/contactUs/index",
      icon: "userContact",
      checkLogin: false
    },
    {
      title: "常见问题",
      path: "/pages/commonProblem/index",
      icon: "question_circle",
      checkLogin: false
    }
  ]
]);

onShow(() => {
  if (token.value) {
    user.getInfo(false);
  }
});

watch(token, () => {
  if (!token.value) {
    menuList.value[0][0].desc = "";
    triggerRef(menuList);
  }
});
</script>

<template>
  <view
    class="mine"
    :style="{
      backgroundSize: '100vw 520rpx',
      backgroundRepeat: 'no-repeat'
    }"
  >
    <view class="user-wrapper">
      <text class="nickname ellipsis">
        {{ token ? user.info?.nick_name : "未登录" }}
      </text>
      <view class="edit-info-btn" @click="toLoginOrEditInfo">
        {{ token ? "编辑资料" : "立即登录" }}
      </view>
    </view>

    <!-- 我的订单 -->
    <view class="sample-block">
      <view class="block-header">
        <view class="block-title"> 我的订单 </view>
        <view class="more-btn" @click="toSampleOrder">
          <text class="text"> 查看更多 </text>
        </view>
      </view>
      <view class="sample-status-wrapper">
        <view
          v-for="item of sampleOrderStatusList"
          :key="item.status"
          class="status-item"
          @click="toSampleList(item.status)"
        >
          <text class="count">
            {{ item.count ? (item.count > 999 ? "999+" : item.count) : 0 }}
          </text>
          <text class="sample-status-title">
            {{ item.title }}
          </text>
        </view>
      </view>
    </view>

    <view v-for="(group, index) of menuList" :key="index" class="menu-group">
      <navigator
        v-for="item of group"
        :key="item.path"
        class="menu-item"
        :url="
          item.checkLogin !== false && !token
            ? '/pages/authorization/index'
            : item.path
        "
        :open-type="
          item.checkLogin !== false && !token ? 'redirect' : 'navigate'
        "
      >
        <text class="title">
          {{ item.title }}
        </text>
        <text v-if="item.desc" class="desc" :style="{ color: item.descColor }">
          {{ item.desc }}
        </text>
      </navigator>
    </view>
  </view>
</template>
<style lang="scss" scoped>
@import "./member.scss";
</style>
