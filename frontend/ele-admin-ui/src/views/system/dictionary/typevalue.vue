<script setup lang="tsx">
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { ElButton, ElPopconfirm } from "element-plus";
import { message } from "@/utils/message";
import DictionaryApi, {
  DicValueFindAllArgs,
  DictionaryValueModel
} from "@/api/system";
import PureTable, { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, onMounted, type Ref, defineProps, watch } from "vue";
import { nextTick } from "process";
import EditValueDialog from "./editValueDialog.vue";

import EditPen from "@iconify-icons/ep/edit-pen";
import Delete from "@iconify-icons/ep/delete";
import Search from "@iconify-icons/ep/search";
import AddFill from "@iconify-icons/ri/add-circle-line";

export interface ValueProps {
  typeId: number;
}
watch(
  () => {
    return props.typeId;
  },
  () => {
    if (props.typeId) {
      form.dictionaryTypeId = props.typeId;
      return onSearch();
    }
  }
);
const formRef = ref();

const props = defineProps<ValueProps>();
const form: DicValueFindAllArgs = reactive({
  name: null,
  sort: 0,
  sortField: null,
  page: 1,
  limit: 10,
  dictionaryTypeId: 0
});
const dataList: Ref<Array<DictionaryValueModel>> = ref([]);
const loading = ref(true);
const pagination = reactive<PaginationProps>({
  total: 0,
  pageSize: 10,
  currentPage: 1,
  background: true
});

const columns: TableColumnList = [
  {
    type: "selection",
    width: 55,
    align: "left",
    hide: ({ checkList }) => !checkList.includes("勾选列")
  },
  {
    label: "序号",
    type: "index",
    width: 70,
    hide: ({ checkList }) => !checkList.includes("序号列")
  },
  {
    label: "名称",
    prop: "name",
    minWidth: 130
  },
  {
    label: "编码",
    prop: "code",
    minWidth: 80
  },
  {
    label: "状态",
    prop: "status",
    minWidth: 90,
    cellRenderer: ({ row }) => (
      <x-status v-model={row.status} type="cell" class="!w-[200px]" />
    )
  },
  {
    label: "操作",
    fixed: "right",
    width: 180,
    slot: "operation"
  }
];
interface EditDialog {
  show: boolean;
  editId: number;
  typeId: number;
}
const editDialog = reactive<EditDialog>({
  show: false,
  editId: 0,
  typeId: props.typeId
});
function handleUpdate(row?: DictionaryValueModel) {
  (editDialog.show = true), (editDialog.editId = row?.id ?? 0);
}

async function handleDelete(row: { id: number }) {
  if (row?.id) {
    await DictionaryApi.removeValue(row.id);
    message(`删除成功`, { type: "success" });
    onSearch();
  }
}

async function onSearch() {
  loading.value = true;
  const data = await DictionaryApi.findValues({
    ...form,
    page: pagination.currentPage,
    limit: pagination.pageSize
  });
  dataList.value = data.rows;
  pagination.total = data.total;
  nextTick(() => {
    loading.value = false;
  });
}

onMounted(() => {
  if (props.typeId) {
    form.dictionaryTypeId = props.typeId;
    onSearch();
  }
});
</script>
<template>
  <div>
    <el-form
      ref="formRef"
      :inline="true"
      :model="form"
      class="bg-bg_color w-[99/100] pl-8 pt-4"
    >
      <el-form-item label="" prop="name">
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
          @click="handleUpdate(null)"
          :icon="useRenderIcon(AddFill)"
        >
          新增字典
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
        </x-table>
      </template>
    </PureTableBar>
    <EditValueDialog
      :id="editDialog.editId"
      :type-id="props.typeId"
      v-model="editDialog.show"
      @refresh="onSearch"
    />
  </div>
</template>
