<template title="售后管理">
    <div>
        <header class="crum-title">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item>售后管理</el-breadcrumb-item>
            </el-breadcrumb>
        </header>
        <x-table :params="params" ref="table" filter-ref="filter" :url="findAll">
            <template slot="form">
                <el-form-item label="售后编号">
                    <el-input placeholder="精准搜索" v-model="params.id"></el-input>
                </el-form-item>
                <el-form-item label="会员账号">
                    <el-input placeholder="精准搜索" v-model="params.username"></el-input>
                </el-form-item>
                <el-form-item label="会员名称">
                    <el-input placeholder="模糊搜索" v-model="params.nickNameLike"></el-input>
                </el-form-item>
                <el-form-item label="店铺编号">
                    <el-input placeholder="精准搜索" v-model="params.shopId"></el-input>
                </el-form-item>
                <el-form-item label="店铺名称">
                    <el-input placeholder="模糊搜索" v-model="params.shopName"></el-input>
                </el-form-item>
                <el-form-item label="状态">
                    <el-select v-model="params.status" placeholder="全部">
                        <el-option v-for="r in statusOptions"
                                   :key="r.value"
                                   :label="r.label"
                                   :value="r.value">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="当前处理人">
                    <el-select v-model="params.handleUserType" placeholder="全部">
                        <el-option v-for="r in handleUserTypeOptions"
                                   :key="r.value"
                                   :label="r.label"
                                   :value="r.value">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="创建时间">
                    <el-select style="width: 180px;" v-model="params.timeType" placeholder="默认创建时间">
                        <el-option v-for="r in timeOptions"
                                   :key="r.value"
                                   :label="r.label"
                                   :value="r.value">
                        </el-option>
                    </el-select>
                    <el-date-picker v-model="params.createTimes"
                                    type="daterange"
                                    :editable="false"
                                    :clearable="false"
                                    :picker-options="$pickerOptions"
                                    range-separator="至"
                                    start-placeholder="开始日期"
                                    end-placeholder="结束日期"
                                    align="right">
                    </el-date-picker>
                </el-form-item>
                <el-row>
                    <el-col :span="18">
                        <el-button type="primary" icon="el-icon-search" native-type="submit">搜索</el-button>
                    </el-col>
                </el-row>
            </template>
            <el-table-column type="expand" label="退换商品" width="100" fixed>
                <template slot-scope="scope">
                    <el-table :data="scope.row.orders" style="width: 100%">
                        <el-table-column prop="orderId" label="子订单编号" fixed width="190"></el-table-column>
                        <el-table-column label="商品" fixed width="450">
                            <template slot-scope="scope2">
                                <el-row :gutter="20">
                                    <el-col :span="8">
                                        <img :src="scope2.row.imgUrl" width="100">
                                    </el-col>
                                    <el-col :span="16">
                                        <span class="product_title">{{scope2.row.productName}}</span>
                                        <p>
                                            <span>{{scope2.row.skuText}}</span> <br />
                                            <span style="color:red;margin-left:15px;"><em>¥</em>{{scope2.row.price}}×{{scope2.row.num}}</span>
                                        </p>
                                    </el-col>
                                </el-row>
                            </template>
                        </el-table-column>
                        <el-table-column prop="num" label="退换数" width="80"></el-table-column>
                    </el-table>
                </template>
            </el-table-column>
            <el-table-column prop="id" label="售后编号" fixed width="190"></el-table-column>

            <el-table-column prop="memberName" label="用户名称" width="100"></el-table-column>
            <el-table-column prop="shopId" label="店铺编号" width="100"></el-table-column>
            <el-table-column prop="shopName" label="店铺名称" width="100"></el-table-column>
            <el-table-column prop="stepName" label="当前流程" width="100"></el-table-column>

            <el-table-column prop="applyAmount" label="申请金额" width="100"></el-table-column>
            <el-table-column prop="returnsAmount" label="已退金额" width="60"></el-table-column>
            <el-table-column prop="sellerPunishFee" label="补贴运费" width="80"></el-table-column>

            <el-table-column prop="updateTime" label="变更时间" v-if="params.timeType==1" :formatter="$dateFormatter" width="160"></el-table-column>
            <el-table-column prop="createTime" label="创建时间" v-else :formatter="$dateFormatter" width="160"></el-table-column>
            <el-table-column label="状态" width="100" fixed="right">
                <template slot-scope="scope">
                    <span v-if="scope.row.status==-1">已取消</span>
                    <span v-else-if="scope.row.status==0">申请中</span>
                    <span v-else-if="scope.row.status==1">等待买家回寄</span>
                    <span v-else-if="scope.row.status==2">待处理</span>
                    <span v-else-if="scope.row.status==3">待处理</span>
                    <span v-else-if="scope.row.status==4">待处理</span>
                    <span v-else>已完成</span>
                </template>
            </el-table-column>
            <el-table-column label="操作" fixed="right">
                <template slot-scope="scope">
                    <router-link class="product_title" :to="{path:'/refund/manage',query:{id:scope.row.id,uId:scope.row.memberId}}" target="_blank">查看</router-link>
                </template>
            </el-table-column>
        </x-table>
    </div>
</template>
<script>
    import { refund } from "../../apis";
    export default {
        data() {
            var now = new Date();
            var date = new Date(now.getTime() - 14 * 24 * 3600 * 1000);
            return {
                findAll: refund.findAll,
                params: { createTimes: [date, now] },
                isCensusOk: false,
                showMultiDelive: false,
                dialogFormVisible: false,
                selectloading: false,
                deliveItem: {
                    logisticsId: '',
                    logisticsFee: '',
                    logisticsCode: '',
                    logisticsCompany: '',
                    address: {
                        id: '',
                        name: '',
                        tel: '',
                        province: '',
                        city: '',
                        county: '',
                        addressDetail: '',
                        postalCode: '',
                        areaCode: '',
                        address: '',
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

                statusOptions: [{
                    value: '',
                    label: '全部'
                }, {
                    value: -1,
                    label: '已取消'
                }, {
                    value: 0,
                    label: '申请中'
                }, {
                    value: 1,
                    label: '等待买家回寄'
                }, {
                    value: 2,
                    label: '待处理'
                }, {
                    value: 5,
                    label: '已完成'
                }],
                timeOptions: [{
                    value: 0,
                    label: '创建时间'
                }, {
                    value: 1,
                    label: '变更时间'
                }],
                handleUserTypeOptions: [
                    {
                        value: '',
                        label: '全部'
                    },
                    {
                        value: 0,
                        label: '商家'
                    },
                    {
                        value: 1,
                        label: '用户'
                    }
                ]
            }
        },
        keepAlive: true,
        methods: {
        }
    }
</script>
