<template>
  <view class="u-padding-top-10 u-padding-bottom-10 u-margin-15 bg-white">
    <u-grid :col="limit" :border="false">
      <u-grid-item
        v-for="(item, index) in ListData"
        :key="index"
        @click="showSliderInfo(item.linkType, item.linkValue)"
      >
        <u-icon
          :name="item.url || item.image"
          width="80"
          height="80"
          :label="item.text"
          :label-size="26"
          label-pos="bottom"
          margin-top="25"
        />
      </u-grid-item>
    </u-grid>
  </view>
</template>
<script lang="ts" setup>
import { computed, onMounted, ref } from "vue";

export interface NavbarParameter {
  list: Array<any>;
  limit?: number;
}
export interface NavbarProps {
  parameters: NavbarParameter;
}
const prop = defineProps<NavbarProps>();
const ListData = ref([]);
onMounted(() => {
  ListData.value = prop.parameters.list;
});
const limit = computed(() => prop.parameters.limit);
const showSliderInfo = (type: any, val: any) => {
  if (type == 1) {
    uni.navigateTo({ url: val });
  }
};
</script>
