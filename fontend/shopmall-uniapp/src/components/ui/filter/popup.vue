<template>
  <u-popup mode="top" data-index="5">
    <view class="content">
      <slot name="filterSlot" />
      <view class="filter-btns-wrap">
        <view class="filter-btn btn-reset" bind:tap="reset">重置</view>
        <view class="filter-btn btn-confirm" bind:tap="confirm" data-index="5">
          确定
        </view>
      </view>
    </view>
  </u-popup>
</template>
<script lang="ts" setup></script>
<style lang="scss" scoped></style>
