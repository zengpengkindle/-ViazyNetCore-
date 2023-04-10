<template>
  <view class="address-item-wrapper item-wrapper-class">
    <u-swipe-action class="swipe-out" :options="option" @click="click">
      <view
        class="address"
        :class="isDrawLine ? 'draw-line' : ''"
        @click="onSelect"
      >
        <view v-if="extraSpace" class="address-left">
          <u-icon
            v-if="address.checked"
            name="check"
            color="#FA4126"
            size="46rpx"
          />
        </view>
        <view class="address-content">
          <view class="title title-class">
            <text class="text-style">{{ address.name }}</text>
            <text>{{ address.tel || "" }}</text>
          </view>
          <view class="label-adds">
            <text class="adds address-info-class">
              <text
                v-if="address.isDefault"
                class="tag tag-default default-tag-class"
                >默认</text
              >
              <text
                v-if="address.postalCode"
                class="tag tag-primary normal-tag-class"
                >{{ address.postalCode }}</text
              >
              <text class="address-text">{{ address.address }}</text>
            </text>
          </view>
        </view>
        <view class="address-edit" @click="onEdit">
          <u-icon name="edit-pen" size="36rpx" color="#BBBBBB" />
        </view>
      </view>
    </u-swipe-action>
  </view>
</template>
<script lang="ts" setup>
import type { AddressModel } from "@/apis/shopmall/address";
export interface AddressProps {
  address: AddressModel;
  isDrawLine: boolean;
  extraSpace: boolean;
}
const props = defineProps<AddressProps>();
const emits = defineEmits(["onEdit", "onDelete", "onSelect"]);
const option = [
  {
    text: "删除",
    style: {
      backgroundColor: "#dd524d"
    }
  }
];
const click = () => {
  emits("onDelete", props.address.id);
};
const onSelect = () => {
  emits("onSelect", props.address);
};
const onEdit = () => {
  emits("onEdit", props.address.id);
};
</script>
<style lang="scss" scoped>
.address-item-wrapper .swipe-out .wr-swiper-cell {
  margin-top: 20rpx;
}
.address-item-wrapper .swipe-out .swipe-right-del {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 144rpx;
  height: 100%;
  background-color: #fa4126;
  color: #fff;
  font-size: 28rpx;
  line-height: 40rpx;
}
.address-item-wrapper .draw-line {
  position: relative;
}
.address-item-wrapper .draw-line::after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 32rpx;
  width: 200%;
  height: 2rpx;
  transform: scale(0.5);
  transform-origin: 0 0;
  box-sizing: border-box;
  border-bottom: #e5e5e5 2rpx solid;
}
.address-item-wrapper .address {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20rpx;
  background-color: #fff;

  .address-edit {
    padding: 20rpx 0 20rpx 46rpx;
  }
  .address-left {
    width: 80rpx;
    display: flex;
    justify-content: center;
  }
  .address-content {
    display: flex;
    flex-direction: column;
    flex: 1;

    .title {
      font-size: 28rpx;
      line-height: 42rpx;
      margin-bottom: 16rpx;
      color: #333333;
      font-weight: bold;
      display: flex;
    }
    .title .text-style {
      margin-right: 8rpx;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      max-width: 280rpx;
    }
    .label-adds {
      display: flex;
    }
    .label-adds .adds {
      display: -webkit-box;
      overflow: hidden;
      text-overflow: ellipsis;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 2;
      color: #999999;
    }
    .label-adds .tag {
      display: inline-block;
      padding: 0rpx 8rpx;
      min-width: 40rpx;
      height: 32rpx;
      border-radius: 18rpx;
      font-size: 20rpx;
      line-height: 32rpx;
      text-align: center;
      margin-right: 8rpx;
      vertical-align: text-top;
    }
    .label-adds .tag-default {
      background: #ffece9;
      color: #fa4126;
    }
    .label-adds .tag-primary {
      background: #f0f1ff;
      color: #5a66ff;
    }
    .label-adds .address-text {
      font-size: 24rpx;
      line-height: 36rpx;
      color: #999999;
    }
  }
}
</style>
