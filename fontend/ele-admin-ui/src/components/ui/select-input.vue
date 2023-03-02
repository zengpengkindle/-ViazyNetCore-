<template>
  <div class="select-input">
    <slot name="input" v-bind:show="showDialog">
      <x-input-tag
        v-if="multiple"
        v-model="modelValue"
        :id="id"
        :label="currentLabel"
        :placeholder="placeholder"
        @new="showDialog"
        :readonly="readonly"
        :max="max"
        ref="tags"
      />
      <el-input
        class="select-input-singleton"
        v-else
        :placeholder="placeholder"
        v-model="display"
        :readonly="label != null"
        @dblclick.native="showDialog(true)"
      >
        <template #append>
          <el-button @click="showDialog">
            <span>{{ selectText }}</span>
          </el-button>
        </template>
        <template #suffix>
          <div class="el-input__clear">
            <icon
              name="circle-close"
              v-show="modelValue"
              @click="setValue(null)"
              title="清除"
            />
          </div>
        </template>
      </el-input>
    </slot>
    <el-dialog
      :title="title"
      v-model="dialogVisible"
      top="66px"
      :width="currentDialogWidth"
      :fullscreen="fullscreen"
      :modal="modal"
      :append-to-body="true"
    >
      <x-table ref="table" :params="params" :url="url" :border="false">
        <template #form>
          <slot name="form" :params="params" />
        </template>
        <slot />
        <el-table-column label="操作">
          <template #default="{ row }">
            <el-button
              type="text"
              size="small"
              @click="handleSelect(row)"
              v-if="!multiple || !hasValue(row)"
              >请选择</el-button
            >
          </template>
        </el-table-column>
      </x-table>
    </el-dialog>
  </div>
</template>
<script lang="ts" setup>
import { ref, computed, onMounted } from "vue";

export interface SelectInputProps {
  readonly url: string | Function;
  readonly label?: string;
  readonly fullscreen: boolean;
  readonly dialogWidth?: string;
  readonly readonly: boolean;
  readonly multiple: boolean;
  readonly max: number;
  readonly modal: boolean;
  readonly placeholder: string;
  readonly params: object;
  readonly selectText: string;
  readonly title: string;
  readonly defaultLabel: string;
  readonly rowHandle: Function;
  readonly id: string;
  modelValue: string | Array<any> | Object;
}
const props = withDefaults(defineProps<SelectInputProps>(), {
  placeholder: "请选择",
  dialogWidth: "50%",
  selectText: "请选择",
  title: "请选择",
  id: "id"
});

const dialogVisible = ref(false);
// const loading = ref(false);
const display = ref("");
const currentDialogWidth = ref("");
onMounted(() => {
  if (props.defaultLabel) display.value = props.defaultLabel;
  if (
    props.modelValue &&
    !props.defaultLabel &&
    (!props.label || props.label == props.id)
  )
    display.value = props.modelValue.toString();

  currentDialogWidth.value = props.dialogWidth == "50%" && props.dialogWidth;
});
const currentLabel = computed(() => {
  return props.label || props.id;
});

function hasValue(row: Object): boolean {
  const arr = props.modelValue as any[];
  for (const i in arr) {
    if (arr[i][props.id] == row[props.id]) return true;
  }
  return false;
}

const emits = defineEmits(["select", "update:modelValue"]);

function setValue(row: Object): void {
  if (row && this.rowHandle) {
    row = this.rowHandle(row);
  }
  if (this.multiple) {
    emits("update:modelValue", (props.modelValue as any[]).push(row));
  } else {
    display.value = row == null ? "" : row[this.currentLabel];
    emits("update:modelValue", row == null ? null : row[this.id]);
  }
}

function showDialog(isDoubleClick: boolean | MouseEvent) {
  if (!props.readonly) {
    if (isDoubleClick === true && !props.label) return;
    dialogVisible.value = true;
  }
}
function handleSelect(row) {
  setValue(row);
  if (
    !props.multiple ||
    (props.max > 1 && (props.modelValue as any[]).length == props.max)
  ) {
    dialogVisible.value = false;
  }
  emits("select", row);
}
</script>
<style scoped>
.select-input .select-input-singleton .el-input__clear {
  display: none;
  align-items: center;
}

.select-input .select-input-singleton:hover .el-input__clear {
  display: flex;
}
</style>
