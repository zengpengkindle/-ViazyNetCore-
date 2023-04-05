<template>
  <view class="selection-detail">
    <x-header
      :header-style="headerStyle"
      :title-color="headerStyle.color"
      title="商品详情"
      class="header"
    />
    <view class="u-skeleton">
      <view class="product-image-wrap u-skeleton-rect">
        <u-swiper :list="imageList" :height="imageHeight * 2" />
      </view>
      <view class="goods-info">
        <view class="goods-number">
          <view class="goods-price">
            <price
              class="class-goods-price"
              symbol="¥"
              symbol-class="class-goods-symbol"
              decimal-smaller
              :price="detail.price"
              type="lighter"
            />
            <view class="goods-price-up">起</view>
            <price
              symbol="¥"
              class="class-goods-del"
              :price="detail.price"
              type="del"
            />
          </view>
          <view class="sold-num">已售</view>
        </view>
        <view
          v-if="activityList.length > 0"
          class="goods-activity"
          bindtap="showPromotionPopup"
        >
          <view class="tags-container">
            <view
              v-for="activity in activityList"
              :key="activity.id"
              wx:if="{{index<4}}"
            >
              <view class="goods-activity-tag">{{ activity.tag }}</view>
            </view>
          </view>
          <view class="activity-show">
            <view class="activity-show-text">领劵</view>
            <u-icon name="chevron-right" size="42rpx" />
          </view>
        </view>
        <view class="goods-title">
          <view class="goods-name">{{ detail.title }}</view>
          <view class="goods-tag">
            <u-button open-type="share" t-class="shareBtn" variant="text">
              <view class="btn-icon">
                <t-icon name="share" size="40rpx" color="#000" />
                <view class="share-text">分享</view>
              </view>
            </u-button>
          </view>
        </view>
        <view class="goods-intro">{{ detail.subTitle }}</view>
      </view>
      <view class="spu-select" @click="showSkuSelectPopup">
        <view class="label">已选</view>
        <view class="content">
          <view class="{{!selectedAttrStr ? 'tintColor' : ''}}">
            {{ selectedAttrStr ? buyNum : "" }}{{ selectedAttrStr || "请选择" }}
          </view>
          <t-icon name="chevron-right" size="40rpx" color="#BBBBBB" />
        </view>
      </view>
      <view class="goods-detail">
        <u-parse :html="detail.detail" class="u-skeleton-rect" />
      </view>
      <u-skeleton :loading="loading" :animation="true" bg-color="#FFF" />
    </view>
    <GoodsSpecesPopup
      v-model="showSpecPopup"
      :sku="detail.skus"
      :title="detail.title"
    />
  </view>
</template>
<script lang="ts" setup>
import XHeader from "@/components/ui/header/index.vue";
import ProductApi, { type ProductInfoModel } from "@/apis/shopmall/product";
import { useHeader, GetRect } from "@/components/ui/hooks/user-head";
import { onLoad, onPageScroll } from "@dcloudio/uni-app";
import { getCurrentInstance, onMounted } from "vue";
import { ref, computed, type Ref } from "vue";
import GoodsSpecesPopup from "./components/goods-specs-popup.vue";
const imageHeight = ref(0);
onMounted(async () => {
  const rect = await GetRect(getCurrentInstance(), ".product-image-wrap");
  imageHeight.value = (rect as UniApp.NodeInfo).width;
  console.log(imageHeight.value);
});
onLoad(query => {
  const id = decodeURIComponent(query!.id || query!.paid || query!.p || "");
  useHeader();
  getDetail(id);
});
let scrollTop = 0;
const headerStyle = ref({
  backgroundColor: "rgba(255,255,255,0)",
  color: "rgba(0, 0, 0, 0)"
});
onPageScroll(e => {
  scrollTop = e.scrollTop;
  let opacity = scrollTop / uni.upx2px(400);
  opacity < 0 && (opacity = 0);
  opacity > 1 && (opacity = 1);
  const color = `rgba(255,255,255,${opacity})`;
  headerStyle.value.backgroundColor = color;
  headerStyle.value.color = `rgba(0, 0, 0, ${opacity})`;
});
const loading = ref(true);
const imageList: Ref<Array<string>> = ref([]);
const defaultItem: ProductInfoModel = {
  productId: "",
  shopId: "",
  shopName: "",
  catId: "",
  catName: "",
  brandId: "",
  brandName: "",
  title: "",
  subTitle: "",
  image: "",
  subImage: "",
  detail: "",
  skuId: "",
  hasOuter: false,
  outerType: "",
  skuText: "",
  cost: 0,
  inStock: 0,
  price: 0,
  refundType: null,
  status: 0,
  freightType: 0,
  freightValue: 0,
  freightStep: 0,
  freightStepValue: 0,
  num: 0,
  skus: undefined
};
const detail: Ref<ProductInfoModel> = ref(defaultItem);
const getDetail = async (id: string) => {
  loading.value = false;
  detail.value = await ProductApi.getProductSku(id, "");
  imageList.value = [detail.value.image];
  if (detail.value.subImage != null && detail.value.subImage != "") {
    imageList.value.push(...detail.value.subImage.split(";"));
  }
  setTimeout(() => {
    loading.value = false;
  }, 1000);
};
/** 商品活动 标签 */
interface ActivityItem {
  id: string;
  tag: string;
}
const activityList: Ref<Array<ActivityItem>> = ref([]);
/** 商品Sku属性 */
const showSpecPopup = ref(false);
const buyNum = ref(0);
const selectedAttrStr = computed(() => "");
const showSkuSelectPopup = () => {
  showSpecPopup.value = true;
};
</script>

<style lang="scss" scoped>
.selection-detail {
  background-color: #f7f8fa;
  min-height: 100vh;
  position: relative;

  .header {
    display: block;
    position: fixed;
    z-index: 10;
    width: 100%;
    top: 0;
    left: 0;
    transition: opacity 0.1s ease;
    .header-title {
      max-width: 300rpx;
    }
  }

  .sticky-bottom {
    display: block;
    position: sticky;
    bottom: 0;
    z-index: 1;
  }
}

.block {
  display: block;
  margin: 24rpx;
}

.sticky-top {
  padding-bottom: 12rpx;
  background: #ffffff;
  position: fixed;
  z-index: 10;
  width: 100%;
  transition: opacity 0.1s ease;
  &:deep(.ytt-header) {
    padding-left: 7px;
  }
  .header-icon {
    background-color: rgba(255, 255, 255, 0.3);
    border: 0.5px solid rgba(151, 151, 151, 0.201002);
    border-radius: 50%;
    overflow: hidden;
    width: 64rpx;
    height: 64rpx;
    box-sizing: border-box;
    display: flex;
    align-items: center;
    justify-content: center;
    .icon {
      height: 48rpx;
      width: 48rpx;
    }
  }
  .header-title {
    max-width: 298rpx;
  }
}
.selection-detail {
  .goods-info {
    margin: 0 auto;
    padding: 26rpx 0 28rpx 30rpx;
    background-color: #fff;
    .goods-price {
      display: flex;
      align-items: baseline;
    }
    .goods-price-up {
      color: #fa4126;
      font-size: 28rpx;
      position: relative;
      bottom: 4rpx;
      left: 8rpx;
    }

    .goods-price .class-goods-price {
      font-size: 64rpx;
      color: #fa4126;
      font-weight: bold;
      font-family: DIN Alternate;
    }

    .goods-price .class-goods-symbol {
      font-size: 36rpx;
      color: #fa4126;
    }

    .goods-price .class-goods-del {
      position: relative;
      font-weight: normal;
      left: 16rpx;
      bottom: 2rpx;
      color: #999999;
      font-size: 32rpx;
    }

    .goods-number {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
    .goods-number .sold-num {
      font-size: 24rpx;
      color: #999999;
      display: flex;
      align-items: flex-end;
      margin-right: 32rpx;
    }

    .goods-activity {
      display: flex;
      margin-top: 16rpx;
      justify-content: space-between;
    }

    .goods-activity .tags-container {
      display: flex;
    }

    .goods-activity .tags-container .goods-activity-tag {
      background: #ffece9;
      color: #fa4126;
      font-size: 24rpx;
      margin-right: 16rpx;
      padding: 4rpx 8rpx;
      border-radius: 4rpx;
    }

    .goods-activity .activity-show {
      display: flex;
      justify-content: center;
      align-items: center;
      color: #fa4126;
      font-size: 24rpx;
      padding-right: 32rpx;
    }

    .goods-activity .activity-show-text {
      line-height: 42rpx;
    }

    .goods-title {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-top: 20rpx;
    }

    .goods-title .goods-name {
      width: 600rpx;
      font-weight: 500;
      display: -webkit-box;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 2;
      overflow: hidden;
      font-size: 32rpx;
      word-break: break-all;
      color: #333333;
    }

    .goods-title .goods-tag {
      width: 104rpx;
      margin-left: 26rpx;
    }

    .goods-title .goods-tag .shareBtn {
      border-radius: 200rpx 0px 0px 200rpx;
      width: 100rpx;
      height: 96rpx;
      border: none;
      padding-right: 36rpx !important;
    }

    .goods-title .goods-tag .shareBtn::after {
      border: none;
    }
    .goods-title .goods-tag .btn-icon {
      font-size: 20rpx;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 96rpx;
      color: #999;
    }
    .goods-title .goods-tag .btn-icon .share-text {
      padding-top: 8rpx;
      font-size: 20rpx;
      line-height: 24rpx;
    }
    .goods-intro {
      font-size: 26rpx;
      color: #888;
      line-height: 36rpx;
      word-break: break-all;
      padding-right: 30rpx;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      white-space: normal;
      overflow: hidden;
    }
  }

  .spu-select {
    height: 80rpx;
    background-color: #fff;
    margin-top: 20rpx;
    display: flex;
    align-items: center;
    padding: 30rpx;
    font-size: 28rpx;
    .label {
      margin-right: 30rpx;
      text-align: center;
      flex-shrink: 0;
      color: #999999;
      font-weight: normal;
    }

    .content {
      display: flex;
      flex: 1;
      justify-content: space-between;
      align-items: center;
    }

    .content .tintColor {
      color: #aaa;
    }
  }
}
</style>
