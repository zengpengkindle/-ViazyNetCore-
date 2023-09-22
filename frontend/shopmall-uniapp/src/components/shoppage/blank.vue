<template>
  <view>
    <u-gap
      :height="prop.parameters.height * 2"
      :bg-color="prop.parameters.backgroundColor"
    />
  </view>
</template>
<script lang="ts" setup>
export interface BlackParameter {
  height: number;
  backgroundColor: string;
}
export interface BlackProps {
  parameters: BlackParameter;
}
const prop = defineProps<BlackProps>();
</script>
