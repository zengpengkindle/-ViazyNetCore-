<script lang="ts" setup>
import { ref, Ref, reactive, watch } from "vue";
import ProductCatApi, { CatEditReq } from "@/api/shopmall/productCat";
import { ComStatus } from "@/api/model";
import { FormInstance, FormRules } from "element-plus";
import { message } from "@/utils/message";
export interface Props {
  modelValue: boolean;
  readonly id: string | null;
  readonly treeData?: any[];
}

const props = defineProps<Props>();
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    if (value) {
      isEdit.value = false;
      getProductCat();
    }
  }
);
const rules = reactive<FormRules>({
  name: [
    { required: true, message: "请输入类目" },
    { min: 1, max: 10, message: "长度在 1 到 10 个字符" }
  ]
});
const defaultItem: CatEditReq = {
  status: ComStatus.Enabled,
  id: "",
  isHidden: false,
  name: "",
  parentId: "",
  isParent: false,
  path: "",
  image: "",
  sort: 0,
  exdata: ""
};
const item: Ref<CatEditReq> = ref(defaultItem);
const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);
async function getProductCat() {
  if (props.id) {
    item.value = await ProductCatApi.get(props.id);
  } else {
    item.value = { ...defaultItem };
  }
}
const formRef = ref<FormInstance>();
const submit = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      await ProductCatApi.edit(item.value);
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
const closeForm = () => {
  handleClose(() => {});
};
</script>
<template title="类目管理">
  <el-drawer
    v-model="visible"
    size="35%"
    :title="id ? '编辑类目类型' : '新增类目类型'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :model="item"
      :rules="rules"
      :validate-on-rule-change="false"
    >
      <el-form-item label="类目" prop="name">
        <el-input v-model="item.name" />
      </el-form-item>
      <el-form-item label="父级">
        <el-tree-select
          :data="treeData"
          v-model="item.parentId"
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
      <el-form-item label="图片" prop="image">
        <x-image v-model="item.image" />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <x-status v-model="item.status" />
      </el-form-item>
      <el-form-item label="排序" prop="sort">
        <el-input-number v-model="item.sort" />
      </el-form-item>
      <el-form-item label="前台隐藏" prop="isHidden">
        <el-switch v-model="item.isHidden" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submit(formRef)">提交</el-button>
        <el-button @click="closeForm">返回</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
