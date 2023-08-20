<template>
  <div class="main">
    <div class="form-render">
      <VFormRender
        ref="preForm"
        :form-json="formJson"
        :form-data="formData"
        :preview-state="true"
      />
      <div class="footer">
        <el-button type="primary" @click="doTest">保存</el-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import VFormRender from "@/components/form-render/index.vue";
import { ref } from "vue";
import { useRoute } from "vue-router";
import FormTemplateApi from "@/api/formTemplate";
import { onMounted } from "vue";
import { deepClone, getDefaultFormConfig } from "@/utils/util";

defineOptions({
  name: "render"
});
const route = useRoute();
const preForm = ref<typeof VFormRender>();
const doTest = async () => {
  const fieldList = await preForm.value.getFormData();
  msg.alert(JSON.stringify(fieldList));
};

const formData = ref({
  // 'userName': '666888',
  // 'productItems': [
  //   {'pName': 'iPhone12', 'pNum': 10},
  //   {'pName': 'P50', 'pNum': 16},
  // ]

  select62173: 2
});

const defaultFormConfig = deepClone(getDefaultFormConfig());

const formJson = ref({
  widgetList: [],
  formConfig: deepClone(defaultFormConfig)
});
onMounted(async () => {
  const formId = parseInt(route.query.formId as string);
  const fields = await FormTemplateApi.getFields(formId);
  formJson.value.widgetList = fields;
});
</script>

<style lang="scss" scoped>
.form-render {
  margin: 0 auto;
  padding: 10px;
  border-radius: 15px;
  background-color: #fff;
}
</style>
