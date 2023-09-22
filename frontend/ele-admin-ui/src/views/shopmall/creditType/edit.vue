<script lang="ts" setup>
import { ref, Ref, reactive, watch } from "vue";
import CreditApi, { CreditModel, CreditType } from "@/api/shopmall/credit";
import { FormInstance, FormRules } from "element-plus";
import { message } from "@/utils/message";
export interface Props {
  modelValue: boolean;
  readonly id: string | null;
}

const props = defineProps<Props>();
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    if (value) {
      isEdit.value = false;
    }
  }
);
const item: Ref<CreditModel> = ref({
  name: null,
  key: null,
  creditType: CreditType.Virtual
});
const creditTypes = reactive([
  { id: CreditType.ReadyMoney, name: "现金" },
  { id: CreditType.Virtual, name: "虚拟货币" }
]);
const rules = reactive<FormRules>({
  name: [
    { required: true, message: "请输入名称" },
    { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
  ],
  creditKey: [
    { required: true, message: "请输入账号" },
    { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
  ]
});

const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);

const formRef = ref<FormInstance>();
const submit = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      await CreditApi.apiCreditAddCredit(item.value);
      message("新增成功", { type: "success" });
      emit("refresh");
      handleClose(() => {});
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
</script>
<template title="货币类型管理">
  <el-drawer
    v-model="visible"
    size="35%"
    :title="id ? '编辑货币类型' : '新增货币类型'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :model="item"
      :rules="rules"
      :validate-on-rule-change="false"
    >
      <el-form-item label="名称" prop="name">
        <el-input v-model="item.name" />
      </el-form-item>
      <el-form-item label="货币标识/符号" prop="key">
        <el-input v-model="item.key" />
      </el-form-item>
      <el-form-item label="货币类型">
        <el-radio-group v-model="item.creditType">
          <el-radio
            v-for="creditType in creditTypes"
            :label="creditType.id"
            :key="creditType.id"
            >{{ creditType.name }}</el-radio
          >
        </el-radio-group>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submit(formRef)">提交</el-button>
        <el-button @click="$router.back()">返回</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
