import { defineComponent } from "vue";
import TradeApi, { BatchDeliveryModel } from "@/api/shopmall/trade";
export default defineComponent({
  data() {
    const now = new Date();
    const date = new Date(now.getTime() - 7 * 24 * 3600 * 1000);
    return {
      findAll: TradeApi.findAll,
      params: { createTimes: [date, now] },
      isCensusOk: false,
      showMultiDelive: false,
      dialogFormVisible: false,
      selectloading: false,
      deliveItem: {
        logisticsId: "",
        logisticsFee: "",
        logisticsCode: "",
        logisticsCompany: "",
        address: {
          id: "",
          name: "",
          tel: "",
          province: "",
          city: "",
          county: "",
          addressDetail: "",
          postalCode: "",
          areaCode: "",
          address: "",
          isDefault: false
        }
      },
      deliveTrades: [],
      wlList: [],
      alertMsg1: "",
      alertMsg2: "",
      alertMsg3: "",
      alertMsg4: "",
      alertMsg5: "",
      alertMsg6: "",

      statusOptions: [
        {
          value: "",
          label: "全部"
        },
        {
          value: -2,
          label: "待提货"
        },
        {
          value: -1,
          label: "未付款"
        },
        {
          value: 0,
          label: "待发货"
        },
        {
          value: 1,
          label: "待收货"
        },
        {
          value: 2,
          label: "已成功"
        },
        {
          value: 4,
          label: "已关闭"
        }
      ],
      timeOptions: [
        {
          value: 1,
          label: "下单时间"
        },
        {
          value: 2,
          label: "付款时间"
        },
        {
          value: 3,
          label: "发货时间"
        },
        {
          value: 4,
          label: "完成时间"
        },
        {
          value: 5,
          label: "状态变更时间"
        }
      ],
      payModeOptions: [
        {
          value: "",
          label: "全部"
        },
        {
          value: -1,
          label: "未付款"
        },
        {
          value: 0,
          label: "微信支付"
        },
        {
          value: 1,
          label: "支付宝"
        },
        {
          value: 2,
          label: "余额"
        },
        {
          value: 3,
          label: "银联支付"
        }
      ]
    };
  },
  keepAlive: true,
  created() {
    TradeApi.findWlList().then(r => {
      this.wlList = r;
    });
  },
  methods: {
    deliveChange() {
      if (this.deliveTrades.length == 1) {
        TradeApi.findTrade(this.deliveTrades[0]).then(r => {
          if (r.result == 0) {
            this.deliveTrades = [];
            this.$alert(r.errmsg, { type: "error" });
          } else {
            this.deliveItem.address.city = r.receiverCity;
            this.deliveItem.address.addressDetail = r.receiverDetail;
            this.deliveItem.address.county = r.receiverDistrict;
            this.deliveItem.address.tel = r.receiverMobile;
            this.deliveItem.address.name = r.receiverName;
            this.deliveItem.address.province = r.receiverProvince;
          }
        });
      }
    },
    deliveRemove() {
      if (this.deliveTrades.length == 0) {
        this.alertMsg1 = "";
        this.alertMsg2 = "";
        this.alertMsg3 = "";
        this.alertMsg4 = "";
        this.alertMsg5 = "";
        this.alertMsg6 = "";

        this.deliveItem.address.city = "";
        this.deliveItem.address.addressDetail = "";
        this.deliveItem.address.county = "";
        this.deliveItem.address.tel = "";
        this.deliveItem.address.name = "";
        this.deliveItem.address.province = "";
      }
    },
    addDeliveTrades(item) {
      if (this.deliveTrades.length > 0) {
        let repeat = false;
        for (let i = 0; i < this.deliveTrades.length; i++) {
          if (this.deliveTrades[i] == item.id) {
            this.alertMsg1 = "重复订单";
            repeat = true;
            break;
          }
        }
        this.alertMsg1 = "";
        this.alertMsg2 = "";
        this.alertMsg3 = "";
        this.alertMsg4 = "";
        this.alertMsg5 = "";
        this.alertMsg6 = "";

        if (repeat) return;
        let temp = 0;
        if (
          this.deliveItem.address.name != "" &&
          this.deliveItem.address.name != item.receiverName
        ) {
          this.alertMsg1 =
            "新加入订单的‘收货人’为:’" +
            item.receiverName +
            "‘。与‘" +
            this.deliveItem.address.name +
            "’不一致。" +
            "\r\n";
        }
        if (
          this.deliveItem.address.tel != "" &&
          this.deliveItem.address.tel != item.receiverMobile
        ) {
          this.alertMsg2 =
            "新加入订单的‘联系电话’为:‘" +
            item.receiverMobile +
            "’。与‘" +
            this.deliveItem.address.tel +
            "’不一致。" +
            "\r\n";
        }
        if (
          this.deliveItem.address.province != "" &&
          this.deliveItem.address.province != item.receiverProvince
        ) {
          this.alertMsg3 =
            "新加入订单的‘省’为:‘" +
            item.receiverProvince +
            "‘。与‘" +
            this.deliveItem.address.province +
            "’不一致。" +
            "\r\n";
        }
        if (
          this.deliveItem.address.city != "" &&
          this.deliveItem.address.city != item.receiverCity
        ) {
          this.alertMsg4 =
            "新加入订单的‘市’为:’" +
            item.receiverCity +
            "’。与‘" +
            this.deliveItem.address.city +
            "’不一致。" +
            "\r\n";
        }
        if (
          this.deliveItem.address.county != "" &&
          this.deliveItem.address.county != item.receiverDistrict
        ) {
          this.alertMsg5 =
            "新加入订单的‘区’为:‘" +
            item.receiverDistrict +
            "‘。与‘" +
            this.deliveItem.address.county +
            "’不一致。" +
            "\r\n";
        }
        if (
          this.deliveItem.address.addressDetail != "" &&
          this.deliveItem.address.addressDetail != item.receiverDetail
        ) {
          this.alertMsg6 =
            "新加入订单的‘详细地址’为:’" +
            item.receiverDetail +
            "’。与‘" +
            this.deliveItem.address.addressDetail +
            "’不一致。" +
            "\r\n";
        }
        temp =
          this.alertMsg1.length +
          this.alertMsg2.length +
          this.alertMsg3.length +
          this.alertMsg4.length +
          this.alertMsg5.length +
          this.alertMsg6.length;
        if (temp <= 0) {
          this.deliveTrades.push(item.id);
        }
      } else {
        this.deliveItem.address.city = item.receiverCity;
        this.deliveItem.address.addressDetail = item.receiverDetail;
        this.deliveItem.address.county = item.receiverDistrict;
        this.deliveItem.address.tel = item.receiverMobile;
        this.deliveItem.address.name = item.receiverName;
        this.deliveItem.address.province = item.receiverProvince;
        this.deliveTrades.push(item.id);
      }
    },
    submitlogistics() {
      if (this.deliveTrades.length == 0) {
        return;
      }
      const valid =
        this.deliveItem.logisticsCode.length > 0 &&
        this.deliveItem.logisticsCode.length <= 200 &&
        this.deliveItem.logisticsCompany.length > 0 &&
        this.deliveItem.logisticsCompany.length <= 20;
      if (!valid) {
        this.$alert("物流单号和物流公司请填写完整", { type: "error" });
        return;
      }
      if (this.deliveTrades.length == 0) {
        return;
      }

      this.$confirm("此操作将变更订单状态为待收货, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          const trade: BatchDeliveryModel = {
            tradeIds: this.deliveTrades,
            delivery: this.deliveItem
          };
          TradeApi.deliverTrades(trade).then(r => {
            if (r.value.Fail > 0) {
              r.value.failIds.forEach(function (value) {
                msg.error(value);
                setTimeout(() => {}, 1000);
              });
            } else {
              msg.success("发货成功");
            }

            this.showMultiDelive = false;
            this.deliveItem.address.city = "";
            this.deliveItem.address.addressDetail = "";
            this.deliveItem.address.county = "";
            this.deliveItem.address.tel = "";
            this.deliveItem.address.name = "";
            this.deliveItem.address.province = "";
            this.deliveTrades = [];
            this.$refs.table.refresh();
          });
        })
        .catch(e => {
          console.log(e.message);
          this.dialogFormVisible = false;
          this.$message({
            type: "info",
            message: "已取消通过"
          });
        });
    }
  }
});