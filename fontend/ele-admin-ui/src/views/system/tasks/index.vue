<script setup lang="ts">
import { ref } from "vue";
import { useTask } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

import Role from "@iconify-icons/ri/admin-line";
import Password from "@iconify-icons/ri/lock-password-line";
import More from "@iconify-icons/ep/more-filled";
import Delete from "@iconify-icons/ep/delete";
import Notebook from "@iconify-icons/ep/notebook";
import EditPen from "@iconify-icons/ep/edit-pen";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import AddFill from "@iconify-icons/ri/add-circle-line";
import editorDialog from "./editDialog.vue";

defineOptions({
  name: "Tasks"
});

const formRef = ref();
const {
  form,
  loading,
  columns,
  dataList,
  pagination,
  buttonClass,
  editDrawer,
  onSearch,
  resetForm,
  handleUpdate,
  handleDelete,
  handleSizeChange,
  handleCurrentChange,
  handleSelectionChange,
  startJob,
  stopJob,
  pauseJob,
  resumeJob,
  reCovery,
  executeJob
} = useTask();
</script>

<template>
  <div class="main">
    <div>
      <el-form
        ref="formRef"
        :inline="true"
        :model="form"
        class="bg-bg_color w-[99/100] pl-8 pt-4"
      >
        <el-form-item label="关键词" prop="key">
          <el-input
            v-model="form.key"
            placeholder="请输入关键词"
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
          <el-button :icon="useRenderIcon(Refresh)" @click="resetForm(formRef)">
            重置
          </el-button>
        </el-form-item>
      </el-form>

      <PureTableBar title="任务管理" @refresh="onSearch">
        <template #buttons>
          <el-button
            type="primary"
            :icon="useRenderIcon(AddFill)"
            @click="handleUpdate(null)"
          >
            新增任务
          </el-button>
        </template>
        <template v-slot="{ size, checkList }">
          <pure-table
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
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          >
            <template #operation="{ row }">
              <el-popover
                trigger="hover"
                :popper-style="{ width: '450px' }"
                placement="left"
                v-if="row.remark"
              >
                <div style="overflow: hidden" v-html="row.remark" />
                <template v-slot:reference>
                  <el-button
                    link
                    type="primary"
                    :size="size"
                    :icon="useRenderIcon(Notebook)"
                  />
                </template>
              </el-popover>
              <el-button
                class="reset-margin"
                link
                type="primary"
                :size="size"
                @click="handleUpdate(row)"
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
              <el-dropdown>
                <el-button
                  class="ml-3 mt-[2px]"
                  link
                  type="primary"
                  :size="size"
                  :icon="useRenderIcon(More)"
                />
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item>
                      <el-button
                        class="buttonClass"
                        link
                        type="info"
                        :size="size"
                        :icon="useRenderIcon('fa-solid:caret-square-right')"
                        @click="startJob(row)"
                      >
                        启动任务
                      </el-button>
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-button
                        class="buttonClass"
                        link
                        type="info"
                        :size="size"
                        :icon="useRenderIcon('fa:stop-circle')"
                        @click="stopJob(row)"
                      >
                        停止任务
                      </el-button>
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-button
                        :class="buttonClass"
                        link
                        type="primary"
                        :size="size"
                        :icon="useRenderIcon('fa:pause-circle')"
                        @click="pauseJob(row)"
                      >
                        暂停任务
                      </el-button>
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-button
                        :class="buttonClass"
                        link
                        type="primary"
                        :size="size"
                        :icon="useRenderIcon('fa:history')"
                        @click="resumeJob(row)"
                      >
                        恢复任务
                      </el-button>
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-button
                        :class="buttonClass"
                        link
                        type="primary"
                        :size="size"
                        :icon="useRenderIcon('fa:refresh')"
                        @click.stop="reCovery(row)"
                      >
                        重启任务
                      </el-button>
                    </el-dropdown-item>
                    <el-dropdown-item>
                      <el-button
                        :class="buttonClass"
                        link
                        type="primary"
                        :size="size"
                        :icon="useRenderIcon('fa:check-square')"
                        @click.stop="executeJob(row)"
                      >
                        立即执行
                      </el-button>
                    </el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
            </template>
          </pure-table>
        </template>
      </PureTableBar>
    </div>
    <editor-dialog
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
