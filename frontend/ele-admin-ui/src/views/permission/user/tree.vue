<script setup lang="ts">
import { handleTree } from "@/utils/tree";
import OrgApi, { OrgListOutput } from "@/api/org";
import { ComStatus } from "@/api/model";
import { ref, watch, onMounted } from "vue";

import Search from "@iconify-icons/ep/search";

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
const loading = ref(false);

const emits = defineEmits(["change"]);

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
  emits("change", value.id);
}

watch(searchValue, val => {
  treeRef.value!.filter(val);
});

onMounted(async () => {
  const data: OrgListOutput[] = await OrgApi.apiOrgGetList();
  data.push({
    id: 0,
    parentId: -1,
    name: "全部部门",
    sort: 0,
    status: ComStatus.Enabled
  });
  treeData.value = handleTree(data);
});
</script>

<template>
  <div class="overflow-auto">
    <el-card class="h-full min-h-[780px]" v-loading="loading">
      <template #header>
        <div class="flex items-center h-[34px]">
          <div class="flex-1 font-bold text-base truncate">部门列表</div>
          <el-input
            style="flex: 2"
            size="small"
            v-model="searchValue"
            placeholder="请输入部门名称"
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
