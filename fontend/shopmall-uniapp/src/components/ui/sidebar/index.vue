<template>
    <view ref="root" class="van-sidebar custom-class">
        <slot />
    </view>
</template>
<script lang="ts" setup>
import { onMounted, ref, getCurrentInstance, computed, provide } from 'vue';
import { useOrderedChildren } from '../hooks/use-ordered-children'
import { type SidebarItemContext, sidebarContextKey } from './constants';

export interface SidebarProps {
    activeKey: number;
}
const props = withDefaults(defineProps<SidebarProps>(), {
    activeKey: 0
});
const currentActive = ref(-1);
const {
    children: items,
    addChild: addItem,
    removeChild: removeItem,
} = useOrderedChildren<SidebarItemContext>(
    getCurrentInstance()!,
    'SidebarItem'
)

onMounted(() => {

    const hasLabel = computed(() => {
        return items.value.some((item) => item.props.label.toString().length > 0)
    })
    setActiveItem(props.activeKey);
})

function setActiveItem(activeKey: number) {

    if (!items.value.length) {
        return Promise.resolve();
    }

    currentActive.value = activeKey;
    console.log(currentActive.value);
    //  const stack: Promise<unknown>[] = [];

    // if (currentActive.value !== activeKey && items.value[currentActive.value]) {
    //     stack.push(items.value[currentActive.value].setActiveItem(false));
    // }

    // if (items.value[activeKey]) {
    //     stack.push(items.value[activeKey].setActiveItem(true));
    // }

    // return Promise.all(stack);
}
defineExpose({
    setActiveItem
})
const root = ref<HTMLElement>();
provide(sidebarContextKey, {
    root,
    loop: false,
    items,
    addItem,
    removeItem,
    setActiveItem
})
</script>
<style scoped lang="scss">
.van-sidebar {
    width: var(--sidebar-width, $sidebar-width);
}
</style>