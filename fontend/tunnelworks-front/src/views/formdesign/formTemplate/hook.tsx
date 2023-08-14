import dayjs from "dayjs";
import { message } from "@/utils/message";
import FormTemplateApi, {
  FormTemplateQueryRequest,
  FormType
} from "@/api/formTemplate";
import { ElMessageBox } from "element-plus";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";

export function useFormTemplate() {
  const form: FormTemplateQueryRequest = reactive({
    nameLike: "",
    status: null,
    formType: null,
    page: 1,
    limit: 10
  });
  const dataList = ref([]);
  const loading = ref(true);
  const switchLoadMap = ref({});
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true
  });
  const router = useRouter();
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
      minWidth: 200
    },
    {
      label: "表单名称",
      prop: "name",
      minWidth: 120,
      cellRenderer: ({ row }) => (
        <el-button type="primary" link onClick={() => goPath(row.id)}>
          {row.name}
        </el-button>
      )
    },
    {
      label: "表单类型",
      prop: "formType",
      minWidth: 150,
      cellRenderer: ({ row, props }) => (
        <el-tag
          size={props.size}
          type={row.formType === 1 ? "danger" : "warning"}
          effect="plain"
        >
          {row.formType == FormType.Flow
            ? "流程"
            : row.formType == FormType.Form
            ? "表单"
            : row.formType == FormType.Question
            ? "问卷"
            : "未知"}
        </el-tag>
      )
    },
    {
      label: "状态",
      minWidth: 130,
      cellRenderer: ({ row }) => <x-status v-model={row.status} type="cell" />
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

  function onChange({ row, index }) {
    ElMessageBox.confirm(
      `确认要<strong>${
        row.status === 0 ? "停用" : "启用"
      }</strong><strong style='color:var(--el-color-primary)'>${
        row.name
      }</strong>角色吗?`,
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
          message("已成功修改角色状态", {
            type: "success"
          });
        }, 300);
      })
      .catch(() => {
        row.status === 0 ? (row.status = 1) : (row.status = 0);
      });
  }
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
    // if (row?.id) {
    //   await FormTemplateApi.apiRoleRemoveRole(row.id);
    //   message("删除成功！", { type: "success" });
    //   onSearch();
    // }
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
    const data = await FormTemplateApi.list({
      ...form,
      page: pagination.pageCount,
      limit: pagination.pageSize
    });
    dataList.value = data.rows;
    pagination.total = data.total;
    setTimeout(() => {
      loading.value = false;
    }, 500);
  }
  const goPath = formId => {
    router.push({ path: "/formdesign/design", query: { formId: formId } });
  };
  const resetForm = formEl => {
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  };

  onMounted(() => {
    onSearch();
  });

  const editForm = row => {
    router.push({ path: "/formdesign/render", query: { formId: row.id } });
  };

  return {
    form,
    loading,
    columns,
    dataList,
    pagination,
    buttonClass,
    editDrawer,
    onSearch,
    editForm,
    resetForm,
    handleUpdate,
    handleDelete,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange
  };
}
