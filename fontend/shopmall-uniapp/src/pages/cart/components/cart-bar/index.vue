<template>
  <view class="cart-bar__placeholder" wx:if="{{fixed}}" />
  <view
    class="cart-bar flex flex-v-center"
    :class="fixed ? 'cart-bar--fixed' : ''"
    :style="{
      bottom: fixed ? bottomHeight + 'rpx' : ''
    }"
  >
    <u-checkbox v-model="allcheck" shape="circle" @change="handleSelectAll()">
      全选
    </u-checkbox>
    <view class="cart-bar__total flex1">
      <view>
        <text class="cart-bar__total--bold text-padding-right">总计</text>
        <price
          :price="totalAmount"
          :fill="false"
          decimal-smaller
          class="cart-bar__total--bold cart-bar__total--price"
        />
        <text class="cart-bar__total--normal">（不含运费）</text>
      </view>
      <view v-if="totalDiscountAmount">
        <text class="cart-bar__total--normal text-padding-right">已优惠</text>
        <price
          class="cart-bar__total--normal"
          :price="totalDiscountAmount"
          :fill="false"
        />
      </view>
    </view>
    <view
      class="account-btn"
      :class="!isDisabled ? '' : 'disabled-btn'"
      :hover-class="!isDisabled ? '' : 'hover-btn'"
      @click="handleToSettle"
    >
      去结算({{ totalGoodsNum }})
    </view>
  </view>
</template>
<script lang="ts" setup>
import { computed, ref, watch, nextTick } from "vue";
export interface CartBarProps {
  modelValue: boolean;
  totalAmount: number;
  totalGoodsNum: number;
  totalDiscountAmount: number;
  bottomHeight: number;
  fixed: boolean;
}
const props = withDefaults(defineProps<CartBarProps>(), {
  bottomHeight: 0,
  fixed: false
});

const allcheck = ref(false);
watch(
  () => props.modelValue,
  nv => {
    allcheck.value = nv;
  }
);

const isDisabled = computed(() => props.totalGoodsNum == 0);
const emits = defineEmits([
  "update:modelValue",
  "handleSelectAll",
  "handleToSettle"
]);
const handleSelectAll = () => {
  nextTick(() => {
    emits("update:modelValue", allcheck.value);
    emits("handleSelectAll");
  });
};
const handleToSettle = () => {
  if (isDisabled.value) return;
  emits("handleToSettle");
};
</script>
<style lang="scss" scoped>
.cart-bar__placeholder {
  height: 100rpx;
}
.flex {
  display: flex;
}
.flex-v-center {
  align-items: center;
}
.flex1 {
  flex: 1;
}
.algin-bottom {
  text-align: end;
}
.cart-bar--fixed {
  position: fixed;
  left: 0;
  right: 0;
  z-index: 99;
  bottom: 0;
}

.cart-bar {
  height: 112rpx;
  background-color: #fff;
  border-top: 1rpx solid #e5e5e5;
  padding: 16rpx 32rpx;
  box-sizing: border-box;
  font-size: 24rpx;
  line-height: 36rpx;
  color: #333;
}

.cart-bar .cart-bar__check {
  margin-right: 12rpx;
}

.cart-bar .cart-bar__total {
  margin-left: 24rpx;
}

.cart-bar .account-btn {
  width: 192rpx;
  height: 80rpx;
  border-radius: 40rpx;
  background-color: $uni-color-primary;
  font-size: 28rpx;
  font-weight: bold;
  line-height: 80rpx;
  color: #ffffff;
  text-align: center;
}
.cart-bar .disabled-btn {
  background-color: #cccccc !important;
}
.cart-bar .hover-btn {
  opacity: 0.5;
}

.cart-bar__total .cart-bar__total--bold {
  font-size: 28rpx;
  line-height: 40rpx;
  color: #333;
  font-weight: bold;
}
.cart-bar__total .cart-bar__total--normal {
  font-size: 24rpx;
  line-height: 32rpx;
  color: #999;
}

.cart-bar__total .cart-bar__total--price {
  color: #fa4126;
  font-weight: bold;
}

.text-padding-right {
  padding-right: 4rpx;
}
</style>
