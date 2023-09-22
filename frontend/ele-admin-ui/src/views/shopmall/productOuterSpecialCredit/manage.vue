<template title="类别/活动管理">
  <el-card v-loading="loading">
    <el-form
      ref="form"
      :model="item"
      :rules="rules"
      :validate-on-rule-change="false"
    >
      <el-form-item label="价格类型标识" prop="objectKey">
        <el-input v-model="item.objectKey" v-if="state" :disabled="true" />
        <el-input v-model="item.objectKey" v-else />
      </el-form-item>
      <el-form-item label="价格类型名称" prop="objectName">
        <el-input v-model="item.objectName" />
      </el-form-item>
      <el-form-item label="类别/活动" prop="outerType">
        <el-input v-model="item.outerType" :disabled="true" />
      </el-form-item>
      <el-form-item label="货币类型">
        <el-select v-model="item.creditKey" placeholder="请选择">
          <el-option
            v-for="opt in credits"
            :key="opt.value"
            :label="opt.label"
            :value="opt.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="价格计算方式">
        <el-select v-model="item.computeType" placeholder="请选择">
          <el-option
            v-for="opt in computeTypes"
            :key="opt.value"
            :label="opt.label"
            :value="opt.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="固定手续费" v-if="item.computeType == 2">
        <el-input-number v-model="item.feeMoney" :precision="2" :step="1" />
      </el-form-item>
      <el-form-item label="百分比手续费" v-if="item.computeType == 3">
        <el-input-number
          v-model="item.feePercent"
          :precision="3"
          :step="0.01"
          :min="0"
          :max="1"
        />
      </el-form-item>
      <el-form-item label="备注" prop="exdata">
        <el-input v-model="item.exdata" type="textarea" :rows="3" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submit(form)">提交</el-button>
        <el-button @click="$router.back()">返回</el-button>
      </el-form-item>
    </el-form>
  </el-card>
</template>
<script lang="ts" setup>
import ProductOuterSpecialCreditApi, {
  ProductOuterSpecialCredit
} from "@/api/shopmall/productOuterSpecialCredit";
import CreditApi from "@/api/shopmall/credit";
import { FormInstance, FormRules } from "element-plus";
import { onMounted, reactive, Ref, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
const form = ref<FormInstance>();
const rules = reactive<FormRules>({
  objectKey: [
    { required: true, message: "请输入价格类型标识" },
    { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
  ],
  objectName: [
    { required: true, message: "请输入价格类型名称" },
    { min: 1, max: 20, message: "长度在 1 到 20 个字符" }
  ]
});
const route = useRoute();
const router = useRouter();
const computeTypes = reactive([
  { value: 0, label: "独立价格" },
  { value: 1, label: "与商品设置价格等价" },
  { value: 2, label: "与商品等价但计算兑换手续费-固定" },
  { value: 3, label: "计算百分比手续费" },
  { value: 4, label: "混合价格" },
  { value: 5, label: "条件式" }
]);
const state = ref(false);
const outerType = ref("");
const loading = ref(true);
const item: Ref<ProductOuterSpecialCredit> = ref({});
const credits = ref([]);
onMounted(() => {
  init();
});
const init = async () => {
  state.value = route.query && route.query.id ? true : false;
  outerType.value = route.query && (route.query.outerType as string);
  loading.value = state.value;
  const list = await CreditApi.getAll();
  credits.value = Object.keys(list).map(function (listCode) {
    return {
      value: listCode,
      label: list[listCode]
    };
  });
  if (state.value) {
    const outerId = route.query.id as string;
    const outerTypes =
      await ProductOuterSpecialCreditApi.getSpecialCreditByOuterKey(
        outerType.value
      );
    item.value = await ProductOuterSpecialCreditApi.get(outerId);
    outerTypes.find(p => p.key == item.value.outerType);
    loading.value = false;
  } else item.value.outerType = outerType.value;
};
function submit(form: FormInstance) {
  form.validate(async valid => {
    if (!valid) return;
    await ProductOuterSpecialCreditApi.manger(item.value);

    msg.info("保存成功！");
    router.back();
  });
}
</script>
