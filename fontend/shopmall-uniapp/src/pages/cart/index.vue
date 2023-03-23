<template>
  <view
    class="content"
    :style="{
      backgroundSize: '100vw 520rpx',
      backgroundRepeat: 'no-repeat'
    }"
  >
    <view v-if="carts.num" class="cart-main trade-main">
      <view class="container">
        <view
          v-for="shop in carts.packages"
          :key="shop.shopId"
          class="trade-card"
        >
          <view class="trade-card-header">
            <view class="trade-card-radio">
              <u-checkbox
                v-model="shop.check"
                shape="circle"
                @change="checkShop(shop)"
              />
            </view>
            <view class="trade-card-header-title">{{ shop.shopName }}</view>
          </view>
          <view class="trade-card-body">
            <view
              v-for="item in shop.items"
              :key="item.id"
              class="trade-card-items"
            >
              <view class="trade-card-radio">
                <u-checkbox
                  v-model="item.check"
                  shape="circle"
                  @change="checkItem(item, shop)"
                />
              </view>
              <view class="trade-card-item">
                <view class="trade-card-item_image">
                  <u-image width="100%" height="100%" :src="item.imgUrl" />
                </view>
                <view class="trade-card-item_content">
                  <view class="trade-card-title">
                    {{ item.pn }}
                  </view>
                  <view class="trade-card-desc">
                    {{ item.skuText }}
                  </view>
                  <view class="trade-card-bottom">
                    <view class="trade-card-price">
                      ￥<text>{{ item.price }}</text>
                    </view>
                    <u-number-box v-model="item.num" :min="1" integer />
                  </view>
                </view>
              </view>
            </view>
          </view>
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, type Ref, onMounted } from "vue";
import ShopCartApi, {
  type ShoppingCart,
  type ShoppingCartPackage,
  type ShoppingCartProduct
} from "@/apis/shopmall/productCart";
onMounted(() => {
  getCarts();
});
const getCarts = async () => {
  carts.value = await ShopCartApi.findCart();
};
const carts: Ref<ShoppingCart> = ref({
  num: 0,
  propertyKeys: "",
  propertyValues: "",
  packages: []
});

function checkShop(shop: ShoppingCartPackage) {
  if (shop.check) {
    shop.items.forEach(item => {
      item.check = shop.check;
    });
  } else {
    let check = true;
    shop.items.forEach(item => {
      if (!item.check) check = false;
    });
    if (check) {
      shop.items.forEach(item => {
        item.check = false;
      });
    }
  }
}

function checkItem(item: ShoppingCartProduct, shop: ShoppingCartPackage) {
  if (item.check) {
    if (shop.items.length == 1) {
      shop.check = true;
    } else {
      let check = true;
      shop.items.forEach(item => {
        if (!item.check) check = false;
      });
      shop.check = check;
    }
  } else {
    shop.check = false;
  }
}
</script>

<style>
@import "./cart.scss";
</style>
