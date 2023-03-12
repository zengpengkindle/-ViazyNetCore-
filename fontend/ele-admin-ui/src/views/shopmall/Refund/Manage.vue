<template title="售后详情">
    <el-form ref="form" v-if="!loading">
        <el-card style="margin-bottom:15px">
            <div slot="header">
                <span>售后详情</span><br />
                <p>编号:{{item.id}}</p>
            </div>
            <div>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="6">
                        <el-card>
                            申请人：<span>{{item.memberName}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="10">
                        <el-card>
                            申请服务：<span>{{item.title}}</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="6">
                        <el-card>
                            申请金额：<span>{{item.applyAmount}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="5">
                        <el-card>
                            已退金额：<span>{{item.returnsAmount}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="5">
                        <el-card>
                            补贴运费：<span>{{item.sellerPunishFee}}</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="16">
                        <el-card>
                            售后状态： <span v-if="item.status==-1">已取消</span>
                            <span v-else-if="item.status==0">申请中</span>
                            <span v-else-if="item.status==1">等待买家回寄</span>
                            <span v-else-if="item.status==2">待处理</span>
                            <span v-else-if="item.status==3">待处理</span>
                            <span v-else-if="item.status==4">待处理</span>
                            <span v-else>已完成</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="8">
                        <el-card>
                            店铺编号：<span>{{item.shopId}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card>
                            店铺名称：<span>{{item.shopName}}</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="16">
                        <el-card>
                            <el-timeline :reverse="true">
                                <el-timeline-item v-for="log in stepLogs" :timestamp="log.createTime | formatDate" placement="top">
                                    <el-card>
                                        <h4>{{log.stepName}}</h4>
                                        <p>附加说明：{{log.message}}</p>
                                    </el-card>
                                </el-timeline-item>
                            </el-timeline>
                        </el-card>
                    </el-col>
                </el-row>
            </div>
        </el-card>
        <el-card style="margin-bottom:15px">
            <div slot="header">
                <span>退换商品详情</span>
            </div>
            <el-card v-for="(order,index) in item.orders" :key="'order'+index" shadow="hover">
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="3">
                        <img :src="order.imgUrl" width="120" height="120">
                    </el-col>
                    <el-col :span="8">
                        <span class="product_title">{{order.productName}}</span>
                        <p>
                            <span>{{order.skuText}}</span>
                        </p>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="12">
                        售价：<span>￥{{order.price}}</span><span style="color:red">&nbsp; ×&nbsp; {{order.num}}</span>
                    </el-col>
                </el-row>
            </el-card>
        </el-card>
        <el-card style="margin-bottom:15px" v-if="item.handleUserType==0&&item.status!=-1&&item.status!=5">
            <el-form ref="form" :model="params" label-width="200px" @submit.native.prevent>
                <el-form-item label="当前流程">
                    <span>{{item.stepName}}</span>
                </el-form-item>
                <el-form-item label="流程下一步">
                    <el-select v-model="params.nextStepConfigId" placeholder="请选择流程下一步" @change="showChange">
                        <el-option v-for="next in params.nextSteps" :label="next.nextStepName" :value="next.nextStepConfigId"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="附加说明" v-if="showExits">
                    <el-input type="textarea" v-model="params.message"></el-input>
                </el-form-item>
                <el-form-item v-if="showFinance">
                    <el-tag type="warning" style="width:100%">
                        提示：“已退金额”：退回给用户的商品金额；“补贴运费”：商家额外承担的用户快递费用；“快递花费”：退换货时再次产生的快递费用。
                    </el-tag>
                </el-form-item>
                <el-form-item label="已退金额" v-if="showFinance">
                    <el-input type="number" v-model="params.returnsAmount"></el-input>
                </el-form-item>
                <el-form-item label="补贴运费" v-if="showFinance">
                    <el-input type="number" v-model="params.sellerPunishFee"></el-input>
                </el-form-item>
                <el-form-item label="快递花费" v-if="showFinance">
                    <el-input type="number" v-model="params.logisticFee"></el-input>
                </el-form-item>
                <el-form-item v-if="showLogistic">
                    <el-tag type="warning" style="width:100%">
                        提示：以下为再次寄出的商品的快递信息
                    </el-tag>
                </el-form-item>
                <el-form-item label="快递单号" v-if="showLogistic">
                    <el-input v-model="params.logisticId"></el-input>
                </el-form-item>
                <el-form-item label="快递公司" v-if="showLogistic">
                    <el-input v-model="params.logisticName"></el-input>
                </el-form-item>
                <el-form-item v-if="showRefund">
                    <el-tag type="warning" style="width:100%">
                        提示：以下为用户寄回时的地址信息
                    </el-tag>
                </el-form-item>
                <el-form-item label="回寄地址" v-if="showRefund">
                    <el-input v-model="params.refundAddress"></el-input>
                </el-form-item>
                <el-form-item label="联系电话" v-if="showRefund">
                    <el-input v-model="params.refundMobile"></el-input>
                </el-form-item>
                <el-form-item label="联系人" v-if="showRefund">
                    <el-input v-model="params.refundName"></el-input>
                </el-form-item>
                <el-form-item label="邮编" v-if="showRefund">
                    <el-input v-model="params.refundPostal"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submit">提交</el-button>
                    <el-button>取消</el-button>
                </el-form-item>
            </el-form>
        </el-card>
    </el-form>
</template>
<script>
    import { refund } from "../../apis";
    export default {
        data() {
            return {
                loading: true,
                formLabelWidth: '120px',
                item: {},
                showRefund: false,
                showLogistic: false,
                showFinance: false,
                showExits: false,
                stepLogs:[],
                detail: {
                    id: '',
                    shopId: '',
                    shopName: '',
                    title: '',
                    status: 0,
                    stepName: '',
                    returnsAmount: 0,
                    applyAmount: 0,
                    sellerPunishFee: 0,
                    consigneeName: '',
                    consigneeMobile: '',
                    consigneeAddress: '',
                    consigneeZip: '',
                    returnExpressNo: '',
                    returnLogisticsName: '',
                    consigneeExpressNo: '',
                    consigneeLogisticsName: '',
                    params: {

                    },
                    handleUserType: 0,
                    orders: []
                },
                params: {
                    nextStepConfigId: '',
                    showRefund: false,
                    showLogistic: false,
                    showFinance: false
                }
            }
        },
        created() {
            var vm = this;
            refund.findRefund(vm.$route.query.uId, vm.$route.query.id).then(r => {
                if (r.status) {
                    msg.error(r.message);
                }
                else {
                    vm.item = r.value.refund.value;
                    vm.stepLogs = r.value.steps.value;
                    console.log(vm.item);
                    vm.params.nowStepId = vm.item.params.nowStepId;
                    vm.params.refundOrderParams = vm.item.params.refundOrderParams;
                    vm.params.nextSteps = vm.item.params.nextSteps;
                    if (vm.params.nextSteps&&vm.params.nextSteps.length > 0) {
                        vm.params.nextStepConfigId = vm.params.nextSteps[0].nextStepConfigId;
                        vm.showExits = true;
                        vm.showFinance = vm.params.nextSteps[0].showFinance;
                        vm.showLogistic = vm.params.nextSteps[0].showLogistic;
                        vm.showRefund = vm.params.nextSteps[0].showRefund;
                    }
                    vm.loading = false;
                }
            });
        },
        methods: {
            
            showChange(next) {
                var vm = this;
                vm.params.nextSteps.forEach((step) => {
                    if (step.nextStepConfigId == next) {
                        vm.showFinance = step.showFinance;
                        vm.showLogistic = step.showLogistic;
                        vm.showRefund = step.showRefund;
                    }
                    }
                )
               
            },
            submit() {
                var vm = this;
                refund.submit(vm.params).then(r => {
                    if (r.status) {
                        msg.error(r.message);
                    }
                    else {
                        msg.success("提交成功");

                    }
                })
            }
        }
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