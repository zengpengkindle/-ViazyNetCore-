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
          scroll-y
          style="width: 100%"
          @scrolltolower="onreachBottom"
        >
          <u-card v-for="trade in tradeList" :key="trade.id" :title="trade.id">
            <template #body>
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
                      symbol="￥"
                      :price="item.price"
                      decimal-smaller
                    />
                    <view class="goods-num">x{{ item.num }}</view>
                  </view>
                </view>
              </view>
            </template>
            <template #foot>
              <view class="order-item-foot">
                <u-icon name="chat-fill" size="34" color="" label="30评论" />
                <view class="trade-card-cell">
                  <u-button
                    v-if="trade.tradeStatus == -1"
                    size="mini"
                    round
                    @click="goCash(trade.id)"
                  >
                    去支付
                  </u-button>
                  <u-button v-if="trade.tradeStatus == -1" size="mini" round>
                    取消订单
                  </u-button>
                  <u-button v-if="trade.tradeStatus == 0" size="mini" round>
                    提醒发货
                  </u-button>
                  <u-button v-if="trade.tradeStatus == 0" size="mini" round
                    >取消订单</u-button
                  >
                  <u-button v-if="trade.tradeStatus == 1" size="mini" round>
                    确认收货
                  </u-button>
                  <u-button v-if="trade.tradeStatus == 2" size="mini" round>
                    立即评价
                  </u-button>
                  <u-button v-if="trade.tradeStatus == 2" size="mini" round>
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
  uni.navigateTo({ url: `/pages/order/cash/index?tradeIds=${id}` });
};
const goCash = (id: string) => {
  uni.navigateTo({ url: `/pages/order/cash/index?tradeIds=${id}` });
};
</script>
<style lang="scss" scoped>
.goods-wrapper {
  width: 100%;
  box-sizing: border-box;
  padding: 16rpx 20rpx;
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  font-size: 24rpx;
  line-height: 32rpx;
  color: #999999;
  background-color: #ffffff;

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
    -webkit-line-clamp: 2;
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
.order-item-foot {
  display: flex;
  justify-content: space-between;
}
.trade-card-cell {
  display: flex;
}
</style>
