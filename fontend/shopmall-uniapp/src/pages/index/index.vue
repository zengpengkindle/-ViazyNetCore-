<template>
  <view class="content">
    <u-navbar back-text="返回" title="剑未配妥，出门已是江湖"></u-navbar>
    <image class="logo" src="/static/logo.png" />
    <view class="text-area">
      <text class="title">{{ title }}</text>
    </view>
    <u-tabs
      :list="list"
      :is-scroll="true"
      v-model="current"
      @change="change"
    ></u-tabs>
  </view>
</template>

<script setup lang="ts">
import { ref, type Ref } from "vue";
const title = ref("Hello");
interface Item {
  name: string;
  count?: number;
}
const list: Ref<Array<Item>> = ref([
  { name: "待收货" },
  {
    name: "待付款",
  },
  {
    name: "待评价",
    count: 5,
  },
]);
const current: Ref<number> = ref(0);

function change() {}
</script>

<style>
.content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.logo {
  height: 200rpx;
  width: 200rpx;
  margin-top: 200rpx;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 50rpx;
}

.text-area {
  display: flex;
  justify-content: center;
}

.title {
  font-size: 36rpx;
  color: #8f8f94;
}
</style>
