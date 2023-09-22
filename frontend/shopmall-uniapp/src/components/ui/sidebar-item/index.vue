<template>
  <view
    :class="{
      'sidebar-item--selected': selected,
      'sidebar-item--disabled': disabled
    }"
    class="sidebar-item"
    @click="handleItemClick"
  >
    <view class="sidebar-item__text">
      <view v-if="title">{{ title }}</view>
      <slot v-if="!title" name="title" />
    </view>
  </view>
</template>
<script lang="ts" setup>
import { getCurrentInstance, onMounted, inject, ref, onUnmounted } from "vue"; // 父组件
import { sidebarContextKey } from "../sidebar/constants";

export interface SidebarItemProps {
  title?: string;
  dot?: boolean;
  info?: string;
  disabled?: boolean;
}
const props = withDefaults(defineProps<SidebarItemProps>(), {});

const instance = getCurrentInstance();
const sidebarContext = inject(sidebarContextKey);
const cindex = ref(0);
onMounted(() => {
  sidebarContext.items.value.push({
    props,
    uid: instance.uid,
    translateItem,
    setActive
  });
  const index = sidebarContext.items.value.findIndex(
    ({ uid }) => uid === instance.uid
  );
  cindex.value = index;
  selected.value = sidebarContext.active == index;
});
onUnmounted(() => {
  const index = sidebarContext.items.value.findIndex(
    ({ uid }) => uid === instance.uid
  );
  sidebarContext.items.value.splice(index, 1);
});

const translateItem = (index: number, activeIndex: number) => {
  const sidebarItemLength = sidebarContext.items.value.length ?? Number.NaN;

  const isActive = index === activeIndex;

  if (!isActive && sidebarItemLength > 2) {
    // index = processIndex(index, activeIndex, sidebarItemLength)
  }
};

function handleItemClick() {
  if (props.disabled) return;
  if (sidebarContext) {
    const index = sidebarContext.items.value.findIndex(
      ({ uid }) => uid === instance.uid
    );
    sidebarContext.setActiveItem(index);
  }
}

const selected = ref(false);
function setActive(isSelected: boolean) {
  selected.value = isSelected;
}
defineExpose({});
</script>
<style lang="scss" scoped>
@import "./index.scss";
</style>
