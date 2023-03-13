<script setup lang="ts">
import { useProduct } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

import Delete from "@iconify-icons/ep/delete";
import EditPen from "@iconify-icons/ep/edit-pen";
import AddFill from "@iconify-icons/ri/add-circle-line";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import { reactive, ref } from "vue";
import { FormInstance } from "element-plus";
defineOptions({
  name: "productOuter"
});
const formRef = ref<FormInstance>();
const options = reactive([
  {
    value: false,
    label: "否"
  },
  {
    value: true,
    label: "是"
  }
]);
const statusOptions = reactive([
  {
    value: "",
    label: "全部"
  },
  {
    value: 0,
    label: "未上架"
  },
  {
    value: 1,
    label: "待审核"
  },
  {
    value: 2,
    label: "出售中"
  }
]);
const {
  form,
  loading,
  columns,
  dataList,
  pagination,
  onSearch,
  resetForm,
  handleUpdate,
  handleDelete,
  handleSizeChange,
  handleCurrentChange,
  handleSelectionChange
} = useProduct();
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
        <el-form-item label="标题">
          <el-input placeholder="模糊搜索" v-model="form.titleLike" />
        </el-form-item>

        <el-form-item label="类目">
          <el-input placeholder="精准搜索" v-model="form.catName" />
        </el-form-item>
        <el-form-item label="前台隐藏">
          <el-select v-model="form.isHidden" placeholder="全部">
            <el-option
              v-for="r in options"
              v-bind:key="r.label"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="form.status" placeholder="全部">
            <el-option
              v-for="r in statusOptions"
              :key="r.value"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="创建时间">
          <el-date-picker
            v-model="form.createTimes"
            type="daterange"
            :shortcuts="$pickerOptions.shortcuts"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
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
      <PureTableBar title="商品类别管理" @refresh="onSearch">
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
            </template>
          </pure-table>
        </template>
      </PureTableBar>
    </div>
  </div>
</template>

<style scoped lang="scss">
:deep(.el-dropdown-menu__item i) {
  margin: 0;
}
</style>
