<script setup lang="ts">
import { ref } from "vue";
import { useDic } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import PureTable from "@pureadmin/table";
import { ElButton, ElPopconfirm } from "element-plus";
import EditPen from "@iconify-icons/ep/edit-pen";
import Delete from "@iconify-icons/ep/delete";
import Search from "@iconify-icons/ep/search";
import AddFill from "@iconify-icons/ri/add-circle-line";
import typevalue from "./typevalue.vue";
import editorDialog from "./editDialog.vue";

defineOptions({
  name: "Dictionary"
});
const formRef = ref();
const {
  form,
  loading,
  columns,
  dataList,
  pagination,
  editDialog,
  currentChange,
  onSearch,
  handleUpdate,
  handleCurrentChange,
  handleDelete
} = useDic();
</script>
<template>
  <div class="main">
    <div class="w-[45%] float-left">
      <el-form
        ref="formRef"
        :inline="true"
        :model="form"
        class="bg-bg_color w-[99/100] pl-8 pt-4"
      >
        <el-form-item label="" prop="username">
          <el-input
            v-model="form.name"
            placeholder="请输入名称或编码"
            clearable
            class="!w-[160px]"
          />
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            :icon="useRenderIcon(Search)"
            :loading="loading"
            @click="onSearch"
          >
            搜索
          </el-button>
        </el-form-item>
      </el-form>
      <PureTableBar title="字典管理" @refresh="onSearch">
        <template #buttons>
          <el-button
            type="primary"
            :icon="useRenderIcon(AddFill)"
            @click="handleUpdate(null)"
          >
            新增字典
          </el-button>
        </template>
        <template v-slot="{ size }">
          <pure-table
            align-whole="center"
            table-layout="auto"
            :loading="loading"
            :size="size"
            :data="dataList"
            :columns="columns"
            :pagination="pagination"
            highlight-current-row
            @row-click="handleCurrentChange"
            :pagination-small="size === 'small' ? true : false"
            :header-cell-style="{
              background: 'var(--el-table-row-hover-bg-color)',
              color: 'var(--el-text-color-primary)'
            }"
          >
            <template #operation="{ row }">
              <el-button
                class="reset-margin"
                link
                type="primary"
                :size="size"
                @click.stop="handleUpdate(row)"
                :icon="useRenderIcon(EditPen)"
              >
                修改
              </el-button>
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
          </pure-table>
        </template>
      </PureTableBar>
    </div>
    <div class="float-right w-[54%]" v-if="currentChange">
      <typevalue :type-id="currentChange" />
    </div>
    <editor-dialog
      v-model="editDialog.show"
      :id="editDialog.editId"
      @refresh="onSearch"
    />
  </div>
</template>
