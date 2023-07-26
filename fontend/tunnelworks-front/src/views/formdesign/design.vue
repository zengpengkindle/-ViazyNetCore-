<template>
  <div>
    <VFormDesigner ref="vfDesignerRef" :global-dsv="globalDsv">
      <!--
        <template #customToolButtons>
          <el-button type="text" @click="doTest">测试btn</el-button>
        </template>
        -->
    </VFormDesigner>
  </div>
</template>

<script lang="ts" setup>
import VFormDesigner from "@/components/form-designer/index.vue";
import { reactive } from "vue";
defineOptions({
  name: "designer"
});
const globalDsv = reactive({
  testApiHost: "http://www.test.com/api",
  testPort: 8080
});

// const vfDesignerRef = ref<VFormDesigner>();
// const doTest = () => {
//   const fieldList = vfDesignerRef.getFieldWidgets(null, true);
//   console.log("test", fieldList);
// };
</script>

<style lang="scss">
#app {
  height: 100%;
}
</style>
