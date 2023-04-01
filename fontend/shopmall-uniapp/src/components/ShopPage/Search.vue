<template>
  <!-- 搜索组件宽度自适应于外层 -->
  <u-sticky :h5-nav-height="0">
    <view :class="prop.parameters.style"
      class="u-padding-top-10 u-padding-bottom-10 u-padding-left-25 u-padding-right-25 u-margin-15 coreshop-bg-white">
      <u-toast ref="uToast" />
      <u-search v-model="keyword" :placeholder="prop.parameters.keywords" shape="square" :show-action="true"
        action-text="搜索" @custom="goSearch" @search="goSearch" />
    </view>
  </u-sticky>
</template>
<script lang="ts" setup>
import { ref, type CSSProperties } from "vue";

export interface SearchParameter {
  keywords: number;
  style: string | CSSProperties;
}
export interface SearchProps {
  parameters: SearchParameter;
}
const prop = defineProps<SearchProps>();
const keyword = ref("");
function goSearch() { }
</script>
