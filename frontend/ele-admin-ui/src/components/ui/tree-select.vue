<template>
  <el-tree-select
    :data="treeData"
    v-model="rvalue"
    default-expand-all
    check-strictly
    :props="{
      value: 'id',
      label: 'name',
      emitPath: false
    }"
    clearable
  />
</template>
<script lang="ts" setup>
import { onMounted } from "vue";
import { ref, watch, Ref } from "vue";
import { http } from "@/utils/http";
import { handleTree } from "@pureadmin/utils";
export interface Props {
  readonly url?: string;
  modelValue?: number;
}
const props = defineProps<Props>();
const rvalue = ref(props.modelValue);
const emit = defineEmits(["update:modelValue"]);
const treeData: Ref<Array<any>> = ref();
onMounted(async () => {
  const result = await http.request<Array<any>>({
    url: props.url,
    method: "post"
  });
  treeData.value = handleTree(result);
});
watch(
  () => {
    return rvalue.value;
  },
  data => {
    emit("update:modelValue", data);
  }
);
watch(
  () => {
    return props.modelValue;
  },
  data => {
    rvalue.value = data;
  }
);
</script>
