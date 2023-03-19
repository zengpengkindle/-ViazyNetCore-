<script lang="ts" setup>
import { computed } from '@vue/reactivity';
import { onMounted, ref } from 'vue';
export interface Props {
    id?: number;
    type: any;
}
const props = defineProps<Props>();

const emits = defineEmits(['update:type', 'update:id', 'choose-link']);
const linkType = [' ', 'URL链接','商品','文章','文章分类' ];
const articleTypeList: Array<any> = ([]);

const selectType = ref(0);
const id = ref(1);
const linkUrl = ref('');
const updateLinkValue = () => {
    emits("update:id", linkUrl.value)
}
const changeSelect = () => {
    emits('update:type', selectType)
    emits("update:id", '')
}
const selectLink = () => {
    emits('choose-link')
}
const updateSelect = () => {
    emits("update:id", id.value)
}
onMounted(() => {
    if (!props.type) {
        emits('update:type', Object.keys(linkType)[0])
    }
});
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

.layout-list .layout-main .btn-delete {
    position: absolute;
    height: 18px;
    line-height: 18px;
    right: 2px;
    bottom: 2px;
    z-index: 90;
    width: 36px;
    text-align: center;
    font-size: 10px;
    color: #fff;
    background: rgba(0, 0, 0, 0.4);
    cursor: pointer;
    z-index: 1300;
}
</style>