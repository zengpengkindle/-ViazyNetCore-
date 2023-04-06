<template>
  <view>
    <view class="wr-class filter-wrap">
      <view class="filter-left-content">
        <view
          class="filter-item"
          :class="overall === 1 ? 'filter-active-item' : ''"
          @click="onOverallAction"
        >
          综合
        </view>
        <view class="filter-item" @click="handlePriseSort">
          <text :style="{ color: sorts !== '' ? color : '' }">价格</text>
          <view class="filter-price">
            <u-icon
              prefix="wr"
              name="arrow_drop_up"
              size="18rpx"
              :style="{ color: sorts === 'asc' ? color : '#bbb' }"
            />
            <u-icon
              prefix="wr"
              name="arrow_drop_down"
              size="18rpx"
              :style="{ color: sorts === 'asc' ? color : '#bbb' }"
            />
          </view>
        </view>
        <view class="filter-item" @click="openFilter">
          筛选
          <u-icon name="filter" prefix="wr" color="#333" size="32rpx" />
        </view>
      </view>
    </view>
  </view>
</template>
<script lang="ts" setup>
import { ref } from "vue";
const sorts = ref("asc");
const color = ref("#000");
const overall = ref(1);
const onOverallAction = () => {};
const handlePriseSort = () => {};
const openFilter = () => {};
</script>
<style lang="scss" scoped>
.filter-wrap {
  width: 100%;
  height: 88rpx;
  display: flex;
  justify-content: space-between;
  position: relative;
  background: #f8f8f8;
}

.filter-right-content {
  height: 100%;
  flex-basis: 100rpx;
  text-align: center;
  line-height: 88rpx;
}

.filter-left-content {
  height: 100%;
  display: flex;
  flex-grow: 2;
  flex-flow: row nowrap;
  justify-content: space-between;

  .filter-item {
    flex: 1;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 26rpx;
    line-height: 36rpx;
    font-weight: 400;
    color: rgba(51, 51, 51, 1);
    .filter-price {
      display: flex;
      flex-direction: column;
      margin-left: 6rpx;
      justify-content: space-between;
    }

    .wr-filter {
      margin-left: 8rpx;
    }
  }

  .filter-active-item {
    color: $u-type-primary;
  }
}
</style>
