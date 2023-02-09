<template>
    <div class="x-table">
        <div class="x-table-filters" shadow="never">
            <el-form ref="filter" :label-position="labelPosition" :inline="inline" @submit.native.prevent>
                <slot name="form" />
                <el-form-item>
                    <slot name="search">
                        <el-button type="primary" native-type="submit" :loading="loading">
                            <icon name="search" v-if="!loading" />搜索
                        </el-button>
                    </slot>
                    <el-button-group>
                        <slot name="buttons" />
                    </el-button-group>
                </el-form-item>
            </el-form>
        </div>
        <div class="x-table-op">
            <slot name="op" />
        </div>
        <div class="x-table-body">
            <el-table :data='tableData' :border="border" ref="table" v-loading="loading" style="max-width:100%"
                filter-ref="filter" :default-sort="sortData" :show-summary="isShowSummary"
                :summary-method="innerGetSummaries" @sort-change="sortChange" @selection-change="selectionChange">
                <slot />
                <template #append>
                    <slot name="footer" />
                </template>
            </el-table>
            <div style="height:12px"></div>
            <el-pagination background ref="pagination" @size-change="onRefresh" @current-change="onRefresh"
                v-model:current-page="lastParams.pageNumber" v-model:page-size="lastParams.pageSize"
                :page-sizes="pageSizes" :layout="pageLayout" :total="total">
            </el-pagination>
        </div>
    </div>
</template>

<script lang="ts" setup>

import { ref, reactive, computed, onMounted, onActivated, nextTick, getCurrentInstance, defineExpose } from 'vue'
import type { Ref } from 'vue'
import { ElTable, ElRow } from 'element-plus';
import type { TableColumnCtx } from 'element-plus/es/components/table/src/table-column/defaults'
import { http } from '@/utils/http';

export interface Props {
    readonly url: string | Function,
    readonly border?: boolean,
    readonly showSummary?: boolean,
    autoSummary?: string | Array<string>,
    summaryFixed?: number | string,
    readonly summaryMethod?: Function,
    readonly parseNumber?: (item: any, prop: string, raw: any) => number,
    readonly params: Record<string, any>,
    readonly size?: number,
    readonly dataFilter?: Function,
    readonly defaultSort?: string,
    readonly defaultSortAsc?: boolean,
    readonly pageSizes?: Array<number>
}
const props = withDefaults(defineProps<Props>(), {
    border: true,
    defaultSort: null,
    showSummary: false,
    defaultSortAsc: false,
    size: 10,
    pageSizes: () => [10, 20, 50, 100]
})

interface SummaryMethodProps {
    columns: TableColumnCtx<any>[]
    data: any[]
}
const FULL_PAGE_LATYOU = 'total, sizes, prev, pager, next, jumper';
const _this: any = getCurrentInstance();
//expose: string[] = ['forceRefresh', 'refresh', 'doLayout', 'tableData'];

const loading = ref(false);
const show = ref(false);
const tableData = ref([]);

const total = ref(0);
const inline = ref(true);
const labelPosition: Ref<"left" | "right" | "top"> = ref('right');
const pageLayout = ref(FULL_PAGE_LATYOU);

interface LastParams {
    pageNumber?: number
    pageSize?: number,
    prop?: string,
    order?: string
}
const lastParams: LastParams = reactive({ pageNumber: 0, pageSize: 0 });
const lastPage = reactive({
    number: 0,
    size: 0,
    lastProp: null,
    lastOrder: null,
});
interface SortData {
    prop: string,
    order: 'ascending' | 'descending',
    init?: any,
    silent?: any
}
var sortData: SortData = reactive({prop:'id',order:'descending'});
const filter = ref(null);
const table: typeof ElTable = ref(null)

const autoSummaryColumns = computed(() => {
    if (!props.autoSummary) return [];
    if (typeof props.autoSummary === 'string') return props.autoSummary.split(',');
    return props.autoSummary;
})

const isShowSummary = computed(() => {
    return autoSummaryColumns.value.length > 0 || props.showSummary;
})

function adjustColumnWidth(table: HTMLElement) {
    const colgroup = table.querySelector("colgroup");
    if (!colgroup) return;
    const colDefs = colgroup.querySelectorAll("col");
    colDefs.forEach((col) => {
        const clsName = col.getAttribute("name");
        const cells = [
            ...Array.from(table.querySelectorAll(`td.${clsName}`)),
            ...Array.from(table.querySelectorAll(`th.${clsName}`)),
        ];

        if (cells[0]?.classList?.contains?.("leave-alone")) {
            return;
        }
        const widthList = cells.map((el) => {
            return el.querySelector(".cell")?.scrollWidth || 0;
        });
        const max = Math.max(...widthList);
        const padding = 32;
        table.querySelectorAll(`col[name=${clsName}]`).forEach((el) => {
            el.setAttribute("width", (max + padding).toString());
        });
    });
}

function getNumber(item, prop: string) {
    const raw = item;
    const _props = prop.split('.');
    for (let p of _props) {
        item = item[p];
    }
    if (typeof item === 'number') return item;
    if (props.parseNumber) return props.parseNumber(item, prop, raw);
    return 0;
}

function innerGetSummaries(param: SummaryMethodProps) {
    if (props.summaryMethod) return props.summaryMethod(param);

    const sums: Array<string | number> = [];
    const data = param.data;
    if (data.length) {
        const summaryColumns = autoSummaryColumns.value;
        if (summaryColumns.length) {
            let firstIndex = 0;
            for (let col of param.columns) {
                if (sums.length == firstIndex) {
                    if (col.type == "selection" || col.type == "expand") {
                        firstIndex++;

                    } else {
                        sums.push(_this.$dict.total);
                        continue;
                    }
                }
                if (summaryColumns.indexOf(col.property) > -1) {
                    const numbers = data.map(r => getNumber(r, col.property));
                    if (numbers.length) {
                        let totalValue = 0;
                        numbers.forEach(function(value) {totalValue += value;})
                        if (totalValue > 0) {
                            sums.push(totalValue.toFixed(+props.summaryFixed));
                            continue;
                        }
                    }
                }
                sums.push('');
            }
        }
    }
    return sums;
}

function resize() {
    inline.value = true;
    labelPosition.value = true ? 'right' : 'top';
    pageLayout.value = true ? FULL_PAGE_LATYOU : 'prev, pager, next';
    doLayout();
}

onMounted(() => {
    let _prop = props.defaultSort;
    let order:'ascending'|'descending' = props.defaultSortAsc ? 'ascending' : 'descending';
    if (!_prop && props.params && props.params.prop) _prop = props.params.prop;
    if (props.params && props.params.order) order = props.params.order;

    sortData = _prop ? { prop:_prop, order } : sortData;
    Object.assign(lastParams, getParams());
    resize();
    nextTick(() => {
        (filter.value.$el as HTMLFormElement).addEventListener('submit', e => {
            e.preventDefault();
            forceRefresh(1);
            return false;
        });
        refresh();
    });
})

onActivated(() => {
    refresh();
})

function beforeUnmount() {
}

function doLayout() {
    if (table) table.value.doLayout();
}

function onRefresh(force) {
    if (loading.value) return;
    if (!props.url) {
        // msg.error("无效的请求地址");
        return;
    }

    if (force !== true
        && lastPage.number == lastParams.pageNumber
        && lastPage.size == lastParams.pageSize) return;

    loading.value = true;
    const fc = typeof props.url === 'function' ? props.url(lastParams) : apiManager.call(props.url, lastParams);
    fc.then(resp => {
        if (test(resp)) {
            lastPage.number = lastParams.pageNumber;
            lastPage.size = lastParams.pageSize;

            const rows = resp.value.rows;
            _this.emit('data-filter', rows);
            _this.emit('refresh', resp.value);
            tableData.value = rows;
            total.value = +resp.value.total;

        }

        nextTick(() => loading.value = false);
    });
}

function sortChange(obj) {
    if (!obj) return;
    lastParams.prop = obj.order ? obj.prop : null;
    lastParams.order = obj.order;
    props.params["prop"] = lastParams.prop;
    lastParams.order = lastParams.order;
    onRefresh(true);
}

function getParams() {
    let ps = Object.assign({}, props.params);
    if (!ps.pageNumber) ps.pageNumber = lastPage.number || 1;
    if (!ps.pageSize) ps.pageSize = lastPage.size || props.size;
    return ps;
}

function forceRefresh(current, size = null) {
    Object.assign(lastParams, getParams());
    refresh(current, size);
}

function refresh(current = null, size = null) {
    if (current)
        lastParams.pageNumber = current;
    if (size)
        lastParams.pageSize = size;
    onRefresh(true);
}

function selectionChange(rows) {
    _this.emit('update:selectedRows', rows);
    _this.emit('selection-change', rows);
}

defineExpose({
    forceRefresh,
    refresh,
    doLayout,
    tableData
})
</script>
<style scoped>
.x-table-filters>.el-form--label-top>.el-form-item {
    margin-bottom: 0;
}

.x-table-filters>.el-form--label-top>.el-form-item>.el-form-item__content>span>.el-select {
    width: 100%;
}

.x-table-filters>.el-form--label-top>.el-form-item:last-child>.el-form-item__content {
    padding: 22px 0;
}

.x-table-body th {
    background-color: #F5F7FA;
}

.x-table .x-table-op {
    padding: 14px;
    background-color: #F5F7FA;
    border-left: 1px solid #EBEEF5;
    border-top: 1px solid #EBEEF5;
    border-right: 1px solid #EBEEF5;
}

.x-table .x-table-op:empty {
    padding: 0;
    height: 0;
    border: 0;
    margin: 0;
}
</style>
