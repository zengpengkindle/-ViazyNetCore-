<script lang="ts" setup>
import { ComStatus } from "@/api/model";
import DictionaryApi, { DictionaryTypeModel } from "@/api/system";
import { message } from "@/utils/message";
import { FormInstance, FormRules } from "element-plus";
import { ref, watch, Ref, reactive } from "vue";

export interface EditProps {
  modelValue: boolean;
  readonly id: number | null;
}
const props = defineProps<EditProps>();
const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);
function init() {
  getDicInfo();
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
const defaultUserInfo: DictionaryTypeModel = {
  id: 0,
  name: "",
  code: "",
  status: ComStatus.Disabled,
  description: ""
};

const dicInfo: Ref<DictionaryTypeModel> = ref({ ...defaultUserInfo });

async function getDicInfo() {
  if (props.id) {
    dicInfo.value = await DictionaryApi.apiGet(props.id);
  } else {
    dicInfo.value = { ...defaultUserInfo };
  }
}
const formRef = ref<FormInstance>();
const rules = reactive<FormRules>({
  username: [{ required: true, message: "用户名不能为空" }]
});
const submitForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      if (props.id !== 0) {
        await DictionaryApi.apiUpdate({
          ...dicInfo.value
        });
      } else {
        await DictionaryApi.apiAdd({
          ...dicInfo.value
        });
      }
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
    :title="id ? '编辑字典' : '新增字典'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :model="dicInfo"
      :rules="rules"
      label-width="100px"
      class="demo-ruleForm"
    >
      <el-form-item label="名称" prop="name">
        <el-input
          v-model="dicInfo.name"
          type="text"
          :disabled="id ? true : false"
        />
      </el-form-item>
      <el-form-item label="编码" prop="code">
        <el-input v-model="dicInfo.code" type="text" autocomplete="off" />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <x-status v-model="dicInfo.status" class="!w-[200px]" />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-input
          :rows="3"
          v-model="dicInfo.description"
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
