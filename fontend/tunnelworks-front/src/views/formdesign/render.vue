<template>
  <div>
    <VFormRender
      ref="preForm"
      :form-json="formJson"
      :form-data="formData"
      :preview-state="true"
    >
      <template #customToolButtons>
        <el-button type="primary" @click="doTest">保存</el-button>
      </template>
    </VFormRender>
  </div>
</template>

<script lang="ts" setup>
import VFormRender from "@/components/form-render/index.vue";
import { ref } from "vue";
import { useRoute } from "vue-router";
import FormTemplateApi from "@/api/formTemplate";
import { onMounted } from "vue";
defineOptions({
  name: "render"
});
const route = useRoute();
const preForm = ref<typeof VFormRender>();
const doTest = async () => {
  const fieldList = await preForm.value.getFormData();

  console.log(fieldList);
};

const formData = ref({
  // 'userName': '666888',
  // 'productItems': [
  //   {'pName': 'iPhone12', 'pNum': 10},
  //   {'pName': 'P50', 'pNum': 16},
  // ]

  select62173: 2
});

const formJson = ref({
  widgetList: []
});
onMounted(async () => {
  const formId = parseInt(route.query.formId as string);
  const fields = await FormTemplateApi.getFields(formId);
  formJson.value.widgetList = fields;
});
</script>

<style lang="scss">
#app {
  height: 100%;
}
</style>
