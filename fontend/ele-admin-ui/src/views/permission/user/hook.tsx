import dayjs from "dayjs";
import { message } from "@/utils/message";
import UserApi from "@/api/user";
import OrgApi from "@/api/org";
import { UserFindAllArgs, UserFindAllModel } from "@/api/model";
import { ElMessageBox } from "element-plus";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";
import { handleTree } from "@/utils/tree";

export function useUser() {
  const orgTree = ref([]);
  onMounted(async () => {
    const data = await OrgApi.apiOrgGetList();
    orgTree.value = handleTree(data);
  });
  const form: UserFindAllArgs = reactive({
    usernameLike: "",
    mobile: "",
    status: null,
    roleId: "",
    sort: 0,
    sortField: null,
    orgId: 0,
    page: 1,
    limit: 10
  });
  const dataList: Ref<Array<UserFindAllModel>> = ref([]);
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
      label: "用户名称",
      prop: "username",
      minWidth: 130
    },
    {
      label: "用户昵称",
      prop: "nickname",
      minWidth: 130
    },
    {
      label: "性别",
      prop: "sex",
      minWidth: 90,
      cellRenderer: ({ row, props }) => (
        <el-tag
          size={props.size}
          type={row.sex === 1 ? "danger" : ""}
          effect="plain"
        >
          {row.sex === 1 ? "女" : "男"}
        </el-tag>
      )
    },
    // {
    //   label: "部门",
    //   prop: "dept",
    //   minWidth: 90,
    //   formatter: ({ dept }) => dept?.name
    // },
    {
      label: "手机号码",
      prop: "mobile",
      minWidth: 90
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

  function onOrgChange(orgId?: number) {
    form.orgId = orgId;
    onSearch();
  }

  interface EditDrawer {
    show: boolean;
    editId: string;
  }
  const editDrawer = reactive<EditDrawer>({
    show: false,
    editId: ""
  });
  const editRoleDrawer = reactive<EditDrawer>({
    show: false,
    editId: ""
  });
  function onChange({ row, index }) {
    ElMessageBox.confirm(
      `确认要<strong>${
        row.status === 0 ? "停用" : "启用"
      }</strong><strong style='color:var(--el-color-primary)'>${
        row.username
      }</strong>用户吗?`,
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
          message("已成功修改用户状态", {
            type: "success"
          });
        }, 300);
      })
      .catch(() => {
        row.status === 0 ? (row.status = 1) : (row.status = 0);
      });
  }

  function handleUpdate(row?: UserFindAllModel) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
  }
  function handleRoleUpdate(row?: UserFindAllModel) {
    (editRoleDrawer.show = true), (editRoleDrawer.editId = row?.id);
  }
  async function handleDelete(row) {
    if (row?.id) {
      await UserApi.apiUserRemove(row.id);
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
    const data = await UserApi.apiUserFindAll({
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

  async function handleResetPassword(row: any) {
    if (row?.id) {
      const data = await UserApi.apiUserResetPassword(row.id);
      ElMessageBox.alert(`重置后密码为<strong> ${data} </strong>`, "提示", {
        confirmButtonText: "确定",
        type: "success",
        dangerouslyUseHTMLString: true
      });
    }
  }

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
    editRoleDrawer,
    orgTree,
    onSearch,
    resetForm,
    handleUpdate,
    handleDelete,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange,
    handleResetPassword,
    handleRoleUpdate,
    onOrgChange
  };
}
