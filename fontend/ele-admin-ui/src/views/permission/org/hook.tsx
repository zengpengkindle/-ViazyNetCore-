import dayjs from "dayjs";
import { handleTree } from "@/utils/tree";
import OrgApi from "@/api/org";
import { message } from "@/utils/message";
import { reactive, ref, onMounted } from "vue";

export function useDept() {
  const form: { key?: string } = reactive({});
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
      label: "主键",
      prop: "id",
      minWidth: 70,
      hide: ({ checkList }) => !checkList.includes("序号列")
    },
    {
      label: "部门名称",
      prop: "name",
      minWidth: 260,
      align: "left"
    },
    {
      label: "排序",
      prop: "sort",
      minWidth: 70,
      sortable: true,
      sortBy: "sort"
    },
    {
      label: "状态",
      prop: "status",
      minWidth: 100,
      cellRenderer: ({ row }) => (
        <x-status type="cell" v-model={row.status}></x-status>
      )
    },
    {
      label: "创建时间",
      minWidth: 200,
      prop: "createTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "操作",
      fixed: "right",
      width: 160,
      slot: "operation"
    }
  ];
  interface EditDrawer {
    show: boolean;
    editId: number;
  }
  const editDrawer = reactive<EditDrawer>({
    show: false,
    editId: 0
  });
  function handleUpdate(row) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }

  async function handleDelete(row) {
    await OrgApi.apiOrgDelete(row.id);
    message("删除成功", { type: "success" });
    await onSearch();
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
    const data = await OrgApi.apiOrgGetList(form.key);
    dataList.value = handleTree(data);
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
    editDrawer,
    onSearch,
    resetForm,
    handleUpdate,
    handleDelete,
    handleSelectionChange
  };
}
