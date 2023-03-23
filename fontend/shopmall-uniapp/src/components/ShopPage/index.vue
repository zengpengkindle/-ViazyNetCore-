<template>
  <view>
    <block v-for="(item, index) in prop.data" :key="index">
      <!--空格-->
      <blank v-if="item.widgetCode == 'blank'" :parameters="item.parameters" />
      <!--搜索-->
      <search
        v-if="item.widgetCode == 'search'"
        :parameters="item.parameters"
      />
    </block>
  </view>
</template>
<script lang="ts" setup>
import Blank from "./Blank.vue";
import Search from "./Search.vue";
export interface ShopData {
  parameters: any;
  widgetCode: string;
}
export interface ShopDataProps {
  data: Array<ShopData>;
}
const prop = defineProps<ShopDataProps>();
</script>
