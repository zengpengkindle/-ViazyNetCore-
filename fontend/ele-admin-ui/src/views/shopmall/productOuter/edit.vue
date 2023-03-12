<script lang="ts" setup>
import { ref, Ref, reactive, watch } from "vue";
import ProductOuterApi, { ProductOuter } from "@/api/shopmall/productOuter";
import { ComStatus } from "@/api/model";
import { FormInstance, FormRules } from "element-plus";
import { message } from "@/utils/message";
export interface Props {
    modelValue: boolean;
    readonly id: string | null;
}

const props = defineProps<Props>();
watch(
    () => props.modelValue,
    value => {
        visible.value = value;
        if (value) {
            isEdit.value = false;
        }
    }
);
const item: Ref<ProductOuter> = ref({
    outerName: "",
    outerType: "",
    status: ComStatus.Enabled,
    description: "",
    beginTime: "",
    endTime: ""
});
const rules = reactive<FormRules>({
    outerName: [
        { required: true, message: '请输入类别/活动' },
        { min: 1, max: 50, message: '长度在 1 到 50 个字符' }
    ],
    outerType: [
        { required: true, message: '请输入类别/活动标识' },
        { min: 1, max: 20, message: '长度在 1 到 20 个字符' }
    ]
});

const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);

const formRef = ref<FormInstance>();
const createTimes = reactive([]);
const submit = (formEl: FormInstance | undefined) => {
    if (!formEl) return;
    if (createTimes && createTimes.length == 2) {
        item.value.beginTime = createTimes[0];
        item.value.endTime = createTimes[1];
    } else {
        item.value.beginTime = null;
        item.value.endTime = null;
    }
    formEl.validate(async valid => {
        if (valid) {
            await ProductOuterApi.apiProductOuterMangerProductOuter(item.value);
            message("新增成功", { type: "success" });
            emit("refresh");
            handleClose(() => { });
        } else {
            console.log("error submit!");
            return false;
        }
    });
}
const handleClose = (done: () => void) => {
    emit("update:modelValue", false);
    done();
};          
</script>
<template title="货币类型管理">
    <el-drawer v-model="visible" size="35%" :title="id ? '编辑货币类型' : '新增货币类型'" :before-close="handleClose">
        <el-form ref="formRef" :model="item" :rules="rules" @submit.native.prevent :validate-on-rule-change="false">
            <el-form-item label="类别/活动" prop="outerName">
                <el-input v-model="item.outerName"></el-input>
            </el-form-item>
            <el-form-item label="类别/活动标识" prop="outerType">
                <el-input v-model="item.outerType" v-if="isEdit" :disabled="true"></el-input>
                <el-input v-model="item.outerType" v-else></el-input>
            </el-form-item>
            <el-date-picker model-value="createTimes" type="daterange" :shortcuts="$pickerOptions.shortcuts"
                range-separator="至" start-placeholder="开始日期" end-placeholder="结束日期" align="right">
            </el-date-picker>
            <el-form-item>
                <el-button type="primary" @click="submit(formRef)">提交</el-button>
                <el-button @click="$router.back()">返回</el-button>
            </el-form-item>
        </el-form>
    </el-drawer></template>