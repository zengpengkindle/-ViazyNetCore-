<script lang="ts" setup>
import { ref, watch, Ref, onMounted } from "vue";
import RoleApi, { RoleSimpleModel } from "@/api/role";
import { message } from "@/utils/message";
export interface RoleProp {
  modelValue: boolean;
  readonly id: string;
}
const props = defineProps<RoleProp>();
const emit = defineEmits(["update:modelValue"]);
const visible = ref(false);
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    handleCheckList();
  }
);

const handleCheckList = async () => {
  if (props?.id) {
    loading.value = true;
    checkList.value = await RoleApi.apiRoleGetUserRoleIds(props.id);
    loading.value = false;
  }
};

const handleClose = (done: () => void) => {
  emit("update:modelValue", false);
  done();
};

const checkList: Ref<Array<string>> = ref([]);
const roleLists: Ref<Array<RoleSimpleModel>> = ref();
onMounted(async () => {
  roleLists.value = await RoleApi.apiRoleGetAll();
});

const submitForm = async () => {
  if (props.id) {
    await RoleApi.apiRoleUpdateUserRoles(props.id, checkList.value);
    message("提交成功", { type: "success" });
  }
};
const loading = ref(true);
</script>
<template>
  <el-drawer
    v-model="visible"
    size="35%"
    title="选择用户角色"
    :before-close="handleClose"
  >
    <div v-loading="loading">
      <el-checkbox-group v-model="checkList">
        <el-checkbox
          v-for="item in roleLists"
          :label="item.id"
          v-bind:key="item.id"
          >{{ item.name }}
        </el-checkbox>
      </el-checkbox-group>
    </div>
    <template #footer>
      <el-divider />
      <div style="flex: auto">
        <el-button type="primary" @click="submitForm">提交</el-button>
        <el-button @click="handleClose(null)">取消</el-button>
      </div>
    </template>
  </el-drawer>
</template>
