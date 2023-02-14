<script lang="ts" setup>
import { usePermission } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import PureTable from "@pureadmin/table";
import { ElButton, ElPopconfirm } from "element-plus";
import { ref } from "vue";


import Delete from "@iconify-icons/ep/delete";
import EditPen from "@iconify-icons/ep/edit-pen";
import AddFill from "@iconify-icons/ri/add-circle-line";

const tableRef = ref();
const {
    loading,
    columns,
    dataList,
    onSearch,
    handleDelete,
    handleUpdate,
    handleSelectionChange,
} = usePermission();
</script>
<template>
    <div class="main">
        <PureTableBar title="权限列表" :tableRef="tableRef?.getTableRef()" @refresh="onSearch">
            <template #buttons>
                <el-button type="primary" :icon="useRenderIcon(AddFill)">
                    新增权限
                </el-button>
            </template>
            <template v-slot="{ size, checkList }">
                <pure-table ref="tableRef" align-whole="center" row-key="id" showOverflowTooltip
                    table-layout="auto" :loading="loading" :size="size" :data="dataList"
                    :columns="columns" :checkList="checkList" :header-cell-style="{
                        background: 'var(--el-table-row-hover-bg-color)',
                        color: 'var(--el-text-color-primary)'
                    }" @selection-change="handleSelectionChange">
                    <template #operation="{ row }">
                        <el-button class="reset-margin" link type="primary" :size="size" @click="handleUpdate(row)"
                            :icon="useRenderIcon(EditPen)">
                            修改
                        </el-button>
                        <el-popconfirm title="是否确认删除?">
                            <template #reference>
                                <el-button class="reset-margin" link type="primary" :size="size"
                                    :icon="useRenderIcon(Delete)" @click="handleDelete(row)">
                                    删除
                                </el-button>
                            </template>
                        </el-popconfirm>
                    </template>
                </pure-table>
            </template>
        </PureTableBar>
    </div>
</template>