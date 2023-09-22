<template>
  <view class="coreshop-copyright">
    <view class="u-font-xs"> Powered by CoreShop </view>
    <view class="beian u-font-xs" v-if="shopBeiAn">
      <view v-if="shopBeiAn"
        ><a
          href="http://beian.miit.gov.cn/"
          target="_blank"
          style="font-size: 24upx"
          >备案号：{{ shopBeiAn }}</a
        ></view
      >
    </view>
  </view>
</template>
<script lant="ts" setup>
import { ref } from "vue";
const shopBeiAn = ref("备案");
</script>
