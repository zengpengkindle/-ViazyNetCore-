<template>
  <view>
    <view class="order-list-tabs">
      <u-tabs-swiper
        ref="uTabs"
        :list="list"
        :current="current"
        :is-scroll="false"
        swiper-width="750"
        @change="tabsChange"
      />
    </view>
    <swiper
      style="width: 100%"
      :style="{ height: 'calc(100vh - 44px)' }"
      :current="swiperCurrent"
      @transition="transition"
      @animationfinish="animationfinish"
    >
      <swiper-item
        v-for="(sitem, index) in list"
        :key="index"
        class="swiper-item"
      >
        <scroll-view
          :scroll-y="true"
          style="width: 100%; background-color: #f2f2f2"
          @scrolltolower="onreachBottom"
        >
          <u-card
            v-for="trade in tradeList"
            :key="trade.id"
            :head-border-bottom="true"
            :foot-border-top="false"
            :title="'订单号:' + trade.id"
            :title-size="26"
            margin="20rpx 20rpx 0"
            :padding="20"
            body-style="padding:10rpx 20rpx 0"
          >
            <template #body>
              <view class="trade-card-items">
                <view
                  v-for="item in trade.items"
                  :key="item.pId + item.skuId"
                  class="trade-card-item goods-wrapper"
                >
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
                      symbol="￥"
                      fill
                      :price="item.price"
                      decimal-smaller
                    />
                    <view class="goods-num">x{{ item.num }}</view>
                  </view>
                </view>
                <view class="pay-detail">
                  <view class="pay-item">
                    <text>运费</text>
                    <price
                      fill
                      symbol="￥"
                      decimal-smaller
                      class="pay-item__right font-bold"
                      :price="trade.totalfeight"
                    />
                  </view>
                  <view class="pay-item">
                    <text>商品总额</text>
                    <price
                      fill
                      symbol="￥"
                      decimal-smaller
                      class="pay-item__right font-bold"
                      :price="trade.totalMoney"
                    />
                  </view>
                </view>
              </view>
            </template>
            <template #foot>
              <view class="order-item-foot">
                <view class="trade-card-cell">
                  <u-button
                    v-if="trade.tradeStatus == -1"
                    size="mini"
                    shape="circle"
                    @click="goCash(trade.id)"
                  >
                    去支付
                  </u-button>
                  <u-button
                    v-if="trade.tradeStatus == -1"
                    size="mini"
                    shape="circle"
                  >
                    取消订单
                  </u-button>
                  <u-button
                    v-if="trade.tradeStatus == 0"
                    size="mini"
                    shape="circle"
                  >
                    提醒发货
                  </u-button>
                  <u-button
                    v-if="trade.tradeStatus == 0"
                    size="mini"
                    shape="circle"
                    >取消订单</u-button
                  >
                  <u-button
                    v-if="trade.tradeStatus == 1"
                    size="mini"
                    shape="circle"
                  >
                    确认收货
                  </u-button>
                  <u-button
                    v-if="trade.tradeStatus == 2"
                    size="mini"
                    shape="circle"
                  >
                    立即评价
                  </u-button>
                  <u-button
                    v-if="trade.tradeStatus == 2"
                    size="mini"
                    shape="circle"
                  >
                    申请售后
                  </u-button>
                </view>
              </view>
            </template>
          </u-card>
        </scroll-view>
      </swiper-item>
    </swiper>
  </view>
</template>
<script lang="ts" setup>
import { onLoad } from "@dcloudio/uni-app";
import { ref, type Ref } from "vue";
import TradeApi, {
  type TradePageReq,
  type TradeDetailModel
} from "@/apis/shopmall/trade";

const list = [
  {
    name: "全部订单"
  },
  {
    name: "待付款"
  },
  {
    name: "待收货"
  },
  {
    name: "待评价",
    count: 5
  },
  {
    name: "售后"
  }
];
const uTabs: Ref<any> = ref();
const swiperCurrent = ref(0);
const current = ref(0);
function tabsChange(index) {
  swiperCurrent.value = index;
}
// swiper-item左右移动，通知tabs的滑块跟随移动
function transition(e) {
  const dx = e.detail.dx;
  uTabs.value.setDx(dx);
}
// 由于swiper的内部机制问题，快速切换swiper不会触发dx的连续变化，需要在结束时重置状态
// swiper滑动结束，分别设置tabs和swiper的状态
function animationfinish(e) {
  const sel_current = e.detail.current;
  uTabs.value.setFinishCurrent(sel_current);
  swiperCurrent.value = sel_current;
  current.value = sel_current;
}
// scroll-view到底部加载更多
function onreachBottom() {}

const type = ref("");
const tradeList: Ref<Array<TradeDetailModel>> = ref([]);
onLoad(async query => {
  type.value = decodeURIComponent(query!.type);
  const args: TradePageReq = {
    tradeStatus: null,
    page: 1,
    limit: 10
  };
  const { rows } = await TradeApi.findMyTrades(args);
  tradeList.value = rows;
});

const goDetail = (id: string) => {
  uni.navigateTo({ url: `/pages/order/detail/index?tradeIds=${id}` });
};
const goCash = (id: string) => {
  uni.navigateTo({ url: `/pages/order/cash/index?tradeIds=${id}` });
};
</script>
<style lang="scss" scoped>
.order-list-tabs {
  background-color: #f8f8f8;
}
.goods-wrapper {
  width: 100%;
  box-sizing: border-box;
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  font-size: 24rpx;
  line-height: 32rpx;
  color: #999999;
  background-color: #ffffff;
  &:not(:first-child) {
    margin-top: 10rpx;
  }
  .goods-image {
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
    -webkit-line-clamp: 1;
    overflow: hidden;
    text-overflow: ellipsis;
    font-size: 24rpx;
    line-height: 40rpx;
    margin-bottom: 12rpx;
    color: #333333;
    margin-right: 20rpx;
  }

  .goods-right {
    min-width: 128rpx;
    display: flex;
    flex-direction: column;
    align-items: flex-end;
  }

  .goods-right .goods-price {
    color: #000;
    font-size: 28rpx;
    line-height: 40rpx;
    font-weight: bold;
  }

  .goods-right .goods-num {
    text-align: right;
  }
}

.pay-detail .pay-item {
  width: 100%;
  height: 56rpx;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  font-size: 24rpx;
  line-height: 32rpx;
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
.order-item-foot {
  display: flex;
  justify-content: flex-end;
}
.trade-card-cell {
  display: flex;
  justify-content: space-between;
  :deep(.u-btn) {
    margin: 0 5rpx;
  }
}
</style>
