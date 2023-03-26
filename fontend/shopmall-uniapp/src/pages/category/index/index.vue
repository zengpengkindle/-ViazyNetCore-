<template>
  <view class="content">
    <sidebar class="left" :active-key="current">
      <sidebar-item title="标签名" />
      <sidebar-item title="标签名" disabled />
      <sidebar-item title="标签名" />
    </sidebar>
    <view clsss="sub-cat">
      ccccc
    </view>
  </view>
</template>

<script setup lang="ts">
import Sidebar from "@/components/ui/sidebar/index.vue";
import SidebarItem from "@/components/ui/sidebar/sidebar-item.vue";
import { ref, type Ref } from "vue";

const current: Ref<number> = ref(0);

function change() { }
</script>

<style lang="scss">
.content {
  display: flex;

  .left {
    flex: 0;
  }

  .sub-cat {
    flex: 1
  }
}
</style>
