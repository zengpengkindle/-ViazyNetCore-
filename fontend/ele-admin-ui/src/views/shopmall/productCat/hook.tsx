import dayjs from "dayjs";
import { message } from "@/utils/message";
import { Pagination } from "@/api/model";
import ProductCatApi, { CatRes } from "@/api/shopmall/productCat";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";

export function useProductOuter() {
  const form: Pagination = reactive({
    sort: 0,
    sortField: null,
    page: 1,
    limit: 10
  });
  const dataList: Ref<Array<CatRes>> = ref([]);
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
      label: "类目名称",
      prop: "name",
      minWidth: 180
    },
    {
      label: "父级Id",
      prop: "parentId",
      minWidth: 180
    },
    {
      label: "排序",
      prop: "sort",
      minWidth: 180
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

  function handleUpdate(row?: CatRes) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }
  async function handleDelete(row) {
    if (row?.id) {
      // await ProductCatApi.modifyStatus(row.id, ComStatus.Deleted);
      // message(`删除成功`, { type: "success" });
      // onSearch();
    }
  }

  function handleSizeChange() {
    onSearch();
  }

  function handleCurrentChange() {
    onSearch();
  }

  function handleSelectionChange(val: number) {
    console.log("handleSelectionChange", val);
  }

  async function onSearch() {
    loading.value = true;

    const data = await ProductCatApi.findPageList(
      0,
      "",
      pagination.currentPage,
      pagination.pageSize
    );
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
