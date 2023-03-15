<script lang="ts" setup>
import { useTrade } from "./hook";
import { PureTableBar } from "@/components/RePureTableBar";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

import AddFill from "@iconify-icons/ri/add-circle-line";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import { FormInstance } from "element-plus";
import { ref } from "vue";

const formRef = ref<FormInstance>();
defineOptions({
  name: "trade"
});
const statusOptions = [
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
];
const payModeOptions = [
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
];

const timeOptions = [
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
];
const {
  form,
  loading,
  columns,
  dataList,
  pagination,
  onSearch,
  resetForm,
  handleUpdate,
  handleSizeChange,
  handleCurrentChange,
  handleSelectionChange
} = useTrade();
</script>
<template>
  <div class="main">
    <div>
      <el-form
        ref="formRef"
        label-width="120px"
        :inline="true"
        :model="form"
        class="bg-bg_color w-[99/100] pl-8 pt-4"
      >
        <el-form-item label="订单编号">
          <el-input placeholder="精准搜索" v-model="form.tradeId" />
        </el-form-item>
        <el-form-item label="会员账号">
          <el-input placeholder="精准搜索" v-model="form.username" />
        </el-form-item>
        <el-form-item label="会员名称">
          <el-input placeholder="模糊搜索" v-model="form.nickNameLike" />
        </el-form-item>
        <el-form-item label="店铺编号">
          <el-input placeholder="精准搜索" v-model="form.shopId" />
        </el-form-item>
        <el-form-item label="店铺名称">
          <el-input placeholder="模糊搜索" v-model="form.shopName" />
        </el-form-item>
        <el-form-item label="订单状态">
          <el-select v-model="form.tradeStatus" placeholder="全部">
            <el-option
              v-for="r in statusOptions"
              v-bind:key="r.value"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="支付方式">
          <el-select v-model="form.payMode" placeholder="全部">
            <el-option
              v-for="r in payModeOptions"
              v-bind:key="r.value"
              :label="r.label"
              :value="r.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <template #label>
            <el-select
              style="width: 180px"
              v-model="form.timeType"
              placeholder="默认状态变更时间"
            >
              <el-option
                v-for="r in timeOptions"
                v-bind:key="r.value"
                :label="r.label"
                :value="r.value"
              /> </el-select
          ></template>
          <el-date-picker
            v-model="form.createTimes"
            type="daterange"
            :editable="false"
            :clearable="false"
            :picker-options="$pickerOptions"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
          />
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            :icon="useRenderIcon(Search)"
            :loading="loading"
            @click="onSearch"
          >
            搜索
          </el-button>
          <el-button :icon="useRenderIcon(Refresh)" @click="resetForm(formRef)">
            重置
          </el-button>
        </el-form-item>
      </el-form>
    </div>
    <div>
      <PureTableBar title="商品类别管理" @refresh="onSearch">
        <template #buttons>
          <el-button
            type="primary"
            :icon="useRenderIcon(AddFill)"
            @click="handleUpdate(null)"
          >
            新增
          </el-button>
        </template>
        <template v-slot="{ size, checkList }">
          <pure-table
            align-whole="center"
            table-layout="auto"
            :loading="loading"
            :size="size"
            :data="dataList"
            :columns="columns"
            :checkList="checkList"
            :pagination="pagination"
            :paginationSmall="size === 'small' ? true : false"
            :header-cell-style="{
              background: 'var(--el-table-row-hover-bg-color)',
              color: 'var(--el-text-color-primary)'
            }"
            default-expand-all
            @selection-change="handleSelectionChange"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          >
            <el-table-column prop="id" label="订单编号" fixed width="190" />
            <el-table-column prop="title" label="申请服务" width="100" />
            <el-table-column prop="userName" label="用户账号" width="100" />
            <el-table-column prop="name" label="用户昵称" width="100" />
            <el-table-column prop="shopId" label="店铺编号" width="100" />
            <el-table-column prop="shopName" label="店铺名称" width="100" />

            <el-table-column
              prop="productMoney"
              label="商品总金额"
              width="100"
            />
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
              v-if="form.timeType == 2"
              width="160"
            />
            <el-table-column
              prop="createTime"
              label="下单时间"
              v-else-if="form.timeType == 1"
              width="160"
            />
            <el-table-column
              prop="consignTime"
              label="发货时间"
              v-else-if="form.timeType == 3"
              width="160"
            />
            <el-table-column
              prop="completeTime"
              label="完成时间"
              v-else-if="form.timeType == 4"
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
            <template #operation="{ row }">
              <router-link
                class="product_title"
                :to="{ path: '/trade/manage', query: { id: row.id } }"
                target="_blank"
              >
                查看
              </router-link>
              <el-button class="reset-margin" link type="primary" :size="size">
                添加发货
              </el-button>
            </template>
          </pure-table>
        </template>
      </PureTableBar>
    </div>
  </div>
</template>
