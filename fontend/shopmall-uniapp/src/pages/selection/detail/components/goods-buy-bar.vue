<template>
  <view v-if="soldout || !isStock" class="flex soldout flex-center wr-sold-out">
    {{ soldout ? "商品已下架" : "商品已售馨" }}
  </view>
  <view class="footer-cont flex flex-between wr-class">
    <view
      v-if="jumpArray?.length > 0"
      class="flex flex-between bottom-operate-left"
    >
      <view
        v-for="(item, index) in jumpArray"
        :key="index"
        class="icon-warp operate-wrap"
        @click="toNav(item)"
      >
        <view>
          <text v-if="shopCartNum > 0 && item.showCartNum" class="tag-cart-num">
            {{ shopCartNum > 99 ? "99+" : shopCartNum }}
          </text>
          <u-icon prefix="wr" :name="item.iconName" size="40rpx" />
          <view class="operate-text">{{ item.title }}</view>
        </view>
      </view>
    </view>
    <block v-if="buttonType === 1">
      <view class="flex buy-buttons">
        <view
          :class="soldout || !isStock ? 'bar-addCart-disabled' : ''"
          class="bar-separately"
          @click="toAddCart"
        >
          加入购物车
        </view>
        <view
          :class="soldout || !isStock ? 'bar-buyNow-disabled' : ''"
          class="bar-buy"
          @click="toBuyNow"
        >
          立即购买
        </view>
      </view>
    </block>
    <block v-if="isSlotButton">
      <slot name="buyButton" />
    </block>
  </view>
</template>
<script lang="ts" setup>
import { useProductSpec } from "./specsHooks";
export interface BuyBarProps {
  soldout: boolean;
  isStock: boolean;
  isSlotButton: boolean;
  buttonType: number;
  jumpArray?: Array<JumpIcon>;
}
export interface JumpIcon {
  url: string;
  showCartNum: boolean;
  iconName: string;
  title: string;
}
const { buyNum: shopCartNum, showSpecPopup, specType } = useProductSpec();
withDefaults(defineProps<BuyBarProps>(), {
  buttonType: 1,
  isSlotButton: false
});

const toNav = (item: JumpIcon) => {
  if (item.url == "home") {
    uni.switchTab({ url: "/pages/index/custom/index" });
  } else if (item.url == "cart") {
    uni.switchTab({ url: "/pages/cart/index" });
  } else uni.navigateTo({ url: item.url });
};
const toAddCart = () => {
  specType.value = "addCart";
  showSpecPopup.value = true;
};
const toBuyNow = () => {
  showSpecPopup.value = true;
};
</script>
<style lang="scss" scoped>
.footer-cont {
  background-color: #fff;
  padding: 16rpx;
}

.icon-warp {
  width: 110rpx;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
}

.operate-wrap {
  position: relative;
}

.bottom-operate-left {
  width: 100%;
}

.bottom-operate-left .icon-warp {
  width: 50%;
}

.tag-cart-num {
  display: inline-block;
  position: absolute;
  left: 50rpx;
  right: auto;
  top: 6rpx;
  color: #fff;
  line-height: 24rpx;
  text-align: center;
  z-index: 99;
  white-space: nowrap;
  min-width: 28rpx;
  border-radius: 14rpx;
  background-color: #fa550f !important;
  font-size: 20rpx;
  font-weight: 400;
  padding: 2rpx 6rpx;
}

.operate-text {
  color: #666;
  font-size: 20rpx;
}

.soldout {
  height: 80rpx;
  background: rgba(170, 170, 170, 1);
  width: 100%;
  color: #fff;
}

.addCart-disabled,
.bar-addCart-disabled {
  background: rgba(221, 221, 221, 1) !important;
  color: #fff !important;
  font-size: 28rpx;
}

.buyNow-disabled,
.bar-buyNow-disabled {
  background: rgba(198, 198, 198, 1) !important;
  color: #fff !important;
  font-size: 28rpx;
}

.bar-separately,
.bar-buy {
  width: 254rpx;
  height: 80rpx;
  color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
}

.bar-separately {
  background: $u-type-primary-disabled;
  color: $u-type-primary;
  border-radius: 40rpx 0 0 40rpx;
}

.bar-buy {
  background-color: $u-type-primary;
  border-radius: 0rpx 40rpx 40rpx 0rpx;
}

.flex {
  display: flex;
  display: -webkit-flex;
}

.flex-center {
  justify-content: center;
  -webkit-justify-content: center;
  align-items: center;
  -webkit-align-items: center;
}

.flex-between {
  justify-content: space-between;
  -webkit-justify-content: space-between;
}
</style>
