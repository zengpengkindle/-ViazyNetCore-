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
<template title="商品管理" props="{id:0}">
    <div>
        <el-form ref="form" :model="item.product" :rules="rules" @submit.native.prevent v-loading="loading">
            <el-card>
                <div slot="header">
                    <span>{{state?'修改商品':'创建商品'}}</span>
                </div>
                <el-row :gutter="20">
                    <el-col :span="12">
                        <el-form-item label="类目" prop="catId" v-if="!state">
                            <el-cascader expand-trigger="hover"
                                         :options="cats"
                                         v-model="selectedOptions"
                                         @change="handleChange"
                                         :props="props">
                            </el-cascader>
                        </el-form-item>
                        <el-form-item label="类目" v-if="state">
                            <el-input v-model="item.product.catPath" :disabled="true"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="品牌" prop="brandId" v-if="!state">
                            <el-select v-model="item.product.brandId" placeholder="可空">
                                <el-option v-for="r in brands"
                                           :key="r.id"
                                           :label="r.name"
                                           :value="r.id">
                                </el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="品牌" v-if="state">
                            <el-input v-model="item.product.brandName" :disabled="true"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row :gutter="20">
                    <el-col :span="12">
                        <el-form-item label="标题" prop="title">
                            <el-input v-model="item.product.title"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="副标题" prop="subTitle">
                            <el-input v-model="item.product.subTitle"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row :gutter="20">
                    <el-col :span="12">
                        <el-form-item label="关键词" prop="keywords">
                            <el-input v-model="item.product.keywords"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="货号" prop="outerId">
                            <el-input v-model="item.product.outerId"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row :gutter="20">
                    <el-col :span="12">
                        <el-form-item label="退换货类型" prop="refundType">
                            <el-select v-model="item.product.refundType">
                                <el-option v-for="r in refundOptions"
                                           :key="r.id"
                                           :label="r.name"
                                           :value="r.id">
                                </el-option>
                            </el-select>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row :gutter="20">
                    <el-col :span="12">
                        <el-form-item label="前台隐藏" prop="isHidden">
                            <el-switch v-model="item.product.isHidden"
                                       active-text="隐藏"
                                       inactive-text="不隐藏">
                            </el-switch>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-form-item label="描述" prop="description">
                    <el-input type="textaere" v-model="item.product.description"></el-input>
                </el-form-item>
            </el-card>
            <el-card class="m-t-5">
                <div slot="header">
                    <el-row>
                        <el-col :span="4">商品属性</el-col>
                        <el-col :span="6">
                            <el-switch v-model="item.product.openSpec"
                                       active-text="启用规格"
                                       inactive-text="关闭规格"
                                       :disabled="state==1">
                            </el-switch>
                        </el-col>
                    </el-row>
                </div>
                <template v-if="item.product.openSpec">
                    <el-form-item label="规格" prop="keywords">
                        <el-tag :key="tag.k_s"
                                v-for="tag in newSpec"
                                :closable="!state"
                                :disable-transitions="false"
                                @close="handleClose(tag,newSpec)">
                            {{tag.k}}
                        </el-tag>
                        <el-input class="input-new-tag"
                                  v-if="inputVisible"
                                  v-model="inputValue"
                                  ref="saveTagInput"
                                  size="small"
                                  @keyup.enter.native="handleInputConfirm"
                                  @blur="handleInputConfirm">
                        </el-input>
                        <el-button v-else class="button-new-tag" size="small" @click="showInput" :disabled="state==1">+ 规格</el-button>
                    </el-form-item>
                    <el-form-item v-bind:label="s.k" prop="keywords" v-for="(s,index) in newSpec">
                        <el-tag :key="value.id"
                                v-for="value in s.v"
                                :closable="value.closable"
                                :disable-transitions="false"
                                @close="handleClose(value,s.v)">
                            {{value.name}}
                        </el-tag>
                        <el-input class="input-new-tag"
                                  v-if="s.inputVisible"
                                  v-model="s.inputValue"
                                  ref="saveTagInput"
                                  size="small"
                                  @keyup.enter.native="handleValueInputConfirm(s)"
                                  @blur="handleValueInputConfirm(s)">
                        </el-input>
                        <el-button v-else class="button-new-tag" size="small" @click="showValueInput(s)">+ {{s.k}}</el-button>
                    </el-form-item>
                    <el-form-item>
                        <el-button type="success" icon="el-icon-plus" @click="makeSkus">生成SKU</el-button>
                    </el-form-item>
                    <el-form-item v-if="spec.length>0">
                        <el-table :data="item.product.skus.list" border class="sku" default-expand-all>
                            <el-table-column type="expand" v-if="credits.length>0">
                                <template slot-scope="scope">
                                    <el-row :gutter="20" v-for="credit in credits" class="sku-credit-row">
                                        <el-col :span="6">{{credit.name}}</el-col>
                                        <el-col :span="6" :offset="6">货币：{{credit.creditKey}}</el-col>
                                        <el-col :span="6">
                                            <input type="number" class="credit-input" v-model.number="scope.row.specialPrices[credit.key]" @change="handleChangePrice" placeholder="请输入售价" />
                                        </el-col>
                                    </el-row>
                                </template>
                            </el-table-column>
                            <el-table-column v-bind:label="row.k" width="180" v-for="(row,index) in spec">
                                <template slot-scope="scope">
                                    <span width="80" v-if="scope.row.key1==row.k">{{scope.row.name1}}</span>
                                    <span width="80" v-if="scope.row.key2==row.k">{{scope.row.name2}}</span>
                                    <span width="80" v-if="scope.row.key3==row.k">{{scope.row.name3}}</span>
                                </template>
                            </el-table-column>

                            <el-table-column prop="stock_num" width="150" label="库存">
                            </el-table-column>

                            <el-table-column label="成本" width="180">
                                <template slot-scope="scope">
                                    <el-input type="number" v-model.number="scope.row.cost" placeholder="请输入成本"></el-input>
                                </template>
                            </el-table-column>
                            <el-table-column label="售价" width="180">
                                <template slot-scope="scope">
                                    <el-input type="number" v-model.number="scope.row.price" placeholder="请输入售价"></el-input>
                                </template>
                            </el-table-column>
                            <el-table-column label="在库库存">
                                <template slot-scope="scope">
                                    <el-input type="number" :disabled="scope.row.id.length>0" v-model.number="scope.row.stock_num" @change="handleChangestockNum" placeholder="请输入初始在库库存"></el-input>
                                </template>
                            </el-table-column>
                        </el-table>
                    </el-form-item>
                </template>
                <el-row :gutter="20">
                    <el-col :span="8">
                        <el-form-item label="成本" prop="cost">
                            <el-input type="number" v-model.number="item.product.cost"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="售价" prop="price" v-if="!item.product.openSpec">
                            <el-input type="number" v-model.number="item.product.price"></el-input>
                        </el-form-item>
                        <el-form-item label="售价" prop="price" v-if="item.product.openSpec">
                            <el-input type="number" v-model.number="item.product.price" :disabled="true"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item :label="state==1?'在库库存':'初始在库库存'" v-if="!item.product.openSpec">
                            <el-input type="number" :disabled="state==1" v-model.number="item.product.stock.inStock"></el-input>
                        </el-form-item>
                        <el-form-item :label="state==1?'在库库存':'初始在库库存'" v-if="item.product.openSpec">
                            <el-input type="number" v-model.number="item.product.stock.inStock" :disabled="true"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row :gutter="20" v-for="credit in credits" class="sku-credit-row" v-if="!item.product.openSpec&&credits">
                    <el-col :span="6">{{credit.name}}</el-col>
                    <el-col :span="6" :offset="6">货币：{{credit.creditKey}}</el-col>
                    <el-col :span="6">
                        <input type="number" class="credit-input" v-model.number="item.product.specialPrices[credit.key]" placeholder="请输入售价" />
                    </el-col>
                </el-row>
            </el-card>
            <el-card class="m-t-5">
                <div slot="header">
                    <span>商品图片</span>
                </div>
                <el-row :gutter="20">
                    <el-col :span="6">
                        <el-form-item label="主图">
                            <x-image v-model="item.product.image" height="240px" width="240px"></x-image>
                        </el-form-item>
                    </el-col>
                    <el-col :span="4">
                        <el-form-item label="副图">
                            <x-image v-model="subImage1" height="200px" width="200px"></x-image>
                        </el-form-item>
                    </el-col>
                    <el-col :span="4">
                        <el-form-item label="副图">
                            <x-image v-model="subImage2" height="200px" width="200px"></x-image>
                        </el-form-item>
                    </el-col>
                    <el-col :span="4">
                        <el-form-item label="副图">
                            <x-image v-model="subImage3" height="200px" width="200px"></x-image>
                        </el-form-item>
                    </el-col>
                    <el-col :span="4">
                        <el-form-item label="副图">
                            <x-image v-model="subImage4" height="200px" width="200px"></x-image>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-card>
            <el-card v-if="item.product.skus.tree.length>0" class="m-t-5">
                <div slot="header">
                    <span>规格图片</span>
                </div>
                <el-row :gutter="20" v-for="sku1 in item.product.skus.tree[0].v">
                    <el-col :span="4">
                        <el-form-item :label="sku1.name">
                            <x-image v-model="sku1.imgUrl" height="70px" width="70px"></x-image>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-card>
            <el-card>
                <div slot="header">
                    <span>商品详情</span>
                </div>
                <el-form-item label="详情">
                    <x-editor v-model="item.product.detail" ref="editor"></x-editor>
                </el-form-item>
            </el-card>
            <el-card>
                <el-form-item>
                    <el-button type="primary" @click="submit">提交</el-button>
                    <!--<el-button @click="returnIndex">返回</el-button>-->
                </el-form-item>
            </el-card>
        </el-form>
    </div>
</template>
<script>
    import { product, productOuterSpecialCredit } from "../../apis";
    import image from '../../components/Image';
    import editor from '../../components/Editor';
    import { ELOOP } from "constants";
    import { Object } from "core-js";
    export default {
        components: {
            'x-image': image,
            'x-editor': editor
        },
        data() {
            var vm = this;
            var state = vm.$route.query.id ? 1 : 0;
            var outerType = this.$route.query && this.$route.query.outerType;
            return {
                state: state,
                loading: true,
                outerType: outerType,
                credits: [],
                item: {
                    product: {
                        brandId: '', type: 0, businessType: 0, detail: '', refundType: 1,
                        skus: { tree: [], list: [] },
                        openSpec: false, stock: { inStock: 0 }, specialPrices: {}
                    },

                },
                cats: [],
                brands: [],
                spec: [],
                newSpec: [],
                newSkus: [],
                inputVisible: false,
                inputValue: '',
                subImage1: '',
                subImage2: '',
                subImage3: '',
                subImage4: '',
                props: {
                    value: 'idValue',
                    label: 'name',
                    children: 'items'
                },
                refundOptions: [
                    {
                        id: 1,
                        name: '可以退换货'
                    },
                    {
                        id: 2,
                        name: '只能退货'
                    },
                    {
                        id: 3,
                        name: '只能换货'
                    },
                    {
                        id: 4,
                        name: '不能退换货'
                    }
                ],
                selectedOptions: [],
                rules: {
                    catId: [
                        { required: true, message: '请选择分类' }
                    ],
                    title: [
                        { required: true, message: '请输入标题' },
                        { min: 1, max: 100, message: '长度在 1 到 100 个字符' }
                    ],
                    cost: [
                        { required: true, message: '请输入成本' },
                        { type: 'number', message: '请输入数字值' }
                    ],
                    price: [
                        { required: true, message: '请输入售价' },
                        { type: 'number', message: '请输入数字值' }
                    ],
                    image: [
                        { required: true, message: '请上传商品主图' }
                    ]
                },
                isHiddenOptions:
                    [{
                        value: false,
                        label: '否'
                    }, {
                        value: true,
                        label: '是'
                    }]
            }
        },
        created() {
            var vm = this;
            productOuterSpecialCredit.getSpecialCreditByOuterKey(vm.outerType).then(cr => {
                vm.credits = cr.value;

                product.find(vm.$route.query.id, vm.outerType).then(r => {
                    vm.item.product = r.value.product;

                    vm.cats = vm.listToTree(r.value.cats);
                    vm.brands = r.value.brands;
                    vm.loading = false;
                    if (!vm.$route.query.id || vm.$route.query.id == '') {
                        //vm.item.skus = r.value.product.skus;
                        vm.newSkus = r.value.product.skus;

                        if (!vm.item.product.openSpec) {
                            let specialPrices = {};
                            if (vm.credits) {
                                vm.credits.forEach(function (val, index, array) {
                                    specialPrices[val.key] = null;
                                });
                            }
                            vm.item.product.specialPrices = specialPrices;
                        }
                    }
                    else {
                        if (!vm.item.product.openSpec) {
                            if (vm.credits) {
                                if (!vm.item.product.specialPrices) {
                                    vm.item.product.specialPrices = {};
                                }
                                vm.credits.forEach(function (val, index, array) {
                                    if (!vm.item.product.specialPrices[val.key]) {
                                        vm.item.product.specialPrices[val.key] = null;
                                    }
                                });
                            }
                        }

                        if (r.value.product.skus.list && r.value.product.skus.list.length > 0) {

                            //vm.newSpec = r.value.product.skus.tree;
                            r.value.product.skus.tree.forEach(function (value, index, array) {

                                var s = { inputVisible: false, inputValue: '', k: value.k, v: value.v, k_s: value.k_s };
                                vm.newSpec.push(s);
                            });

                            vm.spec = r.value.product.skus.tree;
                            //vm.item.product.skus = r.value.product.skus;
                            vm.newSkus = r.value.product.skus;

                            if (vm.credits) {
                                vm.item.product.skus.list.forEach(function (sku, index, array) {
                                    if (!sku.specialPrices) sku.specialPrices = {}
                                    vm.credits.forEach(function (val, index, array) {
                                        if (!sku.specialPrices[val.key]) {
                                            sku.specialPrices[val.key] = 0;
                                        }
                                    });
                                })
                            }
                        }
                        if (vm.item.product.subImage) {
                            var subs = vm.item.product.subImage.split(',');
                            if (subs.length > 0) {
                                if (subs[0])
                                    vm.subImage1 = subs[0];
                                if (subs[1])
                                    vm.subImage2 = subs[1];
                                if (subs[2])
                                    vm.subImage3 = subs[2];
                                if (subs[3])
                                    vm.subImage4 = subs[3];
                            }
                        }
                    }
                    vm.loading = false;
                });
            });
        },

        methods: {
            contains(arr, obj) {
                var i = arr.length;
                while (i--) {
                    if (arr[i] === obj) {
                        return true;
                    }
                }
                return false;
            },
            handleClose(tag, arr) {
                var vm = this;
                arr.splice(arr.indexOf(tag), 1);
                vm.handleSkus();
            },
            createSkusArray(sales, saleIndex, lines) {
                if (!sales.length) return [];
                var skusArray = [];
                var saleArray = sales[saleIndex].v;
                var saleKey = sales[saleIndex].k;
                if (!saleArray.length) {
                    return skusArray;
                }
                var isLast = saleIndex == (sales.length - 1);
                if (lines && lines.length) {
                    for (var j = 0; j < lines.length; j++) {
                        var line = lines[j];
                        for (var i = 0; i < saleArray.length; i++) {
                            var skus = [];
                            for (var n = 0; n < line.length; n++) {
                                skus.push(line[n]);
                            }
                            saleArray[i].key = saleKey;
                            skus.push(saleArray[i]);
                            skusArray.push(skus);
                        }
                    }
                } else {
                    for (var i = 0; i < saleArray.length; i++) {
                        saleArray[i].key = saleKey;
                        skusArray.push([saleArray[i]]);
                    }
                }
                if (!isLast) {
                    skusArray = this.createSkusArray(sales, saleIndex + 1, skusArray);
                }

                return skusArray;
            },
            handleSkus() {
                var vm = this;
                var skus = [];
                var $thisObjs = [];

                var spec = [];
                vm.newSpec.forEach(function (value, index, array) {
                    if (value.v.length > 0)
                        spec.push(value);
                });

                var skusArray = this.createSkusArray(vm.newSpec, 0, []);

                skusArray.forEach(function (sku, index) {
                    var a = { id: '', s1: '0', key1: '', name1: '', s2: '0', key2: '', name2: '', s3: '0', key3: '', name3: '', cost: '', price: '', stock_num: '' };
                    sku.forEach(function (v, vindex) {
                        a['s' + (vindex + 1)] = v.id;
                        a['key' + (vindex + 1)] = v.key;
                        a['name' + (vindex + 1)] = v.name;
                    });
                    $thisObjs.push(a);
                })


                $thisObjs.forEach(function (value, index, array) {
                    var $thisValue = value;

                    var flag = false;
                    vm.item.product.skus.list.forEach(function (value, index, array) {
                        if (value.s1 == $thisValue.s1 && value.s2 == $thisValue.s2 && value.s3 == $thisValue.s3) {
                            skus.push(value);
                            flag = true;
                        }
                    });
                    if (!flag) {
                        //var sku = {
                        //    Id: '', outerSkuId: '', properties: properties, propObj: $thisValue,
                        //    cost: '', price: '', stock: {inStock:0}
                        //};

                        skus.push($thisValue);
                    }
                });
                vm.newSkus = skus;
            },
            makeSkus() {
                var vm = this;
                for (var i = 0; i < vm.newSpec.length; i++) {
                    if (vm.newSpec[i].v.length < 1) {
                        vm.$message({
                            type: 'info',
                            message: '请输入规格【' + vm.newSpec[i].k + '】的值'
                        });
                        return false;
                    }
                }


                vm.$confirm('此操作将修改SKU信息, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    vm.spec = Object.assign([], vm.newSpec);
                    vm.item.product.skus.tree = vm.spec;

                    var skuList = [];
                    vm.newSkus.forEach(function (val, index, array) {
                        if (vm.credits) {
                            let specialPrices = {};
                            vm.credits.forEach(function (val, index, array) {
                                specialPrices[val.key] = 0;
                            });
                            val.specialPrices = specialPrices;
                        }
                        skuList.push(val);
                    });
                    vm.item.product.skus.list = skuList;
                }).catch((e) => {
                    console.log(e)
                    this.$message({
                        type: 'info',
                        message: '已取消生成'
                    });
                });

            },
            showInput() {
                var vm = this;
                vm.inputVisible = true;
                vm.$nextTick(_ => {
                    if (!Array.isArray(vm.$refs.saveTagInput))
                        vm.$refs.saveTagInput.$refs.input.focus();
                    else
                        vm.$refs.saveTagInput[0].$refs.input.focus();
                });
            },
            showValueInput(s) {
                var vm = this;
                s.inputVisible = true;
                vm.$nextTick(_ => {
                    if (!Array.isArray(vm.$refs.saveTagInput))
                        vm.$refs.saveTagInput.$refs.input.focus();
                    else
                        vm.$refs.saveTagInput[0].$refs.input.focus();
                });
            },
            handleInputConfirm() {
                var vm = this;
                let inputValue = vm.inputValue;
                var flag = false;
                if (inputValue) {
                    vm.newSpec.forEach(function (value, index, Array) {
                        if (value.k == inputValue)
                            flag = true;
                    });
                    if (flag) {
                        vm.$message({
                            type: 'info',
                            message: '规格(' + inputValue + ')已存在，不能重复设置'
                        });
                        vm.inputValue = '';
                        return false;
                    }
                    if (vm.newSpec.length >= 3) {
                        vm.$message({
                            type: 'info',
                            message: '最多可添加3个规格'
                        });
                        vm.inputValue = '';
                        return false;
                    }
                    //vm.newSpec.push({ name: inputValue, value: [], inputVisible: false });
                    vm.newSpec.push({ k: inputValue, v: [], k_s: 's' + (vm.newSpec.length + 1), inputVisible: false });
                    vm.handleSkus();
                }
                if (vm.inputValue) {
                    vm.inputVisible = false;
                    vm.inputValue = '';
                    vm.showInput();
                } else {
                    vm.inputVisible = false;
                }
            },
            handleValueInputConfirm(s) {
                var vm = this;
                let inputValue = s.inputValue;
                var flag = false;
                if (inputValue) {
                    s.v.forEach(function (value, index, Array) {
                        if (value.name == inputValue)
                            flag = true;
                    });
                    if (flag) {
                        vm.$message({
                            type: 'info',
                            message: '当前规格的值(' + inputValue + ')已存在，不能重复设置'
                        });
                        s.inputValue = '';
                        return false;
                    }
                    var item = { name: inputValue, id: s.k_s + '000' + (s.v.length + 1), imgUrl: '', closable: true };
                    s.v.push(item);
                    vm.handleSkus();
                }

                if (s.inputValue) {
                    s.inputVisible = false;
                    s.inputValue = '';
                    vm.showValueInput(s);
                } else {
                    s.inputVisible = false;
                }
            },
            handleChange(value) {

                var vm = this;
                vm.item.product.catId = value[value.length - 1]['id'];
                vm.item.product.catName = value[value.length - 1]['name'];
                vm.item.product.catPath = "";
                value.forEach(function (value, index, array) {
                    vm.item.product.catPath += value['name'] + '/';
                });
                vm.item.product.catPath = vm.item.product.catPath.substring(0, vm.item.product.catPath.length - 1);
            },
            handleChangePrice() {
                var vm = this;
                var price = vm.item.product.skus.list[0].price;
                vm.item.product.skus.list.forEach(function (value, index, array) {
                    if (value.price != "" && value.price != null && !isNaN(value.price)) {
                        if (price > value.price) {
                            price = value.price;
                        }
                    }
                });
                vm.item.product.price = price;
            },

            handleChangestockNum() {
                var vm = this;
                var stock = 0;
                vm.item.product.skus.list.forEach(function (value, index, array) {
                    if (value.stock_num != "" && value.stock_num != null && !isNaN(value.stock_num)) {
                        //if (stock > value.stock.inStock) {
                        stock += value.stock_num;
                        //}
                    }
                });

                vm.item.product.stock.inStock = stock;
            },
            listToTree(data, options) {
                options = options || {};
                var ID_KEY = options.idKey || 'id';
                var PARENT_KEY = options.parentKey || 'parentId';
                var CHILDREN_KEY = options.childrenKey || 'items';
                var tree = [],
                    childrenOf = {};
                var item, id, parentId;
                for (var i = 0, length = data.length; i < length; i++) {
                    item = data[i];
                    item.idValue = { id: item.id, name: item.name }
                    id = item[ID_KEY];
                    parentId = item[PARENT_KEY] || 0;
                    if (item.isParent) {
                        childrenOf[id] = childrenOf[id] || [];
                        item[CHILDREN_KEY] = childrenOf[id];
                    }
                    if (parentId != 0) {
                        childrenOf[parentId] = childrenOf[parentId] || [];
                        childrenOf[parentId].push(item);
                    } else {
                        tree.push(item);
                    }
                };

                return tree;
            },
            submit() {
                var vm = this;
                vm.$refs.form.validate(function (valid) {
                    if (!valid) return;
                    if (vm.item.product.openSpec) {
                        vm.item.product.specialPrices = null;
                        if (vm.item.product.skus.length < 1) {
                            vm.$message({
                                type: 'warning',
                                message: '规格已开启，请完善规格信息'
                            });
                            return;
                        }
                    }

                    if (!vm.item.product.image) {
                        vm.$message({
                            type: 'warning',
                            message: '请上传商品主图'
                        });
                        return;
                    }

                    vm.item.product.subImage = '';
                    if (vm.subImage1)
                        vm.item.product.subImage += vm.subImage1 + ',';
                    if (vm.subImage2)
                        vm.item.product.subImage += vm.subImage2 + ',';
                    if (vm.subImage3)
                        vm.item.product.subImage += vm.subImage3 + ',';
                    if (vm.subImage4)
                        vm.item.product.subImage += vm.subImage4 + ',';
                    if (vm.item.product.subImage)
                        vm.item.product.subImage = vm.item.product.subImage.substring(0, vm.item.product.subImage.length - 1);

                    product.submit(vm.outerType, vm.item.product)
                        .then(r => {
                            if (r.status) {
                                vm.$alert(r.message, { type: 'error' });
                            }
                            else {
                                vm.$message('保存成功！');
                                vm.$router.push('/product/index');
                            }
                        });
                });
            },
            returnIndex() {
                var vm = this;
                vm.$router.push('/product/index');
            }
        }
    }
</script>
<style>
    .el-table.sku thead tr {
        background-color: #ECEBEB
    }

    .sku-credit-row:not(:last-child) {
        border-bottom: 1px dashed #ECEBEB;
    }

    .sku-credit-row .el-col {
        min-height: 34px;
        line-height: 34px;
        font-size: 14px;
    }

    .sku-credit-row .credit-input {
        -webkit-appearance: none;
        background-color: #fff;
        background-image: none;
        border-radius: 4px;
        border: 1px solid #dcdfe6;
        box-sizing: border-box;
        color: #606266;
        display: inline-block;
        font-size: inherit;
        height: 32px;
        line-height: 32px;
        outline: none;
        padding: 0 15px;
        transition: border-color .2s cubic-bezier(.645,.045,.355,1);
        width: 100%;
    }
</style>