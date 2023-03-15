import dayjs from "dayjs";
import { message } from "@/utils/message";
import { Pagination } from "@/api/model";
import TradeApi, {
  TradePageArgments,
  TradeDetailModel,
  BatchDeliveryModel
} from "@/api/shopmall/trade";
import { type PaginationProps } from "@pureadmin/table";
import { reactive, ref, computed, onMounted, Ref } from "vue";
import { nextTick } from "process";
import { useRouter } from "vue-router";

export function useTrade() {
  const form: Ref<TradePageArgments> = ref({
    memberId: "",
    tradeId: "",
    nickNameLike: "",
    username: "",
    shopId: "",
    shopName: "",
    createTimes: [],
    begin: "",
    end: "",
    tradeStatus: null,
    timeType: 1,
    payMode: null,
    sort: 0,
    sortField: null,
    page: 1,
    limit: 10
  });
  const router = useRouter();
  const dataList: Ref<Array<TradeDetailModel>> = ref([]);
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
      cellRenderer: ({ row }) => {
        const goPath = pId => ({
          path: "/product/manage",
          query: { id: pId }
        });
        const tabelCloum = {
          default: scope2 => {
            const row2 = scope2.row;
            return (
              <el-row gutter="20">
                <el-col span="8">
                  <el-image
                    style="width: 80px; height: 80px"
                    src={row2.imgUrl}
                    zoom-rate={1.2}
                    preview-src-list={[row2.imgUrl]}
                    fit="cover"
                  />
                </el-col>
                <el-col span="16">
                  <router-link
                    class="product_title"
                    target="_blank"
                    to={goPath(row.pId)}
                  >
                    {row2.pn}
                  </router-link>
                  <p>
                    <span>{row2.skuText}</span> <br />
                    <span style="color: red; margin-left: 15px">
                      <em>¥</em>
                      {row2.price}×{row2.num}
                    </span>
                  </p>
                </el-col>
              </el-row>
            );
          }
        };
        return (
          <el-table data={row.items} style="width: 100%">
            <el-table-column
              prop="orderId"
              label="子订单编号"
              fixed
              width="190"
            />
            <el-table-column
              label="商品"
              fixed
              width="450"
              v-slots={tabelCloum}
            ></el-table-column>
            <el-table-column prop="num" label="购买数" width="80" />
          </el-table>
        );
      }
    },
    {
      label: "订单编号",
      prop: "id",
      minWidth: 180
    },
    {
      label: "申请服务",
      prop: "title",
      minWidth: 180
    },
    {
      label: "用户账号",
      prop: "userName"
    },
    {
      label: "用户昵称",
      prop: "name"
    },
    {
      label: "店铺编号",
      prop: "shopId"
    },
    {
      label: "店铺名称",
      prop: "shopName"
    },
    {
      label: "状态",
      minWidth: 90,
      cellRenderer: ({ row }) => (
        <el-tooltip placement="bottom" effect="light">
          <template v-slot:content>
            收货人：{row.receiverName}
            <br />
            收货地址：{row.address.address}
            <br />
            收货电话：{row.address.tel}
            <br />
            快递单号：{row.logisticsCode}
            <br />
            快递公司：{row.logisticsCompany}
          </template>
          <el-button>收货信息</el-button>
        </el-tooltip>
      )
    },
    {
      label: "商品总金额",
      prop: "productMoney"
    },
    {
      label: "运费总金额",
      prop: "totalfeight"
    },
    {
      label: "订单总金额",
      prop: "totalMoney"
    },
    {
      label: "用户留言",
      cellRenderer: ({ row }) => (
        <el-tooltip content={row.message} placement="bottom" effect="light">
          <i class="el-icon-info" />
        </el-tooltip>
      )
    },
    {
      label: "付款时间",
      minWidth: 120,
      prop: "payTime",
      formatter: ({ payTime }) => dayjs(payTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "下单时间",
      minWidth: 120,
      prop: "createTime",
      formatter: ({ createTime }) =>
        dayjs(createTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "发货时间",
      minWidth: 120,
      prop: "consignTime",
      formatter: ({ consignTime }) =>
        dayjs(consignTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "完成时间",
      minWidth: 120,
      prop: "completeTime",
      formatter: ({ completeTime }) =>
        dayjs(completeTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "状态变更时间",
      minWidth: 120,
      prop: "statusChangedTime",
      formatter: ({ statusChangedTime }) =>
        dayjs(statusChangedTime).format("YYYY-MM-DD HH:mm:ss")
    },
    {
      label: "订单状态",
      minWidth: 120,
      cellRenderer: ({ row }) => {
        let tradeStatus = "";
        if (row.tradeStatus == -1) {
          tradeStatus = "待付款";
        } else if (row.tradeStatus == 0) {
          tradeStatus = "待发货";
        } else if (row.tradeStatus == 1) {
          tradeStatus = "待收货";
        } else if (row.tradeStatus == 2) {
          tradeStatus = "已成功";
        } else if (row.tradeStatus == 3) {
          tradeStatus = "已退款";
        } else {
          tradeStatus = "已关闭";
        }
        return (
          <div>
            <span>{tradeStatus}</span>
          </div>
        );
      }
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
    editId: string;
  }
  const editDrawer = reactive<EditDrawer>({
    show: false,
    editId: ""
  });
  function handleUpdate(row?: TradeDetailModel) {
    (editDrawer.show = true), (editDrawer.editId = row?.id);
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
    const data = await TradeApi.findAll({
      ...form.value
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
    onSearch,
    resetForm,
    handleUpdate,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange
  };
}
