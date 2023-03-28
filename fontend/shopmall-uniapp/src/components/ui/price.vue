<template>
  <view class="price {{type}} wr-class">
    <view
      wx:if="{{type === 'delthrough'}}"
      class="line"
      :style="{ height: addUnit(lineThroughWidth) }"
    />
    <view class="symbol symbol-class">{{ symbol }}</view>
    <view class="pprice">
      <view class="integer inline">{{ prices[0] }}</view>
      <view
        v-if="prices[1]"
        class="decimal inline decimal-class"
        :class="decimalSmaller ? 'smaller' : ''"
        >.{{ prices[1] }}</view
      >
    </view>
  </view>
</template>
<script lang="ts" setup>
import type { HeightProperty } from "csstype";
import { ref, watch } from "vue";

export interface PriceProps {
  priceUnit: String;
  price: String | number;
  type: "main" | "lighter" | "mini" | "del" | "delthrough";
  symbol: String;
  fill: boolean;
  decimalSmaller: boolean;
  lineThroughWidth: string;
}
const props = defineProps<PriceProps>();

const REGEXP = /^\d+(\.\d+)?$/;
const addUnit: (val: string) => HeightProperty<string | number> = (
  value: string
) => {
  if (value == null) {
    return "";
  }
  return REGEXP.test("" + value) ? value + "rpx" : value;
};
const prices = ref([]);
watch(
  () => props.price,
  () => {
    format(props.price);
  }
);
const format = (price: String | number) => {
  price = parseFloat(`${price}`);
  const pArr = [];
  if (!isNaN(price)) {
    const isMinus = price < 0;
    if (isMinus) {
      price = -price;
    }
    if (props.priceUnit === "yuan") {
      const priceSplit = price.toString().split(".");
      pArr[0] = priceSplit[0];
      pArr[1] = !priceSplit[1]
        ? "00"
        : priceSplit[1].length === 1
        ? `${priceSplit[1]}0`
        : priceSplit[1];
    } else {
      price = Math.round(price * 10 ** 8) / 10 ** 8; // 恢复精度丢失
      price = Math.ceil(price); // 向上取整
      pArr[0] = price >= 100 ? `${price}`.slice(0, -2) : "0";
      pArr[1] = `${price + 100}`.slice(-2);
    }
    if (!props.fill) {
      // 如果 fill 为 false， 不显示小数末尾的0
      if (pArr[1] === "00") pArr[1] = "";
      else if (pArr[1][1] === "0") pArr[1] = pArr[1][0];
    }
    if (isMinus) {
      pArr[0] = `-${pArr[0]}`;
    }
  }
  prices.value = pArr;
};
defineExpose({
  addUnit
});
</script>
