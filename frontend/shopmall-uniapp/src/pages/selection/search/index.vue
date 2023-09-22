<template>
  <view class="search-page">
    <u-search
      v-model="keyword"
      placeholder="请输入商品信息"
      :animation="true"
      @search="goSearch"
      @common="goSearch"
    />
    <view class="search-wrap">
      <view class="history-wrap">
        <view class="search-header">
          <text class="search-title">历史搜索</text>
          <text class="search-clear" @click="handleClearHistory">清除</text>
        </view>
        <view class="search-content">
          <view
            v-for="(item, index) in historyWords"
            :key="index"
            class="search-item"
            hover-class="hover-history-item"
            bindlongpress="deleteCurr"
            @click="handleHistoryTap(item)"
          >
            {{ item }}
          </view>
        </view>
      </view>
      <view class="popular-wrap">
        <view class="search-header">
          <text class="search-title">热门搜索</text>
        </view>
        <view class="search-content">
          <view
            v-for="(item, index) in popularWords"
            :key="index"
            class="search-item"
            hover-class="hover-history-item"
            @click="handleHistoryTap(item)"
          >
            {{ item }}
          </view>
        </view>
      </view>
    </view>
    <u-modal
      v-model="show"
      content="确认清空所有历史记录"
      show-cancel-button
      :zoom="false"
      @confirm="confirmClearHistory"
    />
  </view>
</template>
<script lang="ts" setup>
import { stringify } from "qs";
import { ref } from "vue";

const show = ref(false);
const popularWords = ref([]);
const historyWords = ref([]);
const handleClearHistory = () => {
  show.value = true;
};
const handleHistoryTap = (keyword: string) => {
  console.log(keyword);
};
const keyword = ref("");
const confirmClearHistory = () => {};
const goSearch = () => {
  const queries = {
    keyword: keyword.value
  };
  uni.navigateTo({
    url: `/pages/selection/result/index?${stringify(queries)}`
  });
};
</script>
<style lang="scss" scoped>
.search-page {
  box-sizing: border-box;
  width: 100vw;
  height: 100vh;
  padding: 0 30rpx;
  background-color: #f8f8f8;

  .search-wrap {
    margin-top: 44rpx;
  }

  .history-wrap {
    margin-bottom: 20px;
  }

  .search-header {
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
  }

  .search-title {
    font-size: 30rpx;
    font-family: PingFangSC-Semibold, PingFang SC;
    font-weight: 600;
    color: rgba(51, 51, 51, 1);
    line-height: 42rpx;
  }

  .search-clear {
    font-size: 24rpx;
    font-family: PingFang SC;
    line-height: 32rpx;
    color: #999999;
    font-weight: normal;
  }

  .search-content {
    overflow: hidden;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: flex-start;
    margin-top: 24rpx;
  }

  .search-item {
    color: #333333;
    font-size: 24rpx;
    line-height: 32rpx;
    font-weight: normal;
    margin-right: 24rpx;
    margin-bottom: 24rpx;
    background: #f5f5f5;
    border-radius: 38rpx;
    padding: 12rpx 24rpx;
  }

  .hover-history-item {
    position: relative;
    top: 3rpx;
    left: 3rpx;
    box-shadow: 0px 0px 8px rgba(0, 0, 0, 0.1) inset;
  }
}
</style>
