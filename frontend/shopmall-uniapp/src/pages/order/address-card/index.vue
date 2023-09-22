<template>
  <view class="address-card wr-class">
    <u-cell-item
      v-if="selectAddress && selectAddress.address"
      right-icon="arrow-right"
      @click="onAddressTap"
    >
      <template #title>
        <view class="order-address">
          <u-icon name="map" color="#BBBBBB" size="32rpx" />
          <view class="address-content">
            <view class="title">
              <view v-if="selectAddress.postalCode" class="address-tag">
                {{ selectAddress.postalCode }}
              </view>
              {{ selectAddress.province }} {{ selectAddress.city }}
              {{ selectAddress.county }}
            </view>
            <view class="detail">{{ selectAddress.address }}</view>
            <view class="info">
              {{ selectAddress.name }} {{ hidePhoneNum(selectAddress.tel) }}
            </view>
          </view>
        </view>
      </template>
    </u-cell-item>
    <u-cell-item v-else hover title="添加收货地址" @click="onAddTap">
      <template #icon>
        <u-icon name="plus-circle" color="#BBBBBB" size="32rpx" />
      </template>
      <template #title>
        <view>添加收货地址</view>
      </template>
    </u-cell-item>
    <view class="top-line" />
  </view>
</template>
<script lang="ts" setup>
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
  margin: 0rpx 0rpx 24rpx;

  .wr-cell__title {
    color: #999;
    margin-left: 6rpx;
  }
  .order-address {
    display: flex;
    width: 100%;
    .address-content {
      flex: 1;
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
    .title .address-tag {
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
    }
    .info {
      height: 32rpx;
      font-size: 28rpx;
      font-weight: normal;
      color: #333333;
      line-height: 32rpx;
    }
  }
  .address__right {
    align-self: center;
  }

  .top-line {
    width: 100%;
    height: 6rpx;
    background-color: #fff;
    background-image: "/static/images/common/add-res.png";
    background-repeat: repeat-x;
    display: block;
  }
}
</style>
