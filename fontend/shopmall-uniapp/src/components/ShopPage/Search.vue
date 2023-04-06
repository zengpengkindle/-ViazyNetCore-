<template>
  <u-sticky :h5-nav-height="0" :offset-top="boundingRect.height + 'px'">
    <view :class="prop.parameters.style" class="page-search">
      <!-- 搜索组件宽度自适应于外层 -->
      <u-toast ref="uToast" />
      <u-search
        v-model="keyword"
        disabled
        :placeholder="prop.parameters.keywords"
        shape="round"
        :show-action="true"
        action-text="搜索"
        @custom="goSearch"
        @click="goSearch"
        @search="goSearch"
      />
    </view>
  </u-sticky>
</template>
<script lang="ts" setup>
import { onMounted, ref, type CSSProperties } from "vue";
import { useHeader } from "@/components/ui/hooks/user-head";

export interface SearchParameter {
  keywords: number;
  style: string | CSSProperties;
}
export interface SearchProps {
  parameters: SearchParameter;
}
const prop = defineProps<SearchProps>();
const keyword = ref("");
const { boundingRect } = useHeader();
onMounted(() => {});
function goSearch() {
  uni.navigateTo({ url: "/pages/selection/search/index" });
}
</script>
<style lang="scss" scoped>
.page-search {
  z-index: 17000;
  background: #ffffff;
  padding: 15rpx 25rpx;
}
</style>
