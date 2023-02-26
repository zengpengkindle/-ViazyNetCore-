
import dayjs from "dayjs";
import { message } from "@/utils/message";
import DictionaryApi, { DicFindAllArgs, DictionaryFindAllModel } from "@/api/system";
import { ElMessageBox } from "element-plus";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";
import { number } from "echarts/core";



export function useDic() {
    const form: DicFindAllArgs = reactive({
        nameLike: null,
        sort: 0,
        sortField: null,
        page: 1,
        limit: 10
    });
    const dataList: Ref<Array<DictionaryFindAllModel>> = ref([]);
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
            label: "字典名称",
            prop: "name",
            minWidth: 130
        },
        {
            label: "字典编号",
            prop: "code",
            minWidth: 130
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
    ]
    interface EditDialog {
        show: boolean;
        editId: number;
    }
    const editDialog = reactive<EditDialog>({
        show: false,
        editId: 0
    });
    function handleUpdate(row?: DictionaryFindAllModel) {
        (editDialog.show = true), (editDialog.editId = row?.id ?? 0);
    }

    async function handleDelete(row) {
        if (row?.id) {
            await DictionaryApi.remove(row.id);
            message(`删除成功`, { type: "success" });
            onSearch();
        }
    }

    async function onSearch() {
        loading.value = true;
        const data = await DictionaryApi.findAll({
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
    onMounted(() => {
        onSearch();
    });


    return {
        form,
        loading,
        columns,
        dataList,
        pagination,
        editDialog,
        onSearch,
        handleUpdate,
        handleDelete
    };
}