import ApiApi from "@/api/api";
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
      label: "分组名",
      prop: "displayControllerName",
      minWidth: 120,
      align: "left"
    },
    // {
    //   label: "控制器",
    //   prop: "controllerName",
    //   minWidth: 120,
    //   align: "left"
    // },
    {
      label: "接口",
      prop: "displayActionName",
      minWidth: 120,
      align: "left"
    },
    {
      label: "HTTP方法",
      prop: "httpMethod",
      width: 120,
      align: "left",
      formatter: ({ httpMethod }) => {
        if (!httpMethod) return null;
        let type = "warning";
        if (httpMethod == "POST") type = "primary";
        else if (httpMethod == "GET") type = "success";
        else if (httpMethod == "DELETE") type = "error";
        return <el-tag type={type}>{httpMethod}</el-tag>;
      }
    },
    {
      label: "路由模板",
      prop: "routeTemplate",
      minWidth: 260,
      align: "left"
    },
    {
      label: "方法描述",
      prop: "ationDescriptor",
      minWidth: 80,
      align: "left"
    }
    // ,
    // {
    //   label: "",
    //   type: "expand",
    //   fixed: "left",
    //   cellRenderer: ({ row }) => {
    //     const headStyle = {
    //       background: "var(--el-table-row-hover-bg-color)",
    //       color: "var(--el-text-color-primary)"
    //     };
    //     const tableStyle = {
    //       position: "sticky",
    //       left: "60px",
    //       width: "90%",
    //       marginLeft: "5%"
    //     };
    //     return (
    //       <el-table
    //         data={row.apis}
    //         size="small"
    //         border
    //         style={tableStyle}
    //         header-cell-style={headStyle}
    //       >
    //         <el-table-column label="接口" prop="displayActionName" />
    //         <el-table-column label="接口" prop="actionName" />
    //         <el-table-column label="HTTP方法" width="80" prop="httpMethod" />
    //         <el-table-column label="版本号" width="80" prop="apiVersion" />
    //         <el-table-column label="路由模板" prop="routeTemplate" />
    //         <el-table-column label="路由完整地址" prop="routePath" />
    //         <el-table-column label="方法描述" prop="ationDescriptor" />
    //       </el-table>
    //     );
    //   }
    // }
  ];

  function resetForm(formEl) {
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  }

  async function onSearch() {
    loading.value = true;
    const data = await ApiApi.getApis();
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
    onSearch,
    resetForm
  };
}
