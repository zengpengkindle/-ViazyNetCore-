import PermissionApi from "@/api/permission";
import { reactive, ref, onMounted, Ref } from "vue";

export function usePermission() {
  const form = reactive({
    user: "",
    status: ""
  });
  const dataList = ref([]);
  const loading = ref(true);

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
      minWidth: 70,
      hide: ({ checkList }) => !checkList.includes("序号列")
    },
    {
      label: "权限名称",
      prop: "name",
      minWidth: 180,
      align: "left"
    },
    {
      label: "Key",
      prop: "key",
      minWidth: 120,
      align: "left"
    },
    {
      label: "操作",
      width: 150,
      fixed: "right",
      slot: "operation"
    }
  ];
  const selectRow: Ref<{ pid?: string; name?: string; type: number }> = ref({
    pid: null,
    name: null,
    type: 1
  });
  function handleUpdate(row: any, type: number) {
    selectRow.value.pid = row?.key;
    selectRow.value.name = row?.name;
    selectRow.value.type = type ?? 1;
  }

  function handleDelete(row) {
    console.log(row);
  }

  function handleSelectionChange(val) {
    console.log("handleSelectionChange", val);
  }

  function resetForm(formEl) {
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  }

  async function onSearch() {
    loading.value = true;
    const data = await PermissionApi.apiPermissionGetAll();
    dataList.value = data;
    setTimeout(() => {
      loading.value = false;
    }, 500);
  }

  onMounted(() => {
    onSearch();
  });

  return {
    form,
    loading,
    columns,
    dataList,
    selectRow,
    onSearch,
    resetForm,
    handleUpdate,
    handleDelete,
    handleSelectionChange
  };
}
