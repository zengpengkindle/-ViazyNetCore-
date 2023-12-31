import dayjs from "dayjs";
import { message } from "@/utils/message";
import ProductApi, {
  Product,
  ProductStatus,
  FindAllArguments
} from "@/api/shopmall/product";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";
import { useRouter } from "vue-router";

export function useProduct() {
  const form: FindAllArguments = reactive({
    shopId: null,
    titleLike: null,
    catName: null,
    isHidden: null,
    status: null,
    createTimes: [],
    sort: 0,
    sortField: null,
    page: 1,
    limit: 10
  });
  const dataList: Ref<Array<Product>> = ref([]);
  const loading = ref(true);
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

  const columns: TableColumnList = [
    {
      type: "selection",
      width: 55,
      align: "left",
      hide: ({ checkList }) => !checkList.includes("勾选列")
    },
    {
      label: "商品编号",
      prop: "id",
      width: 120,
      align: "left",
      hide: ({ checkList }) => !checkList.includes("序号列")
    },
    {
      label: "商品",
      width: 350,
      fixed: "left",
      cellRenderer: ({ row }) => (
        <div class="flex" style="width:350px">
          <div style="width:60px;" class="">
            <el-image
              style="width: 60px; height: 60px"
              src={row.image}
              fit="cover"
            />
          </div>
          <div class="pl-2" style="width:270px">
            <div class="text-left text-ellipsis">
              <el-tooltip
                class="box-item"
                effect="dark"
                content={row.title}
                placement="top-start"
              >
                <span class="text-ellipsis">{row.title}</span>
              </el-tooltip>
              <div>
                <el-tag type="warning">{row.catName}</el-tag>
              </div>
            </div>
          </div>
        </div>
      )
    },
    {
      label: "类目",
      prop: "catName",
      minWidth: 90
    },
    {
      label: "成本",
      prop: "cost",
      minWidth: 90
    },
    {
      label: "售价",
      prop: "price",
      minWidth: 90
    },
    {
      label: "多规格",
      prop: "openSpec",
      minWidth: 90,
      formatter: ({ openSpec }) =>
        openSpec ? <el-tag>是</el-tag> : <el-tag type="info">否</el-tag>
    },
    {
      label: "前台隐藏",
      minWidth: 120,
      prop: "isHidden",
      formatter: ({ isHidden }) =>
        isHidden ? <el-tag>是</el-tag> : <el-tag type="info">否</el-tag>
    },
    {
      label: "创建时间",
      minWidth: 150,
      prop: "endTime",
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
  const router = useRouter();
  function handleUpdate(row?: Product) {
    // (editDrawer.show = true), (editDrawer.editId = row?.id);
    router.push({
      path: "/shopmall/product/manage",
      query: { id: row?.id, outerType: row?.outerType }
    });
  }
  async function handleDelete(row) {
    if (row?.id) {
      await ProductApi.modifyStatus(row.id, ProductStatus.Delete);
      message(`删除成功`, { type: "success" });
      onSearch();
    }
  }

  function handleSizeChange(val: number) {
    console.log(`${val} items per page`);
    onSearch();
  }

  function handleCurrentChange(val: number) {
    console.log(`current page: ${val}`);
    onSearch();
  }

  function handleSelectionChange(val) {
    console.log("handleSelectionChange", val);
  }

  async function onSearch() {
    loading.value = true;
    const data = await ProductApi.findAll({
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
