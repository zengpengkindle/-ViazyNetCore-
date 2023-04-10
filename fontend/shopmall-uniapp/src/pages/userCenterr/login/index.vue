<template>
  <view class="login">
    <view class="body-wrapper">
      <view class="title">
        {{ isPhoneLogin ? "手机号登录" : "账号密码登录" }}
      </view>

      <input
        v-model="mobile"
        type="number"
        :placeholder="isPhoneLogin ? '请输入手机号码' : '请输入账号'"
        placeholder-style="color: rgba(134, 145, 152, 0.4);"
        class="input"
      />
      <view v-if="isPhoneLogin" class="yzm">
        <input
          v-model="code"
          type="number"
          placeholder="请输入验证码"
          placeholder-style="color: rgba(134, 145, 152, 0.4);"
          class="input"
        />
        <button
          class="get-code-btn"
          :disabled="cd.status.value === 'progressing'"
          @click="getCode"
        >
          {{
            cd.status.value === "progressing"
              ? cd.countdown.value + "s"
              : "获取验证码"
          }}
        </button>
      </view>
      <input
        v-else
        v-model="password"
        type="password"
        placeholder="请输入密码"
        placeholder-style="color: rgba(134, 145, 152, 0.4);"
        class="input"
      />

      <button class="btn" @click="doLogin">登录</button>

      <view class="bottom-wrapper">
        <view class="switch-method-btn" @click="isPhoneLogin = !isPhoneLogin">
          {{ isPhoneLogin ? "账号密码登录" : "手机号登录" }}
        </view>

        <button
          v-if="loginInfo?.is_bind_mobile"
          class="wechat-auth-btn"
          @click="directlyLogin"
        >
          <text class="text"> 微信授权登录 </text>
        </button>
        <button
          v-else
          open-type="getPhoneNumber"
          class="wechat-auth-btn"
          @getphonenumber="getPhoneNumber"
        >
          <text class="text"> 微信授权登录 </text>
        </button>
      </view>
    </view>

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

    <ytt-overlay v-model:visible="popupVisible" central>
      <view class="popup-wrapper" @touchmove.stop @click.stop>
        <view class="popup-header">
          <text class="popup-title"> 温馨提示 </text>
        </view>
        <view class="content"> 该手机号暂未注册，请使用微信用户一键登录 </view>
        <view class="btn-group">
          <view class="cancel-btn" @click="popupVisible = false"> 取消 </view>
          <button
            v-if="loginInfo?.is_bind_mobile"
            class="confirm-btn"
            @click="directlyLogin"
          >
            确定
          </button>
          <button
            v-else
            open-type="getPhoneNumber"
            class="confirm-btn"
            @getphonenumber="getPhoneNumber"
            @click="popupVisible = false"
          >
            确定
          </button>
        </view>
      </view>
    </ytt-overlay>
  </view>
</template>

<script lang="ts" setup>
import { ref, toRef } from "vue";
import { sendSms, loginSms, loginPass } from "@/apis/login";
import { useUserStore } from "@/store/user";
import { useCountdown, useToken, useStorage } from "@/hooks";
import YttOverlay from "@/libs/YttOverlay.vue";
import { wxBind } from "@/apis/login";
import { loginProcess, AGREEMENT_CHECK_KEY } from "@/utils";

const user = useUserStore();
const { setToken } = useToken();

const checked = useStorage(AGREEMENT_CHECK_KEY, false);

const loginInfo = toRef(user, "loginInfo");
if (!loginInfo.value) {
  user.login();
}

// 轻提示
const toast = (title: string) => {
  uni.showToast({
    title: title,
    icon: "none"
  });
};

const cd = useCountdown(60, { id: "login" });

const isPhoneLogin = ref(true); // 手机号登录
const mobile = ref("");
const code = ref("");
const password = ref("");
const ticket = ref("");

const popupVisible = ref(false);
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

    const res = await wxBind({
      auth_code: loginInfo.value!.auth_code,
      encryptedData: e.detail.encryptedData,
      iv: e.detail.iv
    });

    setToken(res.token.access_token);

    if (res.get_user_profile) {
      uni.redirectTo({
        url: "/pages/authProfile/index"
      });
    } else {
      user.getInfo();
    }

    uni.hideLoading();
    uni.showToast({
      title: "登录成功",
      icon: "none"
    });
  } catch (err: any) {
    uni.hideLoading();
    uni.showToast({
      title: err?.data?.data?.message || "登录失败",
      icon: "none"
    });
    console.log(err);
  }
};
const directlyLogin = async () => {
  if (!checked.value) {
    return uni.showToast({
      title: "请阅读并勾选协议",
      icon: "none"
    });
  }

  popupVisible.value = false;
  uni.showLoading({ title: "正在登录", mask: true });
  setToken(loginInfo.value!.token.access_token);
  await user.getInfo();
  loginProcess();
  uni.hideLoading();
  uni.showToast({
    title: "登录成功",
    icon: "none"
  });
};

//获取验证码
const getCode = async () => {
  if (cd.status.value === "progressing") {
    return;
  }

  if (!verifyPhone(mobile.value)) {
    return toast("手机号码错误，请重新输入");
  }
  const res = await sendSms({ mobile: mobile.value, ticket: ticket.value });
  if (typeof res !== "boolean" && res.err_code === 10091) {
    return (popupVisible.value = true);
  }
  cd.start();
};

// 登录
const doLogin = async () => {
  if (!checked.value) {
    return uni.showToast({
      title: "请阅读并勾选协议",
      icon: "none"
    });
  }

  try {
    if (!verifyPhone(mobile.value)) {
      return toast("手机号码错误，请重新输入");
    }
    uni.showLoading({ title: "正在登录", mask: true });

    let accessToken: string | undefined = "";
    let showSetPassword = false;

    if (isPhoneLogin.value) {
      // 手机号登录
      if (!code.value) {
        return toast("请输入验证码");
      }
      const res = await loginSms({ mobile: mobile.value, code: code.value });
      accessToken = res?.token.access_token;
      showSetPassword = res.should_set_password;
    } else {
      // 账号密码登录
      if (!password.value) {
        return toast("请输入密码");
      }
      const res = await loginPass({
        mobile: mobile.value,
        password: password.value,
        ticket: ticket.value
      });
      accessToken = res?.token.access_token;
    }

    if (accessToken) {
      setToken(accessToken);
    }

    user.getInfo();
    if (showSetPassword) {
      uni.navigateTo({
        url: "/pages/setPassword/index"
      });
    } else {
      loginProcess();
    }

    uni.hideLoading();
    uni.showToast({
      title: "登录成功",
      icon: "none"
    });
  } catch (e: any) {
    console.error(e);
    uni.hideLoading();
    if (e?.startsWith("该手机号暂未注册")) {
      popupVisible.value = true;
    } else {
      uni.showToast({
        title: e || "登录失败，请联系管理员",
        icon: "none"
      });
    }
  }
};

//验证手机号
const verifyPhone = (mobile: string) => {
  const reg = /^1[0-9]{10}$/;
  return reg.test(mobile);
};

const toServiceProtocol = () => {
  uni.navigateTo({ url: "/pages/serviceProtocol/index" });
};
const toPrivateProtocol = () => {
  uni.navigateTo({ url: "/pages/privacyProtocol/index" });
};
</script>

<style lang="scss" scoped>
.login {
  .body-wrapper {
    height: calc(100vh - 120rpx);
    padding: 0 50rpx;
    padding-top: 80rpx;
    box-sizing: border-box;
  }

  .title {
    margin-bottom: 18rpx;
    font-weight: 600;
    font-size: 44rpx;
    line-height: 60rpx;
    color: #2f3437;
  }

  .input {
    border-bottom: 1rpx solid #f2f2f2;
    padding: 62rpx 180rpx 32rpx 0;
    font-size: 32rpx;
  }
  .yzm {
    position: relative;
    .get-code-btn {
      position: absolute;
      font-weight: 500;
      font-size: 28rpx;
      color: #de3f4f;
      right: -8rpx;
      top: 56rpx;
      background: #fff;
    }
    .get-code-btn:after {
      border: none;
    }
  }
  .btn {
    width: 100%;
    height: 100rpx;
    margin-top: 80rpx;
    background: #de3f4f;
    border-radius: 8rpx;
    font-size: 32rpx;
    line-height: 100rpx;
    text-align: center;
    color: #fff;
    &::after {
      border: none;
    }
  }

  .bottom-wrapper {
    margin-top: 36rpx;
    height: 114rpx;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 32rpx;
    line-height: 44rpx;
    color: #5e686e;
    .switch-method-btn {
      height: 100%;
      display: flex;
      align-items: center;
    }
    .wechat-auth-btn {
      height: 100%;
      outline: none;
      border: none;
      font-size: inherit;
      line-height: inherit;
      color: inherit;
      margin: 0;
      padding: 0;
      background-color: transparent;
      &::after {
        border: none;
      }
      display: flex;
      align-items: center;
      .icon {
        width: 48rpx;
        height: 48rpx;
        margin-right: 16rpx;
      }
    }
  }
}

.popup-wrapper {
  width: 560rpx;
  box-sizing: border-box;
  padding: 40rpx 40rpx 50rpx;
  background-color: #fff;
  box-shadow: 0px 5px 140px rgba(62, 62, 62, 0.1);
  border-radius: 16rpx;
  .popup-header {
    display: flex;
    align-items: center;
    justify-content: center;
    .icon {
      width: 36rpx;
      height: 36rpx;
    }
    .popup-title {
      margin-left: 20rpx;
      font-weight: 600;
      font-size: 32rpx;
      line-height: 48rpx;
      color: #2f3437;
    }
  }
  .content {
    margin-top: 40rpx;
    font-size: 28rpx;
    line-height: 40rpx;
    color: #5e686e;
  }
  .btn-group {
    margin-top: 52rpx;
    display: flex;
    align-items: center;
    .cancel-btn,
    .confirm-btn {
      flex: 1;
      border-radius: 4px;
      display: flex;
      height: 80rpx;
      align-items: center;
      justify-content: center;
      font-weight: 500;
      font-size: 28rpx;
      line-height: 40rpx;
    }
    .cancel-btn {
      color: #5e686e;
      background: #f8f8f8;
    }
    .confirm-btn {
      margin-left: 24rpx;
      background: #de3f4f;
      color: #fff;
      padding: 0;
      border: none;
      outline: none;
      &::after {
        border: none;
      }
    }
  }
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
</style>
