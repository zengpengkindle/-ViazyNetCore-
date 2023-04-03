<script lang="ts" setup>
import { UserFindModel, ComStatus } from "@/api/model";
import UserApi from "@/api/user";
import { message } from "@/utils/message";
import { FormInstance, FormRules } from "element-plus";
import { ref, watch, Ref, reactive, computed } from "vue";
import { handleTree } from "@/utils/tree";

export interface Props {
  modelValue: boolean;
  readonly id: string | null;
  orgData: any[];
}
const props = defineProps<Props>();
const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);
const orgTree = ref([]);
function init() {
  getUserInfo();
}
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    orgTree.value = handleTree(props.orgData);
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
const defaultUserInfo: UserFindModel = {
  id: null,
  username: "",
  nickname: "",
  status: ComStatus.Disabled,
  extraData: ""
};

const userInfo: Ref<UserFindModel> = ref(defaultUserInfo);

async function getUserInfo() {
  if (props.id) {
    userInfo.value = await UserApi.apiUserFind(props.id);
  } else {
    userInfo.value = { ...defaultUserInfo };
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
      await UserApi.apiUserManage({
        ...userInfo.value,
        roleId: 0
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

const orgData = computed(() => {
  return [...props.orgData].filter(item =>
    userInfo.value.orgIds.includes(item.id)
  );
});
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
      :model="userInfo"
      :rules="rules"
      label-width="100px"
      class="demo-ruleForm"
    >
      <el-form-item label="用户名" prop="username">
        <el-input
          v-model="userInfo.username"
          type="text"
          :disabled="id ? true : false"
        />
      </el-form-item>
      <el-form-item label="部门" prop="nickname">
        <el-tree-select
          :data="orgTree"
          v-model="userInfo.orgIds"
          check-strictly
          multiple
          value-key="id"
          default-expand-all
          show-checkbox
          :props="{
            value: 'id',
            label: 'name',
            emitPath: false
          }"
          clearable
        />
      </el-form-item>
      <el-form-item label="主部门">
        <el-select filterable v-model="userInfo.orgId" clearable>
          <el-option
            v-for="item in orgData"
            :key="item.id"
            :label="item.name"
            :value="item.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="昵称" prop="nickname">
        <el-input v-model="userInfo.nickname" type="text" autocomplete="off" />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <x-status v-model="userInfo.status" class="!w-[200px]" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm(formRef)">提交</el-button>
        <el-button @click="closeForm">取消</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
