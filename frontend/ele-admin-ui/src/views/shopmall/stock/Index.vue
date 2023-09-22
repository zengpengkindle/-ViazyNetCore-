<template title="商品库存管理">
    <div>
        <header class="crum-title">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item>商品库存管理</el-breadcrumb-item>
            </el-breadcrumb>
        </header>
        <x-table :params="params" ref="table" :url="findAll">
            <template slot="form">
                <el-form-item label="标题">
                    <el-input placeholder="模糊搜索" v-model="params.titleLike"></el-input>
                </el-form-item>

                <el-form-item label="类目">
                    <el-input placeholder="精准搜索" v-model="params.catName"></el-input>
                </el-form-item>
                <el-form-item label="前台隐藏">
                    <el-select v-model="params.isHidden" placeholder="全部">
                        <el-option v-for="r in options"
                                   :key="r.value"
                                   :label="r.label"
                                   :value="r.value">
                        </el-option>
                    </el-select>
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
                <el-form-item label="创建时间">
                    <el-date-picker v-model="params.createTimes"
                                    type="daterange"
                                    :picker-options="$pickerOptions"
                                    range-separator="至"
                                    start-placeholder="开始日期"
                                    end-placeholder="结束日期"
                                    align="right">
                    </el-date-picker>
                </el-form-item>
                <el-row>
                    <el-col :span="24" align="right">
                        <el-button type="primary" icon="el-icon-search" native-type="submit">搜索</el-button>
                    </el-col>
                </el-row>
            </template>
            <el-table-column label="标题" fixed width="380">
                <template slot-scope="scope">
                    <el-row :gutter="20">
                        <el-col :span="8">
                            <img :src="scope.row.image" width="120">
                        </el-col>
                        <el-col :span="16">
                            <p class="product_title">
                                {{scope.row.title}}
                            </p>
                            <p>
                                <span>{{scope.row.catPath}}</span>
                            </p>
                        </el-col>
                    </el-row>
                </template>
            </el-table-column>

            <el-table-column prop="catName" label="类目" width="80"></el-table-column>
            <el-table-column prop="shopName" label="商家" width="80"></el-table-column>
            <el-table-column prop="cost" label="成本" width="80"></el-table-column>
            <el-table-column prop="price" label="售价" width="80"></el-table-column>
            <!--<el-table-column prop="stockNum" label="总库存" width="80"></el-table-column>
            <el-table-column prop="totalSellNum" label="销量" width="80"></el-table-column>-->
            <el-table-column label="前台隐藏">
                <template slot-scope="scope">
                    <span v-if="scope.row.isHidden">是</span>
                    <span v-else>否</span>
                </template>
            </el-table-column>
            <el-table-column label="状态">
                <template slot-scope="scope">
                    <span v-if="scope.row.status==0">未上架</span>
                    <span v-else-if="scope.row.status==1">待审核</span>
                    <span v-else-if="scope.row.status==2">出售中</span>
                </template>
            </el-table-column>
            <el-table-column prop="createTime" width="200" label="创建时间" :formatter="$dateFormatter"></el-table-column>
            <el-table-column label="操作" width="100" fixed="right">
                <template slot-scope="scope">
                    <router-link :to="{path:'/stock/manage',query:{id:scope.row.id}}" target="_blank">库存</router-link>
                </template>
            </el-table-column>
        </x-table>
    </div>
</template>
<script>
    import { product } from "../../apis";

    export default {

        data() {
            return {
                findAll: product.findAll,

                params: {},
                statusOptions: [{
                    value: '',
                    label: '全部'
                }, {
                    value: 0,
                    label: '未上架'
                }, {
                    value: 1,
                    label: '待审核'
                }, {
                    value: 2,
                    label: '出售中'
                }],
                options:
                    [{
                        value: false,
                        label: '否'
                    }, {
                        value: true,
                        label: '是'
                    }]
            }
        },
        keepAlive: true,
        methods: {
        }
    }
</script>
