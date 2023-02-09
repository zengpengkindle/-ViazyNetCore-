
<template>
  <template v-if="type=='cell'">
    <el-tag v-if="modelValue===1">启用</el-tag>
    <el-tag type="info" v-else-if="modelValue===0">禁用</el-tag>
    <el-tag type="danger" v-else-if="modelValue===-1">删除</el-tag>
  </template>
  <el-select v-else-if="type=='select'" v-model="modelValue" placeholder="全选" clearable>
    <el-option :value="1" label="启用">
    </el-option>
    <el-option :value="0" label="禁用" />
    <el-option :value="-1" label="删除" />
  </el-select>
  <el-radio-group v-else v-model="modelValue">
    <el-radio-button :label="0">禁用</el-radio-button>
    <el-radio-button :label="1">启用 </el-radio-button>
    <el-radio-button :label="-1">删除</el-radio-button>
  </el-radio-group>

</template>
<script lang="ts" setup>
  export interface Props {
    readonly type: 'cell' | 'select' | string,
    modelValue: number
  }
  defineProps<Props>();
</script>
