<template title="订单管理">
  <div>
    <header class="crum-title">
      <el-breadcrumb separator="/">
        <el-breadcrumb-item>订单管理</el-breadcrumb-item>
      </el-breadcrumb>
    </header>
    <x-table :params="params" ref="table" filter-ref="filter" :url="findAll">
      <template #form>
        <el-form-item label="订单编号">
          <el-input placeholder="精准搜索" v-model="params.tradeId" />
        </el-form-item>
        <el-form-item label="会员账号">
          <el-input placeholder="精准搜索" v-model="params.username" />
        </el-form-item>
        <el-form-item label="会员名称">
          <el-input placeholder="模糊搜索" v-model="params.nickNameLike" />
        </el-form-item>
        <el-form-item label="店铺编号">
          <el-input placeholder="精准搜索" v-model="params.shopId" />
        </el-form-item>
        <el-form-item label="店铺名称">
          <el-input placeholder="模糊搜索" v-model="params.shopName" />
        </el-form-item>
        <el-form-item label="订单状态">
          <el-select v-model="params.tradeStatus" placeholder="全部">
            <el-option
              v-for="r in statusOptions"
              v-bind:key="r.value"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="支付方式">
          <el-select v-model="params.payMode" placeholder="全部">
            <el-option
              v-for="r in payModeOptions"
              v-bind:key="r.value"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="时间">
          <el-select
            style="width: 180px"
            v-model="params.timeType"
            placeholder="默认状态变更时间"
          >
            <el-option
              v-for="r in timeOptions"
              v-bind:key="r.value"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
          <el-date-picker
            v-model="params.createTimes"
            type="daterange"
            :editable="false"
            :clearable="false"
            :picker-options="$pickerOptions"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
          />
        </el-form-item>
        <el-row>
          <el-col :span="18">
            <el-button type="primary" icon="el-icon-search" native-type="submit"
              >搜索</el-button
            >
            <!--<el-button type="primary" icon="el-icon-caret-right" @click="exportReport">导出</el-button>
                <el-button type="primary" icon="el-icon-caret-right" @click="exportLogistics">导出发货明细</el-button>-->
            <!--<el-button type="primary" icon="el-icon-caret-right" @click="census">汇总</el-button>-->
            <el-button
              type="primary"
              icon="el-icon-caret-right"
              @click="showMultiDelive = !showMultiDelive"
              >{{ showMultiDelive ? "关闭发货" : "批量发货" }}</el-button
            >
          </el-col>
        </el-row>
      </template>
      <template #other>
        <el-card v-if="showMultiDelive">
          <el-row :gutter="20">
            <el-col>
              <span style="color: red"
                >本功能用于收货信息（以正确的第一单为准）一致的订单的发货操作。即相同物流单号的订单。</span
              >
            </el-col>
          </el-row>
          <el-row :gutter="20">
            <el-col>
              <span style="color: red">*</span>
            </el-col>
          </el-row>
          <el-row :gutter="20">
            <el-col :span="12">
              <el-card>
                <span>收货人：</span>
                <a>{{ deliveItem.address.name }}</a>
              </el-card>
            </el-col>
          </el-row>
          <el-row :gutter="20">
            <el-col :span="12">
              <el-card>
                <span>收货电话：</span>
                <a>{{ deliveItem.address.tel }}</a>
              </el-card>
            </el-col>
          </el-row>
          <el-row :gutter="20">
            <el-col :span="12">
              <el-card>
                <span>省市区：</span>
                <a>{{
                  deliveItem.address.province +
                  deliveItem.address.city +
                  deliveItem.address.county
                }}</a>
              </el-card>
            </el-col>
          </el-row>
          <el-row :gutter="20">
            <el-col :span="12">
              <el-card>
                <span>详细地址：</span>
                <a>{{ deliveItem.address.addressDetail }}</a>
              </el-card>
            </el-col>
          </el-row>
          <div class="header">
            <span>批量发货</span>
          </div>

          <el-row :gutter="20">
            <el-col :span="12">
              <el-card>
                <span>订单号：</span>
                <el-select
                  v-model="deliveTrades"
                  multiple
                  filterable
                  default-first-option
                  allow-create
                  @change="deliveChange"
                  @remove-tag="deliveRemove"
                  placeholder="批量发货订单号"
                />
              </el-card>
            </el-col>
          </el-row>
          <el-row :gutter="20" v-if="alertMsg1.length > 0">
            <el-col>
              <span style="color: red">{{ alertMsg1 }}</span>
            </el-col>
          </el-row>
          <el-row :gutter="20" v-if="alertMsg2.length > 0">
            <el-col>
              <span style="color: red">{{ alertMsg2 }}</span>
            </el-col>
          </el-row>
          <el-row :gutter="20" v-if="alertMsg3.length > 0">
            <el-col>
              <span style="color: red">{{ alertMsg3 }}</span>
            </el-col>
          </el-row>
          <el-row :gutter="20" v-if="alertMsg4.length > 0">
            <el-col>
              <span style="color: red">{{ alertMsg4 }}</span>
            </el-col>
          </el-row>
          <el-row :gutter="20" v-if="alertMsg5.length > 0">
            <el-col>
              <span style="color: red">{{ alertMsg5 }}</span>
            </el-col>
          </el-row>
          <el-row :gutter="20" v-if="alertMsg6.length > 0">
            <el-col>
              <span style="color: red">{{ alertMsg6 }}</span>
            </el-col>
          </el-row>
          <el-row :gutter="20" :model="deliveItem">
            <el-col :span="12" prop="logisticsCode">
              <el-card>
                <span>物流单号：</span>
                <el-input
                  v-model="deliveItem.logisticsCode"
                  :minlength="1"
                  :maxlength="200"
                  auto-complete="off"
                  placeholder="请输入物流单号"
                />
              </el-card>
            </el-col>
          </el-row>
          <el-row :gutter="20" :model="deliveItem">
            <el-col :span="12" prop="logisticsCompany">
              <el-card>
                <span>物流公司：</span>
                <el-select
                  v-model="deliveItem.logisticsCompany"
                  placeholder="请选择物流公司"
                >
                  <el-option
                    v-for="item in wlList"
                    :key="item.id"
                    :label="item.name"
                    :value="item.name"
                  />
                </el-select>
              </el-card>
            </el-col>
          </el-row>
          <el-row :gutter="20" :model="deliveItem">
            <el-col :span="12" prop="logisticsFee">
              <el-card>
                <span>物流成本：</span>
                <el-input
                  v-model="deliveItem.logisticsFee"
                  type="number"
                  auto-complete="off"
                  placeholder="请输入物流成本"
                />
              </el-card>
            </el-col>
          </el-row>
          <el-button type="primary" @click="submitlogistics"
            >确认发货</el-button
          >
          <el-button type="primary" @click="showMultiDelive = !showMultiDelive"
            >关闭</el-button
          >
        </el-card>
      </template>
      <el-table-column type="expand" label="子订单" width="100" fixed>
        <template #default="scope">
          <el-table :data="scope.row.items" style="width: 100%">
            <el-table-column
              prop="orderId"
              label="子订单编号"
              fixed
              width="190"
            />
            <el-table-column label="商品" fixed width="450">
              <template #default="scope2">
                <el-row :gutter="20">
                  <el-col :span="8">
                    <img :src="scope2.row.imgUrl" width="100" />
                  </el-col>
                  <el-col :span="16">
                    <router-link
                      class="product_title"
                      :to="{
                        path: '/product/manage',
                        query: { id: scope2.row.pId }
                      }"
                      target="_blank"
                      >{{ scope2.row.pn }}</router-link
                    >
                    <p>
                      <span>{{ scope2.row.skuText }}</span> <br />
                      <span style="color: red; margin-left: 15px"
                        ><em>¥</em>{{ scope2.row.price }}×{{
                          scope2.row.num
                        }}</span
                      >
                    </p>
                  </el-col>
                </el-row>
              </template>
            </el-table-column>
            <el-table-column prop="num" label="购买数" width="80" />
          </el-table>
        </template>
      </el-table-column>
      <el-table-column prop="id" label="订单编号" fixed width="190" />
      <el-table-column prop="title" label="申请服务" width="100" />
      <el-table-column prop="userName" label="用户账号" width="100" />
      <el-table-column prop="name" label="用户昵称" width="100" />
      <el-table-column prop="shopId" label="店铺编号" width="100" />
      <el-table-column prop="shopName" label="店铺名称" width="100" />
      <el-table-column label="收货信息" width="120">
        <template #default="scope">
          <el-tooltip placement="bottom" effect="light">
            <template #content>
              收货人：{{ scope.row.receiverName }}
              <br />
              收货地址：{{ scope.row.address.address }}
              <br />
              收货电话：{{ scope.row.address.tel }}
              <br />
              快递单号：{{ scope.row.logisticsCode }}
              <br />
              快递公司：{{ scope.row.logisticsCompany }}
            </template>
            <el-button>收货信息</el-button>
          </el-tooltip>
        </template>
      </el-table-column>

      <el-table-column prop="productMoney" label="商品总金额" width="100" />
      <el-table-column prop="totalfeight" label="运费总金额" width="60" />
      <el-table-column prop="totalMoney" label="订单总金额" width="80" />
      <el-table-column label="用户留言" width="50">
        <template #default="scope">
          <el-tooltip
            :content="scope.row.message"
            placement="bottom"
            v-if="scope.row.message"
            effect="light"
          >
            <i class="el-icon-info" />
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column label="支付方式" width="80">
        <template #default="{ row }">
          <span v-if="row.payMode == -1">未付款</span>
          <span v-else-if="row.payMode == 0">微信支付</span>
          <span v-else-if="row.payMode == 1">支付宝</span>
          <span v-else-if="row.payMode == 2">余额</span>
        </template>
      </el-table-column>

      <el-table-column
        prop="payTime"
        label="付款时间"
        v-if="params.timeType == 2"
        width="160"
      />
      <el-table-column
        prop="createTime"
        label="下单时间"
        v-else-if="params.timeType == 1"
        width="160"
      />
      <el-table-column
        prop="consignTime"
        label="发货时间"
        v-else-if="params.timeType == 3"
        width="160"
      />
      <el-table-column
        prop="completeTime"
        label="完成时间"
        v-else-if="params.timeType == 4"
        width="160"
      />
      <el-table-column
        prop="statusChangedTime"
        label="状态变更时间"
        v-else
        width="160"
      />

      <el-table-column label="状态" width="100" fixed="right">
        <template #default="scope">
          <span v-if="scope.row.tradeStatus == -2">待提货</span>
          <span v-else-if="scope.row.tradeStatus == -1">待付款</span>
          <span v-else-if="scope.row.tradeStatus == 0">待发货</span>
          <span v-else-if="scope.row.tradeStatus == 1">待收货</span>
          <span v-else-if="scope.row.tradeStatus == 2">已成功</span>
          <span v-else-if="scope.row.tradeStatus == 3">已退款</span>
          <span v-else>已关闭</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" fixed="right">
        <template #default="scope">
          <router-link
            class="product_title"
            :to="{ path: '/trade/manage', query: { id: scope.row.id } }"
            target="_blank"
          >
            查看
          </router-link>
          <button
            style="
              border: none;
              background-color: #fff;
              color: blue;
              text-decoration-line: underline;
            "
            v-if="showMultiDelive && scope.row.tradeStatus == 0"
            @click="addDeliveTrades(scope.row)"
          >
            添加发货
          </button>
        </template>
      </el-table-column>
    </x-table>
  </div>
</template>
<script lang="ts">
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
</script>
