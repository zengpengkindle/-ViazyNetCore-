<template>
  <template v-if="type == 'cell'">
    <el-tag>{{ dictName }}</el-tag>
  </template>
  <el-select
    v-else-if="type == 'select'"
    v-model="status"
    placeholder="全选"
    clearable
  >
    <el-option
      v-for="item in options"
      :key="item.id"
      :value="props.valueType == 'code' ? item.code : item.name"
      :label="item.name"
    />
  </el-select>
  <el-radio-group v-else v-model="status">
    <el-radio-button
      v-for="item in options"
      :key="item.id"
      :label="props.valueType == 'code' ? item.code : item.name"
    >
      {{ item.name }}
    </el-radio-button>
  </el-radio-group>
</template>
<script lang="ts" setup>
import { ref, watch, Ref } from "vue";
import DictionaryApi, { DictionaryValueViewResult } from "@/api/system";
import { onMounted } from "vue";
import { computed } from "vue";
export interface Props {
  readonly type?: "cell" | "select" | "radio";
  readonly code: string;
  readonly valueType?: "code" | "value";
  modelValue?: string;
}
const props = withDefaults(defineProps<Props>(), {
  type: "select",
  valueType: "code"
});
const status = ref(props.modelValue);
const emit = defineEmits(["update:modelValue"]);
const options: Ref<DictionaryValueViewResult[]> = ref([]);
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
    return props.modelValue;
  },
  data => {
    status.value = data;
  }
);
onMounted(async () => {
  const data = await DictionaryApi.getvalues(props.code);
  options.value = data;
});
const dictName = computed(() => {
  return options.value.find(item => item.code == props.modelValue)?.name;
});
</script>
