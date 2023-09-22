<template title="商品管理">
  <div>
    <el-form
      ref="form"
      :model="item"
      :rules="rules"
      v-loading="loading"
      label-width="120px"
    >
      <el-card>
        <el-tabs tab-position="left">
          <el-tab-pane v-if="item" label="基础信息">
            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="类目" prop="catId">
                  <el-cascader
                    expand-trigger="hover"
                    :options="cats"
                    :props="catProps"
                    v-model="item.catId"
                    @change="handleChange"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="品牌" prop="brandId">
                  <el-select v-model="item.brandId" :disabled="state === 1">
                    <el-option
                      v-for="r in brands"
                      :key="r.id"
                      :label="r.name"
                      :value="r.id"
                    />
                  </el-select>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="商品名称" prop="title">
                  <el-input v-model="item.title" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="副标题" prop="subTitle">
                  <el-input v-model="item.subTitle" />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="关键词" prop="keywords">
                  <el-input v-model="item.keywords" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="商品编码" prop="outerId">
                  <el-input v-model="item.outerType" />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="退换货类型" prop="refundType">
                  <el-select v-model="item.refundType">
                    <el-option
                      v-for="r in refundOptions"
                      :key="r.id"
                      :label="r.name"
                      :value="r.id"
                    />
                  </el-select>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <el-col :span="12">
                <el-form-item label="前台隐藏" prop="isHidden">
                  <el-switch
                    v-model="item.isHidden"
                    active-text="隐藏"
                    inactive-text="不隐藏"
                  />
                </el-form-item>
              </el-col>
            </el-row>
            <el-form-item label="商品简介" prop="description">
              <el-input
                type="textarea"
                :row="3"
                v-model="item.description"
                maxlength="250"
                show-word-limit
              />
            </el-form-item>
          </el-tab-pane>
          <el-tab-pane class="mt-3" label="SKU/货品设置">
            <el-form-item label="启用规格" prop="openSpec">
              <el-switch
                v-model="item.openSpec"
                active-text="启用规格"
                inactive-text="关闭规格"
                :disabled="state == 1"
              />
            </el-form-item>
            <template v-if="item.openSpec">
              <el-form-item label="规格" prop="keywords">
                <el-tag
                  :key="tag.k_s"
                  v-for="tag in newSpec"
                  :closable="!state"
                  :disable-transitions="false"
                  @close="handleClose(tag, newSpec)"
                >
                  {{ tag.k }}
                </el-tag>
                <el-input
                  class="input-new-tag"
                  v-if="inputVisible"
                  v-model="inputValue"
                  ref="saveTagInput"
                  size="small"
                  @blur="handleInputConfirm"
                  @keyup.enter="handleInputConfirm"
                />
                <el-button
                  v-else
                  class="button-new-tag"
                  size="small"
                  @click="showInput"
                  :disabled="state == 1"
                  >+ 规格</el-button
                >
              </el-form-item>
              <el-form-item
                v-bind:label="s.k"
                prop="keywords"
                v-for="s in newSpec"
                v-bind:key="s"
              >
                <el-tag
                  :key="value.id"
                  v-for="value in s.v"
                  :closable="value.closable"
                  :disable-transitions="false"
                  @close="handleClose(value, s.v)"
                >
                  {{ value.name }}
                </el-tag>
                <el-input
                  class="input-new-tag"
                  v-if="s.inputVisible"
                  v-model="s.inputValue"
                  ref="saveTagInput"
                  size="small"
                  @keyup.enter="handleValueInputConfirm(s)"
                  @blur="handleValueInputConfirm(s)"
                />
                <el-button
                  v-else
                  class="button-new-tag"
                  size="small"
                  @click="showValueInput(s)"
                  >+ {{ s.k }}</el-button
                >
              </el-form-item>
              <el-form-item>
                <el-button type="success" icon="el-icon-plus" @click="makeSkus"
                  >生成SKU</el-button
                >
              </el-form-item>
              <el-form-item v-if="spec.length > 0">
                <el-table :data="item.skus.list" class="sku" default-expand-all>
                  <el-table-column type="expand" v-if="credits.length > 0">
                    <template #default="scope">
                      <el-row
                        :gutter="20"
                        v-for="credit in credits"
                        v-bind:key="credit.key"
                        class="sku-credit-row"
                      >
                        <el-col :span="6">{{ credit.name }}</el-col>
                        <el-col :span="6" :offset="6"
                          >货币：{{ credit.creditKey }}</el-col
                        >
                        <el-col :span="6">
                          <input
                            type="number"
                            class="credit-input"
                            v-model.number="scope.row.specialPrices[credit.key]"
                            @change="handleChangePrice"
                            placeholder="请输入售价"
                          />
                        </el-col>
                      </el-row>
                    </template>
                  </el-table-column>
                  <el-table-column
                    v-bind:label="row.k"
                    width="180"
                    v-for="row in spec"
                    v-bind:key="row.k"
                  >
                    <template #default="scope">
                      <span width="80" v-if="scope.row.key1 == row.k">{{
                        scope.row.name1
                      }}</span>
                      <span width="80" v-if="scope.row.key2 == row.k">{{
                        scope.row.name2
                      }}</span>
                      <span width="80" v-if="scope.row.key3 == row.k">{{
                        scope.row.name3
                      }}</span>
                    </template>
                  </el-table-column>
                  <el-table-column prop="stock_num" width="150" label="库存" />
                  <el-table-column label="成本" width="180">
                    <template #default="scope">
                      <el-input
                        type="number"
                        v-model.number="scope.row.cost"
                        placeholder="请输入成本"
                      />
                    </template>
                  </el-table-column>
                  <el-table-column label="售价" width="180">
                    <template #default="scope">
                      <el-form-item>
                        <el-input
                          type="number"
                          v-model.number="scope.row.price"
                          placeholder="请输入售价"
                      /></el-form-item>
                    </template>
                  </el-table-column>
                  <el-table-column label="在库库存">
                    <template #default="scope">
                      <el-input
                        type="number"
                        :disabled="scope.row.id.length > 0"
                        v-model.number="scope.row.stock_num"
                        @change="handleChangestockNum"
                        placeholder="请输入初始在库库存"
                      />
                    </template>
                  </el-table-column>
                </el-table>
              </el-form-item>
            </template>
            <el-row :gutter="20">
              <el-col :span="8">
                <el-form-item label="成本" prop="cost">
                  <el-input type="number" v-model.number="item.cost" />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="售价" prop="price" v-if="!item.openSpec">
                  <el-input type="number" v-model.number="item.price" />
                </el-form-item>
                <el-form-item label="售价" prop="price" v-if="item.openSpec">
                  <el-input
                    type="number"
                    v-model.number="item.price"
                    :disabled="true"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  :label="state == 1 ? '在库库存' : '初始在库库存'"
                  v-if="!item.openSpec"
                >
                  <el-input
                    type="number"
                    :disabled="state == 1"
                    v-model.number="item.stock.inStock"
                  />
                </el-form-item>
                <el-form-item
                  :label="state == 1 ? '在库库存' : '初始在库库存'"
                  v-if="item.openSpec"
                >
                  <el-input
                    type="number"
                    v-model.number="item.stock.inStock"
                    :disabled="true"
                  />
                </el-form-item>
              </el-col>
            </el-row>
            <template v-if="!item.openSpec && credits">
              <el-row
                :gutter="20"
                v-for="credit in credits"
                class="sku-credit-row"
                v-bind:key="credit.key"
              >
                <el-col :span="6">{{ credit.name }}</el-col>
                <el-col :span="6" :offset="6"
                  >货币：{{ credit.creditKey }}</el-col
                >
                <el-col :span="6">
                  <input
                    type="number"
                    class="credit-input"
                    v-model.number="item.specialPrices[credit.key]"
                    placeholder="请输入售价"
                  />
                </el-col>
              </el-row>
            </template>

            <el-card v-if="item && item.skus.tree.length > 0" class="mt-3">
              <template #header>
                <span>规格图片</span>
              </template>
              <el-row
                :gutter="20"
                v-for="sku1 in item.skus.tree[0].v"
                v-bind:key="sku1.id"
              >
                <el-col :span="12">
                  <el-form-item :label="sku1.name">
                    <x-image
                      v-model="sku1.imgUrl"
                      height="200px"
                      width="200px"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
            </el-card>
          </el-tab-pane>
          <el-tab-pane class="mt-3" v-if="item" label="图片/图集">
            <template #header>
              <span>商品图片</span>
            </template>
            <el-row :gutter="20">
              <el-col :span="6">
                <el-form-item label="主图">
                  <x-image v-model="item.image" height="240px" width="240px" />
                </el-form-item>
              </el-col>
              <el-col :span="4">
                <el-form-item>
                  <x-image v-model="subImage1" height="200px" width="200px" />
                </el-form-item>
              </el-col>
              <el-col :span="4">
                <el-form-item>
                  <x-image v-model="subImage2" height="200px" width="200px" />
                </el-form-item>
              </el-col>
              <el-col :span="4">
                <el-form-item>
                  <x-image v-model="subImage3" height="200px" width="200px" />
                </el-form-item>
              </el-col>
              <el-col :span="4">
                <el-form-item>
                  <x-image v-model="subImage4" height="200px" width="200px" />
                </el-form-item>
              </el-col>
            </el-row>
          </el-tab-pane>
          <el-tab-pane class="mt-3" label="商品详情">
            <template #header>
              <span>商品详情</span>
            </template>
            <div class="wangeditor">
              <Toolbar
                style="border-bottom: 1px solid #ccc"
                :editor="editorRef"
                :defaultConfig="toolbarConfig"
                :mode="mode"
              />
              <Editor
                style="height: 500px; overflow-y: hidden"
                v-model="item.detail"
                :defaultConfig="editorConfig"
                :mode="mode"
                @onCreated="handleCreated"
              />
            </div>
          </el-tab-pane>
        </el-tabs>
      </el-card>
      <el-affix position="bottom">
        <el-card class="mt-3">
          <el-form-item>
            <el-button type="primary" @click="submit(form)">提交</el-button>
            <!--<el-button @click="returnIndex">返回</el-button>-->
          </el-form-item>
        </el-card>
      </el-affix>
    </el-form>
  </div>
</template>
<script lang="ts" setup>
import "@wangeditor/editor/dist/css/style.css"; // 引入 css
import { handleTree } from "@/utils/tree";
import { Editor, Toolbar } from "@wangeditor/editor-for-vue";
import {
  ElMessageBox,
  FormInstance,
  FormRules,
  CascaderProps
} from "element-plus";
import {
  ref,
  onMounted,
  reactive,
  nextTick,
  Ref,
  shallowRef,
  onBeforeUnmount
} from "vue";
import { useRoute, useRouter } from "vue-router";
import ProductOuterSpecialCreditApi, {
  OuterKeySpecialCredit
} from "@/api/shopmall/productOuterSpecialCredit";
import ProductApi, {
  ProductManageModel,
  ProductSkuManageModel,
  ProductBrand,
  SkuTree,
  ProductStatus,
  RefundType
} from "@/api/shopmall/product";
import { message } from "@/utils/message";
import { IDomEditor, IEditorConfig, IToolbarConfig } from "@wangeditor/editor";
import { http } from "@/utils/http";
const route = useRoute();
const state = ref(0);
const outerType = ref("");
const loading = ref(true);
onMounted(() => {
  state.value = route.query.id ? 1 : 0;
  outerType.value = route.query && (route.query.outerType as string);
  init();
});
const catProps: CascaderProps = { label: "name", value: "id", emitPath: false };

const item: Ref<ProductManageModel> = ref({
  id: null,
  brandId: null,
  brandName: null,
  catId: null,
  catName: null,
  shopId: null,
  shopName: null,
  catPath: null,
  title: null,
  subTitle: null,
  keywords: null,
  description: null,
  cost: null,
  price: null,
  isFreeFreight: 0,
  freight: 0,
  freightStep: 0,
  freightValue: 0,
  isHidden: 0,
  statusChangeTime: null,
  image: null,
  subImage: null,
  openSpec: null,
  detail: "",
  status: ProductStatus.OnSale,
  refundType: RefundType.NoSupport,
  stock: { inStock: 0 },
  createTime: null,
  modifyTime: null,
  searchContent: null,
  hasOuter: false,
  outerType: null,
  exdata: null,
  propertyKeys: null,
  propertyValues: null,
  specialPrices: [],
  skus: { tree: [], list: [] }
});

const rules = reactive<FormRules>({
  catId: [{ required: true, message: "请选择分类" }],
  title: [
    { required: true, message: "请输入标题" },
    { min: 1, max: 100, message: "长度在 1 到 100 个字符" }
  ],
  cost: [
    { required: true, message: "请输入成本" },
    { type: "number", message: "请输入数字值" }
  ],
  // price: [
  //   { required: true, message: "请输入售价" },
  //   { type: "number", message: "请输入数字值" }
  // ],
  image: [{ required: true, message: "请上传商品主图" }]
});

const refundOptions = reactive([
  {
    id: 1,
    name: "可以退换货"
  },
  {
    id: 2,
    name: "只能退货"
  },
  {
    id: 3,
    name: "只能换货"
  },
  {
    id: 4,
    name: "不能退换货"
  }
]);
const credits: Ref<Array<OuterKeySpecialCredit>> = ref([]);
const spec: Ref<Array<SkuTree>> = ref([]);
const cats = ref([]);
const brands: Ref<Array<ProductBrand>> = ref([]);
const newSpec = ref([]);
const inputVisible = ref(false);
const inputValue = ref("");
const subImage1 = ref("");
const subImage2 = ref("");
const subImage3 = ref("");
const subImage4 = ref("");
const newSkus: Ref<ProductSkuManageModel> = ref();
const init = async () => {
  if (outerType.value) {
    credits.value =
      await ProductOuterSpecialCreditApi.getSpecialCreditByOuterKey(
        outerType.value
      );
  }
  const catlst = await ProductApi.getCats();
  cats.value = handleTree(catlst);

  const brandlst = await ProductApi.getBrands();
  brands.value = brandlst;
  if (!route.query.id || route.query.id == "") {
    //item.skus = r.value.product.skus;
    newSkus.value = { tree: [], list: [] };
    item.value.skus = newSkus.value;
    if (!item.value.openSpec) {
      const specialPrices = {};
      if (credits.value) {
        credits.value.forEach(function (val) {
          specialPrices[val.key] = null;
        });
      }
      item.value.specialPrices = specialPrices;
    }
  } else {
    const productModel = await ProductApi.find(
      route.query.id as string,
      outerType.value
    );
    item.value = productModel.product;

    if (!item.value.openSpec) {
      if (credits.value) {
        if (!item.value.specialPrices) {
          item.value.specialPrices = {};
        }
        credits.value.forEach(function (val) {
          if (!item.value.specialPrices[val.key]) {
            item.value.specialPrices[val.key] = null;
          }
        });
      }
    }

    if (
      productModel.product.skus.list &&
      productModel.product.skus.list.length > 0
    ) {
      //newSpec = r.value.product.skus.tree;
      productModel.product.skus.tree.forEach(function (value) {
        const s = {
          inputVisible: false,
          inputValue: "",
          k: value.k,
          v: value.v,
          k_s: value.k_s
        };
        newSpec.value.push(s);
      });

      spec.value = productModel.product.skus.tree;
      //item.skus = r.value.product.skus;
      newSkus.value = productModel.product.skus;

      if (credits.value) {
        item.value.skus.list.forEach(function (sku) {
          if (!sku.specialPrices) sku.specialPrices = {};
          credits.value.forEach(function (val) {
            if (!sku.specialPrices[val.key]) {
              sku.specialPrices[val.key] = 0;
            }
          });
        });
      }
    }
    if (item.value.subImage) {
      const subs = item.value.subImage.split(",");
      if (subs.length > 0) {
        if (subs[0]) subImage1.value = subs[0];
        if (subs[1]) subImage2.value = subs[1];
        if (subs[2]) subImage3.value = subs[2];
        if (subs[3]) subImage4.value = subs[3];
      }
    }
  }
  loading.value = false;
};

// 编辑器实例，必须用 shallowRef
const editorRef = shallowRef();
const toolbarConfig: Partial<IToolbarConfig> = {
  excludeKeys: ["fullScreen"]
};
type InsertFnType = (url: string, alt?: string, href?: string) => void;
const editorConfig: Partial<IEditorConfig> = {
  placeholder: "请输入内容...",
  MENU_CONF: {
    uploadImage: {
      server: "api/common/upload/image",
      // 自定义插入图片
      async customUpload(file: File, insertFn: InsertFnType) {
        const param = new FormData(); // 创建form对象
        //注意files是对应后台的参数名哦
        param.append("file", file);
        const res = await http.request({
          url: "/api/common/upload/image",
          method: "post",
          headers: { "Content-Type": "multipart/form-data" },
          data: param
        });
        insertFn(res as string);
      }
    }
  }
};

const mode = "simple";
// 组件销毁时，也及时销毁编辑器
onBeforeUnmount(() => {
  const editor = editorRef.value;
  if (editor == null) return;
  editor.destroy();
});
const handleCreated = (editor: IDomEditor) => {
  editorRef.value = editor; // 记录 editor 实例，重要！
};

function handleClose(tag: any, arr: any[]) {
  arr.splice(arr.indexOf(tag), 1);
  handleSkus();
}
function createSkusArray(
  sales: string | any[],
  saleIndex: number,
  lines: string | any[]
) {
  if (!sales.length) return [];
  let skusArray = [];
  const saleArray = sales[saleIndex].v;
  const saleKey = sales[saleIndex].k;
  if (!saleArray.length) {
    return skusArray;
  }
  const isLast = saleIndex == sales.length - 1;
  if (lines && lines.length) {
    for (let j = 0; j < lines.length; j++) {
      const line = lines[j];
      for (let i = 0; i < saleArray.length; i++) {
        const skus = [];
        for (let n = 0; n < line.length; n++) {
          skus.push(line[n]);
        }
        saleArray[i].key = saleKey;
        skus.push(saleArray[i]);
        skusArray.push(skus);
      }
    }
  } else {
    for (let i = 0; i < saleArray.length; i++) {
      saleArray[i].key = saleKey;
      skusArray.push([saleArray[i]]);
    }
  }
  if (!isLast) {
    skusArray = createSkusArray(sales, saleIndex + 1, skusArray);
  }

  return skusArray;
}
function handleSkus() {
  const skus = [];
  const $thisObjs = [];

  const spec = [];
  newSpec.value.forEach(function (value) {
    if (value.v.length > 0) spec.push(value);
  });

  const skusArray = createSkusArray(newSpec.value, 0, []);

  skusArray.forEach(function (sku) {
    const a = {
      id: "",
      s1: "0",
      key1: "",
      name1: "",
      s2: "0",
      key2: "",
      name2: "",
      s3: "0",
      key3: "",
      name3: "",
      cost: "",
      price: "",
      stock_num: ""
    };
    sku.forEach(function (v: { id: any; key: any; name: any }, vindex: number) {
      a["s" + (vindex + 1)] = v.id;
      a["key" + (vindex + 1)] = v.key;
      a["name" + (vindex + 1)] = v.name;
    });
    $thisObjs.push(a);
  });

  $thisObjs.forEach(function (value) {
    const $thisValue = value;
    let flag = false;
    item.value.skus.list.forEach(function (value) {
      if (
        value.s1 == $thisValue.s1 &&
        value.s2 == $thisValue.s2 &&
        value.s3 == $thisValue.s3
      ) {
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
  newSkus.value.list = skus;
}
function makeSkus() {
  for (let i = 0; i < newSpec.value.length; i++) {
    if (newSpec.value[i].v.length < 1) {
      message(`请输入规格【` + newSpec.value[i].k + `】的值`, {
        type: "info"
      });
      return false;
    }
  }

  ElMessageBox.confirm("此操作将修改SKU信息, 是否继续?", "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning"
  })
    .then(() => {
      spec.value = Object.assign([], newSpec.value);
      item.value.skus.tree = spec.value;

      const skuList = [];
      newSkus.value.list.forEach(function (val) {
        if (credits.value) {
          const specialPrices = {};
          credits.value.forEach(function (val) {
            specialPrices[val.key] = 0;
          });
          val.specialPrices = specialPrices;
        }
        skuList.push(val);
      });
      item.value.skus.list = skuList;
    })
    .catch(e => {
      console.log(e);
      message("已取消生成", {
        type: "info"
      });
    });
}
function showInput() {
  inputVisible.value = true;
  nextTick(() => {
    // if (!Array.isArray($refs.saveTagInput))
    //   $refs.saveTagInput.$refs.input.focus();
    // else $refs.saveTagInput[0].$refs.input.focus();
  });
}
function showValueInput(s: { inputVisible: boolean }) {
  s.inputVisible = true;
  // nextTick(_ => {
  //   if (!Array.isArray($refs.saveTagInput))
  //     $refs.saveTagInput.$refs.input.focus();
  //   else $refs.saveTagInput[0].$refs.input.focus();
  // });
}
function handleInputConfirm() {
  let flag = false;
  if (inputValue.value) {
    newSpec.value.forEach(function (value) {
      if (value.k == inputValue.value) flag = true;
    });
    if (flag) {
      message(`规格({inputValue})已存在，不能重复设置`, {
        type: "info"
      });
      inputValue.value = "";
      return false;
    }
    if (newSpec.value.length >= 3) {
      message("最多可添加3个规格", {
        type: "info"
      });
      inputValue.value = "";
      return false;
    }
    //newSpec.push({ name: inputValue, value: [], inputVisible: false });
    newSpec.value.push({
      k: inputValue.value,
      v: [],
      k_s: "s" + (newSpec.value.length + 1),
      inputVisible: false
    });
    inputValue.value = "";
    handleSkus();
  }
  if (inputValue.value) {
    inputVisible.value = false;
    inputValue.value = "";
    showInput();
  } else {
    inputVisible.value = false;
  }
}
function handleValueInputConfirm(s: any) {
  const inputValue = s.inputValue;
  let flag = false;
  if (inputValue) {
    s.v.forEach(function (value: { name: any }) {
      if (value.name == inputValue) flag = true;
    });
    if (flag) {
      message("当前规格的值(" + inputValue + ")已存在，不能重复设置", {
        type: "info"
      });
      s.inputValue = "";
      return false;
    }
    const sitem = {
      name: inputValue,
      id: s.k_s + "000" + (s.v.length + 1),
      imgUrl: "",
      closable: true
    };
    s.v.push(sitem);
    handleSkus();
  }

  if (s.inputValue) {
    s.inputVisible = false;
    s.inputValue = "";
    showValueInput(s);
  } else {
    s.inputVisible = false;
  }
}
function handleChange(value: any) {
  // item.value.catId = value[0];
}
function handleChangePrice() {
  let price = item.value.skus.list[0].price;
  item.value.skus.list.forEach(function (value) {
    if (value.price && value.price != null && !isNaN(value.price)) {
      if (price > value.price) {
        price = value.price;
      }
    }
  });
  item.value.price = price;
}

function handleChangestockNum() {
  let stock = 0;
  item.value.skus.list.forEach(function (value) {
    if (value.stock_num && value.stock_num != null && !isNaN(value.stock_num)) {
      //if (stock > value.stock.inStock) {
      stock += value.stock_num;
      //}
    }
  });

  item.value.stock.inStock = stock;
}
const form = ref<FormInstance>();
const router = useRouter();
const submit = (formEl: FormInstance | undefined) =>
  formEl.validate(async valid => {
    if (!valid) return;
    if (item.value.openSpec) {
      item.value.specialPrices = null;
      if (item.value.skus.list.length < 1) {
        message("规格已开启，请完善规格信息", {
          type: "warning"
        });
        return;
      }
      item.value.price = item.value.skus.list.sort(p => p.price)[0].price;
    }

    if (!item.value.image) {
      message("请上传商品主图", {
        type: "warning"
      });
      return;
    }

    item.value.subImage = "";
    if (subImage1.value) item.value.subImage += subImage1.value + ",";
    if (subImage2.value) item.value.subImage += subImage2.value + ",";
    if (subImage3.value) item.value.subImage += subImage3.value + ",";
    if (subImage4.value) item.value.subImage += subImage4.value + ",";
    if (item.value.subImage)
      item.value.subImage = item.value.subImage.substring(
        0,
        item.value.subImage.length - 1
      );

    await ProductApi.submit(outerType.value, item.value);
    message("保存成功！");
    router.push("/shopmall/product/index");
  });
</script>
<style scoped>
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
.el-table.sku thead tr {
  background-color: #ecebeb;
}

.sku-credit-row:not(:last-child) {
  border-bottom: 1px dashed #ecebeb;
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
  transition: border-color 0.2s cubic-bezier(0.645, 0.045, 0.355, 1);
  width: 100%;
}
</style>
