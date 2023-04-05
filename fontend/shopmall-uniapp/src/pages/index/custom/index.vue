<template>
  <view class="main">
    <!--提示框组件-->
    <u-toast ref="uToast" />
    <!--无网络组件-->
    <u-no-network />
    <!--头部组件-->
    <u-navbar
      id="indexnavbar"
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
import { ref, getCurrentInstance, onMounted, nextTick, type Ref } from "vue";
import PageApi, { type DesginItem } from "@/apis/shopmall/page";
import { GetRect, useHeader } from "@/components/ui/hooks/user-head";
const appTitle = ref("ViazyNetCore");
const {
  appContext: {
    config: { globalProperties }
  }
} = getCurrentInstance();
const background = globalProperties.$themes.MainNavBar.background;
const titleColor = globalProperties.$themes.MainNavBar.titleColor;
const pageData: Ref<Array<DesginItem>> = ref([]);
const { boundingRect } = useHeader();
onMounted(async () => {
  pageData.value = await PageApi.getPageData("mobile_home");
  nextTick(async () => {
    const haedRect = (await GetRect(
      instance,
      "#indexnavbar"
    )) as UniApp.NodeInfo;
    boundingRect.value.height = haedRect.height;
  });
});

const instance = getCurrentInstance();
</script>
