<template>
  <view>
    <view class="goodsBox">
      <!-- 列表平铺两列三列 -->
      <view
        v-if="
          (parameters.column == 2 && parameters.display == 'list') ||
          (parameters.column == 3 && parameters.display == 'list')
        "
        :class="'column' + parameters.column"
      >
        <view
          class="u-margin-left-15 u-margin-right-15 u-margin-top-15 u-margin-bottom-15"
        >
          <u-section
            v-if="parameters.title != ''"
            :title="parameters.title"
            :arrow="parameters.lookMore"
            :sub-title="parameters.lookMore ? '更多' : ''"
            font-size="30"
            @click="
              parameters.lookMore
                ? goGoodsList({
                    catId: parameters.classifyId,
                    brandId: parameters.brandId
                  })
                : ''
            "
          />
        </view>
        <view v-if="count" class="">
          <u-grid :col="parameters.column" :border="false" :align="center">
            <u-grid-item
              v-for="(item, index) in parameters.list"
              :key="index"
              :custom-style="{ padding: '0rpx' }"
              bg-color="transparent"
              @click="goGoodsDetail(item.id)"
            >
              <view class="good_box">
                <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                <u-lazy-load
                  threshold="-150"
                  :image="item.image"
                  :index="index"
                />
                <view class="good_title u-line-2">
                  {{ item.name }}
                </view>
                <view class="good-price">
                  <price
                    :price="item.price || 100.01"
                    symbol="¥"
                    type="lighter"
                    decimal-smaller
                  />
                  <price
                    :price="item.mktprice || 100.01"
                    symbol="¥"
                    type="del"
                  />
                </view>
                <view v-if="item.isRecommend" class="good-tag-recommend">
                  推荐
                </view>
                <view v-if="item.isHot" class="good-tag-hot"> 热门 </view>
              </view>
            </u-grid-item>
          </u-grid>
        </view>
        <view v-else-if="!count && !parameters.listAjax">
          <u-grid col="3" :border="false" :align="center">
            <u-grid-item
              v-for="item in 3"
              :key="item"
              bg-color="transparent"
              :custom-style="{ padding: '0rpx' }"
            >
              <view class="good_box">
                <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                <!--<u-lazy-load threshold="-450" border-radius="10" image="/static/images/common/empty.png"></u-lazy-load>-->
                <view class="good_title u-line-2"> 无 </view>
                <view class="good-price"> 0元 </view>
                <view class="good-tag-recommend"> 推荐 </view>
                <view class="good-tag-hot"> 热门 </view>
              </view>
            </u-grid-item>
          </u-grid>
        </view>
      </view>

      <!-- 列表平铺单列 -->
      <view v-if="parameters.column == 1 && parameters.display == 'list'">
        <view
          class="u-margin-left-15 u-margin-right-15 u-margin-top-15 u-margin-bottom-15"
        >
          <u-section
            v-if="parameters.title != ''"
            :title="parameters.title"
            :arrow="parameters.lookMore"
            :sub-title="parameters.lookMore ? '更多' : ''"
            font-size="30"
            @click="
              parameters.lookMore
                ? goGoodsList({
                    catId: parameters.classifyId,
                    brandId: parameters.brandId
                  })
                : ''
            "
          />
        </view>
        <view v-if="count">
          <u-grid :col="1" :border="false" :align="center">
            <u-grid-item
              v-for="item in parameters.list"
              :key="item.id"
              bg-color="transparent"
              :custom-style="{ padding: '0rpx' }"
              @click="goGoodsDetail(item.id)"
            >
              <view class="good_box">
                <u-row gutter="10" justify="space-between">
                  <u-col span="4">
                    <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                    <u-lazy-load
                      threshold="-150"
                      border-radius="10"
                      :image="item.image"
                      :index="item.id"
                    />
                    <view v-if="item.isRecommend" class="good-tag-recommend2">
                      推荐
                    </view>
                    <view v-if="item.isHot" class="good-tag-hot"> 热门 </view>
                  </u-col>
                  <u-col span="8">
                    <view class="good_title-xl u-line-3 u-padding-10">
                      {{ item.name }}
                    </view>
                    <view class="good-price u-padding-10">
                      <price :price="item.price" symbol="￥" />
                      <price
                        v-if="item.mktprice > 0"
                        :price="item.mktprice"
                        type="mini"
                        symbol="￥"
                        decimal-smaller
                        style="margin-left: 10rpx"
                      />
                    </view>
                  </u-col>
                </u-row>
              </view>
            </u-grid-item>
          </u-grid>
        </view>
        <view v-else class="order-none">
          <image
            class="order-none-img"
            src="/static/images/order.png"
            mode=""
          />
        </view>
      </view>

      <!-- 横向滚动 -->
      <view
        v-if="
          (parameters.column == 2 && parameters.display == 'slide') ||
          (parameters.column == 3 && parameters.display == 'slide')
        "
        :class="'slide' + parameters.column"
      >
        <view
          class="u-margin-left-15 u-margin-right-15 u-margin-top-15 u-margin-bottom-15"
        >
          <u-section
            v-if="parameters.title != ''"
            font-size="30"
            :title="parameters.title"
            :arrow="parameters.lookMore"
            :sub-title="parameters.lookMore ? '更多' : ''"
            @click="
              parameters.lookMore
                ? goGoodsList({
                    catId: parameters.classifyId,
                    brandId: parameters.brandId
                  })
                : ''
            "
          />
        </view>
        <view>
          <view v-if="count">
            <swiper
              :class="
                parameters.column == 3
                  ? 'swiper3'
                  : parameters.column == 2
                  ? 'swiper2'
                  : ''
              "
              @change="change"
            >
              <swiper-item v-for="no of pageCount" :key="no">
                <u-grid
                  :col="parameters.column"
                  :border="false"
                  :align="center"
                >
                  <template v-for="(item, index) in parameters.list">
                    <u-grid-item
                      v-if="
                        index >= parameters.column * no &&
                        index <= parameters.column * (no + 1)
                      "
                      :key="index"
                      bg-color="transparent"
                      :custom-style="{ padding: '0rpx' }"
                      @click="goGoodsDetail(item.id)"
                    >
                      <view class="good_box">
                        <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                        <u-lazy-load
                          threshold="-150"
                          border-radius="10"
                          :image="item.image"
                          :index="item.id"
                        />
                        <view class="good_title u-line-2">
                          {{ item.name }}
                        </view>
                        <view class="good-price">
                          {{ item.price }}元
                          <span
                            class="u-font-xs coreshop-text-through u-margin-left-15 coreshop-text-gray"
                            >{{ item.mktprice }}元</span
                          >
                        </view>
                        <view
                          v-if="item.isRecommend"
                          class="good-tag-recommend"
                        >
                          推荐
                        </view>
                        <view v-if="item.isHot" class="good-tag-hot">
                          热门
                        </view>
                      </view>
                    </u-grid-item>
                  </template>
                </u-grid>
              </swiper-item>
            </swiper>
            <view class="indicator-dots">
              <view
                v-for="no of pageCount"
                :key="no"
                class="indicator-dots-item"
                :class="[current == no ? 'indicator-dots-active' : '']"
              />
            </view>
          </view>
          <view v-else>
            <scroll-view class="swiper-list" scroll-x="true" />
          </view>
        </view>
      </view>
    </view>
  </view>
</template>
<script lang="ts" setup>
import { computed, ref } from "vue";
import { stringify } from "qs";

export interface GoodParameter {
  column: number;
  display?: string;
  title: string;
  list: Array<any>;
  lookMore?: boolean;
  classifyId?: number;
  brandId?: number;
  listAjax?: boolean;
}
export interface BlackProps {
  parameters: GoodParameter;
}
const prop = defineProps<BlackProps>();
const goGoodsList = ({ catId, brandId }) => {
  console.log(catId, brandId);
};
const pageCount = computed(() => {
  let count = prop.parameters.list.length / prop.parameters.column;
  if (prop.parameters.column * count < prop.parameters.list.length) {
    count = count + 1;
  }
  return count;
});
const count = computed(() => {
  return prop.parameters.list.length;
});
const goGoodsDetail = (productId: string) => {
  const queries = {
    id: productId
  };
  uni.navigateTo({
    url: `/pages/selection/detail/index?${stringify(queries)}`
  });
};
const center = ref("center");
const current = ref(0);
const change = (e: any) => {
  current.value = e.detail.current;
};
</script>
<style scoped lang="scss">
.goodsBox {
  color: #333333 !important;
  padding: 10rpx 10rpx 0;
  background-color: #f8f8f8;
  .good_box {
    border-radius: 8px;
    background-color: #ffffff;
    position: relative;
    width: calc(100% - 6px);
    overflow: hidden;
    margin-top: 15rpx;
    .good_title {
      font-size: 26rpx;
      padding: 10rpx 15rpx 0;
      color: $u-main-color;
    }
    .u-line-2 {
      display: -webkit-box;
      overflow: hidden;
      text-overflow: ellipsis;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
    }
    .good_title-xl {
      font-size: 28rpx;
      margin-top: 5px;
      color: $u-main-color;
    }
    .good-tag-hot {
      display: flex;
      margin-top: 5px;
      position: absolute;
      top: 15rpx;
      left: 15rpx;
      background-color: $u-type-error;
      color: #ffffff;
      display: flex;
      align-items: center;
      padding: 4rpx 14rpx;
      border-radius: 50rpx;
      font-size: 20rpx;
      line-height: 1;
    }
    .good-tag-recommend {
      display: flex;
      margin-top: 5px;
      position: absolute;
      top: 15rpx;
      right: 15rpx;
      background-color: $u-type-primary;
      color: #ffffff;
      margin-left: 10px;
      border-radius: 50rpx;
      line-height: 1;
      padding: 4rpx 14rpx;
      display: flex;
      align-items: center;
      border-radius: 50rpx;
      font-size: 20rpx;
    }
    .good-tag-recommend2 {
      display: flex;
      margin-top: 5px;
      position: absolute;
      bottom: 15rpx;
      left: 15rpx;
      background-color: $u-type-primary;
      color: #ffffff;
      border-radius: 50rpx;
      line-height: 1;
      padding: 4rpx 14rpx;
      display: flex;
      align-items: center;
      border-radius: 50rpx;
      font-size: 20rpx;
    }
    .good-price {
      font-size: 30rpx;
      padding: 10rpx 15rpx;
      color: $u-type-error;
    }
  }

  .indicator-dots {
    margin-top: 20rpx;
    margin-bottom: 20rpx;
    display: flex;
    justify-content: center;
    align-items: center;
    .indicator-dots-item {
      background-color: $u-tips-color;
      height: 6px;
      width: 6px;
      border-radius: 10px;
      margin: 0 3px;
    }
    .indicator-dots-active {
      background-color: $u-type-primary;
    }
  }
}
</style>
