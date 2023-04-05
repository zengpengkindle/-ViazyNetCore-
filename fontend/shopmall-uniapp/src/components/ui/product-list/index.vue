<template>
  <view
    class="product-list"
    :style="{
      minHeight
    }"
  >
    <view v-if="currentLayout === 'list'" class="list-wrapper">
      <list-item
        v-for="(item, index) of productList"
        :key="item.id"
        :item="item"
        :index="index"
        :show-index="showIndex"
        :sales-label="salesLabel"
        class="list-item"
      />
    </view>
    <view v-else-if="currentLayout === 'card'" class="card-wrapper">
      <view class="left-column">
        <card-item
          v-for="item of leftList"
          :key="item.id"
          :item="item"
          :index="item.index"
          :show-index="showIndex"
          :sales-label="salesLabel"
          class="card-item"
        />
      </view>
      <view class="right-column">
        <card-item
          v-for="item of rightList"
          :key="item.id"
          :item="item"
          :index="item.index"
          :show-index="showIndex"
          :sales-label="salesLabel"
          class="card-item"
        />
      </view>
    </view>

    <view v-if="!productList.length && !loading" class="empty-wrapper">
      <u-empty mode="search" />
    </view>

    <view v-else-if="loading" class="loading-wrapper"> 加载中... </view>

    <view v-else-if="noMore && productList.length" class="no-more">
      没有更多数据
    </view>
  </view>
</template>

<script lang="ts" setup>
import type { SelectionFeedListDto } from "@/apis/shopmall/selection";
import { ref } from "vue";
import CardItem from "./card-item.vue";
import ListItem from "./list-item.vue";

type IndexedProductItem = SelectionFeedListDto & {
  index: number;
};

defineProps({
  salesLabel: {
    type: String,
    default: "销量"
  },
  showIndex: {
    type: Boolean,
    default: false
  },
  firstLoading: {
    type: Boolean,
    default: false
  },
  empty: {
    type: Boolean,
    default: false
  },
  loading: {
    type: Boolean,
    default: true
  },
  noMore: {
    type: Boolean,
    default: false
  },
  minHeight: {
    type: String,
    default: ""
  },
  currentLayout: {
    type: String,
    default: "list"
  }
});

const productList = ref<SelectionFeedListDto[]>([]);
const { addItemsToWaterfall, clearWaterfall, leftList, rightList } =
  useWaterfallList();

const addItems = (items: SelectionFeedListDto[]) => {
  productList.value.push(...items);
  addItemsToWaterfall(items);
};
const clear = () => {
  productList.value = [];

  clearWaterfall();
};

function useWaterfallList() {
  const leftList = ref<IndexedProductItem[]>([]);
  const rightList = ref<IndexedProductItem[]>([]);

  let index = 0;
  const addItemsToWaterfall = (items: SelectionFeedListDto[]) => {
    if (leftList.value.length === 0 && rightList.value.length === 0) {
      // 无数据时直接将第一批数据顺序加入
      for (let i = 0, len = items.length; i < len; ++i) {
        (items[i] as IndexedProductItem).index = index++;
        if (i % 2) {
          // 显示热销卡片时从右列开始 不显示则从左列开始
          rightList.value.push(items[i] as IndexedProductItem);
        } else {
          leftList.value.push(items[i] as IndexedProductItem);
        }
      }
    } else {
      // TODO: 瀑布流两列添加项的逻辑
      for (let i = 0, len = items.length; i < len; ++i) {
        (items[i] as IndexedProductItem).index = index++;
        if (i % 2) {
          rightList.value.push(items[i] as IndexedProductItem);
        } else {
          leftList.value.push(items[i] as IndexedProductItem);
        }
      }
    }
  };
  const clearWaterfall = () => {
    leftList.value = [];
    rightList.value = [];
    index = 0;
  };

  return {
    leftList,
    rightList,
    addItemsToWaterfall,
    clearWaterfall
  };
}

defineExpose({
  addItems,
  clear
});
</script>

<style lang="scss" scoped>
.product-list {
  padding-top: 8rpx;
  padding-bottom: calc(16rpx + env(safe-area-inset-bottom));
  box-sizing: border-box;
  .list-wrapper {
    .list-item {
      display: block;
    }
    .list-item + .list-item {
      margin-top: 24rpx;
    }
    .first-loading-wrapper {
      .placeholder {
        display: block;
        width: 702rpx;
        height: 272rpx;
      }
      .placeholder + .placeholder {
        margin-top: 24rpx;
      }
    }
  }
  .card-wrapper {
    display: flex;
    justify-content: space-between;
    .left-column,
    .right-column {
      .card-item {
        display: block;
      }
      .card-item + .card-item {
        margin-top: 24rpx;
      }
    }
    .left-column .first-loading-wrapper {
      .placeholder-1 {
        display: block;
        width: 340rpx;
        height: 588rpx;
      }
      .placeholder-1 + .placeholder-1 {
        margin-top: 24rpx;
      }
    }
    .right-column .first-loading-wrapper {
      .placeholder-2 {
        display: block;
        width: 340rpx;
        height: 556rpx;
      }
      .placeholder-2 + .placeholder-2 {
        margin-top: 24rpx;
      }
    }
  }
  .empty-wrapper {
    display: flex;
    flex-direction: column;
    align-items: center;
    .empty-img {
      width: 200rpx;
      height: 200rpx;
    }
    .message {
      margin-top: 24rpx;
      font-size: 24rpx;
      line-height: 40rpx;
      color: #869198;
    }
  }
  .loading-wrapper,
  .no-more {
    margin-top: 24rpx;
    display: flex;
    justify-content: center;
    font-weight: 400;
    font-size: 24rpx;
    line-height: 40rpx;
    color: #869198;
  }
}
</style>
