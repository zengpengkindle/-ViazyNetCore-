<template>
  <view class="cash">
    <view v-if="tradeSet?.totalMoney == 0">
      <u-empty mode="order" />
    </view>
    <view v-else>
      <view class="trade-header">
        <view class="trade-header-info">
          <view class="trade-header-title">订单待付款</view>
          <view class="trade-header-info-desc" />
        </view>
        <view class="header-icon" />
      </view>
      <view class="trade-main">
        <view class="container">
          <view v-for="trade in trades" :key="trade.shopId" class="trade-card">
            <view class="trade-card-header">
              <view class="trade-card-title">
                <u-icon name="shop" /> {{ trade.shopName }}
              </view>
            </view>
            <view class="trade-card-body">
              <view
                v-for="item in trade.items"
                :key="item.pId + item.skuId"
                class="trade-card-items"
              >
                <view class="trade-card-item goods-wrapper">
                  <u-image
                    :src="item.imgUrl"
                    class="goods-image"
                    :width="146"
                    :height="146"
                    mode="aspectFill"
                  />
                  <view class="goods-content">
                    <view class="goods-title">{{ item.pn }}</view>
                    <view>{{ item.skuText }}</view>
                  </view>
                  <view class="goods-right">
                    <price
                      class="goods-price"
                      :price="item.price"
                      decimal-smaller
                    />
                    <view class="goods-num">x{{ item.num }}</view>
                  </view>
                </view>
              </view>
              <van-cell title="留言" :value="trade.message" />
            </view>
          </view>
        </view>
      </view>
      <view class="container">
        <view class="pay-detail">
          <view class="pay-item">
            <text>商品总额</text>
            <price
              fill
              decimal-smaller
              class="pay-item__right font-bold"
              :price="tradeSet.totalMoney"
            />
          </view>

          <view class="pay-item">
            <text>订单备注</text>
            <view class="pay-item__right">
              <text class="pay-remark">选填，建议先和商家沟通确认</text>
              <t-icon name="chevron-right" size="32rpx" color="#BBBBBB" />
            </view>
          </view>
        </view>
      </view>
    </view>
  </view>
</template>
<script lang="ts" setup>
import { onShow } from "@dcloudio/uni-app";
import { computed } from "vue";
import { useTradeCash } from "./hook";
const trades = computed(() => tradeSet.value.shopTrades);
const { tradeSet } = useTradeCash();
onShow(() => {
  // const id = decodeURIComponent(query!.tradeIds);
});
</script>
<style lang="scss" scoped>
.trade-header {
  display: flex;
  padding: 20px 30px;
  background: linear-gradient(to left, #408ce2, #58b5f2);
  align-items: center;
}
.container {
  margin: 20rpx 20rpx 0;
  padding: 0 10rpx;
  border-radius: 20rpx;
  background-color: #ffffff;
  overflow: hidden;
  box-sizing: border-box;
}
.trade-header-info {
  flex: 2;
  margin: 0 auto;
  color: #fff;
}
.trade-card {
  box-sizing: border-box;
}
.trade-header-info .trade-header-info-title {
  font-size: 21px;
  line-height: 1.4;
}

.trade-header-info-desc {
  font-size: 16px;
  line-height: 1.4;
}

.header-icon {
  flex: 1;
}

.address {
  -webkit-box-align: center;
  -webkit-align-items: center;
  align-items: center;
  padding: 15px;
  border-radius: 10px;
}

.address-item-tel {
  font-size: 10px;
  color: #666;
}

.address-item-address {
  font-size: 11px;
  color: #333;
}

.address-item-edit {
  line-height: 48px;
  height: 48px;
}

.pay-type {
  overflow: hidden;
  color: #888;
}

.pay-type-img {
  height: 50px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.pay-type-text {
  word-wrap: normal;
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
  text-align: center;
  color: #333;
  padding: 0 1px;
}

.trade-info {
  padding: 10px;
  background: #fff;
}

.trade-info-header {
  position: relative;
  padding: 0px 5px;
  margin-bottom: 5px;
  font-size: 14px;
  line-height: 18px;
  color: #000;
}

.trade-info-header-text {
  position: relative;
  overflow: visible;
}

.trade-info-header-text::before {
  content: " ";
  position: absolute;
  pointer-events: none;
  box-sizing: border-box;
  overflow: visible;
  top: 0;
  left: -7px;
  bottom: 0;
  -webkit-transform: scaleX(0.5);
  transform: scaleX(0.5);
  border-left: 3px solid $u-type-primary;
}
.trade-card-header {
  padding: 16rpx 16rpx;
}
.goods-wrapper {
  width: 100%;
  box-sizing: border-box;
  padding: 16rpx 16rpx;
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  font-size: 24rpx;
  line-height: 32rpx;
  color: #999999;
  background-color: #ffffff;

  .goods-image {
    width: 176rpx;
    height: 176rpx;
    border-radius: 8rpx;
    overflow: hidden;
    margin-right: 16rpx;
  }
  .goods-content {
    flex: 1;
  }

  .goods-content .goods-title {
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    overflow: hidden;
    text-overflow: ellipsis;
    font-size: 28rpx;
    line-height: 40rpx;
    margin-bottom: 12rpx;
    color: #333333;
    margin-right: 16rpx;
  }

  .goods-right {
    min-width: 128rpx;
    display: flex;
    flex-direction: column;
    align-items: flex-end;
  }

  .goods-right .goods-price {
    color: #333333;
    font-size: 32rpx;
    line-height: 48rpx;
    font-weight: bold;
    margin-bottom: 16rpx;
  }

  .goods-right .goods-num {
    text-align: right;
  }
}
.pay-detail {
  padding: 16rpx 16rpx;
}
.pay-detail .pay-item {
  width: 100%;
  height: 72rpx;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 26rpx;
  line-height: 36rpx;
  color: #666666;
  .pay-item__right {
    color: #333333;
    font-size: 24rpx;
    display: flex;
    align-items: center;
    justify-content: flex-end;
    max-width: 400rpx;
  }
  .pay-item__right .pay-remark {
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    max-width: 400rpx;
    text-overflow: ellipsis;
    overflow: hidden;
  }
  .font-bold {
    font-weight: bold;
  }
  .primary {
    color: #fa4126;
  }
}
.pay-type .van-col {
  padding: 10px;
}

.cash {
  background: #f2f2f2;
  min-height: 100vh;
}
</style>
