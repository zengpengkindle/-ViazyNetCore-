<template>
  <view ref="root" class="van-sidebar custom-class">
    <slot />
  </view>
</template>
<script lang="ts" setup>
import { onMounted, ref, getCurrentInstance, provide, watch } from "vue";
import { useOrderedChildren } from "../hooks/use-ordered-children";
import { type SidebarItemContext, sidebarContextKey } from "./constants";

export interface SidebarProps {
  modelValue: number;
}
const props = withDefaults(defineProps<SidebarProps>(), { modelValue: 0 });
const currentActive = ref(-1);
const {
  children: items,
  addChild: addItem,
  removeChild: removeItem
} = useOrderedChildren<SidebarItemContext>(
  getCurrentInstance()!,
  "SidebarItem"
);

onMounted(() => {
  //   const hasLabel = computed(() => {
  //     return items.value.some(item => item.props.label.toString().length > 0);
  //   });
  currentActive.value = props.modelValue;
});
const emits = defineEmits(["change", "update:modelValue"]);
function setActiveItem(activeKey: number) {
  if (!items.value.length) {
    return;
  }

  if (currentActive.value !== activeKey && items.value[currentActive.value]) {
    items.value[currentActive.value].setActive(false);
  }
  if (items.value[activeKey]) {
    items.value[activeKey].setActive(true);
  }
  currentActive.value = activeKey;
}
watch(
  () => currentActive.value,
  value => {
    emits("change", value);
    emits("update:modelValue", value);
  }
);
watch(
  () => items.value,
  () => {
    if (items.value.length > 0) setActiveItem(props.modelValue);
  }
);
defineExpose({});
const root = ref<HTMLElement>();
provide(sidebarContextKey, {
  root,
  items,
  active: props.modelValue,
  addItem,
  removeItem,
  setActiveItem
});
</script>
<style scoped lang="scss">
.van-sidebar {
  width: var(--sidebar-width, $sidebar-width);
}
</style>
