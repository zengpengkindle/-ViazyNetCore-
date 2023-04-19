import dayjs from "dayjs";
import { message } from "@/utils/message";
import TaskApi, { TasksQz } from "@/api/task";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";

export function useTask() {
  const form = reactive({
    key: ""
  });
  const dataList: Ref<Array<TasksQz>> = ref([]);
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
      label: "",
      type: "expand",
      fixed: "left",
      cellRenderer: ({ row }) => {
        const headStyle = {
          background: "var(--el-table-row-hover-bg-color)",
          color: "var(--el-text-color-primary)"
        };
        const tableStyle = {
          // position: "sticky",
          left: "60px"
          // width: "800px"
        };
        return (
          <el-table
            data={row.triggers}
            size="small"
            border
            style={tableStyle}
            header-cell-style={headStyle}
          >
            <el-table-column prop="jobId" label="任务编号" fixed width="190" />
            <el-table-column prop="jobGroup" label="任务" fixed width="190" />
            <el-table-column
              prop="triggerId"
              label="触发器ID"
              fixed
              width="190"
            />
            <el-table-column
              prop="triggerName"
              label="触发器名称"
              fixed
              width="190"
            />
            <el-table-column
              prop="triggerGroup"
              label="触发器分组"
              fixed
              width="190"
            />
            <el-table-column
              prop="triggerStatus"
              label="触发器状态"
              fixed
              width="190"
            />
          </el-table>
        );
      }
    },
    {
      label: "任务编号",
      prop: "id",
      minWidth: 450
    },
    {
      label: "任务组",
      prop: "jobGroup",
      minWidth: 230
    },
    {
      label: "名称",
      prop: "name",
      minWidth: 230
    },
    {
      label: "任务类型",
      minWidth: 130,
      cellRenderer: ({ row }) => (
        <el-tag
          type={row.triggerType == 1 ? "success" : ""}
          disable-transitions
        >
          {row.triggerType == 1 ? "Cron" : "Simple"}
        </el-tag>
      )
    },
    {
      label: "状态",
      prop: "status",
      minWidth: 120,
      cellRenderer: scope => (
        <el-tag
          type={scope.row.isStart ? "success" : "danger"}
          disable-transitions
        >
          {scope.row.isStart ? "启用" : "停止"}
        </el-tag>
      )
    },
    {
      label: "Cron表达式",
      prop: "cron",
      minWidth: 130
    },
    {
      label: "程序集",
      prop: "assemblyName",
      minWidth: 90
    },
    {
      label: "执行类",
      prop: "className",
      minWidth: 90
    },
    {
      label: "并行数",
      prop: "triggerCount",
      minWidth: 90
    },

    {
      label: "当前并行数",
      minWidth: 90,
      prop: "triggers",
      formatter: ({ triggers }) => triggers?.length
    },
    {
      label: "开始时间",
      minWidth: 90,
      prop: "beginTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "结束时间",
      minWidth: 90,
      prop: "endTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "累计运行(次)",
      prop: "runTimes",
      minWidth: 90
    },
    {
      label: "循环周期(秒)",
      prop: "intervalSecond",
      minWidth: 90
    },
    {
      label: "循环(次)",
      prop: "cycleRunTimes",
      minWidth: 90
    },
    {
      label: "已循环(次)",
      prop: "cycleHasRunTimes",
      minWidth: 90
    },
    {
      label: "创建时间",
      minWidth: 90,
      prop: "createTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "创建时间",
      minWidth: 90,
      prop: "createTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },

    {
      label: "运行状态",
      prop: "status",
      fixed: "right",
      minWidth: 120,
      cellRenderer: scope => (
        <el-tag
          type={
            scope.row.triggers[0].triggerStatus == "正常" ? "success" : "danger"
          }
          disable-transitions
        >
          {scope.row.triggers[0].triggerStatus}
        </el-tag>
      )
    },
    {
      label: "操作",
      fixed: "right",
      width: 220,
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
  const editRoleDrawer = reactive<EditDrawer>({
    show: false,
    editId: 0
  });

  function handleUpdate(row?: TasksQz) {
    (editDrawer.editId = row?.id), (editDrawer.show = true);
  }
  async function handleDelete(row) {
    if (row?.id) {
      await TaskApi.delete(row.id);
      message(`删除成功`, { type: "success" });
      onSearch();
    }
  }
  async function startJob(row) {
    if (row?.id) {
      await TaskApi.startJob(row.id);
      message(`启动计划任务成功`, { type: "success" });
      onSearch();
    }
  }
  async function stopJob(row) {
    if (row?.id) {
      await TaskApi.stopJob(row.id);
      message(`成功停止计划任务`, { type: "success" });
      onSearch();
    }
  }
  async function pauseJob(row) {
    if (row?.id) {
      await TaskApi.pauseJob(row.id);
      message(`暂停计划任务成功`, { type: "success" });
      onSearch();
    }
  }
  async function resumeJob(row) {
    if (row?.id) {
      await TaskApi.resumeJob(row.id);
      message(`恢复计划任务成功`, { type: "success" });
      onSearch();
    }
  }
  async function reCovery(row) {
    if (row?.id) {
      await TaskApi.reCovery(row.id);
      message(`重启计划任务成功`, { type: "success" });
      onSearch();
    }
  }

  async function executeJob(row) {
    console.log(row);
    if (row?.id) {
      await TaskApi.executeJob(row.id);
      message(`立即执行任务成功`, { type: "success" });
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
    const data = await TaskApi.getList(
      pagination.currentPage,
      pagination.pageSize,
      form.key
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
    editRoleDrawer,
    onSearch,
    resetForm,
    handleUpdate,
    handleDelete,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange,
    startJob,
    stopJob,
    pauseJob,
    resumeJob,
    reCovery,
    executeJob
  };
}
