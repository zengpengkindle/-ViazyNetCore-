
import dayjs from "dayjs";
import { message } from "@/utils/message";
import DictionaryApi, { DicFindAllArgs, DictionaryFindAllModel } from "@/api/system";
import { ElMessageBox } from "element-plus";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, type Ref } from "vue";
import { nextTick } from "process";



export function useDicValue() {
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
            label: "名称",
            prop: "name",
            minWidth: 130
        },
        {
            label: "编码",
            prop: "code",
            minWidth: 130
        },
        {
            label: "值",
            prop: "value",
            minWidth: 130
        },
        {
            label: "操作",
            fixed: "right",
            width: 180,
            slot: "operation"
        }
    ]
    function handleUpdate(row?: DictionaryFindAllModel) {
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
        onSearch,
        handleUpdate,
        handleDelete
    };
}