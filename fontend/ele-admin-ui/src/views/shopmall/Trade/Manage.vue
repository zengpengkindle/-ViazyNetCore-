<template title="订单详情">
  <div class="main">
    <div v-if="!loading">
      <el-card style="margin-bottom: 15px">
        <template #header>
          <span>订单详情</span>
        </template>
        <div>
          <el-descriptions class="margin-top" :column="3" border>
            <el-descriptions-item label="订单号" :span="3">
              {{ item.id }}
            </el-descriptions-item>
            <el-descriptions-item label="购买人" :span="2">
              {{ item.name }}
            </el-descriptions-item>
            <el-descriptions-item label="订单状态">
              <span v-if="item.tradeStatus == -2">待提货</span>
              <span v-else-if="item.tradeStatus == -1" style="color: red"
                >未付款</span
              >
              <span v-else-if="item.tradeStatus == 0">待发货</span>
              <span v-else-if="item.tradeStatus == 1">待收货</span>
              <span v-else-if="item.tradeStatus == 2">已成功</span>
              <span v-else-if="item.tradeStatus == 4">已关闭</span>
            </el-descriptions-item>
            <el-descriptions-item label="支付方式" :span="3">
              <span v-if="item.payMode == -1">未付款</span>
              <span v-else-if="item.payMode == 0">微信</span>
              <span v-else-if="item.payMode == 1">支付宝</span>
              <span v-else-if="item.payMode == 2">余额</span>
              <span v-else-if="item.payMode == 3">银联</span>
            </el-descriptions-item>
            <el-descriptions-item label="商品总价">
              {{ item.productMoney }}
            </el-descriptions-item>
            <el-descriptions-item label="运费总价">
              {{ item.totalFreight }}
            </el-descriptions-item>
            <el-descriptions-item label="订单总价" min-width="33%">
              <span style="color: red">{{ item.totalMoney }}</span>
            </el-descriptions-item>
            <el-descriptions-item label="店铺编号">
              {{ item.shopId }}
            </el-descriptions-item>
            <el-descriptions-item label="店铺名称" :span="2">
              {{ item.shopName }}
            </el-descriptions-item>
            <el-descriptions-item label="购买人留言" :span="3">
              {{ item.message }}
            </el-descriptions-item>
            <el-descriptions-item label="收货人姓名">
              {{ item.receiverName }}
            </el-descriptions-item>
            <el-descriptions-item label="收货电话" :span="2">
              {{ item.receiverMobile }}
            </el-descriptions-item>
            <el-descriptions-item label="收货地址" :span="3">
              {{ item.address.address }}
            </el-descriptions-item>

            <el-descriptions-item label="快递单号">
              {{ item.logisticsCode }}
            </el-descriptions-item>
            <el-descriptions-item label="快递公司">
              {{ item.logisticsCompany }}
            </el-descriptions-item>
            <el-descriptions-item label="运费">
              {{ item.logisticsFee }}
            </el-descriptions-item>
          </el-descriptions>
        </div>
      </el-card>
      <el-card class="mt-4">
        <div>
          <el-timeline :reverse="true">
            <el-timeline-item
              v-for="(activity, index) in activities"
              :key="index"
              :timestamp="activity.timestamp"
            >
              {{ activity.content }}
            </el-timeline-item>
          </el-timeline>
        </div>
      </el-card>
      <el-card class="mt-4">
        <el-table :data="item.items" :gutter="12" style="margin-bottom: 15px">
          <el-table-column :span="3" label="子订单编号">
            <template #default="{ row }">
              <el-image
                style="width: 60px; height: 60px"
                :src="row.imgUrl"
                :zoom-rate="1.2"
                preview-teleported
                :preview-src-list="[row.imgUrl]"
                fit="cover"
              />
            </template>
          </el-table-column>
          <el-table-column property="pn" label="名称" width="120" />
          <el-table-column property="skuText" label="规格" width="120" />
          <el-table-column property="price" label="售价" width="120" />
          <el-table-column property="num" label="数量" width="120" />
        </el-table>
      </el-card>
    </div>
    <el-card class="mt-4">
      <el-form ref="form" v-if="!loading">
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
      </el-form>
    </el-card>

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
        <el-button @click="dialogLogisticeFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="submitlogistics">确认发货</el-button>
      </template>
    </el-dialog>
    <el-dialog title="修改物流信息" v-model="editLogisticeCodeFormVisible">
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
        <el-button @click="editLogisticeCodeFormVisible = false">
          取 消
        </el-button>
        <el-button type="primary" @click="changeLogistics">确认修改</el-button>
      </template>
    </el-dialog>
    <el-dialog title="修改订单收货信息" v-model="editTradeAddressFormVisible">
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
  </div>
</template>
<script lang="ts" setup>
import TradeApi, {
  DeliveryModel,
  SimpleLogisticsCompany,
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
const logisticsCompanys: Ref<Array<SimpleLogisticsCompany>> = ref([]);
const rules = reactive<FormRules>({
  logisticsCode: [
    { required: true, message: "请输入物流单号" },
    { min: 1, max: 200, message: "长度在 1 到 200 个字符" }
  ],
  logisticsCompany: [{ min: 1, max: 20, message: "长度在 1 到 20 个字符" }],
  logisticsFee: [{ type: "number", message: "物流花费必须为数字值" }]
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
  logisticsCompanys.value = await TradeApi.findWlList();
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
      TradeApi.modifyTradeAddress({
        receiverName: item.value.receiverName,
        receiverMobile: item.value.receiverMobile,
        receiverProvince: item.value.receiverProvince,
        receiverCity: item.value.receiverCity,
        receiverDistrict: item.value.receiverDistrict,
        receiverDetail: item.value.receiverDetail,
        id: item.value.id
      }).then(() => {
        message("修改成功", { type: "success" });
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
      message("修改失败", {
        type: "error"
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
