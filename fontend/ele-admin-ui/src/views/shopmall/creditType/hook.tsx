import dayjs from "dayjs";
import { message } from "@/utils/message";
import CreditApi, { Pagination, Credits } from "@/api/shopmall/credit";
import { ElMessageBox } from "element-plus";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";
import { ComStatus } from "@/api/model";

export function useCreditType() {
  const form: Pagination = reactive({
    sort: 0,
    sortField: null,
    page: 1,
    limit: 10
  });
  const dataList: Ref<Array<Credits>> = ref([]);
  const loading = ref(true);
  const switchLoadMap = ref({});
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
      label: "用户编号",
      prop: "id",
      minWidth: 130
    },
    {
      label: "货币名称",
      prop: "name",
      minWidth: 130
    },
    {
      label: "货币标识",
      prop: "creditKey",
      minWidth: 130
    },
    {
      label: "货币类型",
      prop: "creditType",
      minWidth: 90,
      cellRenderer: ({ row, props }) => (
        <el-tag
          size={props.size}
          type={row.creditType === 1 ? "danger" : ""}
          effect="plain"
        >
          {row.creditType === 1 ? "现金" : "虚拟货币"}
        </el-tag>
      )
    },
    {
      label: "状态",
      prop: "status",
      minWidth: 90,
      cellRenderer: scope => (
        <x-status v-model={scope.row.status} type="cell" class="!w-[200px]" />
      )
    },
    {
      label: "创建时间",
      minWidth: 90,
      prop: "createTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "操作",
      fixed: "right",
      width: 180,
      slot: "operation"
    }
  ];
  const buttonClass = computed(() => {
    return [
      "!h-[20px]",
      "reset-margin",
      "!text-gray-500",
      "dark:!text-white",
      "dark:hover:!text-primary"
    ];
  });
  interface EditDrawer {
    show: boolean;
    editId: string;
  }
  const editDrawer = reactive<EditDrawer>({
    show: false,
    editId: ""
  });
  function handleUpdate(row?: Credits) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }
  async function handleDelete(row) {
    if (row?.id) {
      await CreditApi.apiCreditModifyStatus(row.id, ComStatus.Deleted);
      message(`删除成功`, { type: "success" });
      onSearch();
    }
  }

  function handleSizeChange(val: number) {
    console.log(`${val} items per page`);
  }

  function handleCurrentChange(val: number) {
    console.log(`current page: ${val}`);
  }

  function handleSelectionChange(val) {
    console.log("handleSelectionChange", val);
  }

  async function onSearch() {
    loading.value = true;
    const data = await CreditApi.apiCreditFindAll({
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

  const resetForm = formEl => {
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  };

  onMounted(() => {
    onSearch();
  });

  return {
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
    handleSelectionChange
  };
}
