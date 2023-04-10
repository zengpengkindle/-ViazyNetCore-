import { ref, watch } from "vue";
import type { Ref } from "vue";

export function useStorage(key: string, initialValue: string): Ref<string>;
export function useStorage(key: string, initialValue: number): Ref<number>;
export function useStorage(key: string, initialValue: boolean): Ref<boolean>;
export function useStorage(key: string, initialValue: any): Ref<any>;
export function useStorage<T = any>(key: string, initialValue: T) {
  const value = ref(initialValue) as Ref<T>;
  value.value = uni.getStorageSync(key) as T;

  watch(value, val => {
    uni.setStorageSync(key, val);
  });

  return value;
}
