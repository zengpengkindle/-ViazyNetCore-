<template>
  <div class="generate">
    <!--js脚本示例-->
    <!-- <div>
      <div class="api-title" v-html="$t('script.JSExample')"></div>
      <editor-script :value="jsCode"></editor-script>
    </div> -->
    <!--ts脚本示例-->
    <div>
      <div class="api-title" v-html="$t('script.TSExample')"></div>
      <editor-script :value="tsCode" :tsMode="true"></editor-script>
    </div>
  </div>
</template>
<script>
import { defineAsyncComponent } from 'vue'
import KUtils from "@/core/utils";
import swaggerBuilder, { findSwaggerDependency } from '@/core/swaggerGenerator'
import { useGlobalsStore } from '@/store/modules/global'
import { computed } from 'vue'

export default {
  name: "ScriptView",
  components: {
    "EditorScript": defineAsyncComponent(() => import('./EditorScript.vue'))
  },
  props: {
    api: {
      type: Object,
      required: true
    },
    swaggerInstance: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      jsCode: '',
      tsCode: ''
    };
  },
  setup() {
    const store = useGlobalsStore()
    const swagger = computed(() => {
      return store.swagger
    })
    const swaggerCurrentInstance = computed(() => {
      return store.swaggerCurrentInstance
    })

    return {
      swagger,
      swaggerCurrentInstance
    }
  },
  async created() {
    // 复制一份
    var openApi =KUtils.json5parse( KUtils.json5stringify(this.swaggerCurrentInstance.swaggerData));
    var apiData = KUtils.json5parse(KUtils.json5stringify(this.api.openApiRaw));
    var schemas = findSwaggerDependency(openApi, this.api.openApiRaw.paths);
    apiData.components.schemas=schemas;
    let swaggerDoc = await swaggerBuilder({
      // SwaggerUrl:KUtils.json5stringify(this.api.openApiRaw)
      SwaggerUrl: KUtils.json5stringify(apiData)
    })
    this.tsCode = swaggerDoc;
  },
  methods: {}
};
</script>
<style lang="less" scoped>
.api-title {
  margin-top: 10px;
  margin-bottom: 5px;
  font-size: 16px;
  font-weight: 600;
  height: 30px;
  line-height: 30px;
  border-left: 4px solid #00ab6d;
  text-indent: 8px;
}
</style>
