<template>
  <view :style="customStyle" class="u-icon" :class="['u-icon--' + labelPos]">
    <image class="u-icon__img" :src="url" :mode="imgMode" :style="imgStyle" />
    <text
      v-if="label !== '' && label !== null"
      :style="decimalIconStyle"
      :class="decimalIconClass"
      :hover-class="hoverClass"
      class="u-icon__label"
      >{{ label }}
    </text>
  </view>
</template>
<script lang="ts" setup>
import { computed, defineComponent, type CSSProperties } from "vue";
import addUnit from "../utils/addUint";
export interface IconProps {
  url: string;
  color?: string;
  size?: number | string;
  bold?: boolean;
  hoverClass?: string;
  label?: string | number;
  labelPos?: "right" | "top" | "bottom";
  labelSize?: number | string;
  labelColor?: string;
  marginTop?: number | string;
  imgMode?: "widthFix";
  customStyle?: CSSProperties;
  width: string | number;
  height?: string | number;
  top?: string | number;
  percent: number;
}
const props = withDefaults(defineProps<IconProps>(), {
  size: "inherit",
  labelPos: "bottom",
  labelSize: 15,
  labelColor: "#606266",
  marginTop: 6,
  imgMode: "widthFix",
  width: "auto",
  percent: 50
});
defineComponent({
  name: "Icon"
});
const decimalIconStyle = computed(() => {
  let style: CSSProperties = {};
  style = {
    fontSize:
      props.labelSize == "inherit" ? "inherit" : addUnit(props.labelSize),
    fontWeight: props.bold ? "bold" : "normal",
    // 某些特殊情况需要设置一个到顶部的距离，才能更好的垂直居中
    top: addUnit(props.top)
  };
  // 非主题色值时，才当作颜色值
  style.color = props.color;
  return style;
});
const decimalIconClass = computed(() => {
  const classes = "";
  return classes;
});
const imgStyle = computed(() => {
  const style: CSSProperties = {};
  // 如果设置width和height属性，则优先使用，否则使用size属性
  style.width = props.width ? addUnit(props.width) : addUnit(props.size);
  style.height = props.height ? addUnit(props.height) : addUnit(props.size);
  return style;
});
</script>
<style scoped lang="scss">
.u-icon {
  display: inline-flex;
  align-items: center;

  &--left {
    flex-direction: row-reverse;
    align-items: center;
  }

  &--right {
    flex-direction: row;
    align-items: center;
  }

  &--top {
    flex-direction: column-reverse;
    justify-content: center;
  }

  &--bottom {
    flex-direction: column;
    justify-content: center;
  }

  &__icon {
    position: relative;
  }

  &__decimal {
    position: absolute;
    top: 0;
    left: 0;
    display: inline-block;
    overflow: hidden;
  }

  &__img {
    height: auto;
    border-radius: 32rpx;
    border: 1px solid transparent;
    will-change: transform;
  }

  &__label {
    display: inline-block;
    margin-top: 10rpx;
    padding: 3rpx;
    border-radius: 7px;
    text-align: center;
    line-height: 1;
  }
}
.active .u-icon__label {
  border-radius: 7px;
  color: #ffffff;
  background-color: var(--sidebar-active-color, $sidebar-active-color);
}
</style>
