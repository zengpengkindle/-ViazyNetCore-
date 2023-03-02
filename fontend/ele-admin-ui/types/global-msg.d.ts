import { App, Ref, ComponentPublicInstance } from 'vue';
import { LocaleManger } from 'locale';
import { NotificationHandle } from 'element-plus';
declare type MessageType = '' | 'success' | 'warning' | 'info' | 'error';

interface GlobalMsg {
    confirm(message?: any): Promise<boolean>,
    alert(message?: any, title?: string, type?: MessageType): Promise,
    success(message?: any): Promise;
    info(message?: any): Promise;
    warning(message?: any): Promise,
    error(message?: any): Promise,
    loading(message?: string, runAfterMS?: number): void,
    loadingClose(),
    prompt(message?: any, value?: string): Promise<string?>
}

interface GlobalFilters {
    number(val: string | number, fixed?: number | string): string;
    date(val: any, format?: 'now' | string): string;
    zone(val: any, timeOffset: string, format?: string): string;
    calendar(val: any, format: string): string;
}

interface GlobalNotify {
    alert(message: string, title?: string): NotificationHandle,
    success(message: string, title?: string): NotificationHandle,
    info(message: string, title?: string): NotificationHandle,
    warning(message: string, title?: string): NotificationHandle,
    error(message: string, title?: string): NotificationHandle,
}

declare global {
    declare const __API_HOST: string | undefined;

    declare type ApiCallback<T extends IResult | void> = (this: Vue) => Promise<T> | string;
    declare type JS_TYPE = 'undefined' | 'number' | 'bigint' | 'string' | 'boolean' | 'function' | 'regexp' | 'array' | 'date' | 'error' | 'symbol' | 'object' | 'null';

    declare type ColorType = '' | 'success' | 'info' | 'danger' | 'warning';
    declare interface LoadingComponentPublicInstance extends ComponentPublicInstance {
        loading: boolean;
    };
    declare var msg: GlobalMsg;
    declare var notify: GlobalNotify;
    declare var filters: GlobalFilters;
    declare var $APP: App<any>;

    /**
     * 测试指定的结果是否成功。
     * @param result 结果。
     */
    declare function test<TValue>(result: IResult<TValue>): boolean;

    declare interface IResult<TValue = any> {
        code?: string,
        message?: string,
        value: TValue,
    }

    declare interface TableComponent<TValue = any> extends ComponentPublicInstance {
        doLayout(): void;
        refresh(current?: number, size?: number): void;
        forceRefresh(current?: number, size?: number): void;
        tableData: TValue[];
    }

    declare function typeName(o: any): JS_TYPE;
    declare function copyDeepTo(from: any, to: any): any;
    declare interface Window {
        plus: any;
    }

    declare type DateRangeCode = number[]
        | 't' | 'today'
        | 'y' | 'yesterday'
        | 'w' | 'week' | 'prev-week' | 'last-week'
        | 'm' | 'month' | 'prev-month' | 'last-month' | 'last-month3';

    declare interface DateConstructor {
        today(): Date;
        defaultTimes: string[];
        range(code: DateRangeCode, defaultTimes?: string[] | boolean): Date[];
    }

    declare interface Date {
        clearTime(): Date;
        lastWeek(weeks?: number);
        lastMonth(months?: number);
        lastYear(years?: number);
        thisWeek(weeks?: number);
        thisMonth(months?: number);
        addDate(days: number);
    }

    declare interface String {
        format(...args: any[]): string;
        getDataLength(): number;
        hashCode(): number;
        padStart(targetLength: number, padString?: string);
        padEnd(targetLength: number, padString?: string);
    }

    declare interface JSON {
        serialize(obj: any, prefix?: string): string;
    }
}

declare module '@vue/runtime-core' {
    export interface ComponentCustomProperties {
        $filters: GlobalFilters;
        $ref<TElement>(name: string): TElement,
        $valid(fc: Function, onFail?: Function): void,
        $validForm(name: string, fc: Function, onFail?: Function): void,
        // $run<T extends IResult | void>(fc: ApiCallback<T>, loadingText?: string): boolean,

        // $submit<T extends IResult | void>(fc: ApiCallback<T>): void,
        // $submitConfirm<T extends IResult | void>(fc: ApiCallback<T>, op?: string): void,
        // $submitRemove<T extends IResult | void>(fc: ApiCallback<T>): void,
        // $submit<T extends IResult | void>(confirmTip: string, fc: ApiCallback<T>): void,

        $copyText(text: string): void;

        $n0(val: string | number): string;
        $n6(val: string | number): string;
        $number(val: string | number, fixed?: number | string): string;
        $date(val: any, format?: 'now' | string): string;
        $copyText(val: string): void;
    }
}