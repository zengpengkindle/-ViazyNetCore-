<template>
    <view class="sidebar-item" @click="handleItemClick" hover-class="sidebar-item--hover">
        <view class="sidebar-item__text">
            <view v-if="title">{{ title }}</view>
            <slot v-if="!title" name="title" />
        </view>
    </view>
</template>
<script lang="ts" setup>
import {
    defineAsyncComponent,
    getCurrentInstance,
    ref,
    nextTick,
    provide,
    onMounted,
    onBeforeMount,
    inject
} from 'vue'; // 父组件
import { sidebarContextKey } from './constants'

export interface SidebarItemProps {
    title?: string;
    dot?: boolean;
    info?: string;
    disabled?: boolean;
}
const instance = getCurrentInstance();
const sidebarContext = inject(sidebarContextKey)!
onMounted(() => {
    // sidebarContext.addItem({
    //     props,
    //     uid: instance.uid,
    //     translateItem,
    // })
})
const translateItem = (
    index: number,
    activeIndex: number,
    oldIndex?: number
) => {
    const sidebarItemLength = sidebarContext.items.value.length ?? Number.NaN

    const isActive = index === activeIndex

    // if (!isActive && sidebarItemLength > 2 && sidebarContext.loop) {
    //   index = processIndex(index, activeIndex, sidebarItemLength)
    // }
}
onBeforeMount(() => {
    //parent.children.push(instance);
})
const parent = instance.appContext.config.globalProperties.$u.$parent;
onMounted(() => {
    // parent.children.push(instance);
})
const emits = defineEmits(["click"]);
var props = defineProps<SidebarItemProps>();
function onClick() {
    if (props.disabled) return;
    // const index = parent.children.indexOf(instance);
    // nextTick(() => {
    //     emits('click', index);
    //     //parent.emits('change');
    // });

}
function handleItemClick() {
    if (sidebarContext) {
        const index = sidebarContext.items.value.findIndex(
            ({ uid }) => uid === instance.uid
        )
        sidebarContext.setActiveItem(index)
    }
}
defineExpose({

})
</script>
<style lang="scss" scoped>
@import './index.scss';
</style>