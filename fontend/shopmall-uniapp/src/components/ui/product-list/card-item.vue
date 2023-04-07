<template>
  <view class="card-item" @click="itemClick">
    <u-image
      :src="item.image"
      width="340rpx"
      height="340rpx"
      :duration="100"
      border-radius="16rpx 16rpx 0 0"
      class="cover"
    />

    <view class="info-wrapper">
      <view class="title-wrapper">
        <text class="title multi-ellipsis--l2">
          {{ item.title }}
        </text>
      </view>

      <view class="money-row">
        <view class="price-wrapper">
          <price :price="item.price" symbol="¥" :bold="true" decimal-smaller />
        </view>
        <view class="sales">
          <price :price="item.price" symbol="¥" type="del" />
        </view>
      </view>
    </view>
  </view>
</template>

<script lang="ts" setup>
import type { PropType } from "vue";
import type { SelectionFeedListDto } from "@/apis/shopmall/selection";
import { stringify } from "qs";

const props = defineProps({
  item: {
    type: Object as PropType<SelectionFeedListDto>,
    required: true
  },
  salesLabel: {
    type: String,
    default: "销量"
  },
  index: {
    type: Number,
    default: 0
  },
  showIndex: {
    type: Boolean,
    default: false
  }
});

const itemClick = () => {
  const queries = {
    id: props.item.id
  };
  uni.navigateTo({
    url: `/pages/selection/detail/index?${stringify(queries)}`
  });
};
</script>

<style lang="scss" scoped>
.card-item {
  width: 340rpx;
  border-radius: 16rpx;
  background-color: #ffffff;
  position: relative;

  .rank-tag {
    position: absolute;
    z-index: 1;
    top: -2rpx;
    left: 16rpx;
    width: 40rpx;
    height: 50rpx;
    display: flex;
    align-items: center;
    justify-content: center;
    &.top-3 {
      top: -8rpx;
    }
    .bg {
      width: 40rpx;
      height: 50rpx;
      position: absolute;
      left: 0;
      top: 0;
      z-index: -1;
    }
    .rank-num {
      margin-top: -8rpx;
      font-weight: 600;
      font-size: 24rpx;
      line-height: 32rpx;
      color: #ffffff;
      text-shadow: 0px 1px 1px rgba(108, 120, 154, 0.25);
    }
  }

  .info-wrapper {
    padding: 8rpx 16rpx 16rpx;
    .title-wrapper {
      display: flex;
      align-items: center;
      .tag {
        width: 40rpx;
        height: 24rpx;
      }
      .tag + .tag {
        margin-left: 4rpx;
      }
      .title {
        margin-left: 8rpx;
        flex: 1;
        font-size: 24rpx;
        line-height: 36rpx;
        color: #2f3437;
        display: -webkit-box;
        overflow: hidden;
        text-overflow: ellipsis;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
      }
    }
    .money-row {
      margin-top: 16rpx;
      display: flex;
      align-items: center;
      color: $u-type-error;
      .sales {
        margin-left: 8rpx;
        color: #869198;
      }
    }
    .bottom-wrapper {
      margin-top: 16rpx;
      display: flex;
      align-items: center;
      justify-content: space-between;
      .benefit-wrapper {
        display: flex;
        align-items: center;
        .high-rate-label,
        .earn-wrapper {
          height: 40rpx;
          padding: 0 8rpx;
          box-sizing: border-box;
          display: flex;
          align-items: center;
        }
        .high-rate-label {
          color: #fff;
          background-color: #de3f4f;
          border-radius: 4rpx 0 0 4rpx;
          .label,
          .rate {
            font-size: 20rpx;
            line-height: 32rpx;
          }
          .rate {
            margin-left: 4rpx;
            font-weight: 600;
          }
        }
        .earn-wrapper {
          color: #de3f4f;
          border: 2rpx solid #de3f4f;
          border-radius: 0 4rpx 4rpx 0;
          .text,
          .unit {
            font-size: 20rpx;
            line-height: 32rpx;
          }
          .unit,
          .amount {
            font-weight: 700;
            font-size: 20rpx;
            line-height: 20rpx;
            font-family: D-DIN;
          }
        }
      }
    }
  }
}
</style>
