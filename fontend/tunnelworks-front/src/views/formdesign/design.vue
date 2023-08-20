<template>
  <div>
    <VFormDesigner ref="vfDesignerRef" :widgetListApi="widgetListApi">
      <template #customToolButtons>
        <el-button type="primary" @click="doTest">保存</el-button>
        <el-button type="primary" @click="getContainer">查看容器</el-button>
      </template>
    </VFormDesigner>
  </div>
</template>

<script lang="ts" setup>
import VFormDesigner from "@/components/form-designer/index.vue";
import { reactive, ref } from "vue";
import { useRoute } from "vue-router";
import { message } from "@/utils/message";
import FormTemplateApi from "@/api/formTemplate";
defineOptions({
  name: "designer"
});
// const globalDsv = reactive({
//   testApiHost: "http://www.test.com/api",
//   testPort: 8080
// });
const route = useRoute();
const widgetListApi = reactive({
  URL: "/api/formTemplate/getFields?id=" + route.query.formId,
  method: "get"
});
const vfDesignerRef = ref<typeof VFormDesigner>();
const doTest = async () => {
  const fieldList = vfDesignerRef.value.getFieldWidgets();
  await FormTemplateApi.saveFields(
    parseInt(route.query.formId as string),
    fieldList
  );
  message("保存成功", { type: "success" });
};
const getContainer = async () => {
  const fieldList = vfDesignerRef.value.getContainerWidgets();

  message(JSON.stringify(fieldList), { type: "success" });
};
</script>

<style lang="scss">
#app {
  height: 100%;
}
</style>
