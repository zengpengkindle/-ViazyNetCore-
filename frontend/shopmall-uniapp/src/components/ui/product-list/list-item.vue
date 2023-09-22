<template>
  <view class="list-item" @click="itemClick">
    <u-row gutter="5">
      <u-col span="4">
        <u-image
          :src="item.image"
          width="160rpx"
          height="160rpx"
          :duration="100"
          border-radius="10rpx"
        />
      </u-col>
      <u-col span="8">
        <view class="right-wrapper">
          <view class="title-wrapper multi-ellipsis--l2">
            {{ item.title }}
          </view>
          <view class="bottom-row">
            <view class="price-wrapper">
              <price
                :price="item.price"
                symbol="¥"
                :bold="true"
                decimal-smaller
                type="lighter"
              />
              <price :price="item.price" symbol="¥" type="del" />
            </view>
          </view>
        </view>
      </u-col>
    </u-row>
  </view>
</template>

<script lang="ts" setup>
import type { SelectionFeedListDto } from "@/apis/shopmall/selection";
import { stringify } from "qs";
import type { PropType } from "vue";

type Jumper = (item: SelectionFeedListDto) => void;

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
  },
  customJumper: {
    type: Function as PropType<Jumper>,
    default: null
  }
});

const itemClick = () => {
  if (props.customJumper) {
    props.customJumper(props.item);
  } else {
    const queries = {
      id: props.item.id
    };
    uni.navigateTo({
      url: `/pages/selection/detail/index?${stringify(queries)}`
    });
  }
};
</script>

<style lang="scss" scoped>
.list-item {
  background-color: #fff;
  padding: 16rpx 0 16rpx 16rpx;
  border-radius: 16rpx;
  position: relative;

  .right-wrapper {
    padding-right: 24rpx;
    box-sizing: border-box;
    margin-left: 16rpx;
    display: flex;
    flex-direction: column;
    .title-wrapper {
      font-size: 24rpx;
      line-height: 32rpx;
      color: #2f3437;
      .tag {
        width: 40rpx;
        height: 24rpx;
        display: inline-block;
        vertical-align: text-bottom;
        margin-bottom: 4rpx;
      }
      .tag + .tag {
        margin-left: 4rpx;
      }
    }
    .colonel-wrapper {
      margin-top: 16rpx;
      display: flex;
      align-items: center;
      .avatar {
        width: 32rpx;
        height: 32rpx;
        border-radius: 50%;
      }
      .nickname {
        flex: 1;
        margin-left: 8rpx;
        margin-right: 16rpx;
        font-weight: 400;
        font-size: 20rpx;
        line-height: 32rpx;
        color: #869198;
      }
    }
    .benefit-wrapper {
      margin-top: 16rpx;
      display: flex;
      align-items: center;
      .high-rate-label,
      .earn-wrapper {
        font-size: 20rpx;
        line-height: 32rpx;
        height: 40rpx;
        padding: 0 8rpx;
        box-sizing: border-box;
        display: flex;
        align-items: center;
      }
      .high-rate-label {
        color: #fff;
        box-sizing: border-box;
        background-color: #de3f4f;
        border-radius: 4rpx 0 0 4rpx;
        .rate {
          margin-left: 4rpx;
          font-weight: 600;
        }
      }
      .earn-wrapper {
        color: #de3f4f;
        border: 2rpx solid #de3f4f;
        border-radius: 0 4rpx 4rpx 0;
        .unit,
        .amount {
          font-weight: 700;
        }
        .amount {
          font-family: D-DIN;
        }
      }
    }
    .bottom-row {
      margin-top: auto;
      margin-bottom: 4rpx;
      display: flex;
      align-items: center;
      justify-content: space-between;
      .price-wrapper {
        display: inline-block;
        align-items: baseline;
        color: #de3f4f;
        .unit {
          font-weight: 600;
          font-size: 24rpx;
          line-height: 32rpx;
        }
        .integer-part {
          margin-left: 4rpx;
          font-family: D-DIN;
          font-weight: 700;
          font-size: 44rpx;
          line-height: 48rpx;
          color: #de3f4f;
        }
        .decimal-part {
          margin-left: 4rpx;
          font-family: D-DIN;
          font-weight: 700;
          font-size: 32rpx;
          line-height: 48rpx;
          color: #de3f4f;
        }
      }
      .sales {
        font-size: 20rpx;
        line-height: 32rpx;
        color: #869198;
      }
    }
  }
}
</style>
