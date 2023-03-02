<script lang="ts" setup>
import { ComStatus } from "@/api/model";
import OrgApi, { OrgUpdateInput } from "@/api/org";
import { message } from "@/utils/message";
import { FormInstance, FormRules } from "element-plus";
import { ref, watch, Ref, reactive } from "vue";

export interface Props {
  modelValue: boolean;
  readonly id: number | null;
  readonly treeData?: any[];
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
const defaultOrgInfo: OrgUpdateInput = {
  id: null,
  name: "",
  status: ComStatus.Disabled,
  parentId: 0,
  description: "",
  code: "",
  value: "",
  sort: 0
};

const orgInfo: Ref<OrgUpdateInput> = ref(defaultOrgInfo);

async function getUserInfo() {
  if (props.id) {
    const result = await OrgApi.apiOrgGet(props.id);
    if (result?.parentId === 0) result.parentId = null;
    orgInfo.value = result;
  } else {
    orgInfo.value = { ...defaultOrgInfo };
  }
}
const formRef = ref<FormInstance>();

const submitForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      if (props.id == 0) {
        await OrgApi.apiOrgAdd({
          ...orgInfo.value
        });
      } else {
        await OrgApi.apiOrgUpdate({
          ...orgInfo.value
        });
      }
      message("修改成功", { type: "success" });
      emit("refresh");
      handleClose(() => {});
    } else {
      return false;
    }
  });
};

const closeForm = () => {
  handleClose(() => {});
};
const rules = reactive<FormRules>({
  name: [{ required: true, message: "请输入菜单名称", trigger: "blur" }]
});
</script>
<template>
  <el-drawer
    v-model="visible"
    size="35%"
    :title="id ? '编辑菜单' : '新增菜单'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :rules="rules"
      :model="orgInfo"
      label-width="100px"
      class="demo-ruleForm"
    >
      <el-form-item label="菜单名称" prop="name">
        <el-input v-model="orgInfo.name" type="text" />
      </el-form-item>
      <el-form-item label="父节点">
        <el-tree-select
          :data="treeData"
          v-model="orgInfo.parentId"
          default-expand-all
          check-strictly
          :props="{
            value: 'id',
            label: 'name',
            emitPath: false
          }"
          clearable
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-switch
          v-model="orgInfo.status"
          :active-value="ComStatus.Enabled"
          active-text="启用"
          inactive-text="禁用"
          :inactive-value="ComStatus.Disabled"
        />
      </el-form-item>
      <el-form-item label="排序" prop="sort">
        <el-input
          v-model.number="orgInfo.sort"
          type="number"
          autocomplete="off"
        />
      </el-form-item>
      <el-form-item label="扩展信息" prop="description">
        <el-input
          :rows="5"
          v-model="orgInfo.description"
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
