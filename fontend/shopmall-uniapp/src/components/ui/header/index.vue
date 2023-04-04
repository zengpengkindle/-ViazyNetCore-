<template>
  <view class="header-main">
    <view
      class="haeder-status-bar"
      :style="{ height: statusBarHeight + 'px' }"
    />
    <view
      class="x-header"
      :class="{ 'sticky-top': stickyTop }"
      :style="{
        ...rectStyle,
        ...headerStyle
      }"
    >
      <slot>
        <slot name="content">
          <view
            class="title ellipsis"
            :style="{
              top: rect.top + rect.height / 2 + 'px',
              color: titleColor
            }"
          >
            <slot name="title">
              {{ title }}
            </slot>
          </view>
        </slot>
      </slot>
    </view>
  </view>
</template>

<script lang="ts" setup>
import { getCurrentInstance, onMounted, reactive, ref } from "vue";
import type { CSSProperties, PropType } from "vue";
import { useHeader } from "../hooks/user-head";

const props = defineProps({
  title: {
    type: String,
    default: ""
  },
  titleColor: {
    type: String,
    default: "#000"
  },
  stickyTop: {
    type: Boolean,
    default: true
  },
  headerStyle: {
    type: Object as PropType<CSSProperties>,
    default: () => ({})
  },
  beforeBackClick: {
    type: Function as PropType<() => boolean | void>,
    default() {
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      return () => {};
    }
  },
  boundingRect: {
    type: Boolean,
    default: false
  }
});
const systemInfo = uni.getSystemInfoSync();
const statusBarHeight = ref(systemInfo.statusBarHeight);

const rect = uni.getMenuButtonBoundingClientRect();
const rectStyle = reactive({});

const { boundingRect } = useHeader();
onMounted(() => {
  if (props.boundingRect) {
    uni
      .createSelectorQuery()
      .in(getCurrentInstance())
      .select(".header-main")
      .boundingClientRect(res => {
        boundingRect.value.width = (res as UniApp.NodeInfo).width || 0;
        boundingRect.value.height = (res as UniApp.NodeInfo).height || 0;
        boundingRect.value.left = (res as UniApp.NodeInfo).left || 0;
        boundingRect.value.right = (res as UniApp.NodeInfo).right || 0;
        boundingRect.value.top = (res as UniApp.NodeInfo).top || 0;
        boundingRect.value.bottom = (res as UniApp.NodeInfo).bottom || 0;
      })
      .exec();
  }
});
</script>

<style lang="scss" scoped>
.header-main {
  z-index: 90074;
}
.x-header {
  width: 100vw;
  position: relative;
  padding-bottom: 6px;
  display: flex;
  align-items: center;
  &.sticky-top {
    position: sticky;
    top: 0;
  }
  .back-icon {
    margin-left: 10px;
    width: 24px;
    height: 24px;
  }
  .title {
    max-width: 320rpx;
    font-weight: 400;
    font-size: 16px;
    line-height: 22px;
    letter-spacing: 0.05em;
    color: #000;
    position: absolute;
    left: 50%;
    transform: translateX(-50%) translateY(-50%);
  }
}
</style>
