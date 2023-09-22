<script setup lang="ts">
import { handleTree } from "@/utils/tree";
import PermissionApi from "@/api/permission";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { ref, computed, watch, onMounted } from "vue";

import Reset from "@iconify-icons/ri/restart-line";
import Search from "@iconify-icons/ep/search";
import { message } from "@/utils/message";
import { ElMessageBox } from "element-plus";

export interface TreeProps {
  pid?: string;
  pname?: string;
}
const props = defineProps<TreeProps>();

interface Tree {
  id: number;
  name: string;
  highlight?: boolean;
  children?: Tree[];
}

const treeRef = ref();
const treeData = ref([]);
const searchValue = ref("");
const highlightMap = ref({});
const defaultProps = {
  children: "children",
  label: "name"
};
const keyMenus = ref([]);
const loading = ref(false);
watch(
  () => {
    return props.pid;
  },
  async () => {
    if (props.pid) {
      loading.value = true;
      keyMenus.value =
        await PermissionApi.apiPermissionGetMenuKeysInPermissionKey(props.pid);
      resetSelect();
      loading.value = false;
    }
  }
);

const filterNode = (value: string, data: Tree) => {
  if (!value) return true;
  return data.name.includes(value);
};

function nodeClick(value) {
  const nodeId = value.$treeNodeId;
  highlightMap.value[nodeId] = highlightMap.value[nodeId]?.highlight
    ? Object.assign({ id: nodeId }, highlightMap.value[nodeId], {
        highlight: false
      })
    : Object.assign({ id: nodeId }, highlightMap.value[nodeId], {
        highlight: true
      });
  Object.values(highlightMap.value).forEach((v: Tree) => {
    if (v.id !== nodeId) {
      v.highlight = false;
    }
  });
}

watch(searchValue, val => {
  treeRef.value!.filter(val);
});

onMounted(async () => {
  const data = await PermissionApi.apiPermissionGetMenus();
  treeData.value = handleTree(data);
});
const showCheckBox = computed(() => {
  return props.pid !== null && props.pid !== "";
});

function resetSelect() {
  treeRef.value!.setCheckedKeys(keyMenus.value, false);
}
function submitTreeNode() {
  ElMessageBox.confirm("确认修改权限菜单?", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning"
  }).then(async () => {
    const menuIds = treeRef.value!.getCheckedKeys(false);
    await PermissionApi.apiPermissionUpdateMenusInPermission(
      props.pid,
      menuIds
    );
    keyMenus.value = menuIds;
    message("提交成功！", { type: "success" });
  });
}
</script>

<template>
  <div class="overflow-auto">
    <el-card class="h-full mt-4 min-h-[780px]" v-loading="loading">
      <template #header>
        <div class="flex items-center h-[34px]">
          <div class="flex-1 font-bold text-base truncate" title="菜单列表">
            {{ props?.pid ? "[" + props.pname + "] 权限" : "菜单权限" }}
          </div>
          <el-input
            style="flex: 2"
            size="small"
            v-model="searchValue"
            placeholder="请输入菜单名称"
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
      <el-tree
        ref="treeRef"
        :data="treeData"
        node-key="id"
        size="small"
        :props="defaultProps"
        default-expand-all
        :default-checked-keys="keyMenus"
        :show-checkbox="showCheckBox"
        :expand-on-click-node="false"
        :check-strictly="true"
        :filter-node-method="filterNode"
        @node-click="nodeClick"
      >
        <template #default="{ node }">
          <span
            :class="[
              'pl-1',
              'pr-1',
              'rounded',
              'flex',
              'items-center',
              'select-none',
              searchValue.trim().length > 0 &&
                node.label.includes(searchValue) &&
                'text-red-500',
              highlightMap[node.id]?.highlight ? 'dark:text-primary' : ''
            ]"
            :style="{
              background: highlightMap[node.id]?.highlight
                ? 'var(--el-color-primary-light-7)'
                : 'transparent'
            }"
          >
            {{ node.label }}
          </span>
        </template>
      </el-tree>
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

.menu-list {
  border-radius: 4px;
}
</style>
