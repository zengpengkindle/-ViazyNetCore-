<template>
  <template v-if="type == 'cell'">
    <el-tag v-if="modelValue === 1">启用</el-tag>
    <el-tag type="info" v-else-if="modelValue === 0">禁用</el-tag>
    <el-tag type="danger" v-else-if="modelValue === -1">删除</el-tag>
  </template>
  <el-select
    v-else-if="type == 'select'"
    v-model="status"
    placeholder="全选"
    clearable
  >
    <el-option :value="1" label="启用" />
    <el-option :value="0" label="禁用" />
    <el-option :value="-1" label="删除" />
  </el-select>
  <el-radio-group v-else v-model="status">
    <el-radio-button :label="1">启用 </el-radio-button>
    <el-radio-button :label="-1">删除</el-radio-button>
    <el-radio-button :label="0">禁用</el-radio-button>
  </el-radio-group>
</template>
<script lang="ts" setup>
import { ref, watch } from "vue";
export interface Props {
  readonly type?: "cell" | "select" | "";
  modelValue?: number;
}
const pops = defineProps<Props>();
const status = ref(pops.modelValue);
const emit = defineEmits(["update:modelValue"]);
watch(
  () => {
    return status.value;
  },
  data => {
    emit("update:modelValue", data);
  }
);
watch(
  () => {
    return pops.modelValue;
  },
  data => {
    status.value = data;
  }
);
</script>
