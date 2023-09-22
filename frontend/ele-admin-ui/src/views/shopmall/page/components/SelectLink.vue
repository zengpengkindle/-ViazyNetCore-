<script lang="ts" setup>
import { onMounted, ref, watch } from "vue";
export interface Props {
  modelValue?: number;
  type: any;
}
const props = defineProps<Props>();

const emits = defineEmits(["update:type", "update:modelValue", "choose-link"]);
const linkType = [" ", "URL链接", "商品", "文章", "文章分类"];
const articleTypeList: Array<any> = [];

const selectType = ref(props.type);
const typeValue = ref(props.modelValue);
watch(
  () => props.modelValue,
  () => {
    typeValue.value = props.modelValue;
  }
);
watch(
  () => props.type,
  () => {
    selectType.value = props.type;
  }
);
const updateLinkValue = () => {
  emits("update:modelValue", typeValue.value);
};
const changeSelect = () => {
  emits("update:type", selectType);
  // emits("update:modelValue", "");
};
const selectLink = () => {
  emits("choose-link");
};
const updateSelect = () => {
  emits("update:modelValue", typeValue.value);
};
onMounted(() => {
  selectType.value = props.type;
  typeValue.value = props.modelValue;
});
</script>
<template>
  <div>
    <el-form-item label="链接类型：">
      <el-select
        v-model="selectType"
        placeholder="请选择"
        @change="changeSelect"
      >
        <el-option
          v-for="(val, key, i) in linkType"
          :key="i"
          :label="val"
          :value="key"
        />
      </el-select>
    </el-form-item>
    <el-form-item label="链接指向：">
      <div v-if="selectType == 1">
        <el-input
          type="textarea"
          autosize
          placeholder="http开头为webview跳转，其他为站内页面跳转"
          v-model="typeValue"
          @change="updateLinkValue"
        />
      </div>
      <div v-if="selectType == 2">
        <el-input
          type="text"
          v-model="typeValue"
          class="selectLinkVal"
          :readonly="true"
          @click="selectLink"
          placeholder="请选择"
        />
      </div>
      <div v-if="selectType == 3">
        <el-input
          type="text"
          v-model="typeValue"
          class="selectLinkVal"
          :readonly="true"
          @click="selectLink"
          placeholder="请选择"
        />
      </div>
      <div v-if="selectType == 4">
        <el-select
          v-model="typeValue"
          placeholder="请选择分类"
          class="updateSelect"
          @change="updateSelect"
        >
          <el-option
            :value="item.id"
            :label="item.name"
            v-for="item in articleTypeList"
            v-bind:key="item.id"
          />
        </el-select>
      </div>
      <div v-if="selectType == 5">
        <el-input
          type="text"
          v-model="typeValue"
          class="selectLinkVal"
          :readonly="true"
          @click="selectLink"
          placeholder="请选择"
        />
      </div>
    </el-form-item>
  </div>
</template>
<style lang="scss">
.layout-list .layout-main .btn-clone {
  position: absolute;
  height: 18px;
  line-height: 18px;
  right: 50px;
  bottom: 2px;
  z-index: 90;
  width: 36px;
  text-align: center;
  font-size: 10px;
  color: #fff;
  background: #409eff;
  cursor: pointer;
  z-index: 1300;
}
</style>
