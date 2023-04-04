<template>
  <u-sticky :h5-nav-height="0" :offset-top="top">
    <view
      :class="prop.parameters.style"
      class="u-padding-top-10 u-padding-bottom-10 u-padding-left-25 u-padding-right-25 u-margin-15"
    >
      <!-- 搜索组件宽度自适应于外层 -->
      <u-toast ref="uToast" />
      <u-search
        v-model="keyword"
        :placeholder="prop.parameters.keywords"
        shape="square"
        :show-action="true"
        action-text="搜索"
        @custom="goSearch"
        @search="goSearch"
      />
    </view>
  </u-sticky>
</template>
<script lang="ts" setup>
import { getCurrentInstance, onMounted, ref, type CSSProperties } from "vue";

export interface SearchParameter {
  keywords: number;
  style: string | CSSProperties;
}
export interface SearchProps {
  parameters: SearchParameter;
}
const prop = defineProps<SearchProps>();
const keyword = ref("");
const top = ref(0);
onMounted(() => {
  uni
    .createSelectorQuery()
    .in(getCurrentInstance())
    .select(".u-navbar")
    .boundingClientRect(res => {
      top.value = (res as UniApp.NodeInfo).height;
    })
    .exec();
});
function goSearch() {}
</script>
