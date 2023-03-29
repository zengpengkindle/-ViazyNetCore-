<template>
  <view class="content">
    <image-tabs
      v-model="catId"
      :list="catLists"
      item-width="150"
      :gutter="18"
    />
    <view class="sidebar-content">
      <scroll-view
        class="left"
        scroll-x="false"
        scroll-y="true"
        style="height: calc(100vh - 92px)"
        upper-threshold="50"
        lower-threshold="50"
        scroll-top="0"
        scroll-left="0"
        scroll-into-view=""
        scroll-with-animation="false"
        enable-back-to-top="false"
        bindscrolltoupper=""
        bindscrolltolower=""
        bindscroll=""
      >
        <sidebar v-model="current" @change="change">
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
          <sidebar-item title="标签名" />
        </sidebar>
      </scroll-view>
      <view clsss="sub-cat"> ccccc {{ catId }} - {{ current }}</view>
    </view>
  </view>
</template>

<script setup lang="ts">
import Sidebar from "@/components/ui/sidebar/index.vue";
import ImageTabs from "@/components/ui/image-tabs/index.vue";
import SidebarItem from "@/components/ui/sidebar-item/index.vue";
import { ref, type Ref } from "vue";
const catId = ref(0);
const current: Ref<number> = ref(0);
interface CatItem {
  id: string;
  image: string;
  text: string;
}
const catLists: Array<CatItem> = [
  { id: "1", image: "/static/images/cat/img-1.webp", text: "男服饰" },
  { id: "2", image: "/static/images/cat/img-9.webp", text: "女装" },
  { id: "3", image: "/static/images/cat/muy-3b.webp", text: "儿童磨绒" },
  { id: "4", image: "/static/images/cat/img-1.webp", text: "服饰" },
  { id: "5", image: "/static/images/cat/img-9.webp", text: "服饰" },
  { id: "6", image: "/static/images/cat/muy-3b.webp", text: "服饰" },
  { id: "6", image: "/static/images/cat/img-1.webp", text: "服饰" },
  { id: "7", image: "/static/images/cat/img-9.webp", text: "服饰" },
  { id: "8", image: "/static/images/cat/muy-3b.webp", text: "服饰" }
];
function change(index: number) {
  // current.value = index;
  console.log(index);
}
</script>

<style lang="scss">
.cat-card-item {
  display: inline-block;
  width: 110rpx;
  text-align: center;
  margin-left: 10rpx;
}
.sidebar-content {
  display: flex;

  .left {
    flex: 0;
  }

  .sub-cat {
    flex: 1;
  }
}
</style>
