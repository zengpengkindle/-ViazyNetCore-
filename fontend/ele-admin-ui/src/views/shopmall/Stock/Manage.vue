<template title="商品库存">
    <el-form ref="form" v-if="!loading">
        <el-card style="margin-bottom:15px">
            <div slot="header">
                <span>商品信息</span><br />
                <p>商品编号:{{item.productId}}</p>
            </div>
            <div>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="16">
                        <el-card :body-style="{ padding: '0px',display:'flex' }">
                            <img :src="item.imgUrl" width="120" height="120">
                            <div style="padding:15px">
                                <router-link class="product_title" :to="{path:'/product/manage',query:{id:item.productId}}" target="_blank">{{item.title}}</router-link>
                                <p>
                                    <span>{{item.skuText}}</span>
                                </p>
                            </div>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="8">
                        <el-card>
                            在库数：<span>{{item.inStock}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card>
                            锁定数：<span>{{item.lock}}</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="8">
                        <el-card>
                            出库数：<span>{{item.outStock}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card>
                            销量：<span>{{item.sellNum}}</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-row :gutter="12" style="margin-bottom:15px;">
                    <el-col :span="8">
                        <el-card>
                            退货数：<span>{{item.refund}}</span>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card>
                            换货数：<span>{{item.exchange}}</span>
                        </el-card>
                    </el-col>
                </el-row>
                <el-form-item>
                    <el-button @click="showUpdate(item)" type="primary" v-if="item.stockId">修改库存</el-button>
                </el-form-item>
            </div>
        </el-card>
        <el-card v-if="item.stockId" style="margin-bottom:15px">
            <div slot="header">
                <span>库存操作记录</span>
            </div>
            <template>
                <el-table :data="item.logs"
                          style="width: 100%">
                    <el-table-column prop="oldInStock"
                                     label="变更前在库数"
                                     width="180">
                    </el-table-column>
                    <el-table-column prop="newInStock"
                                     label="变更后在库数">
                    </el-table-column>
                    <el-table-column prop="amount"
                                     label="在库数变更数"
                                     width="180">
                    </el-table-column>
                    <el-table-column prop="remark"
                                     label="备注">
                    </el-table-column>
                    <el-table-column prop="createTime"
                                     label="创建时间"
                                     :formatter="$dateFormatter">
                    </el-table-column>
                </el-table>
            </template>
        </el-card>
        <el-card v-if="item.skuStockLogModel.length>0">
            <div slot="header">
                <span>商品规格</span>
            </div>
            <template>
                <el-table :data="item.skuStockLogModel"
                          border
                           highlight-current-row
                          show-summary>
                    <el-table-column prop="skuText" label="SKU" width="250"></el-table-column>
                    <el-table-column label="库存信息">
                        <el-table-column prop="inStock" label="在库数"></el-table-column>
                        <el-table-column prop="lock" label="锁定数"></el-table-column>
                        <el-table-column prop="outStock" label="出库数"></el-table-column>
                        <el-table-column prop="sellNum" label="销量"></el-table-column>
                        <el-table-column prop="refund" label="退货数"></el-table-column>
                        <el-table-column prop="exchange" label="换货数"></el-table-column>
                    </el-table-column>
                    <el-table-column label="操作" fixed="right">
                        <template slot-scope="scope">
                            <el-button type="primary" @click="showUpdate(scope)">修改库存</el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </template>
        </el-card>
        <el-dialog title="库存变更（变更数为正表示增加库存，为负减少库存）" :visible.sync="dialogStockFormVisible">
            <el-form :model="updateItem" ref="dialog" :rules="rules">
                <el-form-item label="在库数变更数" prop="amount" :label-width="formLabelWidth">
                    <el-input v-model.number="updateItem.amount" auto-complete="off" @change="changeUpdate" placeholder="请输入在库数变更数"></el-input>
                </el-form-item>
                <el-form-item label="变更前在库数" :label-width="formLabelWidth">
                    <el-input v-model.number="updateItem.inStock" auto-complete="off" :disabled="true"></el-input>
                </el-form-item>
                <el-form-item label="变更后在库数" :label-width="formLabelWidth">
                    <el-input v-model.number="updateItem.bouns" auto-complete="off" :disabled="true"></el-input>
                </el-form-item>
                <el-form-item label="备注" prop="remark" :label-width="formLabelWidth">
                    <el-input v-model="updateItem.remark" type="textarea" :rows="2" auto-complete="off" placeholder="请输入变更备注"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogStockFormVisible = false">取 消</el-button>
                <el-button type="primary" @click="submitUpdate">确认变更</el-button>
            </div>
        </el-dialog>
    </el-form>
</template>
<script>
    import { stock } from "../../apis";
    export default {
        data() {
            return {
                args: { productId: '', skuId: '' },
                item: {},
                loading: true,
                activeNames: ['1'],
                dialogStockFormVisible: false,
                updateItem: {
                    inStock: 0,
                    amount: 0,
                    bouns: 0,
                    stockId: '',
                    remark: ''
                },
                formLabelWidth: '120px',
                rules: {
                    remark: [
                        { required: true, message: '请输入备注' },
                        { min: 1, max: 200, message: '长度在 1 到 200 个字符' }
                    ],
                    amount: [
                        { required: true, message: '请输入库存变更值' },
                        { type: 'number', message: '物流花费必须为数字值' }
                    ]
                },
            }
        },
        created() {
            var vm = this;
            vm.args = { productId: vm.$route.query.id, skuId: vm.$route.query.skuId };
            vm.$nextTick(vm.load);
        },
        methods: {
            load() {
                var vm = this;
                stock.findStockLogs(vm.args).then(r => {
                    if (r.status) {
                        msg.error(r.message);
                    }
                    else {

                        vm.item = r.value;
                        vm.loading = false;
                    }
                });
            },
            showUpdate(item) {
                if (!item.stockId)
                    return;
                var vm = this;
                vm.updateItem = {
                    inStock: item.inStock,
                    amount: 0,
                    bouns: item.inStock,
                    stockId: item.stockId,
                    remark: ''
                };
                vm.dialogStockFormVisible = true;
            },
            changeUpdate() {
                var vm = this;
                vm.updateItem.bouns = vm.updateItem.inStock + vm.updateItem.amount;
            },
            submitUpdate() {
                var vm = this;
                vm.$refs.dialog.validate(function (valid) {
                    if (!valid)
                        return;
                    if (vm.updateItem.amount == 0) {
                        msg.error("库存变更值不能为0");
                        return;
                    }
                    msg.confirm("确定修改在库数？", function () {
                        stock.updateStock(vm.updateItem.stockId, vm.updateItem.amount, vm.updateItem.remark).then(r => {
                            if (r.status) {
                                msg.error(r.message);
                            }
                            else {
                                msg.success("库存变更成功");
                                vm.dialogStockFormVisible = false;
                                vm.$nextTick(vm.load);
                            }
                        })
                    });
                });
            }
        }
    }
</script>
