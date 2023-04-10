<template>
  <view
    class="u-cell"
    :class="{
      'u-border-bottom': borderBottom,
      'u-border-top': borderTop,
      'u-col-center': center,
      'u-cell--required': required
    }"
    hover-stay-time="150"
    :hover-class="hoverClass"
    :style="{
      backgroundColor: bgColor
    }"
    @tap="click"
  >
    <u-icon
      v-if="icon"
      :size="iconSize"
      :name="icon"
      :custom-style="iconStyle"
      class="u-cell__left-icon-wrap"
    />
    <view v-else class="u-flex">
      <slot name="icon" />
    </view>
    <view class="u-cell_title" :style="titleCStyle">
      <slot name="title" />

      <view
        v-if="label || $slots.label"
        class="u-cell__label"
        :style="labelStyle"
      >
        <block v-if="label !== ''">{{ label }}</block>
        <slot v-else name="label" />
      </view>
    </view>

    <view class="u-cell__value" :style="valueStyle">
      <block v-if="value !== ''" class="u-cell__value">{{ value }}</block>
      <slot v-else />
    </view>
    <view v-if="$slots['right-icon']" class="u-flex u-cell_right">
      <slot name="right-icon" />
    </view>
    <u-icon
      v-if="arrow"
      name="arrow-right"
      :style="[arrowStyle]"
      class="u-icon-wrap u-cell__right-icon-wrap"
    />
  </view>
</template>
<script lang="ts" setup>
import { computed } from "vue";
import type { CSSProperties } from "vue";

export interface CellItemProps {
  icon?: string;
  title?: string;
  value?: string | number;
  label?: string | number;
  borderBottom?: boolean;
  borderTop?: boolean;
  hoverClass: string;
  arrow: boolean;
  center: boolean;
  required: boolean;
  titleWidth?: string | number;
  arrowDirection: "right" | "up" | "down";
  titleStyle?: CSSProperties;
  valueStyle?: CSSProperties;
  labelStyle?: CSSProperties;
  bgColor: string;
  index?: string | number;
  useLabelSlot: boolean;
  iconSize: number | string;
  iconStyle?: CSSProperties;
}
const props = withDefaults(defineProps<CellItemProps>(), {
  borderBottom: false,
  hoverClass: "u-cell-hover",
  arrow: true,
  center: false,
  required: false,
  arrowDirection: "right",
  bgColor: "transparent",
  iconSize: 34,
  useLabelSlot: false
});
const emits = defineEmits(["click"]);
function click() {
  emits("click", props.index);
}
const titleCStyle = computed(() => {
  let style: CSSProperties = {};
  if (props.titleStyle) style = props.titleStyle;
  style.width = props.titleWidth ? props.titleWidth + "rpx" : "auto";
  return style;
});

const arrowStyle = computed(() => {
  const style: CSSProperties = {};
  if (props.arrowDirection == "up") style.transform = "rotate(-90deg)";
  else if (props.arrowDirection == "down") style.transform = "rotate(90deg)";
  else style.transform = "rotate(0deg)";
  return style;
});
</script>

<style lang="scss" scoped>
.u-cell {
  display: flex;
  align-items: center;
  position: relative;
  /* #ifndef APP-NVUE */
  box-sizing: border-box;
  /* #endif */
  width: 100%;
  padding: 26rpx 32rpx;
  font-size: 28rpx;
  line-height: 54rpx;
  color: $u-content-color;
  background-color: #fff;
  text-align: left;
}

.u-cell_title {
  font-size: 28rpx;
}

.u-cell__left-icon-wrap {
  margin-right: 10rpx;
  font-size: 32rpx;
}

.u-cell__right-icon-wrap {
  margin-left: 10rpx;
  color: #969799;
  font-size: 28rpx;
}

.u-cell__left-icon-wrap,
.u-cell__right-icon-wrap {
  display: flex;
  align-items: center;
  height: 48rpx;
}

.u-cell-border:after {
  position: absolute;
  /* #ifndef APP-NVUE */
  box-sizing: border-box;
  content: " ";
  pointer-events: none;
  border-bottom: 1px solid $u-border-color;
  /* #endif */
  right: 0;
  left: 0;
  top: 0;
  transform: scaleY(0.5);
}

.u-cell-border {
  position: relative;
}

.u-cell__label {
  margin-top: 6rpx;
  font-size: 26rpx;
  line-height: 36rpx;
  color: $u-tips-color;
  /* #ifndef APP-NVUE */
  word-wrap: break-word;
  /* #endif */
}

.u-cell__value {
  overflow: hidden;
  text-align: right;
  /* #ifndef APP-NVUE */
  vertical-align: middle;
  /* #endif */
  color: $u-tips-color;
  font-size: 26rpx;
}

.u-cell__title,
.u-cell__value {
  flex: 1;
}

.u-cell--required {
  /* #ifndef APP-NVUE */
  overflow: visible;
  /* #endif */
  display: flex;
  align-items: center;
}

.u-cell--required:before {
  position: absolute;
  /* #ifndef APP-NVUE */
  content: "*";
  /* #endif */
  left: 8px;
  margin-top: 4rpx;
  font-size: 14px;
  color: $u-type-error;
}

.u-cell_right {
  line-height: 1;
}
</style>
