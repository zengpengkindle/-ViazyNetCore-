<template>
  <view v-if="fixed" class="pay-bar__placeholder" />
  <view
    class="pay-bar flex flex-v-center"
    :class="fixed ? 'cart-bar--fixed' : ''"
    :style="{
      bottom: fixed ? bottomHeight + 'rpx' : ''
    }"
  >
    <view class="pay-bar__total flex1">
      <view>
        <text class="pay-bar__total--normal text-padding-right">
          共{{ totalGoodsNum }}件
        </text>
        <text class="pay-bar__total--bold text-padding-right">总计</text>
        <price
          :price="totalAmount"
          symbol="￥"
          decimal-smaller
          class="pay-bar__total--bold cart-bar__total--price"
        />
      </view>
      <view v-if="totalDiscountAmount">
        <text class="pay-bar__total--discount text-padding-right">已优惠</text>
        <price
          class="pay-bar__total--discount"
          symbol="￥"
          :price="totalDiscountAmount"
        />
      </view>
    </view>
    <view
      class="account-btn"
      :class="!isDisabled ? '' : 'disabled-btn'"
      :hover-class="!isDisabled ? '' : 'hover-btn'"
      @click="handleToPay"
    >
      去支付
    </view>
  </view>
</template>
<script lang="ts" setup>
import { computed } from "vue";
export interface PayBarProps {
  totalAmount: number;
  totalGoodsNum: number;
  totalDiscountAmount: number;
  bottomHeight: number;
  fixed: boolean;
}
const props = withDefaults(defineProps<PayBarProps>(), {
  bottomHeight: 0,
  fixed: true
});

const isDisabled = computed(() => props.totalGoodsNum == 0);
const emits = defineEmits(["handleToPay"]);
const handleToPay = () => {
  if (isDisabled.value) return;
  emits("handleToPay");
};
</script>
<style lang="scss" scoped>
.pay-bar__placeholder {
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

.pay-bar {
  height: 112rpx;
  background-color: #fff;
  border-top: 1rpx solid #e5e5e5;
  padding: 16rpx 32rpx;
  box-sizing: border-box;
  font-size: 24rpx;
  line-height: 36rpx;
  color: #333;

  .cart-bar__check {
    margin-right: 12rpx;
  }

  .pay-bar__total {
    margin-right: 24rpx;
    text-align: right;
  }

  .account-btn {
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
  .disabled-btn {
    background-color: #cccccc !important;
  }
  .hover-btn {
    opacity: 0.5;
  }
}

.pay-bar__total {
  .pay-bar__total--bold {
    font-size: 28rpx;
    line-height: 40rpx;
    color: #333;
    font-weight: bold;
  }
  .pay-bar__total--normal {
    font-size: 24rpx;
    line-height: 32rpx;
    color: #999;
  }
  .pay-bar__total--discount {
    font-size: 24rpx;
    line-height: 32rpx;
    color: $u-type-warning;
  }
  .cart-bar__total--price {
    color: #fa4126;
    font-weight: bold;
  }
}
.text-padding-right {
  padding-right: 4rpx;
}
</style>
