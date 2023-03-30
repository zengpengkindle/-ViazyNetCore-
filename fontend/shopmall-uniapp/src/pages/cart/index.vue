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
                <u-checkbox v-model="item.check" shape="circle" />
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
                      <price
                        :price="item.price"
                        symbol="¥"
                        :bold="true"
                        decimal-smaller
                        type="lighter"
                      />
                      <!-- <price :price="item.mktprice" symbol="¥" type="del" /> -->
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
    <cart-bar
      v-model="allcheck"
      :total-discount-amount="0"
      :total-goods-num="totalCount"
      :fixed="true"
      :total-amount="totalAmount"
      @handle-select-all="checkAll"
    />
  </view>
</template>

<script setup lang="ts">
import { ref, type Ref, onMounted, computed, nextTick, watch } from "vue";
import ShopCartApi, {
  type ShoppingCart,
  type ShoppingCartPackage
} from "@/apis/shopmall/productCart";
import CartBar from "./components/cart-bar/index.vue";
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
const totalCount = computed(() => {
  let count = 0;
  carts.value.packages.forEach(pack => {
    pack.items.forEach(item => {
      if (item.check) {
        count += item.num;
      }
    });
  });
  return count;
});
const totalAmount = computed(() => {
  let amount = 0;
  carts.value.packages.forEach(pack => {
    pack.items.forEach(item => {
      if (item.check) {
        amount += item.price * item.num;
      }
    });
  });
  return amount;
});
watch(
  () => carts.value,
  new_carts => {
    let check = true;
    new_carts.packages.forEach(pack => {
      let shopcheck = true;
      pack.items.forEach(item => {
        check &&= item.check;
        shopcheck &&= item.check;
      });
      pack.check = shopcheck;
      allcheck.value = check;
    });
  },
  { deep: true }
);
const allcheck = ref(false);
watch(
  () => allcheck.value,
  () => {
    checkAll();
  }
);
function checkAll() {
  carts.value.packages.forEach(pack => {
    pack.check = allcheck.value;
    checkShop(pack);
  });
}

function checkShop(shop: ShoppingCartPackage) {
  nextTick(() => {
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
  });
}
</script>

<style>
@import "./cart.scss";
</style>
