<template>
  <view>
    <!--提示框组件-->
    <u-toast ref="uToast" />
    <!--无网络组件-->
    <u-no-network />
    <!--头部组件-->
    <u-navbar
      :is-back="false"
      :title="appTitle"
      :background="background"
      :title-color="titleColor"
    />
  </view>
</template>
<script lang="ts" setup>
import { ref, getCurrentInstance } from "vue";
const appTitle = ref("");
const {
  appContext: {
    config: { globalProperties }
  }
} = getCurrentInstance();
const background = globalProperties.$themes.MainNavBar.backIconColor;
const titleColor = globalProperties.$themes.MainNavBar.titleColor;
</script>
