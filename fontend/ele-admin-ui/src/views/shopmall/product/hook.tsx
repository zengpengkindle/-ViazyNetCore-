import dayjs from "dayjs";
import { message } from "@/utils/message";
import { Pagination } from "@/api/model";
import ProductOuterApi, { ProductOuter } from "@/api/shopmall/productOuter";
import { ElMessageBox } from "element-plus";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";
import { ComStatus } from "@/api/model";
import { useRouter } from "vue-router";

export function useProductOuter() {
  const form: Pagination = reactive({
    sort: 0,
    sortField: null,
    page: 1,
    limit: 10
  });
  const router = useRouter();
  const dataList: Ref<Array<ProductOuter>> = ref([]);
  const loading = ref(true);
  const switchLoadMap = ref({});
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  interface CreditType {
    value: string;
    label: string;
  }
  const getCreditTypes = (): Array<CreditType> => {
    const defaultCreditTypes: Array<CreditType> = [
      { value: "", label: "普通商品" }
    ];

    return [...defaultCreditTypes];
    // await ProductOuterApi.apiProductOuterGetAll()
  };

  const goCreditPath = (row: ProductOuter) => {
    router.push({
      path: "../productOuterSpecialCredit/index",
      query: {
        outerType: row.outerType
      }
    });
  };
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
      label: "活动编号",
      prop: "id",
      minWidth: 180
    },
    {
      label: "类别/活动名称",
      prop: "outerName",
      minWidth: 180
    },
    {
      label: "类别/活动标识",
      prop: "outerType",
      cellRenderer: ({ row }) => (
        <el-button type="primary" text onClick={() => goCreditPath(row)}>
          {row.outerType}
        </el-button>
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
      label: "开始时间",
      minWidth: 120,
      prop: "beginTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "结束时间",
      minWidth: 120,
      prop: "endTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "创建时间",
      minWidth: 120,
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
  function onChange({ row, index }) {
    ElMessageBox.confirm(
      `确认要<strong>${
        row.status === 0 ? "停用" : "启用"
      }</strong><strong style='color:var(--el-color-primary)'>${
        row.username
      }</strong>吗?`,
      "系统提示",
      {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        dangerouslyUseHTMLString: true,
        draggable: true
      }
    )
      .then(() => {
        switchLoadMap.value[index] = Object.assign(
          {},
          switchLoadMap.value[index],
          {
            loading: true
          }
        );
        setTimeout(() => {
          switchLoadMap.value[index] = Object.assign(
            {},
            switchLoadMap.value[index],
            {
              loading: false
            }
          );
          message("已成功修改状态", {
            type: "success"
          });
        }, 300);
      })
      .catch(() => {
        row.status === 0 ? (row.status = 1) : (row.status = 0);
      });
  }

  function handleUpdate(row?: ProductOuter) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }
  async function handleDelete(row) {
    if (row?.id) {
      await ProductOuterApi.apiProductOuterModifyStatus(
        row.id,
        ComStatus.Deleted
      );
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
    const data = await ProductOuterApi.apiProductOuterFindAll({
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
    getCreditTypes();
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
