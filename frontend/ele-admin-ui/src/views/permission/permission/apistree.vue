<script lang="ts" setup>
import { Ref, onMounted, ref, watch } from "vue";
import PermissionApi, {
  ApiItemDto,
  PermissionApiUpdateDto
} from "@/api/permission";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import ApiApi, { ApiGroupDescriptor } from "@/api/api";

import Reset from "@iconify-icons/ri/restart-line";
import Search from "@iconify-icons/ep/search";
import { ElMessageBox } from "element-plus";
import { message } from "@/utils/message";

export interface TreeProps {
  pid?: string;
  pname?: string;
}
const props = defineProps<TreeProps>();

const apiGroups: Ref<Array<ApiGroupDescriptor>> = ref([]);
onMounted(async () => {
  apiGroups.value = await ApiApi.getApis();
});

const keyMenus: Ref<Array<ApiItemDto>> = ref([]);
const loading = ref(false);
watch(
  () => {
    return props.pid;
  },
  async () => {
    if (props.pid) {
      loading.value = true;
      keyMenus.value = await PermissionApi.getApisInPermissionKey(props.pid);
      resetSelect();
      loading.value = false;
    }
  }
);

const searchValue = ref("");
function resetSelect() {
  apiGroups.value.map(group => {
    group.checks = [];
    group.allcheck = false;
    group.isIndeterminate = false;
    group.apis.map(api => {
      const exist =
        keyMenus.value.filter(
          p => p.httpMethod === api.httpMethod && p.path === api.routeTemplate
        ).length > 0;
      if (exist) {
        group.checks.push(api.httpMethod + "|" + api.routeTemplate);
      }
    });
    group.allcheck = group.checks.length == group.apis.length;
    group.isIndeterminate =
      group.checks.length > 0 && group.checks.length < group.apis.length;
  });
}

const handleCheckAllChange = (group: ApiGroupDescriptor, val: boolean) => {
  group.checks = val
    ? group.apis.map(api => api.httpMethod + "|" + api.routeTemplate)
    : [];
  group.isIndeterminate = false;
};
const handleCheckedGrouupChange = (group: ApiGroupDescriptor) => {
  const checkedCount = group.checks.length;
  //checkAll.value = checkedCount === cities.length;
  group.allcheck = group.apis.length === group.checks.length;
  group.isIndeterminate = checkedCount > 0 && checkedCount < group.apis.length;
};

function submitTreeNode() {
  ElMessageBox.confirm("确认修改权限菜单?", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning"
  }).then(async () => {
    const apis = apiGroups.value
      .filter(group => group.checks != null && group.checks.length > 0)
      .map(group => group.checks)
      .flat()
      .map(api => {
        const apis = api.split("|");
        return { httpMethod: apis[0], path: apis[1] };
      });

    const updateModel: PermissionApiUpdateDto = {
      permissionKey: props.pid,
      apis: apis
    };

    await PermissionApi.updateApisInPermission(updateModel);
    keyMenus.value = apis;
    message("提交成功！", { type: "success" });
  });
}
</script>
<template>
  <div class="overflow-auto">
    <el-card class="h-full mt-4 min-h-[780px]" v-loading="loading">
      <template #header>
        <div class="flex items-center h-[34px]">
          <div class="flex-1 font-bold text-base truncate" title="接口列表">
            {{ props?.pid ? "[" + props.pname + "] 接口权限" : "接口权限" }}
          </div>
          <el-input
            style="flex: 2"
            size="small"
            v-model="searchValue"
            placeholder="请输入接口名称"
            clearable
          >
            <template #suffix>
              <el-icon class="el-input__icon">
                <IconifyIconOffline
                  v-show="searchValue.length === 0"
                  :icon="Search"
                />
              </el-icon>
            </template>
          </el-input>
        </div>
      </template>
      <template v-for="group in apiGroups" :key="group.controllerName">
        <el-checkbox
          v-model="group.allcheck"
          :indeterminate="group.isIndeterminate"
          @change="handleCheckAllChange(group, group.allcheck)"
        >
          {{ group.displayControllerName }}
        </el-checkbox>
        <el-checkbox-group
          v-model="group.checks"
          @change="handleCheckedGrouupChange(group)"
        >
          <el-checkbox
            v-for="api in group.apis"
            :key="api.id"
            :label="`${api.httpMethod}|${api.routeTemplate}`"
          >
            {{ api.displayActionName }}
          </el-checkbox>
        </el-checkbox-group>
      </template>
      <div class="p-4">
        <el-button type="primary" @click="submitTreeNode()">提交</el-button>
        <el-button :icon="useRenderIcon(Reset)" @click="resetSelect()">
          重置
        </el-button>
      </div>
    </el-card>
  </div>
</template>
<style lang="scss" scoped>
:deep(.el-divider) {
  margin: 0;
}
</style>
