<script lang="ts" setup>
import { ComStatus } from "@/api/model";
import PermissionApi, { BmsMenus, MenuType } from "@/api/permission";
import { message } from "@/utils/message";
import { FormInstance, FormRules } from "element-plus";
import { ref, watch, Ref, reactive } from "vue";
import IconSelect from "@/components/ReIcon/src/Select.vue";

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
const defaultMenuInfo: BmsMenus = {
  id: null,
  name: "",
  status: ComStatus.Disabled,
  exdata: "",
  createTime: "",
  parentId: "",
  type: MenuType.MidNode,
  url: "",
  sysId: "",
  orderId: 0,
  description: "",
  projectId: "",
  openType: 0,
  isHomeShow: true,
  icon: ""
};

const menuInfo: Ref<BmsMenus> = ref(defaultMenuInfo);

async function getUserInfo() {
  if (props.id) {
    menuInfo.value = await PermissionApi.apiPermissionGetMenu(props.id);
    if (!menuInfo.value.icon) menuInfo.value.icon = "ep:";
  } else {
    menuInfo.value = { ...defaultMenuInfo };
  }
}
const formRef = ref<FormInstance>();

const submitForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      await PermissionApi.apiPermissionUpdateMenu({
        ...menuInfo.value
      });
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
  name: [{ required: true, message: "请输入菜单名称", trigger: "blur" }],
  url: [{ required: true, message: "请输入Url路径", trigger: "blur" }]
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
      :model="menuInfo"
      label-width="100px"
      class="demo-ruleForm"
    >
      <el-form-item label="菜单名称" prop="name">
        <el-input v-model="menuInfo.name" type="text" />
      </el-form-item>
      <el-form-item label="路径" prop="url">
        <el-input v-model="menuInfo.url" type="text" autocomplete="off" />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-switch
          v-model="menuInfo.status"
          :active-value="ComStatus.Enabled"
          active-text="启用"
          inactive-text="禁用"
          :inactive-value="ComStatus.Disabled"
        />
      </el-form-item>
      <el-form-item label="类型" prop="type">
        <el-radio-group v-model="menuInfo.type">
          <el-radio-button :label="MenuType.Node" size="default"
            >节点</el-radio-button
          >
          <el-radio-button :label="MenuType.MidNode" size="default"
            >中间节点</el-radio-button
          >
          <el-radio-button :label="MenuType.Button" size="default"
            >按钮</el-radio-button
          >
        </el-radio-group>
      </el-form-item>
      <el-form-item label="图标" prop="Icon">
        <el-input v-model="menuInfo.icon" type="text" autocomplete="off" />
        <icon-select v-model="menuInfo.icon" />
      </el-form-item>
      <el-form-item label="排序" prop="orderId">
        <el-input
          v-model.number="menuInfo.orderId"
          type="number"
          autocomplete="off"
        />
      </el-form-item>
      <el-form-item label="扩展信息" prop="extraData">
        <el-input
          :rows="5"
          v-model="menuInfo.exdata"
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
