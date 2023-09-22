<template>
  <view>
    <block v-for="(item, index) in prop.data" :key="index">
      <!--空格-->
      <blank v-if="item.type == 'blank'" :parameters="item.value" />
      <!--搜索-->
      <search v-if="item.type == 'search'" :parameters="item.value" />
      <goods v-if="item.type == 'goods'" :parameters="item.value" />
      <image-slide v-if="item.type == 'imgSlide'" :parameters="item.value" />
      <image-window v-if="item.type == 'imgWindow'" :parameters="item.value" />
      <navbar v-if="item.type == 'navBar'" :parameters="item.value" />
    </block>
  </view>
</template>
<script lang="ts" setup>
import Blank from "./blank.vue";
import Search from "./Search.vue";
import Goods from "./goods.vue";
import ImageSlide from "./image-slide.vue";
import ImageWindow from "./image-window.vue";
import Navbar from "./navbar.vue";
export interface ShopData {
  value: any;
  type: string;
}
export interface ShopDataProps {
  data: Array<ShopData>;
}
const prop = defineProps<ShopDataProps>();
</script>
