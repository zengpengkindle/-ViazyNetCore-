<template>
  <view
    class="header-main"
    :class="{ 'sticky-top': stickyTop }"
    :style="{
      ...rectStyle,
      ...headerStyle
    }"
  >
    <view
      class="haeder-status-bar"
      :style="{ height: statusBarHeight + 'px' }"
    />
    <view class="x-header">
      <slot>
        <slot name="icon">
          <view class="back-icon">
            <u-icon
              color="#000000"
              name="nav-back"
              :size="44"
              @click="backClick"
            />
          </view>
        </slot>
        <slot name="content">
          <view
            class="title ellipsis"
            :style="{
              top: rect.height / 2 + 'px',
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
import { getCurrentInstance, reactive, ref } from "vue";
import type { CSSProperties, PropType } from "vue";
import { useHeader, GetRect } from "../hooks/user-head";
import { onShow } from "@dcloudio/uni-app";

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
const emit = defineEmits(["back-click"]);

const systemInfo = uni.getSystemInfoSync();
const statusBarHeight = ref(systemInfo.statusBarHeight);

const rect = uni.getMenuButtonBoundingClientRect();
const rectStyle = reactive({});
const backClick = () => {
  emit("back-click");
  if (!props.beforeBackClick()) {
    return;
  }
  if (getCurrentPages().length > 1) {
    uni.navigateBack();
  } else {
    uni.switchTab({
      url: "/pages/index/custom/index"
    });
  }
};
const { boundingRect } = useHeader();
onShow(async () => {
  if (props.boundingRect) {
    const res = (await GetRect(
      getCurrentInstance(),
      ".header-main"
    )) as UniApp.NodeInfo;

    boundingRect.value.width = res.width || 0;
    boundingRect.value.height = res.height || 0;
    boundingRect.value.left = res.left || 0;
    boundingRect.value.right = res.right || 0;
    boundingRect.value.top = res.top || 0;
    boundingRect.value.bottom = res.bottom || 0;
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
  padding: 6px;
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
    flex: 1;
    text-align: center;
    max-width: 320rpx;
    font-weight: 400;
    font-size: 16px;
    line-height: 22px;
    letter-spacing: 0.05em;
    color: #000;
    position: absolute;
    top: 0;
    left: 50%;
    transform: translateX(-50%) translateY(-50%);
  }
}
</style>
