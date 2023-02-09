import { defineComponent, h, VNode } from 'vue';
import { ElTableColumn, ElTag } from 'element-plus';
import type { TableColumnCtx } from 'element-plus/es/components/table/src/table-column/defaults'
import shortText from '~/components/ui/short-text.vue';

export const Column = defineComponent({
    name: 'ElTableColumn',
    props: {
        prop: {
            type: String,
        },
        minWidth: {
            type: String,
            default: '100'
        },
        sorted: {
            type: Boolean,
            default: false
        },
        amount: {
            type: [String, Boolean, Number],
            default: false
        },
        time: {
            type: [String, Boolean],
            default: false
        },
        short: {
            type: [String, Boolean, Number],
            default: false
        },
        action: {
            type: Boolean,
            default: false
        },
        bool: {
            type: Boolean,
            default: false
        }
    },
    render() {
        const $props = this.$props;
        const childProps: Record<string, any> = {};
        const slots: Record<string, (props: Record<string, any>) => VNode> = {};
        
        if ($props.sorted) {
            childProps.sortable = 'custom';
        }

        if ($props.time !== false) {
            let time = $props.time === true ? '' : $props.time;
            childProps.minWidth = '170';

            childProps.formatter = function (row, column: TableColumnCtx<any>) {
                //return window.filters.date(row[column.property], time);
            }
        }
        if ($props.amount !== false) {
            let amount = +($props.amount === true || !$props.amount ? '0' : $props.amount);
            childProps.minWidth = (100 + amount * 5).toString();
            childProps.align = 'right';
            childProps.formatter = function (row, column: TableColumnCtx<any>) {
                let val = row[column.property];
                if (!val) return '';
                //return window.filters.number(val, amount);
            }
        }
        if ($props.short !== false) {
            let start = 0, end = 8;
            let short = (($props.short === true || !$props.short ? '8' : $props.short) + '').split(',');
            if (short.length == 1) end = +short[0];
            else if (short.length == 2) {
                start = +short[0];
                end = +short[1];
            }
            slots.default = props => {
                const { row } = props;
                return h(shortText, {
                    text: row[$props.prop!],
                    start: start,
                    end: end
                });
            };
        }

        if ($props.bool !== false) {
            slots.default = props => {
                const { row } = props;
                return h(ElTag, {
                    type: row[$props.prop!] ? 'success' : 'info',
                    effect: 'dark',
                }, () => row[$props.prop!]);
            };
        }
        const columnProps = Object.assign({}, $props, childProps, this.$attrs);
        const colNode = h(ElTableColumn, columnProps, slots.default ? slots : this.$slots);
        return colNode;
    }
});
