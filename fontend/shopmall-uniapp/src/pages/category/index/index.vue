<template>
  <view class="content">
    <view class="header-content">
      <x-header bounding-rect :header-style="headerStyle">
        <view class="search-wrapper" @click="handleSearch">
          <view class="placeholder-container">
            <text>请输入搜索商品信息</text>
          </view>
        </view>
      </x-header>
      <image-tabs
        v-model="bigCatId"
        :list="catLists"
        item-width="150"
        :gutter="12"
        @change="mainTabChange"
    /></view>
    <view class="sidebar-content">
      <scroll-view
        class="left"
        scroll-x="false"
        scroll-y="true"
        :style="{ height: 'calc(100vh -' + boundingRect.height + 'px)' }"
        upper-threshold="50"
        lower-threshold="50"
        scroll-top="0"
        scroll-left="0"
        scroll-into-view=""
        scroll-with-animation="false"
        enable-back-to-top="false"
        bindscrolltoupper=""
        bindscrolltolower=""
        bindscroll=""
      >
        <sidebar v-model="subCatActive" @change="change">
          <sidebar-item
            v-for="item in subItems"
            :key="item.id"
            :title="item.text"
          />
        </sidebar>
      </scroll-view>
      <view class="sub-main">
        <view class="good_box">
          <product-list
            ref="productListRef"
            :loading="loading"
            :no-more="!hasMore"
            :min-height="`100%`"
            class="product-list"
          />
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import Sidebar from "@/components/ui/sidebar/index.vue";
import ImageTabs from "@/components/ui/image-tabs/index.vue";
import SidebarItem from "@/components/ui/sidebar-item/index.vue";
import XHeader from "@/components/ui/header/index.vue";
import ProductList from "@/components/ui/product-list/index.vue";
import ProductCatApi, { type ProductCat } from "@/apis/shopmall/productCat";
import SelectionApi from "@/apis/shopmall/selection";
import {
  onMounted,
  getCurrentInstance,
  ref,
  computed,
  watch,
  reactive,
  type CSSProperties,
  type Ref
} from "vue";
import { GetRect, useHeader } from "@/components/ui/hooks/user-head";
import { onPullDownRefresh, onReachBottom, onShow } from "@dcloudio/uni-app";

const bigCatId = ref(0);
const subCatActive: Ref<number> = ref(0);
interface CatItem {
  id: string;
  image: string;
  text: string;
}
const headerStyle: CSSProperties = {
  backgroundRepeat: "no-repeat",
  backgroundSize: "100vw 320rpx"
};

const { boundingRect } = useHeader();
function handleSearch() {
  uni.navigateTo({ url: "/pages/selection/search/index" });
}

const catLists: Ref<Array<CatItem>> = ref([
  { id: "1", image: "/static/images/cat/img-1.webp", text: "分类" }
]);
function change(index: number) {
  // current.value = index;
}
const subItems: Ref<Array<CatItem>> = ref([]);
function mainTabChange(value: { index: number; item: CatItem }) {
  const selected = catLists.value[value.index];
  subItems.value = [];
  subCatActive.value = -1;
  cats.value.forEach(cat => {
    if (cat.parentId == selected.id) {
      subItems.value.push({
        id: cat.id,
        image: cat.image || "",
        text: cat.name
      });
    }
  });
  if (subItems.value.length > 0) subCatActive.value = 0;
}
const cats: Ref<Array<ProductCat>> = ref();
onMounted(async () => {
  cats.value = await ProductCatApi.getCats();
  catLists.value = [];
  cats.value.forEach(cat => {
    if (cat.isParent) {
      catLists.value.push({
        id: cat.id,
        image: cat.image || "/static/images/cat/img-1.webp",
        text: cat.name
      });
    }
  });
  mainTabChange({ index: 0, item: catLists.value[0] });
  await getProductList(true);
});
onShow(async () => {
  const haedRect = (await GetRect(
    getCurrentInstance(),
    ".header-content"
  )) as UniApp.NodeInfo;
  boundingRect.value.height = haedRect.height;
  boundingRect.value.width = haedRect.width;
  boundingRect.value.top = haedRect.top;
  boundingRect.value.bottom = haedRect.bottom;
});
const hasMore = ref(true);
const loading = ref(false);
const firstLoading = ref(true);
const params = reactive({
  page: 1,
  limit: 10
});
const productListRef = ref<InstanceType<typeof ProductList>>();
const queryCatId = computed(() => {
  if (subCatActive.value >= 0 && subItems.value.length >= 0) {
    return subItems.value[subCatActive.value]?.id;
  } else {
    return catLists.value[bigCatId.value]?.id;
  }
});
watch(
  () => queryCatId.value,
  () => {
    getProductList(true);
  }
);
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
    catId: queryCatId.value
  });
  productListRef.value?.addItems(res.rows);
  hasMore.value = res.hasMore;
  loading.value = false;
  firstLoading.value = false;
};
onReachBottom(() => {
  getProductList();
});
onPullDownRefresh(() => {
  uni.stopPullDownRefresh();
  getProductList(true);
});
</script>

<style lang="scss">
.cat-card-item {
  display: inline-block;
  width: 110rpx;
  text-align: center;
  margin-left: 10rpx;
}
.sidebar-content {
  display: flex;
  ::-webkit-scrollbar {
    width: 0;
    height: 0;
    color: transparent;
  }
  .left {
    flex: 0;
  }

  .sub-main {
    flex: 1;
  }
}
</style>
<style lang="scss" scoped>
.header-content {
  position: sticky;
  z-index: 3;
  top: 0;
  left: 0;
  right: 0;
  background-image: url("/static/images/bg/category-bg.svg");
  background-size: 100% auto;
  background-color: #ffffff;
  background-position: center top;
}
.search-wrapper {
  margin-left: 12px;
  width: 520rpx;
  padding: 6px 12px;
  box-sizing: border-box;
  border-radius: 100rpx;
  background: #fff;
  display: flex;
  align-items: center;
  .placeholder-container {
    overflow: hidden;
    height: 20px;
    position: relative;
  }
}
.good_box {
  padding: 10rpx;
  background-color: #fff;
  position: relative;
  width: calc(100% - 6px);
  .good_title {
    font-size: 26rpx;
    margin-top: 5px;
    color: $u-main-color;
  }
  .good_title-xl {
    font-size: 28rpx;
    margin-top: 5px;
    color: $u-main-color;
  }
  .good-tag-hot {
    display: flex;
    margin-top: 5px;
    position: absolute;
    top: 15rpx;
    left: 15rpx;
    background-color: $u-type-error;
    color: #ffffff;
    display: flex;
    align-items: center;
    padding: 4rpx 14rpx;
    border-radius: 50rpx;
    font-size: 20rpx;
    line-height: 1;
  }
  .good-tag-recommend {
    display: flex;
    margin-top: 5px;
    position: absolute;
    top: 15rpx;
    right: 15rpx;
    background-color: $u-type-primary;
    color: #ffffff;
    margin-left: 10px;
    border-radius: 50rpx;
    line-height: 1;
    padding: 4rpx 14rpx;
    display: flex;
    align-items: center;
    border-radius: 50rpx;
    font-size: 20rpx;
  }
  .good-tag-recommend2 {
    display: flex;
    margin-top: 5px;
    position: absolute;
    bottom: 15rpx;
    left: 15rpx;
    background-color: $u-type-primary;
    color: #ffffff;
    border-radius: 50rpx;
    line-height: 1;
    padding: 4rpx 14rpx;
    display: flex;
    align-items: center;
    border-radius: 50rpx;
    font-size: 20rpx;
  }
  .good-price {
    font-size: 30rpx;
    color: $u-type-error;
    margin-top: 5px;
  }
}
</style>
