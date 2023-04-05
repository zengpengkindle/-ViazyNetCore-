<template>
  <view class="u-skeleton">
    <view class="product-image-wrap u-skeleton-rect">
      <u-swiper :list="imageList" :height="imageHeight * 2" />
    </view>
    <view style="margin: 20px 0">
      <view v-for="(item, index) in 2" :key="index" class="lists">
        <!--u-skeleton-rect 绘制矩形-->
        <text class="u-skeleton-rect">{{ item }}</text>
      </view>
    </view>
    <u-skeleton :loading="loading" :animation="true" bg-color="#FFF" />
  </view>
</template>
<script lang="ts" setup>
import ProductApi, { type ProductInfoModel } from "@/apis/shopmall/product";
import { useHeader, GetRect } from "@/components/ui/hooks/user-head";
import { onLoad } from "@dcloudio/uni-app";
import { getCurrentInstance, onMounted } from "vue";
import { ref, type Ref } from "vue";
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
const loading = ref(true);
const imageList: Ref<Array<string>> = ref([]);
const prodcutItem: Ref<ProductInfoModel> = ref();
const getDetail = async (id: string) => {
  prodcutItem.value = await ProductApi.getProductInfo(id);
  imageList.value = [prodcutItem.value.image];
  if (prodcutItem.value.subImage != null && prodcutItem.value.subImage != "") {
    imageList.value.push(...prodcutItem.value.subImage.split(";"));
  }
  loading.value = false;
};
</script>
