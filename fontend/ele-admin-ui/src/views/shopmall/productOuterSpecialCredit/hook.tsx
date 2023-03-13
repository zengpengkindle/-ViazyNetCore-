import dayjs from "dayjs";
import { message } from "@/utils/message";
import CreditApi from "@/api/shopmall/credit";
import ProductOuterSpecialCreditApi, {
  ProductOuterSpecialCredit,
  SpecialCreditPagination
} from "@/api/shopmall/productOuterSpecialCredit";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";
import { ComStatus } from "@/api/model";
import { useRoute } from "vue-router";

export function useProductOuterCredit() {
  const form: SpecialCreditPagination = reactive({
    outerType: null,
    sort: 0,
    sortField: null,
    page: 1,
    limit: 10
  });
  const dataList: Ref<Array<ProductOuterSpecialCredit>> = ref([]);
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
      label: "编号",
      prop: "id",
      minWidth: 180
    },
    {
      label: "价格类型标识",
      prop: "objectKey",
      minWidth: 180
    },
    {
      label: "价格类型名称",
      prop: "objectName",
      minWidth: 180
    },
    {
      label: "活动/类别",
      prop: "outerType",
      minWidth: 180
    },
    {
      label: "货币名称",
      minWidth: 180,
      cellRenderer: scope => (
        <span>
          {creditOptions[scope.row.creditKey]}({scope.row.creditKey})
        </span>
      )
    },
    {
      label: "价格计算方式",
      cellRenderer: scope => (
        <template>
          <span v-if={scope.row.computeType == 0}>独立价格</span>
          <span v-else-if={scope.row.computeType == 1}>与商品设置价格等价</span>
          <span v-else-if={scope.row.computeType == 2}>
            计算兑换手续费-固定 {scope.row.feeMoney}
          </span>
          <span v-else-if={scope.row.computeType == 3}>
            计算百分比手续费 {scope.row.feePercent}%
          </span>
          <span v-else-if={scope.row.computeType == 4}>混合价格</span>
          <span v-else-if={scope.row.computeType == 5}>条件式</span>
        </template>
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
  const creditOptions: Ref<any> = ref({});

  function handleUpdate(row?: ProductOuterSpecialCredit) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }
  async function handleDelete(row) {
    if (row?.id) {
      await ProductOuterSpecialCreditApi.apiProductOuterSpecialCreditModifyStatus(
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
    const data =
      await ProductOuterSpecialCreditApi.apiProductOuterSpecialCreditFindAll({
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

  const route = useRoute();
  onMounted(async () => {
    creditOptions.value = await CreditApi.apiCreditGetAll();
    console.log(route.query);
    const outerType = route.query.outerType as string;
    form.outerType = outerType;
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
