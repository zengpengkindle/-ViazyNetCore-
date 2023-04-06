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
import ShopPage from "@/components/ShopPage/page.vue";
import { ref, getCurrentInstance, onMounted, type Ref } from "vue";
import PageApi, { type DesginItem } from "@/apis/shopmall/page";
import { GetRect, useHeader } from "@/components/ui/hooks/user-head";
import { onShow } from "@dcloudio/uni-app";
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
});
onShow(async () => {
  const haedRect = (await GetRect(instance, "#indexnavbar")) as UniApp.NodeInfo;
  boundingRect.value.height = haedRect.height;
  boundingRect.value.width = haedRect.width;
  boundingRect.value.top = haedRect.top;
  boundingRect.value.bottom = haedRect.bottom;

  uni.pageScrollTo({ scrollTop: 0 });
});
const instance = getCurrentInstance();
</script>
