<template>
  <u-search
    :placeholder="keyword || '请输入商品信息'"
    :animation="false"
    :show-action="false"
    disabled
    @click="goSearch"
  />
  <Filter />
  <view class="good-box">
    <product-list
      ref="productListRef"
      :loading="loading"
      :no-more="!hasMore"
      :min-height="`100%`"
      class="product-list"
      current-layout="card"
    />
  </view>
</template>
<script lang="ts" setup>
import { reactive, ref } from "vue";
import Filter from "@/components/ui/filter/index.vue";
import ProductList from "@/components/ui/product-list/index.vue";
import SelectionApi from "@/apis/shopmall/selection";
import { onMounted } from "vue";
const keyword = ref("");
const goSearch = () => {
  uni.navigateTo({
    url: `/pages/selection/search/index`
  });
};
const hasMore = ref(true);
const loading = ref(false);
const firstLoading = ref(true);
const params = reactive({
  page: 1,
  limit: 10
});
onMounted(async () => {
  await getProductList(true);
});
const productListRef = ref<InstanceType<typeof ProductList>>();
const getProductList = async (reset = false) => {
  if (reset) {
    uni.pageScrollTo({ scrollTop: 0 });
    params.page = 1;
    productListRef.value?.clear();
    firstLoading.value = true;
  } else {
    if (!hasMore.value) {
      return;
    }
    params.page++;
  }

  loading.value = true;

  const res = await SelectionApi.feed(params.page, params.limit, {
    keyword: keyword.value
  });
  productListRef.value?.addItems(res.rows);
  hasMore.value = res.hasMore;
  loading.value = false;
  firstLoading.value = false;
};
</script>
<style lang="scss" scoped>
.good-box {
  background-color: #f2f2f2;
}
</style>
