<template title="订单详情">
  <el-form ref="form" v-if="!loading">
    <el-card style="margin-bottom: 15px">
      <template #header>
        <span>订单详情</span><br />
        <p>订单号:{{ item.id }}</p>
      </template>
      <div>
        <el-descriptions
          class="margin-top"
          title="With border"
          :column="3"
          border
        >
        </el-descriptions>
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
<script lang="ts" setup>
import TradeApi, {
  DeliveryModel,
  TradeDetailModel
} from "@/api/shopmall/trade";
import { message } from "@/utils/message";
import { ElMessageBox, FormRules } from "element-plus";
import { onMounted, ref, Ref, reactive } from "vue";
import { useRoute } from "vue-router";

const loading = ref(true);
const formLabelWidth = "120px";
const item: Ref<TradeDetailModel> = ref();
const dialogLogisticeFormVisible = ref(false);
const editLogisticeCodeFormVisible = ref(false);
const dialogFormVisible = ref(false);
const editTradeAddressFormVisible = ref(false);
const rules = reactive<FormRules>({
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
});
const rules2 = reactive<FormRules>({
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
});
const activities = reactive([]);
const route = useRoute();
onMounted(async () => {
  const trade = await TradeApi.findTrade(route.query.id as string);
  activities.push({
    content: "下单时间",
    timestamp: trade.createTime
  });
  if (trade.payTime) {
    activities.push({
      content: "支付时间",
      timestamp: trade.payTime
    });
  }
  if (trade.consignTime) {
    activities.push({
      content: "发货时间",
      timestamp: trade.consignTime
    });
  }
  if (trade.completeTime) {
    activities.push({
      content: "完成时间",
      timestamp: trade.completeTime
    });
  }
  if (trade.statusChangedTime) {
    activities.push({
      content: "状态变更时间",
      timestamp: trade.statusChangedTime
    });
  }
  item.value = trade;
  loading.value = false;
});
function submitlogistics() {
  ElMessageBox({
    message: "此操作将变更订单状态为待收货, 是否继续?",
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning"
  })
    .then(() => {
      const deliveTrades = [item.value.id];
      const deliveItem: DeliveryModel = {
        logisticsId: item.value.logisticsId,
        logisticsFee: item.value.logisticsFee,
        logisticsCode: item.value.logisticsCode,
        logisticsCompany: item.value.logisticsCompany,
        address: null
      };
      TradeApi.deliverTrades({
        tradeIds: deliveTrades,
        delivery: deliveItem
      }).then(r => {
        if (r.fail > 0) {
          r.failIds.forEach(function (value) {
            msg.error(value);
            setTimeout(() => {}, 1000);
          });
        } else {
          msg.success("发货成功");
        }
        dialogLogisticeFormVisible.value = false;
      });
    })
    .catch(e => {
      console.log(e.message);
      dialogFormVisible.value = false;
      message("已取消通过");
    });
}
function changeLogistics() {
  ElMessageBox({
    message: "是否修改订单的物流信息?",
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning"
  })
    .then(() => {
      const deliveItem: DeliveryModel = {
        logisticsId: item.value.logisticsId,
        logisticsFee: item.value.logisticsFee,
        logisticsCode: item.value.logisticsCode,
        logisticsCompany: item.value.logisticsCompany,
        address: null
      };
      TradeApi.changeTradeDeliver(item.value.id, deliveItem).then(_ => {
        message("修改成功", { type: "success" });
        editLogisticeCodeFormVisible.value = false;
      });
    })
    .catch(e => {
      console.log(e.message);
      dialogFormVisible.value = false;
      message("已取消通过", {
        type: "info"
      });
    });
}
function changeAddress() {
  ElMessageBox({
    message: "是否修改订单的收货信息?",
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning"
  })
    .then(() => {
      //string name, string mobile, string province, string city, string district, string detail
      TradeApi.changeTradeAddressModel({
        receiverName: item.value.receiverName,
        receiverMobile: item.value.receiverMobile,
        receiverProvince: item.value.receiverProvince,
        receiverCity: item.value.receiverCity,
        receiverDistrict: item.value.receiverDistrict,
        receiverDetail: item.value.receiverDetail,
        id: item.value.id
      }).then(() => {
        message("修改成功");
        item.value.address.address =
          item.value.receiverProvince +
          item.value.receiverCity +
          item.value.receiverDistrict +
          item.value.receiverDetail;
        editTradeAddressFormVisible.value = false;
      });
    })
    .catch(e => {
      console.log(e.message);
      dialogFormVisible.value = false;
      message("已取消通过", {
        type: "info"
      });
    });
}
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
