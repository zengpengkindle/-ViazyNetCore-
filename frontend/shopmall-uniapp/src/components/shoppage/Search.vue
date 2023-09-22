<template>
  <u-sticky :h5-nav-height="0" :offset-top="boundingRect.height - 1 + 'px'">
    <view :style="searchStyle" class="page-search">
      <!-- 搜索组件宽度自适应于外层 -->
      <u-toast ref="uToast" />
      <u-search
        v-model="keyword"
        disabled
        :placeholder="prop.parameters.keywords"
        :shape="prop.parameters.style"
        :show-action="true"
        action-text="搜索"
        :action-style="{
          width: 'auto',
          color: '#fff',
          display: 'block',
          position: 'absolute',
          right: '30rpx',
          background: customHeaderStyle?.background?.backgroundImage,
          height: '28px',
          lineHeight: '28px',
          borderRadius: '14px',
          padding: '0 10px'
        }"
        @custom="goSearch"
        @click="goSearch"
        @search="goSearch"
      />
    </view>
  </u-sticky>
</template>
<script lang="ts" setup>
import { onMounted, ref, computed, type CSSProperties } from "vue";
import { useHeader } from "@/components/ui/hooks/user-head";

export interface SearchParameter {
  keywords: number;
  style: string;
}
export interface SearchProps {
  parameters: SearchParameter;
}
const prop = defineProps<SearchProps>();
const keyword = ref("");
const { boundingRect, customHeaderStyle } = useHeader();
onMounted(() => {});
const searchStyle = computed(() => {
  let style: CSSProperties = {};
  if (style != null)
    style.background = customHeaderStyle.value?.background?.backgroundImage;
  else {
    style = {};
    style.background = customHeaderStyle.value?.background?.backgroundImage;
  }
  return style;
});
function goSearch() {
  uni.navigateTo({ url: "/pages/selection/search/index" });
}
</script>
<style lang="scss" scoped>
.page-search {
  z-index: 17000;
  background: #ffffff;
  padding: 15rpx 25rpx;
  box-sizing: border-box;
}
</style>
