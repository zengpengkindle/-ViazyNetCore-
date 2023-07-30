<script lang="ts" setup>
import FormTemplateApi, {
  FormTemplateEditRequest,
  FormType
} from "@/api/formTemplate";
import { message } from "@/utils/message";
import {
  ElButton,
  ElDrawer,
  ElForm,
  ElFormItem,
  ElInput,
  FormInstance,
  FormRules
} from "element-plus";
import { ref, watch, Ref, reactive } from "vue";

export interface Props {
  modelValue: boolean;
  readonly id: number | null;
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
const defaultFormTmpInfo: FormTemplateEditRequest = {
  id: null,
  name: "",
  description: null,
  formType: FormType.Form
};

const fromTmpInfo: Ref<FormTemplateEditRequest> = ref(defaultFormTmpInfo);

async function getUserInfo() {
  if (props.id) {
    fromTmpInfo.value = await FormTemplateApi.getInfo(props.id);
  } else {
    fromTmpInfo.value = { ...defaultFormTmpInfo };
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
      await FormTemplateApi.edit({
        ...fromTmpInfo.value
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
    :title="id ? '编辑表单' : '新增表单'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :model="fromTmpInfo"
      label-width="100px"
      class="demo-ruleForm"
      :rules="rules"
    >
      <el-form-item label="表单名称">
        <el-input v-model="fromTmpInfo.name" type="text" />
      </el-form-item>
      <el-form-item label="类别" prop="formType">
        <el-radio-group v-model="fromTmpInfo.formType">
          <el-radio :label="FormType.Flow">流程</el-radio>
          <el-radio :label="FormType.Form">表单</el-radio>
          <el-radio :label="FormType.Question">问卷</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="扩展信息" prop="description">
        <el-input
          :rows="5"
          v-model="fromTmpInfo.description"
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
