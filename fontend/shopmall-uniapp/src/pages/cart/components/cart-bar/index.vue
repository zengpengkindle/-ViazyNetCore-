<template>
  <view class="cart-bar__placeholder" wx:if="{{fixed}}" />
  <view
    class="cart-bar flex flex-v-center"
    :class="fixed ? 'cart-bar--fixed' : ''"
    :style="{
      bottom: fixed
        ? 'calc(' + bottomHeight + 'rpx + env(safe-area-inset-bottom))'
        : ''
    }"
  >
    <t-icon
      size="40rpx"
      :color="isAllSelected ? '#FA4126' : '#BBBBBB'"
      :name="isAllSelected ? 'check-circle-filled' : 'circle'"
      class="cart-bar__check"
      @click="handleSelectAll"
    />
    <text>全选</text>
    <view class="cart-bar__total flex1">
      <view>
        <text class="cart-bar__total--bold text-padding-right">总计</text>
        <price
          :price="totalAmount"
          :fill="false"
          decimalSmaller
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
import { computed } from "vue";
export interface CartBarProps {
  isAllSelected: boolean;
  totalAmount: number;
  totalGoodsNum: number;
  totalDiscountAmount: number;
  bottomHeight: number;
  fixed: boolean;
}
const props = withDefaults(defineProps<CartBarProps>(), { bottomHeight: 100 });
const isDisabled = computed(() => props.totalGoodsNum == 0);
const emits = defineEmits([
  "update:isAllSelected",
  "handleSelectAll",
  "handleToSettle"
]);
const handleSelectAll = () => {
  emits("update:isAllSelected", !props.isAllSelected);
  emits("handleSelectAll");
};
const handleToSettle = () => {
  if (isDisabled.value) return;
  emits("handleToSettle");
};
</script>
