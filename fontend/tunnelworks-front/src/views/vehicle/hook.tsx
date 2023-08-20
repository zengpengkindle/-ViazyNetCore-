import { message } from "@/utils/message";
import VehicleApi, { VehicleQueryRequest } from "@/api/vehicle";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted } from "vue";

export function useVehicle() {
  const form: VehicleQueryRequest = reactive({
    catId: 0,
    orgId: 0,
    code: "",
    originPlace: "",
    licenseCode: "",
    frameCode: "",
    engineCode: ""
  });
  const dataList = ref([]);
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
      label: "单件编码",
      prop: "code",
      minWidth: 200
    },
    {
      label: "所属单位",
      prop: "orgName",
      minWidth: 120
    },
    {
      label: "车辆类别",
      prop: "type",
      minWidth: 150
    },
    {
      label: "车牌号",
      minWidth: 130,
      prop: "licenseCode"
    },
    {
      label: "发动机号",
      prop: "type",
      minWidth: 150
    },
    {
      label: "厂牌型号",
      prop: "spec",
      minWidth: 150
    },
    {
      label: "国别",
      minWidth: 180,
      prop: "originPlace"
    },
    {
      label: "车辆状态",
      minWidth: 180,
      prop: "vehicleStatus"
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
    editId: number;
  }
  const editDrawer = reactive<EditDrawer>({
    show: false,
    editId: 0
  });
  const editPermissionDrawer = reactive<EditDrawer>({
    show: false,
    editId: 0
  });
  function handleUpdate(row) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }
  function handlePermissionUpdate(row) {
    (editPermissionDrawer.show = true), (editPermissionDrawer.editId = row?.id);
  }
  async function handleDelete(row) {
    if (row?.id) {
      // await VehicleApi.Remove(row.id);
      message("删除成功！", { type: "success" });
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
    const data = await VehicleApi.list(
      {
        page: pagination.pageCount,
        limit: pagination.pageSize
      },
      {
        ...form
      }
    );
    dataList.value = data.rows;
    pagination.total = data.total;
    setTimeout(() => {
      loading.value = false;
    }, 500);
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
    editPermissionDrawer,
    onSearch,
    resetForm,
    handleUpdate,
    handlePermissionUpdate,
    handleDelete,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange
  };
}
