<script lang="ts" setup>
import OrgApi from "@/api/org";
import VehicleApi, { VehicleEditRequest, VehicleStatus } from "@/api/vehicle";
import { message } from "@/utils/message";
import { handleTree } from "@/utils/tree";
import {
  ElButton,
  ElDrawer,
  ElForm,
  ElFormItem,
  ElInput,
  FormInstance,
  FormRules
} from "element-plus";
import { onMounted } from "vue";
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
const defaultVehicleInfo: VehicleEditRequest = {
  id: 0,
  code: "",
  vehicleImg: "",
  orgId: 0,
  vehicleStatus: VehicleStatus.New,
  catId: 0,
  spec: "",
  originEnterprise: "",
  originPlace: "",
  licenseCode: "",
  frameCode: "",
  engineCode: ""
};

const vehicleInfo: Ref<VehicleEditRequest> = ref(defaultVehicleInfo);

async function getUserInfo() {
  if (props.id) {
    vehicleInfo.value = await VehicleApi.getInfo(props.id);
  } else {
    vehicleInfo.value = { ...defaultVehicleInfo };
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
      await VehicleApi.edit({
        ...vehicleInfo.value
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

const loading = ref(true);
const dataList = ref([]);
async function onSearch() {
  loading.value = true;
  const data = await OrgApi.apiOrgGetList();
  dataList.value = handleTree(data);
  loading.value = false;
}
onMounted(async () => {
  await onSearch();

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
      :model="vehicleInfo"
      label-width="100px"
      class="demo-ruleForm"
      :rules="rules"
    >
      <el-form-item label="单件装备编码">
        <el-input
          v-model="vehicleInfo.code"
          type="text"
          :disabled="id ? true : false"
        />
      </el-form-item>
      <el-form-item label="车辆照片">
        <x-image v-model="vehicleInfo.vehicleImg" />
      </el-form-item>
      <el-form-item label="机构编码" prop="orgId">
        <el-tree-select
          :data="dataList"
          v-model="vehicleInfo.orgId"
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

      <el-form-item label="装备状态" prop="vehicleStatus">
        <el-radio-group v-model="vehicleInfo.vehicleStatus">
          <el-radio-button :label="1">新品 </el-radio-button>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="装备分类" prop="catId">
        <el-input v-model="vehicleInfo.catId" type="text" />
      </el-form-item>
      <el-form-item label="规格型号" prop="spec">
        <el-input v-model="vehicleInfo.spec" type="text" />
      </el-form-item>
      <el-form-item label="生产企业" prop="originEnterprise">
        <el-input v-model="vehicleInfo.originEnterprise" type="text" />
      </el-form-item>
      <el-form-item label="产地国别" prop="originPlace">
        <el-input v-model="vehicleInfo.originPlace" type="text" />
      </el-form-item>
      <el-form-item label="车牌号" prop="licenseCode">
        <el-input v-model="vehicleInfo.licenseCode" type="text" />
      </el-form-item>
      <el-form-item label="车架号码" prop="frameCode">
        <el-input v-model="vehicleInfo.frameCode" type="text" />
      </el-form-item>
      <el-form-item label="发动机号" prop="engineCode">
        <el-input v-model="vehicleInfo.engineCode" type="text" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm(formRef)">提交</el-button>
        <el-button @click="closeForm">取消</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
