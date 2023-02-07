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
import { defineAsyncComponent} from 'vue'
import KUtils from "@/core/utils";
import swaggerBuilder from '@/core/swaggerGenerator'

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
  async created() {
    console.log('paramters',this.api.parameters);
    let swaggerDoc=await swaggerBuilder({
      SwaggerUrl:KUtils.json5stringify(this.api.openApiRaw)
    })
    this.tsCode=swaggerDoc;
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
