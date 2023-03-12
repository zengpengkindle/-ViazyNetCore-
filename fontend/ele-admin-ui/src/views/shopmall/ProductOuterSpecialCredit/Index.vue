<template title="商品类别定价管理">
    <div>
        <header class="crum-title">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item>商品管理</el-breadcrumb-item>
                <el-breadcrumb-item>类别管理</el-breadcrumb-item>
                <el-breadcrumb-item>定价管理</el-breadcrumb-item>
            </el-breadcrumb>
        </header>
        <x-table :params="params" ref="table" :url="findAll">
            <template slot="form">
                <el-row>
                    <el-col :span="18">
                        <el-button type="success" round icon="el-icon-plus" @click="$router.push('./manage?outerType='+params.outerType)">新增</el-button>
                    </el-col>
                    <el-col :span="6" align="right">
                        <el-button type="primary" icon="el-icon-search" native-type="submit">搜索</el-button>
                    </el-col>
                </el-row>
            </template>
            <el-table-column prop="objectKey" label="价格类型标识" width="180"></el-table-column>
            <el-table-column prop="objectName" label="价格类型名称" width="180"></el-table-column>
            <el-table-column prop="outerType" label="活动/类别" width="220"></el-table-column>
            <el-table-column label="货币名称">
                <template slot-scope="scope">
                    <span>{{creditOptions[scope.row.creditKey]}}({{scope.row.creditKey}})</span>
                </template>
            </el-table-column>
            <el-table-column label="价格计算方式">
                <template slot-scope="scope">
                    <span v-if="scope.row.computeType==0">独立价格</span>
                    <span v-else-if="scope.row.computeType==1">与商品设置价格等价</span>
                    <span v-else-if="scope.row.computeType==2">计算兑换手续费-固定 {{scope.row.feeMoney}}</span>
                    <span v-else-if="scope.row.computeType==3">计算百分比手续费 {{scope.row.feePercent}}%</span>
                    <span v-else-if="scope.row.computeType==4">混合价格</span>
                    <span v-else-if="scope.row.computeType==5">条件式</span>
                </template>
            </el-table-column>
            <el-table-column label="状态">
                <template slot-scope="scope">
                    <span v-if="scope.row.status==0">禁用</span>
                    <span v-else-if="scope.row.status==1">启用</span>
                    <span v-else-if="scope.row.status==-1">逻辑删除</span>
                </template>
            </el-table-column>
            <el-table-column prop="createTime" width="200" label="创建时间" :formatter="$dateFormatter"></el-table-column>
            <el-table-column label="操作" width="100" fixed="right">
                <template slot-scope="scope">
                    <el-button @click="$router.push('./manage?id='+scope.row.id)" type="text" size="small">编辑</el-button>
                    <el-button @click="modifyStatus(scope.row,0)" v-if="scope.row.status==1" type="text" size="small">禁用</el-button>
                    <el-button @click="modifyStatus(scope.row,1)" v-if="scope.row.status==0" type="text" size="small">启用</el-button>
                </template>
            </el-table-column>
        </x-table>
    </div>
</template>
<script>
    import { productOuterSpecialCredit, credit } from "../../apis";

    export default {

        data() {
            var outerType = this.$route.query && this.$route.query.outerType;
            return {
                findAll: productOuterSpecialCredit.findAll,
                params: { outerType: outerType },
                creditOptions: []
            }
        },
        created() {
            credit.getAll().then(cr => {
                this.creditOptions = cr.value;
            })
        },
        keepAlive: true,
        methods: {
            modifyStatus(row, status) {
                var vm = this;
                productOuterSpecialCredit.modifyStatus(row.id, status)
                    .then(r => {
                        if (r.status) {
                            vm.$alert(r.message, { type: 'error' });
                        }
                        else {
                            vm.$message('操作成功！');
                            vm.$refs.table.refresh();
                        }
                    });

            }
        }
    }
</script>
