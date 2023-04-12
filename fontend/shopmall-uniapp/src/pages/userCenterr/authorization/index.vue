<template>
  <view class="authorization">
    <image
      class="logo"
      src="https://cdn.yuantuitui.com/ytt_mini/icon/xpy-logo.svg"
    />
    <image
      class="brand"
      src="https://cdn.yuantuitui.com/ytt_mini/icon/xpy-brand-text.svg"
    />

    <button
      v-if="true"
      open-type="getPhoneNumber"
      class="auth-phone-btn"
      @getphonenumber="getPhoneNumber"
    >
      微信用户一键登录
    </button>
    <button v-else class="directly-login-btn" @click="directlyLogin">
      微信用户一键登录
    </button>

    <view class="phone-login" @click="toPhoneLogin"> 手机号验证登录 </view>

    <view class="footer-wrapper" @click="checked = !checked">
      <view class="agreement-text">
        若账号未注册，登录即完成注册并同意猿推推用户<text
          class="protocol"
          @click="toServiceProtocol"
        >
          《服务协议》 </text
        >和<text class="protocol" @click="toPrivateProtocol">
          《隐私协议》
        </text>
      </view>
    </view>
  </view>
</template>

<script lang="ts" setup>
import WechatApi from "@/apis/wechat";
import { useStorage, useToken } from "@/hooks";
import { AGREEMENT_CHECK_KEY, asyncLogin } from "@/utils";
import { useUserStore } from "@/store/user";

const { setToken } = useToken();
const user = useUserStore();

const checked = useStorage(AGREEMENT_CHECK_KEY, false);

const directlyLogin = () => {
  //
};

const getPhoneNumber = async (e: any) => {
  if (!checked.value) {
    return uni.showToast({
      title: "请阅读并勾选协议",
      icon: "none"
    });
  }

  if (!e?.detail?.encryptedData) {
    uni.showToast({
      title: "授权失败",
      icon: "none"
    });
    return;
  }

  try {
    uni.showLoading({ title: "正在登录", mask: true });
    const loginRes = await asyncLogin();
    let res = await WechatApi.getJscode({ code: loginRes.code });
    user.setLoginInfo(res);
    if (!res.isBindMobile) {
      res = await WechatApi.bindmobile({
        authCode: res.authCode,
        encryptedData: e.detail.encryptedData,
        iv: e.detail.iv,
        mobile: "",
        smsCode: ""
      });
    }
    setToken(res.token.accessToken);

    console.log(res);
    if (res.get_user_profile) {
      uni.redirectTo({ url: "/pages/authProfile/index" });
    } else {
      user.getInfo();
      uni.reLaunch({ url: "/pages/selection/index" });
    }

    uni.hideLoading();
    uni.showToast({ title: "登录成功", icon: "none" });
  } catch (err: any) {
    console.log(err);
    uni.hideLoading();
    uni.showToast({
      title: err?.data?.data?.message || "登录失败",
      icon: "none"
    });
  }
};

const toPhoneLogin = () => {
  uni.navigateTo({ url: "/pages/login/index" });
};

const toServiceProtocol = () => {
  uni.navigateTo({ url: "/pages/serviceProtocol/index" });
};
const toPrivateProtocol = () => {
  uni.navigateTo({ url: "/pages/privacyProtocol/index" });
};
</script>

<style lang="scss" scoped>
.authorization {
  min-height: 100vh;
  padding: 200rpx 50rpx 0;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  align-items: center;
  .logo {
    width: 160rpx;
    height: 160rpx;
    border-radius: 48rpx;
    box-shadow: 0px 4px 8px 1px rgba(222, 63, 79, 0.15);
  }
  .brand {
    margin-top: 40rpx;
    width: 160rpx;
    height: 48rpx;
  }
  .directly-login-btn,
  .auth-phone-btn {
    margin-top: 400rpx;
    width: 100%;
    height: 104rpx;
    border-radius: 8rpx;
    color: #fff;
    background-color: $u-type-primary;
    border: none;
    outline: none;
    font-size: 32rpx;
    line-height: 44rpx;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .phone-login {
    margin-top: 36rpx;
    width: 100%;
    height: 104rpx;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 28rpx;
    line-height: 44rpx;
    text-decoration: underline;
    color: rgba(134, 145, 152, 0.6);
  }

  .footer-wrapper {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    padding: 0 40rpx calc(env(safe-area-inset-bottom) + 40rpx);
    display: flex;
    align-items: flex-start;
    height: 72rpx;
    .check-icon {
      margin-top: 4rpx;
      flex-shrink: 0;
      width: 32rpx;
      height: 32rpx;
    }
    .agreement-text {
      margin-left: 8rpx;
      font-size: 24rpx;
      line-height: 36rpx;
      color: #999;
      .protocol {
        color: #3f88c5;
      }
    }
  }
}
</style>
