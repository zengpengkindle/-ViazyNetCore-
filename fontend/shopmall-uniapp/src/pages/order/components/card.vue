<template>
  <view
    class="u-card"
    :class="{
      'u-border': border,
      'u-card-full': full,
      'u-card--border': borderRadius > 0
    }"
    :style="{
      borderRadius: borderRadius + 'rpx',
      margin: margin,
      boxShadow: boxShadow
    }"
    @tap.stop="click"
  >
    <view
      v-if="showHead"
      class="u-card__head"
      :style="{ padding: padding + 'rpx' }"
      :class="{
        'u-border-bottom': headBorderBottom
      }"
      @tap="headClick"
    >
      <view v-if="!$slots.head" class="u-flex u-row-between">
        <view class="u-card__head--left u-flex u-line-1" v-if="title">
          <image
            v-if="thumb"
            :src="thumb"
            class="u-card__head--left__thumb"
            mode="aspectFill"
            :style="{
              height: thumbWidth + 'rpx',
              width: thumbWidth + 'rpx',
              borderRadius: thumbCircle ? '100rpx' : '6rpx'
            }"
          />
          <text
            class="u-card__head--left__title u-line-1"
            :style="{
              fontSize: titleSize + 'rpx',
              color: titleColor
            }"
          >
            {{ title }}
          </text>
        </view>
        <view class="u-card__head--right u-line-1" v-if="subTitle">
          <text
            class="u-card__head__title__text"
            :style="{
              fontSize: subTitleSize + 'rpx',
              color: subTitleColor
            }"
          >
            {{ subTitle }}
          </text>
        </view>
      </view>
      <slot v-else name="head" />
    </view>
    <view
      :style="{ padding: padding + 'rpx' }"
      class="u-card__body"
      @tap="bodyClick"
    >
      <slot name="body" />
    </view>
    <view
      v-if="showFoot"
      class="u-card__foot"
      :style="{ padding: $slots.foot ? padding + 'rpx' : 0 }"
      :class="{
        'u-border-top': footBorderTop
      }"
      @tap="footClick"
    >
      <slot name="foot" />
    </view>
  </view>
</template>
<script lang="ts" setup>
import type { CSSProperties } from "vue";

export interface CardProps {
  full: boolean;
  // 标题
  title: string;
  // 标题颜色
  titleColor: string;
  // 标题字体大小，单位rpx
  titleSize: number | string;
  // 副标题
  subTitle?: string;
  // 副标题颜色
  subTitleColor: string;
  // 副标题字体大小，单位rpx
  subTitleSize: number | string;
  // 是否显示外部边框，只对full=false时有效(卡片与边框有空隙时)
  border: boolean;
  // 用于标识点击了第几个
  index?: number | string | object;
  // 用于隔开上下左右的边距，带单位的写法，如："30rpx 30rpx"，"20rpx 20rpx 30rpx 30rpx"
  margin: string;
  // card卡片的圆角
  borderRadius: number;
  // 头部自定义样式，对象形式
  headStyle?: CSSProperties;
  // 主体自定义样式，对象形式
  bodyStyle?: CSSProperties;
  // 底部自定义样式，对象形式
  footStyle?: CSSProperties;
  // 头部是否下边框
  headBorderBottom: boolean;
  // 底部是否有上边框
  footBorderTop: boolean;
  // 标题左边的缩略图
  thumb?: string;
  // 缩略图宽高，单位rpx
  thumbWidth: string | number;
  // 缩略图是否为圆形
  thumbCircle: boolean;
  // 给head，body，foot的内边距
  padding: string | number;
  // 是否显示头部
  showHead: boolean;
  // 是否显示尾部
  showFoot: boolean;
  // 卡片外围阴影，字符串形式
  boxShadow: string;
}
const props = withDefaults(defineProps<CardProps>(), {
  titleColor: "#303133",
  titleSize: 30,
  subTitleColor: "#909399",
  subTitleSize: 26,
  border: true,
  margin: "30rpx",
  borderRadius: 16,
  headBorderBottom: true,
  footBorderTop: true,
  thumbWidth: 60,
  padding: 30,
  showHead: true,
  boxShadow: "none"
});
const emits = defineEmits(["click", "head-click", "body-click", "foot-click"]);
function click() {
  emits("click", props.index);
}
function headClick() {
  emits("head-click", props.index);
}
function bodyClick() {
  emits("body-click", props.index);
}
function footClick() {
  emits("foot-click", props.index);
}
</script>

<style lang="scss" scoped>
.u-card {
  position: relative;
  overflow: hidden;
  font-size: 28rpx;
  background-color: #ffffff;
  box-sizing: border-box;

  &-full {
    // 如果是与屏幕之间不留空隙，应该设置左右边距为0
    margin-left: 0 !important;
    margin-right: 0 !important;
    width: 100%;
  }

  &--border:after {
    border-radius: 16rpx;
  }

  &__head {
    &--left {
      color: $u-main-color;

      &__thumb {
        margin-right: 16rpx;
      }

      &__title {
        max-width: 400rpx;
      }
    }

    &--right {
      color: $u-tips-color;
      margin-left: 6rpx;
    }
  }

  &__body {
    color: $u-content-color;
  }

  &__foot {
    color: $u-tips-color;
  }
}
</style>
