<template>
  <view class="address-card wr-class">
    <cell-item
      v-if="selectAddress && selectAddress.address"
      right-icon="arrow-right"
      icon="map"
      @click="onAddressTap"
    >
      <template #title>
        <view class="order-address">
          <view class="address-content">
            <view class="detail">
              {{ selectAddress.name }} {{ hidePhoneNum(selectAddress.tel) }}
            </view>
            <view class="info">
              <view v-if="selectAddress.postalCode" class="address-tag">
                {{ selectAddress.postalCode }}
              </view>
              {{ selectAddress.address }}
            </view>
          </view>
        </view>
      </template>
    </cell-item>
    <cell-item v-else icon="plus-circle" @click="onAddTap">
      <template #title>
        <view>添加收货地址</view>
      </template>
    </cell-item>
    <view class="top-line" />
  </view>
</template>
<script lang="ts" setup>
import CellItem from "./cell-item.vue";
import { onShow } from "@dcloudio/uni-app";
import { useAddress } from "@/pages/member/address/hooks";
import { stringify } from "qs";
const { selectAddress } = useAddress();
const onAddressTap = () => {
  const queries = {
    id: selectAddress.value.id,
    selectMode: true,
    isOrderSure: true
  };
  uni.navigateTo({ url: `/pages/member/address/list?${stringify(queries)}` });
};
const onAddTap = () => {
  const queries = {
    selectMode: true
  };
  uni.navigateTo({ url: `/pages/member/address/list?${stringify(queries)}` });
};
onShow(async () => {
  // await refreshAddress();
  // console.log(selectAddress.value);
});

const hidePhoneNum = function (array) {
  if (!array) return;
  const mphone = array.substring(0, 3) + "****" + array.substring(7);
  return mphone;
};
</script>
<style lang="scss" scoped>
.address-card {
  background: #fff;

  .wr-cell__title {
    color: #999;
    margin-left: 6rpx;
  }
  .order-address {
    display: flex;
    width: 100%;
    .address-content {
      flex: 1;
      padding-left: 20rpx;
      box-sizing: border-box;
    }
    .title {
      display: flex;
      align-items: center;
      height: 32rpx;
      font-size: 24rpx;
      font-weight: normal;
      color: #999999;
      line-height: 32rpx;
    }
    .address-tag {
      width: 52rpx;
      height: 29rpx;
      border: 1rpx solid #0091ff;
      background-color: rgba(122, 167, 251, 0.1);
      text-align: center;
      line-height: 29rpx;
      border-radius: 8rpx;
      color: #0091ff;
      font-size: 20rpx;
      margin-right: 12rpx;
    }
    .detail {
      overflow: hidden;
      text-overflow: ellipsis;
      display: -webkit-box;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 2;
      font-size: 26rpx;
      font-weight: bold;
      color: #333333;
      line-height: 36rpx;
      margin: 8rpx 0;
      overflow: hidden;
      white-space: nowrap;
      text-overflow: ellipsis;
    }
    .info {
      height: 32rpx;
      font-size: 28rpx;
      font-weight: normal;
      color: #333333;
      line-height: 32rpx;
      overflow: hidden;
      white-space: nowrap;
      text-overflow: ellipsis;
    }
  }
  .address__right {
    align-self: center;
  }

  .top-line {
    width: 100%;
    height: 6rpx;
    background-color: #fff;
    background-image: url("/static/images/common/add-rs.png");
    background-repeat: repeat-x;
    display: block;
  }
}
</style>
