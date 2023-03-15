<template title="订单详情">
  <el-form ref="form" v-if="!loading">
    <el-card style="margin-bottom: 15px">
      <template #header>
        <span>订单详情</span><br />
        <p>订单号:{{ item.id }}</p>
      </template>
      <div>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="16">
            <el-card>
              购买人：<span>{{ item.name }}</span>
            </el-card>
          </el-col>
        </el-row>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="16">
            <el-card>
              订单状态：<span v-if="item.tradeStatus == -2">待提货</span>
              <span v-else-if="item.tradeStatus == -1" style="color: red"
                >未付款</span
              >
              <span v-else-if="item.tradeStatus == 0">待发货</span>
              <span v-else-if="item.tradeStatus == 1">待收货</span>
              <span v-else-if="item.tradeStatus == 2">已成功</span>
              <span v-else-if="item.tradeStatus == 4">已关闭</span>
            </el-card>
          </el-col>
        </el-row>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="16">
            <el-card>
              <el-row style="margin-bottom: 15px">
                <el-col :span="12">
                  支付方式：<span v-if="item.payMode == -1">未付款</span>
                  <span v-else-if="item.payMode == 0">微信</span>
                  <span v-else-if="item.payMode == 1">支付宝</span>
                  <span v-else-if="item.payMode == 2">余额</span>
                  <span v-else-if="item.payMode == 3">银联</span>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="24">
                  <el-row :gutter="12">
                    <el-col :span="8">
                      商品总价：<span>{{ item.productMoney }}</span>
                    </el-col>

                    <el-col :span="8">
                      运费总价：<span>{{ item.totalFreight || 0 }}</span>
                    </el-col>
                    <el-col :span="8">
                      订单总价：<span style="color: red">{{
                        item.totalMoney
                      }}</span>
                    </el-col>
                  </el-row>
                </el-col>
              </el-row>
            </el-card>
          </el-col>
        </el-row>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="8">
            <el-card>
              店铺编号：<span>{{ item.shopId }}</span>
            </el-card>
          </el-col>
          <el-col :span="8">
            <el-card>
              店铺名称：<span>{{ item.shopName }}</span>
            </el-card>
          </el-col>
        </el-row>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="16">
            <el-card>
              购买人留言：<span>{{ item.message }}</span>
            </el-card>
          </el-col>
        </el-row>
        <el-row
          :gutter="12"
          style="margin-bottom: 15px"
          v-if="item.tradeStatus >= 0 && item.tradeStatus != 4"
        >
          <el-col :span="16">
            <el-card>
              收货人姓名：<span>{{ item.receiverName }}</span>
              <br />
              收货电话：<span>{{ item.receiverMobile }}</span>
              <br />
              收货地址：<span>{{ item.address.address }}</span>
            </el-card>
          </el-col>
        </el-row>
        <el-row
          :gutter="12"
          style="margin-bottom: 15px"
          v-if="item.tradeStatus >= 1 && item.tradeStatus != 4"
        >
          <el-col :span="16">
            <el-card>
              快递单号：<span>{{ item.logisticsCode }}</span
              ><br />
              快递公司：<span>{{ item.logisticsCompany }}</span
              ><br />

              物流花费：<span>￥：{{ item.logisticsFee }}</span>
            </el-card>
          </el-col>
        </el-row>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="16">
            <el-card>
              <el-timeline :reverse="true">
                <el-timeline-item
                  v-for="(activity, index) in activities"
                  :key="index"
                  :timestamp="activity.timestamp"
                >
                  {{ activity.content }}
                </el-timeline-item>
              </el-timeline>
            </el-card>
          </el-col>
        </el-row>
      </div>
    </el-card>
    <el-card style="margin-bottom: 15px">
      <template #header>
        <span>商品详情</span>
      </template>
      <el-card v-for="order in item.items" :key="order.orderId" shadow="hover">
        <template #header>
          子订单编号：<span>{{ order.orderId }}</span>
        </template>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="3">
            <img :src="order.imgUrl" width="120" height="120" />
          </el-col>
          <el-col :span="8">
            <router-link
              class="product_title"
              :to="{ path: '/product/manage', query: { id: order.pId } }"
              target="_blank"
              >{{ order.pn }}</router-link
            >
            <p>
              <span>{{ order.skuText }}</span>
            </p>
          </el-col>
        </el-row>
        <el-row :gutter="12" style="margin-bottom: 15px">
          <el-col :span="12">
            售价：<span>￥{{ order.price }}</span
            ><span style="color: red">&nbsp; ×&nbsp; {{ order.num }}</span>
          </el-col>
        </el-row>
      </el-card>
    </el-card>
    <el-card style="margin-bottom: 15px">
      <el-form-item>
        <el-button
          @click="dialogLogisticeFormVisible = true"
          v-if="item.tradeStatus == 0"
          >发货</el-button
        >
        <el-button
          @click="editTradeAddressFormVisible = true"
          v-if="item.tradeStatus == 0 || item.tradeStatus == 1"
          >修改订单收货信息</el-button
        >
        <el-button
          @click="editLogisticeCodeFormVisible = true"
          v-if="item.tradeStatus == 1"
          >修改物流信息</el-button
        >
      </el-form-item>

      <el-dialog title="商品发货" :visible="dialogLogisticeFormVisible">
        <el-form :model="item" ref="dialog" :rules="rules">
          <el-form-item
            label="物流单号"
            prop="logisticsCode"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.logisticsCode"
              auto-complete="off"
              placeholder="请输入物流单号"
            />
          </el-form-item>
          <el-form-item
            label="物流花费"
            prop="logisticsCompany"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model.number="item.logisticsCompany"
              auto-complete="off"
              placeholder="请输入物流花费"
            />
          </el-form-item>
        </el-form>
        <template #footer>
          <el-button @click="dialogLogisticeFormVisible = false"
            >取 消</el-button
          >
          <el-button type="primary" @click="submitlogistics"
            >确认发货</el-button
          >
        </template>
      </el-dialog>
      <el-dialog title="修改物流信息" :visible="editLogisticeCodeFormVisible">
        <el-form :model="item" ref="dialog" :rules="rules">
          <el-form-item
            label="物流单号"
            prop="logisticsCode"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.logisticsCode"
              auto-complete="off"
              placeholder="请输入物流单号"
            />
          </el-form-item>
          <el-form-item
            label="物流公司"
            prop="logisticsCompany"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.logisticsCompany"
              auto-complete="off"
              placeholder="请输入物流公司"
            />
          </el-form-item>
          <el-form-item
            label="物流花费"
            prop="logisticsFee"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model.number="item.logisticsFee"
              type="number"
              auto-complete="off"
              placeholder="请输入物流花费"
            />
          </el-form-item>
        </el-form>
        <template #footer>
          <el-button @click="editLogisticeCodeFormVisible = false"
            >取 消</el-button
          >
          <el-button type="primary" @click="changeLogistics"
            >确认修改</el-button
          >
        </template>
      </el-dialog>
      <el-dialog
        title="修改订单收货信息"
        :visible="editTradeAddressFormVisible"
      >
        <el-form :model="item" ref="dialog" :rules="rules2">
          <el-form-item
            label="收货人"
            prop="receiverName"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.receiverName"
              auto-complete="off"
              placeholder="请输入收货人名字"
            />
          </el-form-item>
          <el-form-item
            label="收货人电话"
            prop="receiverMobile"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.receiverMobile"
              auto-complete="off"
              placeholder="请输入收货人电话"
            />
          </el-form-item>
          <el-form-item
            label="收货地址-省"
            prop="receiverProvince"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.receiverProvince"
              auto-complete="off"
              placeholder="请输入收货地址-省"
            />
          </el-form-item>
          <el-form-item
            label="收货地址-市"
            prop="receiverCity"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.receiverCity"
              auto-complete="off"
              placeholder="请输入收货地址-市"
            />
          </el-form-item>
          <el-form-item
            label="收货地址-区"
            prop="receiverDistrict"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.receiverDistrict"
              auto-complete="off"
              placeholder="请输入收货地址-区"
            />
          </el-form-item>
          <el-form-item
            label="收货地址-详细地址"
            prop="receiverDetail"
            :label-width="formLabelWidth"
          >
            <el-input
              v-model="item.receiverDetail"
              auto-complete="off"
              placeholder="请输入收货地址-详细地址"
            />
          </el-form-item>
        </el-form>
        <template #footer>
          <el-button @click="editTradeAddressFormVisible = false"
            >取 消</el-button
          >
          <el-button type="primary" @click="changeAddress">确认修改</el-button>
        </template>
      </el-dialog>
    </el-card>
  </el-form>
</template>
<script lang="ts">
import { trade } from "../../apis";
export default {
  data() {
    return {
      loading: true,
      formLabelWidth: "120px",
      item: {},
      activeNames: ["1"],
      dialogLogisticeFormVisible: false,
      editLogisticeCodeFormVisible: false,
      editTradeAddressFormVisible: false,
      rules: {
        logisticsCode: [
          { required: true, message: "请输入物流单号" },
          { min: 1, max: 200, message: "长度在 1 到 200 个字符" }
        ],
        logisticsCompany: [
          { required: true, message: "请输入物流公司" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
        ],
        logisticsFee: [
          { required: true, message: "请输入物流花费" },
          { type: "number", message: "物流花费必须为数字值" }
        ]
      },
      rules2: {
        receiverName: [
          { required: true, message: "请输入收货人姓名" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
        ],
        receiverMobile: [
          { required: true, message: "请输入收货人电话" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
        ],
        receiverProvince: [
          { required: true, message: "请输入收货地址-省" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
        ],
        receiverCity: [
          { required: true, message: "请输入收货地址-市" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
        ],
        receiverDistrict: [
          { required: true, message: "请输入收货地址-区" },
          { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
        ],
        receiverDetail: [
          { required: true, message: "请输入收货地址-详细地址" },
          { min: 1, max: 100, message: "长度在 1 到 100 个字符" }
        ]
      },
      activities: []
    };
  },
  created() {
    trade.findTrade(this.$route.query.id).then(r => {
      if (r.status) {
        msg.error(r.message);
      } else {
        this.item = r.value;
        this.activities.push({
          content: "下单时间",
          timestamp: this.item.createTime
        });
        if (this.item.payTime) {
          this.activities.push({
            content: "支付时间",
            timestamp: this.item.payTime
          });
        }
        if (this.item.consignTime) {
          this.activities.push({
            content: "发货时间",
            timestamp: this.item.consignTime
          });
        }
        if (this.item.completeTime) {
          this.activities.push({
            content: "完成时间",
            timestamp: this.item.completeTime
          });
        }
        if (this.item.statusChangedTime) {
          this.activities.push({
            content: "状态变更时间",
            timestamp: this.item.statusChangedTime
          });
        }
        this.loading = false;
      }
    });
  },
  methods: {
    submitlogistics() {
      this.$confirm("此操作将变更订单状态为待收货, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          const deliveTrades = [this.item.id];
          const deliveItem = {
            logisticsId: this.item.logisticsId,
            logisticsFee: this.item.logisticsFee,
            logisticsCode: this.item.logisticsCode,
            logisticsCompany: this.item.logisticsCompany
          };
          trade.deliverTrades(deliveTrades, deliveItem).then(r => {
            if (r.value.Fail > 0) {
              r.value.failIds.forEach(function (value) {
                msg.error(value);
                setTimeout(() => {}, 1000);
              });
            } else {
              msg.success("发货成功");
            }
            this.dialogLogisticeFormVisible = false;
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
    },
    changeLogistics() {
      this.$confirm("是否修改订单的物流信息?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          const deliveItem = {
            logisticsId: this.item.logisticsId,
            logisticsFee: this.item.logisticsFee,
            logisticsCode: this.item.logisticsCode,
            logisticsCompany: this.item.logisticsCompany
          };
          trade.changeTradeDeliver(this.item.id, deliveItem).then(r => {
            if (r.status) {
              msg.error(r.message);
            } else {
              msg.success("修改成功");
            }
            this.editLogisticeCodeFormVisible = false;
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
    },
    changeAddress() {
      this.$confirm("是否修改订单的收货信息?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          //string name, string mobile, string province, string city, string district, string detail
          trade
            .chageTradeAddress(
              this.item.id,
              this.item.receiverName,
              this.item.receiverMobile,
              this.item.receiverProvince,
              this.item.receiverCity,
              this.item.receiverDistrict,
              this.item.receiverDetail
            )
            .then(r => {
              if (r.status) {
                msg.error(r.message);
              } else {
                msg.success("修改成功");
                item.address.address =
                  this.item.receiverProvince +
                  this.item.receiverCity +
                  this.item.receiverDistrict +
                  this.item.receiverDetail;
              }
              this.editTradeAddressFormVisible = false;
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
};
</script>
<style>
.el-tag + .el-tag {
  margin-left: 10px;
}

.button-new-tag {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
}

.input-new-tag {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>
