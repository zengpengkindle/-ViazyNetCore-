<script lang="ts" setup>
import { ComStatus } from "@/api/model";
import RoleApi, { BmsRole } from "@/api/role";
import { message } from "@/utils/message";
import {
  ElButton,
  ElDrawer,
  ElForm,
  ElFormItem,
  ElInput,
  ElSwitch,
  FormInstance,
  FormRules
} from "element-plus";
import { ref, watch, Ref, reactive } from "vue";

export interface Props {
  modelValue: boolean;
  readonly id: string | null;
}
const props = defineProps<Props>();
const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);
function init() {
  getUserInfo();
}
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    if (value) {
      isEdit.value = false;
      init();
    }
  }
);
const handleClose = (done: () => void) => {
  emit("update:modelValue", false);
  done();
};
const defaultRoleInfo: BmsRole = {
  id: null,
  name: "",
  status: ComStatus.Disabled,
  extraData: "",
  createTime: "",
  modifyTime: ""
};

const roleInfo: Ref<BmsRole> = ref(defaultRoleInfo);

async function getUserInfo() {
  if (props.id) {
    roleInfo.value = await RoleApi.apiRoleFindRole(props.id);
  } else {
    roleInfo.value = { ...defaultRoleInfo };
  }
}
const formRef = ref<FormInstance>();
const rules = reactive<FormRules>({
  name: [{ required: true, message: "请输入角色名称" }]
});
const submitForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      await RoleApi.apiRoleUpdateRole({
        ...roleInfo.value,
        keys: []
      });
      message("修改成功", { type: "success" });
      emit("refresh");
      handleClose(() => {});
    } else {
      console.log("error submit!");
      return false;
    }
  });
};

const closeForm = () => {
  handleClose(() => {});
};
</script>
<template>
  <el-drawer
    v-model="visible"
    size="35%"
    :title="id ? '编辑用户' : '新增用户'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :model="roleInfo"
      label-width="100px"
      class="demo-ruleForm"
      :rules="rules"
    >
      <el-form-item label="角色名称">
        <el-input
          v-model="roleInfo.name"
          type="text"
          :disabled="id ? true : false"
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-switch
          v-model="roleInfo.status"
          :active-value="ComStatus.Enabled"
          active-text="启用"
          inactive-text="禁用"
          :inactive-value="ComStatus.Disabled"
        />
      </el-form-item>
      <el-form-item label="扩展信息" prop="extraData">
        <el-input
          :rows="5"
          v-model="roleInfo.extraData"
          type="textarea"
          autocomplete="off"
        />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm(formRef)">提交</el-button>
        <el-button @click="closeForm">取消</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
