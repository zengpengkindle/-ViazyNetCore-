<template>
  <u-search
    :placeholder="keyword || '请输入商品信息'"
    :animation="false"
    :show-action="false"
    disabled
    @click="goSearch"
  />
  <Filter />
</template>
<script lang="ts" setup>
import { ref } from "vue";
import Filter from "@/components/ui/filter/index.vue";
const keyword = ref("");
const goSearch = () => {
  uni.navigateTo({
    url: `/pages/selection/search/index`
  });
};
</script>
