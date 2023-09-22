<template>
  <u-popup v-model="showSpecPopup" mode="bottom" closeable>
    <view class="popup-container">
      <view class="popup-sku-header">
        <u-image
          class="popup-sku-header__img"
          :width="146"
          :height="146"
          :src="imageSrc"
          :duration="100"
          border-radius="10rpx"
        />
        <view class="popup-sku-header__goods-info">
          <view class="popup-sku__goods-name">{{ title }}</view>
          <view class="goods-price-container">
            <slot name="goods-price" />
          </view>
          <!-- 已选规格 -->
          <view v-if="!sku.none_sku" class="popup-sku__selected-spec">
            <view>选择：</view>
            <view v-for="item in specList" :key="item.specId">
              <block
                v-for="selectedItem in item.specValueList"
                :key="selectedItem.specValueId"
              >
                <view
                  v-if="selectedItem.isSelected"
                  class="popup-sku__selected-item"
                >
                  {{ selectedItem.specValue }}
                </view>
              </block>
            </view>
          </view>
        </view>
      </view>
      <view class="popup-sku-body">
        <view class="popup-sku-group-container">
          <view
            v-for="item in specList"
            :key="item.specId"
            class="popup-sku-row"
          >
            <view class="popup-sku-row__title">{{ item.title }}</view>
            <block
              v-for="valuesItem in item.specValueList"
              :key="valuesItem.specValueId"
            >
              <view
                class="popup-sku-row__item"
                :class="[
                  valuesItem.isSelected ? 'popup-sku-row__item--active' : '',
                  !valuesItem.hasStockObj.hasStock || !isStock
                    ? 'disabled-sku-selected'
                    : ''
                ]"
                @click="toChooseItem(valuesItem, item.specValueList)"
              >
                {{ valuesItem.specValue }}
              </view>
            </block>
          </view>
        </view>
        <view class="popup-sku-stepper-stock">
          <view class="popup-sku-stepper-container">
            <view class="popup-sku__stepper-title">
              购买数量
              <view v-if="limitBuyInfo" class="limit-text">
                ({{ limitBuyInfo }})
              </view>
            </view>
            <u-number-box v-model="buyNum" :min="1" theme="filled" />
          </view>
        </view>
      </view>
      <view
        v-if="outOperateStatus"
        class="single-confirm-btn"
        :class="!isStock && spec.id == '' ? 'disabled' : ''"
        @click="specsConfirm"
      >
        确定
      </view>
      <view
        v-if="!outOperateStatus"
        class="popup-sku-actions flex flex-between"
        :class="!isStock ? 'popup-sku-disabled' : ''"
      >
        <view class="sku-operate">
          <view
            class="selected-sku-btn sku-operate-addCart"
            :class="!isStock ? 'disabled' : ''"
            @click="addCart"
          >
            加入购物车
          </view>
        </view>
        <view class="sku-operate">
          <view
            class="selected-sku-btn sku-operate-buyNow"
            :class="!isStock ? 'disabled' : ''"
            @click="buyNow"
          >
            立即购买
          </view>
        </view>
      </view>
      <slot name="bottomSlot" />
    </view>
  </u-popup>
  <u-toast id="t-toast" />
</template>
<script lang="ts" setup>
import type { ProductSkuModel } from "@/apis/shopmall/product";
import ProductCartApi, {
  type ShoppingCartEditDto
} from "@/apis/shopmall/productCart";
import { ref, watch, computed, type Ref } from "vue";
import { useProductSpec } from "./specsHooks";
export interface GoodsSpecsPopupProps {
  sku: ProductSkuModel;
  title: string;
}
const props = defineProps<GoodsSpecsPopupProps>();

interface SkuProps {
  specId: string;
  title: string;
  specValueList: Array<SpecItem>;
}
interface StockItem {
  hasStock: boolean;
}
interface SpecItem {
  specValue: string;
  specValueId: string;
  isSelected: boolean;
  hasStockObj: StockItem;
}
const imageSrc = ref("");
const specList: Ref<Array<SkuProps>> = ref();
const limitBuyInfo = ref(0);
const isStock = computed(() => {
  return !(props.sku?.hide_stock ?? true);
});
const outOperateStatus = ref(1);
watch(
  () => props.sku,
  () => {
    init();
  }
);
const init = () => {
  specList.value = [];
  if (props.sku.none_sku) return;
  imageSrc.value = props.sku.tree[0].v[0].imgUrl;
  props.sku.tree.forEach(item => {
    const spec = {
      specId: item.k_s,
      title: item.k,
      specValueList: []
    };

    item.v.forEach(vItem => {
      const specItem: SpecItem = {
        specValue: vItem.name,
        specValueId: vItem.id,
        isSelected: false,
        hasStockObj: {
          hasStock: true
        }
      };
      spec.specValueList.push(specItem);
    });
    specList.value.push(spec);
  });
};
const addCart = async () => {
  if (spec.value.id != "" || props.sku.none_sku) {
    const cart: ShoppingCartEditDto = {
      pId: props.sku.collection_id,
      skuId: spec.value.id,
      num: buyNum.value
    };
    await ProductCartApi.addCart(cart);
    uni.showToast({ title: "添加成功" });
  }
};
const buyNow = () => {};
const toChooseItem = (selectItem: SpecItem, item: Array<SpecItem>) => {
  selectItem.isSelected = !selectItem.isSelected;
  item.forEach(subItem => {
    if (subItem.specValueId != selectItem.specValueId) {
      subItem.isSelected = false;
    }
  });
};

watch(
  () => specList.value,
  nval => {
    const selectValue = [];
    nval.forEach(element => {
      element.specValueList.forEach(specValue => {
        if (specValue.isSelected) selectValue.push(specValue.specValueId);
      });
    });
    spec.value = { id: "" };
    for (const index in props.sku.list) {
      const item = props.sku.list[index];
      if (
        item.s1 == selectValue[0] &&
        (item.s2 == "0" || item.s2 == selectValue[1]) &&
        (item.s3 == "0" || item.s3 == selectValue[2])
      ) {
        spec.value = item;
      }
    }
  },
  { deep: true }
);
const { buyNum, spec, showSpecPopup, specType } = useProductSpec();
const specsConfirm = async () => {
  if (specType.value == "addCart") {
    await addCart();
  }
  specType.value = "";
  showSpecPopup.value = !showSpecPopup.value;
};
</script>
<style lang="scss" scoped>
.popup-container {
  background-color: #ffffff;
  position: relative;
  z-index: 100;
  border-radius: 16rpx 16rpx 0 0;
  padding-bottom: calc(env(safe-area-inset-bottom) + 20rpx);
  .popup-close {
    position: absolute;
    right: 30rpx;
    top: 30rpx;
    z-index: 9;
    color: #999999;
  }
}

.popup-sku-header {
  display: flex;
  padding: 30rpx 28rpx 0 30rpx;

  .popup-sku-header__img {
    width: 146rpx;
    height: 146rpx;
    border-radius: 10rpx;
    background: #d8d8d8;
    margin-right: 24rpx;
  }

  .popup-sku-header__goods-info {
    position: relative;
    width: 500rpx;
    .popup-sku__goods-name {
      font-size: 26rpx;
      line-height: 36rpx;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      white-space: normal;
      overflow: hidden;
      width: 430rpx;
      text-overflow: ellipsis;
    }
    .popup-sku__selected-spec {
      display: flex;
      color: #333333;
      font-size: 26rpx;
      line-height: 36rpx;
    }
    .popup-sku__selected-spec .popup-sku__selected-item {
      margin-right: 10rpx;
    }
  }
}

.popup-sku-body {
  margin: 0 30rpx 40rpx;
  max-height: 600rpx;
  overflow-y: scroll;
  -webkit-overflow-scrolling: touch;

  .popup-sku-group-container .popup-sku-row {
    padding: 32rpx 0;
    border-bottom: 1rpx solid #f5f5f5;
    .popup-sku-row__title {
      font-size: 26rpx;
      color: #333;
    }
    .popup-sku-row__item {
      font-size: 24rpx;
      color: #333;
      min-width: 128rpx;
      height: 56rpx;
      background-color: #f5f5f5;
      border-radius: 8rpx;
      border: 2rpx solid #f5f5f5;
      margin: 19rpx 26rpx 0 0;
      padding: 0 16rpx;
      display: inline-flex;
      align-items: center;
      justify-content: center;
    }
    .popup-sku-row__item.popup-sku-row__item--active {
      border: 2rpx solid $u-type-primary;
      color: $u-type-primary;
      background: #ffffff;
    }
    .disabled-sku-selected {
      background: #f5f5f5 !important;
      color: #cccccc;
    }
  }

  .popup-sku-stepper-stock .popup-sku-stepper-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin: 40rpx 0;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-sku__stepper-title {
    display: flex;
    font-size: 26rpx;
    color: #333;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-sku__stepper-title
    .limit-text {
    margin-left: 10rpx;
    color: #999999;
  }

  .popup-sku-stepper-stock .popup-sku-stepper-container .popup-stepper {
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    font-size: 28px;
    height: 48rpx;
    line-height: 62rpx;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .input-btn,
  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .input-num-wrap {
    position: relative;
    height: 100%;
    text-align: center;
    background-color: #f5f5f5;
    border-radius: 4rpx;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .input-num-wrap {
    color: #282828;
    display: flex;
    max-width: 76rpx;
    align-items: center;
    justify-content: space-between;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .input-num-wrap
    .input-num {
    height: 100%;
    width: auto;
    font-weight: 600;
    font-size: 30rpx;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .input-btn {
    width: 48rpx;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .popup-stepper__minus {
    margin-right: 4rpx;
    border-radius: 4rpx;
    color: #9a979b;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .popup-stepper__plus {
    margin-left: 4rpx;
    border-radius: 4rpx;
    color: #9a979b;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .popup-stepper__plus::after {
    width: 24rpx;
    height: 3rpx;
    background-color: #999999;
  }

  .popup-sku-stepper-stock
    .popup-sku-stepper-container
    .popup-stepper
    .popup-stepper__plus::before {
    width: 3rpx;
    height: 24rpx;
    background-color: #999999;
  }
}
.popup-sku-actions {
  font-size: 32rpx;
  height: 80rpx;
  text-align: center;
  line-height: 80rpx;
  padding: 0 20rpx;
}
.popup-sku-actions {
  .sku-operate {
    height: 80rpx;
    width: 50%;
    color: #fff;
    border-radius: 48rpx;
  }

  .sku-operate .sku-operate-addCart {
    background-color: $u-type-primary-light;
    color: $u-type-primary;
    border-radius: 48rpx 0 0 48rpx;
  }

  .sku-operate .sku-operate-addCart.disabled {
    background: rgb(221, 221, 221);
    color: #fff;
  }

  .sku-operate .sku-operate-buyNow {
    background-color: $u-type-primary;
    border-radius: 0 48rpx 48rpx 0;
  }

  .sku-operate .sku-operate-buyNow.disabled {
    color: #fff;
    background: rgb(198, 198, 198);
  }

  .sku-operate .selected-sku-btn {
    width: 100%;
  }
}
.popup-container {
  .single-confirm-btn {
    border-radius: 48rpx;
    color: #ffffff;
    margin: 0 32rpx;
    font-size: 32rpx;
    height: 80rpx;
    text-align: center;
    line-height: 88rpx;
    background-color: $u-type-primary;
  }

  .single-confirm-btn.disabled {
    font-size: 32rpx;
    color: #fff;
    background-color: #dddddd;
  }
}
</style>
