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
    <shop-page :data="pageData" />
  </view>
</template>
<script lang="ts" setup>
import ShopPage from "@/components/shoppage/page.vue";
import { ref, getCurrentInstance, onMounted, type Ref } from "vue";
import PageApi, { type DesginItem } from "@/apis/shopmall/page";
const appTitle = ref("ViazyNetCore");
const {
  appContext: {
    config: { globalProperties }
  }
} = getCurrentInstance();
const background = globalProperties.$themes.MainNavBar.background;
const titleColor = globalProperties.$themes.MainNavBar.titleColor;
const pageData: Ref<Array<DesginItem>> = ref([]);
onMounted(async () => {
  pageData.value = await PageApi.getPageData("mobile_home");
});
</script>
