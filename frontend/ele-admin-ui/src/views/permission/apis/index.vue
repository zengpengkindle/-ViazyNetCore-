<script setup lang="ts">
import { ref } from "vue";
import { useDept } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";

defineOptions({
  name: "Apis"
});

const formRef = ref();
const tableRef = ref();
const { form, loading, columns, dataList, onSearch, resetForm } = useDept();
</script>

<template>
  <div class="main">
    <el-form
      ref="formRef"
      :inline="true"
      :model="form"
      class="bg-bg_color w-[99/100] pl-8 pt-4"
    >
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

    <PureTableBar
      title="接口列表"
      :tableRef="tableRef?.getTableRef()"
      @refresh="onSearch"
    >
      <template v-slot="{ size, checkList }">
        <x-table
          ref="tableRef"
          align-whole="center"
          row-key="id"
          showOverflowTooltip
          table-layout="auto"
          default-expand-all
          :loading="loading"
          :size="size"
          :data="dataList"
          :columns="columns"
          :checkList="checkList"
          :header-cell-style="{
            background: 'var(--el-table-row-hover-bg-color)',
            color: 'var(--el-text-color-primary)'
          }"
          :tree-props="{ hasChildren: 'hasChildren', children: 'apis' }"
        />
      </template>
    </PureTableBar>
  </div>
</template>
