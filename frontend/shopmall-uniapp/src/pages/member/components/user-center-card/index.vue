<template>
  <view class="user-center-card">
    <!-- 未登录的情况 -->
    <block v-if="currAuthStep === AuthStepType.ONE">
      <view class="user-center-card__header" @click="gotoUserEditPage">
        <u-avatar
          :size="100"
          sex-icon="man"
          :src="userInfo.avatarUrl || defaultAvatarUrl"
          class="user-center-card__header__avatar"
        />
        <view class="user-center-card__header__name">请登录</view>
      </view>
    </block>
    <!-- 已登录但未授权用户信息情况 -->
    <block v-if="currAuthStep === AuthStepType.TWO">
      <view class="user-center-card__header">
        <u-avatar
          :src="userInfo.avatarUrl || defaultAvatarUrl"
          class="user-center-card__header__avatar"
        />
        <view class="user-center-card__header__name">{{
          userInfo.nickName || "微信用户"
        }}</view>
        <!-- 需要授权用户信息，通过slot添加弹窗 -->
        <view
          v-if="isNeedGetUserInfo"
          class="user-center-card__header__transparent"
        >
          <slot name="getUserInfo" />
        </view>
        <!-- 不需要授权用户信息，仍然触发gotoUserEditPage事件 -->
        <view
          v-else
          class="user-center-card__header__transparent"
          @click="gotoUserEditPage"
        />
      </view>
    </block>
    <!-- 已登录且已经授权用户信息的情况 -->
    <block v-if="currAuthStep === AuthStepType.THREE">
      <view class="user-center-card__header" @click="gotoUserEditPage">
        <u-avatar
          mode="aspectFill"
          class="user-center-card__header__avatar"
          :src="userInfo.avatarUrl || defaultAvatarUrl"
        />
        <view class="user-center-card__header__name">{{
          userInfo.nickName || "微信用户"
        }}</view>
      </view>
    </block>
  </view>
</template>
<script lang="ts" setup>
import { ref, type Ref } from "vue";
enum AuthStepType {
  ONE = 1,
  TWO = 2,
  THREE = 3
}
const currAuthStep = ref(AuthStepType.ONE);
interface UserInfo {
  avatarUrl: string;
  nickName: string;
}
const defaultAvatarUrl = ref("");
const isNeedGetUserInfo = ref(false);
const userInfo: Ref<UserInfo> = ref({
  avatarUrl: "",
  nickName: ""
});
const gotoUserEditPage = () => {};
</script>
<style class="scss" scoped>
.user-center-card {
  top: 0;
  left: 0;
  width: 100%;
  height: 480rpx;
  background-image: url("/static/images/bg/user-center-bg.svg");
  background-size: cover;
  background-repeat: no-repeat;
  padding: 0 24rpx;
  box-sizing: border-box;
}
.user-center-card__header {
  padding-top: 192rpx;
  line-height: 48rpx;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: #333;
  width: 100%;
}
.user-center-card__header__avatar {
  box-shadow: #333 3rpx 5rpx 0.5;
}

.user-center-card__header__name {
  font-size: 36rpx;
  line-height: 48rpx;
  color: #333;
  font-weight: bold;
  margin-left: 24rpx;
  margin-right: 16rpx;
}
.user-center-card__header__transparent {
  position: absolute;
  left: 0;
  top: 0;
  background-color: transparent;
  height: 100%;
  width: 100%;
}
.user-center-card__icon {
  line-height: 96rpx;
}
</style>
