import dayjs from "dayjs";
import { handleTree } from "@/utils/tree";
import PermissionApi from "@/api/permission";
import { message } from "@/utils/message";
import { reactive, ref, onMounted } from "vue";
import { IconifyIconOffline, IconifyIconOnline } from "@/components/ReIcon";

export function useDept() {
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
      label: "菜单名称",
      // prop: "name",
      minWidth: 260,
      align: "left",
      cellRenderer: ({ row, props }) => (
        <span>
          <span v-show={row.icon?.includes(":") === false}>
            <el-icon size={props.size}>
              <IconifyIconOffline icon={row.icon} />
            </el-icon>
            {row.icon?.includes(":")}
          </span>
          <span v-show={row.icon?.includes(":")}>
            <el-icon size={props.size}>
              <IconifyIconOnline icon={row.icon}></IconifyIconOnline>
            </el-icon>
          </span>
          {row.name}
        </span>
      )
    },
    {
      label: "排序",
      prop: "orderId",
      minWidth: 70
    },
    {
      label: "状态",
      prop: "status",
      minWidth: 100,
      cellRenderer: ({ row, props }) => (
        <el-tag
          size={props.size}
          type={row.status === 1 ? "success" : "danger"}
          effect="plain"
        >
          {row.status === 0 ? "关闭" : "开启"}
        </el-tag>
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
      label: "备注",
      prop: "remark",
      minWidth: 200
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
    editId: string;
  }
  const editDrawer = reactive<EditDrawer>({
    show: false,
    editId: ""
  });
  function handleUpdate(row) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }

  async function handleDelete(row) {
    await PermissionApi.apiPermissionRemoveMenu(row.id);
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
    const data = await PermissionApi.apiPermissionGetMenus();
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
