<script lang="ts" setup>
import { ref, Ref, reactive, watch } from "vue";
import ShopPageApi, { ShopPageEditModel, PageLayout, PageType } from "@/api/shopmall/shoppage";
import { ComStatus } from "@/api/model";
import { FormInstance, FormRules, SingleOrRange } from "element-plus";
import { message } from "@/utils/message";
export interface Props {
  modelValue: boolean;
  readonly id: number | string | null;
}

const props = defineProps<Props>();
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    if (value) {
      isEdit.value = false;
      getProductOuter();
    }
  }
);
const rules = reactive<FormRules>({
  code: [
    { required: true, message: "可视化区域编码" },
    { min: 1, max: 50, message: "长度在 1 到 50 个字符" }
  ],
  name: [
    { required: true, message: "可编辑区域名称" },
    { min: 1, max: 50, message: "长度在 1 到 50 个字符" }
  ],
  description: [
    { required: false, message: "可编辑区域名称" },
    { min: 0, max: 500, message: "长度在 1 到 500 个字符" }]
});
const defaultItem: ShopPageEditModel = {
  code: "",
  name: "",
  status: ComStatus.Enabled,
  description: "",
  id: 0,
  layout: PageLayout.Mobile,
  type: PageType.Mobile
};
const item: Ref<ShopPageEditModel> = ref(defaultItem);
const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);
async function getProductOuter() {
  if (props.id) {
    // item.value = await ShopPageApi.get(props.id);
  } else {
    item.value = { ...defaultItem };
  }
}
const formRef = ref<FormInstance>();
const createTimes: Ref<SingleOrRange<string>> = ref();
const submit = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      await ShopPageApi.updatePage(item.value);
      message("提交成功", { type: "success" });
      emit("refresh");
      handleClose(() => { });
    } else {
      console.log("error submit!");
      return false;
    }
  });
};
const handleClose = (done: () => void) => {
  emit("update:modelValue", false);
  done();
};
const closeForm = () => {
  handleClose(() => { });
};
</script>
<template title="货币类型管理">
  <el-drawer v-model="visible" size="35%" :title="id ? '编辑货币类型' : '新增货币类型'" :before-close="handleClose">
    <el-form ref="formRef" :model="item" :rules="rules" :validate-on-rule-change="false">
      <el-form-item label="区域编码" prop="code">
        <el-input v-model="item.code" :disabled="isEdit" />
      </el-form-item>
      <el-form-item label="区域名称" prop="name">
        <el-input v-model="item.name" v-if="isEdit" :disabled="true" />
        <el-input v-model="item.name" v-else />
      </el-form-item>
      <el-form-item label="描述" prop="description">
        <el-input v-model="item.description" type="textarea" :row="3" />
      </el-form-item>
      <el-form-item label="布局样式" prop="layout">
        <el-select v-model="item.layout">
          <el-option :value="PageLayout.Mobile" label="手机端"/>
          <el-option :value="PageLayout.Pc" label="PC端"/>
        </el-select>
      </el-form-item>
      <el-form-item label="布局类型" prop="type">
        <el-select v-model="item.type">
          <el-option :value="PageType.Mobile" label="手机端" />
          <el-option :value="PageType.Pc" lable="PC端" />
        </el-select>
      </el-form-item>
      <el-form-item label="布局类型" prop="type">
        <x-status v-model="item.status" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submit(formRef)">提交</el-button>
        <el-button @click="closeForm">返回</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
