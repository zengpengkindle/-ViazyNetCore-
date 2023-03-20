<template>
  <view
    class="mine"
    :style="{
      backgroundSize: '100vw 520rpx',
      backgroundRepeat: 'no-repeat',
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

    <!-- 样品订单 -->
    <view class="sample-block">
      <view class="block-header">
        <view class="block-title"> 样品订单 </view>
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

<script lang="ts" setup>
import { computed, ref, shallowRef, toRef, triggerRef, watch } from "vue";
import { useToken } from "@/hooks";
import { useUserStore } from "@/store/user";

import AttentionOfficial from "@/components/AttentionOfficial.vue";
import YttHeader from "@/libs/YttHeader.vue";

import type { MenuGroup } from "./memberconfig";
import { useBusinessStore } from "@/store/business";
import { onShow } from "@dcloudio/uni-app";

const { token } = useToken();
const user = useUserStore();
const business = useBusinessStore();

const toLoginOrEditInfo = () => {
  if (token.value) {
    uni.navigateTo({ url: "/pages/accountSetting/index" });
  } else {
    uni.navigateTo({ url: "/pages/authorization/index" });
  }
};

const pickStatus = toRef(business, "pickStatus");
const sampleOrderStatusList = ref([
  {
    title: "待审核",
    status: 0,
    count: computed(() => pickStatus.value?.wait_review),
  },
  {
    title: "待发货",
    status: 100,
    count: computed(() => pickStatus.value?.wait_ship),
  },
  {
    title: "运输中",
    status: 200,
    count: computed(() => pickStatus.value?.transport),
  },
  {
    title: "待交付",
    status: 300,
    count: computed(() => pickStatus.value?.wait_delivery),
  },
  {
    title: "已完成",
    status: 400,
    count: computed(() => pickStatus.value?.finished),
  },
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
      descColor: "#869198",
    },
    { title: "抖客收益", path: "/pages/cpsBenefit/index", icon: "folder" },
    { title: "推送管理", path: "/pages/pushManage/index", icon: "userPush" },
    { title: "赏金记录", path: "/pages/rewardRecord/index", icon: "record" },
  ],
  [
    {
      title: "我的关注",
      path: "/pages/myCollection/index",
      icon: "userCollect",
    },
    { title: "抖音号管理", path: "/pages/dyhManage/index", icon: "userDy" },
    { title: "我的橱窗", path: "/pages/myShopwindow/index", icon: "userStore" },
    { title: "货源管理", path: "/pages/channelManage/index", icon: "userTz" },
  ],
  [
    {
      title: "联系我们",
      path: "/pages/contactUs/index",
      icon: "userContact",
      checkLogin: false,
    },
    {
      title: "常见问题",
      path: "/pages/commonProblem/index",
      icon: "question_circle",
      checkLogin: false,
    },
  ],
]);

onShow(() => {
  if (token.value) {
    business.getSampleOrderStatus();
    user.getInfo(false);
    user.getBalance().then(() => {
      menuList.value[0][0].desc = user.balance.toFixed(2);
      triggerRef(menuList);
    });
  }
});

watch(token, () => {
  if (!token.value) {
    business.clearSampleOrderStats();
    user.clearBalance();
    menuList.value[0][0].desc = "";
    triggerRef(menuList);
  }
});
</script>

<style lang="scss" scoped>
.mine {
  min-height: 100vh;
  padding-bottom: 24rpx;
  box-sizing: border-box;
  background-color: #f7f8fc;
  .user-wrapper {
    padding: 0 24rpx;
    margin-top: 36rpx;
    display: flex;
    align-items: center;
    .avatar {
      width: 128rpx;
      height: 128rpx;
      border-radius: 50%;
      border: 2rpx solid #fff;
      box-shadow: 2px 4px 6px rgba(0, 0, 0, 0.08);
      &.default {
        border: none;
      }
    }
    .nickname {
      margin-left: 24rpx;
      font-weight: 600;
      font-size: 32rpx;
      line-height: 48rpx;
      color: #2f3437;
      max-width: 240rpx;
    }
    .edit-info-btn {
      margin-left: auto;
      padding: 4rpx 24rpx;
      border: 1px solid #de3f4f;
      border-radius: 40rpx;
      color: #de3f4f;
      font-size: 24rpx;
      line-height: 40rpx;
    }
  }
  .sample-block {
    margin: 40rpx 24rpx 24rpx 24rpx;
    padding: 24rpx;
    background: #ffffff;
    border-radius: 16rpx;
    .block-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      .block-title {
        margin-left: 16rpx;
        font-weight: 600;
        font-size: 28rpx;
        line-height: 40rpx;
      }
      .more-btn {
        display: flex;
        align-items: center;
        .text {
          font-size: 24rpx;
          line-height: 40rpx;
          color: #869198;
        }
        .arrow_right {
          margin-left: 8rpx;
          width: 32rpx;
          height: 32rpx;
        }
      }
    }
    .sample-status-wrapper {
      margin-top: 24rpx;
      padding: 0 12rpx;
      display: flex;
      align-items: center;
      .status-item {
        flex: 1;
        padding: 2rpx 0;
        display: flex;
        flex-direction: column;
        align-items: center;
        .count {
          font-weight: bold;
          font-size: 36rpx;
          line-height: 48rpx;
          color: #2f3437;
        }
        .sample-status-title {
          margin-top: 4rpx;
          font-size: 24rpx;
          line-height: 40rpx;
          color: #869198;
        }
      }
      .status-item + .status-item {
        margin-left: 24rpx;
      }
    }
  }
  .menu-group {
    background-color: #fff;
    border-radius: 16rpx;
    overflow: hidden;
    margin: 24rpx 24rpx 0;
    .menu-item {
      padding: 32rpx;
      display: flex;
      align-items: center;
      .icon {
        width: 48rpx;
        height: 48rpx;
      }
      .title {
        margin-left: 24rpx;
        margin-right: auto;
        font-size: 28rpx;
        line-height: 40rpx;
        color: #2f3437;
      }
      .desc {
        margin-left: auto;
        font-size: 26rpx;
      }
      .arrow-icon {
        width: 32rpx;
        height: 32rpx;
      }
    }
    .menu-item + .menu-item {
      border-top: 1px solid #f2f3f5;
    }
  }
}
</style>
