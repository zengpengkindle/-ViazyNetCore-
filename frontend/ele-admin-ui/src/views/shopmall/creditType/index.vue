<script setup lang="ts">
import { useCreditType } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

import Delete from "@iconify-icons/ep/delete";
import AddFill from "@iconify-icons/ri/add-circle-line";
import edit from "./edit.vue";

defineOptions({
  name: "credit"
});

const {
  loading,
  columns,
  dataList,
  pagination,
  editDrawer,
  onSearch,
  handleUpdate,
  handleDelete,
  handleSizeChange,
  handleCurrentChange,
  handleSelectionChange
} = useCreditType();
</script>

<template>
  <div class="main">
    <div>
      <PureTableBar title="交易货币管理" @refresh="onSearch">
        <template #buttons>
          <el-button
            type="primary"
            :icon="useRenderIcon(AddFill)"
            @click="handleUpdate(null)"
          >
            新增
          </el-button>
        </template>
        <template v-slot="{ size, checkList }">
          <x-table
            align-whole="center"
            table-layout="auto"
            :loading="loading"
            :size="size"
            :data="dataList"
            :columns="columns"
            :checkList="checkList"
            :pagination="pagination"
            :paginationSmall="size === 'small' ? true : false"
            :header-cell-style="{
              background: 'var(--el-table-row-hover-bg-color)',
              color: 'var(--el-text-color-primary)'
            }"
            @selection-change="handleSelectionChange"
            @page-size-change="handleSizeChange"
            @page-current-change="handleCurrentChange"
          >
            <template #operation="{ row }">
              <el-popconfirm title="是否确认删除?" @confirm="handleDelete(row)">
                <template #reference>
                  <el-button
                    class="reset-margin"
                    link
                    type="primary"
                    :size="size"
                    :icon="useRenderIcon(Delete)"
                  >
                    删除
                  </el-button>
                </template>
              </el-popconfirm>
            </template>
          </x-table>
        </template>
      </PureTableBar>
    </div>
    <edit
      v-model="editDrawer.show"
      :id="editDrawer.editId"
      @refresh="onSearch"
    />
  </div>
</template>

<style scoped lang="scss">
:deep(.el-dropdown-menu__item i) {
  margin: 0;
}
</style>
