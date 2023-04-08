<template>
  <view class="address-container">
    <view v-if="addressList.length > 0" class="address-list">
      <block v-for="(address, index) in addressList" :key="address.id">
        <address-item
          :is-draw-line="index + 1 !== addressList.length"
          :extra-space="extraSpace"
          class-prefix="ym"
          :address="address"
          @on-select="selectHandle"
          @on-delete="deleteAddressHandle"
          @on-edit="editAddressHandle"
        />
      </block>
    </view>
    <view v-else class="no-address">
      <u-empty mode="address" text="暂无收货地址，赶快添加吧" />
    </view>
    <view class="bottom-fixed">
      <view class="btn-wrap">
        <view
          class="address-btn"
          :class="addressList.length >= 20 ? 'btn-default' : ''"
          @click="createHandle"
        >
          <u-icon name="plus" size="38rpx" color="#fff" class="custom-class" />
          <text>新建收货地址</text>
        </view>
      </view>
      <view v-if="addressList.length >= 20" class="footer">
        最多支持添加20个收货地址
      </view>
    </view>
  </view>
</template>
<script lang="ts" setup>
import { useAddress } from "./hooks";
import AddressItem from "../components/address-item/index.vue";
import type { AddressModel } from "@/apis/shopmall/address";
import { ref } from "vue";
import { onLoad, onShow } from "@dcloudio/uni-app";
const { addressList, refreshAddress, SelectAddress } = useAddress();
const extraSpace = ref(false);
const selectHandle = (item: AddressModel) => {
  if (mode.value) {
    hasSelect.value = true;
    SelectAddress(item);
    uni.navigateBack({ delta: 1 });
  }
};
const deleteAddressHandle = () => {};
const editAddressHandle = () => {};
const createHandle = () => {
  uni.navigateTo({ url: "/pages/member/address/edit" });
};
onShow(async () => {
  await refreshAddress();
});
const mode = ref(false);
const selectId = ref("");
const OrderSure = ref(false);
const hasSelect = ref(false);
onLoad(query => {
  const { selectMode = false, isOrderSure = false, id = "" } = query;
  mode.value = selectMode;
  selectId.value = id;
  OrderSure.value = isOrderSure;
});
</script>
<style lang="scss" scoped>
.address-container {
  display: flex;
  flex-direction: column;
  align-items: stretch;
  padding-bottom: calc(env(safe-area-inset-bottom) + 172rpx);

  .address-list {
    font-size: 24rpx;
    background-color: #ffffff;
    -webkit-overflow-scrolling: touch;
  }
  .no-address {
    width: 750rpx;
    padding-top: 30vh;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
  }
  .no-address__icon {
    width: 224rpx;
    height: 224rpx;
  }
  .no-address__text {
    font-size: 28rpx;
    line-height: 40rpx;
    color: #999999;
    margin-top: 24rpx;
  }
  .bottom-fixed {
    border-top: 1rpx solid #e5e5e5;
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    z-index: 1000;
    background: #fff;
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
    padding: 12rpx 32rpx calc(env(safe-area-inset-bottom) + 12rpx) 32rpx;
  }
  .btn-wrap {
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-size: 32rpx;
    .location-btn {
      width: 332rpx;
      height: 88rpx;
      display: flex;
      justify-content: center;
      align-items: center;
      background-color: #ffffff;
      color: #333;
      position: relative;
    }
    .location-btn::after {
      content: "";
      position: absolute; /* 把父视图设置为relative，方便定位*/
      top: 0;
      left: 0;
      width: 200%;
      height: 200%;
      transform: scale(0.5);
      transform-origin: 0 0;
      box-sizing: border-box;
      border-radius: 88rpx;
      border: #dddddd 2rpx solid;
    }
    .address-btn {
      flex: 1;
      height: 68rpx;
      display: flex;
      justify-content: center;
      align-items: center;
      background-color: $u-type-primary;
      border-radius: 34rpx;
      color: #fff;
    }
    .btn-default {
      background: #c6c6c6;
    }
  }

  .bottom-fixed .footer {
    margin-top: 10rpx;
    display: inline-block;
    width: 100%;
    text-align: center;
    font-size: 24rpx;
    font-weight: 400;
    color: #ff2525;
    line-height: 60rpx;
    height: 60rpx;
  }
  .message {
    margin-top: 48rpx;
  }
  .custom-class {
    margin-right: 12rpx;
    font-weight: normal;
  }
}
</style>
