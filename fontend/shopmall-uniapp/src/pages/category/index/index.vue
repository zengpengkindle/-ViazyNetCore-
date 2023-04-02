<template>
  <view class="content">
    <view class="header-content">
      <x-header bounding-rect :header-style="headerStyle">
        <view class="search-wrapper" @click="handleSearch">
          <view class="placeholder-container" />
        </view>
      </x-header>
      <image-tabs
        v-model="catId"
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
        style="height: calc(100vh - 92px)"
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
        <view>ccccc {{ catId }} - {{ subCatActive }}</view>
        <view class="good_box">
          <u-row gutter="5">
            <u-col span="4">
              <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
              <u-lazy-load
                threshold="-150"
                border-radius="10"
                :image="productItem.image"
                :index="productItem.id"
              />
              <view v-if="productItem.isRecommend" class="good-tag-recommend2">
                推荐
              </view>
              <view v-if="productItem.isHot" class="good-tag-hot"> 热门 </view>
            </u-col>
            <u-col span="8">
              <view class="good_title-xl u-line-3 u-padding-10">
                {{ productItem.name }}
              </view>
              <view class="good-price u-padding-10">
                <price
                  :price="productItem.price"
                  symbol="¥"
                  :bold="true"
                  decimal-smaller
                  type="lighter"
                />
                <price :price="productItem.mktprice" symbol="¥" type="del" />
              </view>
            </u-col>
          </u-row>
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
import ProductCatApi, { type ProductCat } from "@/apis/shopmall/productCat";
import { onMounted, ref, type CSSProperties, type Ref } from "vue";
import { useHeader } from "@/components/ui/hooks/user-head";
const catId = ref(5);
const subCatActive: Ref<number> = ref(0);
interface CatItem {
  id: string;
  image: string;
  text: string;
}
interface ProductItem {
  id: string;
  image: string;
  name: string;
  isRecommend: boolean;
  isHot: boolean;
  price: number;
  mktprice: number;
}
const headerStyle: CSSProperties = {
  backgroundRepeat: "no-repeat",
  backgroundSize: "100vw 320rpx"
};

const { boundingRect } = useHeader();
function handleSearch() {}

const productItem: Ref<ProductItem> = ref({
  id: "pid_1",
  image: "/static/images/cat/img-1.webp",
  name: "商品名称[商品名称]",
  isRecommend: false,
  isHot: true,
  price: 15.99,
  mktprice: 29.99
});
const catLists: Ref<Array<CatItem>> = ref([
  { id: "1", image: "/static/images/cat/img-1.webp", text: "分类" },
]);
function change(index: number) {
  // current.value = index;
  console.log(index);
}
const subItems: Ref<Array<CatItem>> = ref([]);
function mainTabChange(value: { index: number; item: CatItem }) {
  const selected = catLists.value[value.index];
  console.log("item", value.item);
  subItems.value = [];
  subCatActive.value = -1;
  cats.value.forEach(cat => {
    if (cat.parentId == selected.id) {
      subItems.value.push({
        id: cat.id,
        image: cat.image || "/static/images/cat/img-1.webp",
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
  background-image: url("/static/images/bg/user-center-bg.svg");
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
  border-radius: 8px;
  margin: 3px;
  background-color: #ffffff;
  padding: 5px;
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
