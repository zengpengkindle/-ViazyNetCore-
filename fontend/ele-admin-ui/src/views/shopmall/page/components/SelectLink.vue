<script lang="ts" setup>
import { ref } from 'vue';

const linkType: Array<any> = ([]);
const articleTypeList: Array<any> = ([]);

const selectType = ref(0);
const id=ref(1);
const linkUrl=ref('');
const updateLinkValue = () => {

}
const changeSelect=()=>{

}
const selectLink=()=>{

}
const updateSelect=()=>{

}
</script>
<template>
    <div>
        <el-form-item label="链接类型：">
            <el-select v-model="selectType" placeholder="请选择" @change="changeSelect">
                <el-option v-for="(val, key, i) in linkType" :key="i" :label="val" :value="key">
                </el-option>
            </el-select>
        </el-form-item>
        <el-form-item label="链接指向：">
            <div v-if="selectType == 1">
                <el-input type="textarea" autosize placeholder="http开头为webview跳转，其他为站内页面跳转" v-model="linkUrl"
                    @change="updateLinkValue"></el-input>
            </div>
            <div v-if="selectType == 2">
                <input type="text" v-model="id" class="selectLinkVal" :readonly="true" @click="selectLink"
                    placeholder="请选择">
            </div>
            <div v-if="selectType == 3">
                <input type="text" v-model="id" class="selectLinkVal" :readonly="true" @click="selectLink"
                    placeholder="请选择">
            </div>
            <div v-if="selectType == 4">
                <el-select v-model="id" placeholder="请选择分类" class="updateSelect" @change="updateSelect">
                    <template v-for="item in articleTypeList">
                        <el-option :value="item.id" :label="item.name"></el-option>
                    </template>
                </el-select>
            </div>
            <div v-if="selectType == 5">
                <input type="text" v-model="id" class="selectLinkVal" :readonly="true" @click="selectLink"
                    placeholder="请选择">
            </div>
        </el-form-item>
</div></template>