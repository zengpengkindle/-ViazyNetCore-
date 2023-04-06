import { ref, type Ref } from "vue";
import type { SkuModel } from "@/apis/shopmall/product";
const spec: Ref<SkuModel> = ref({
  id: "",
  s1: "",
  key1: "",
  name1: "",
  s2: "",
  key2: "",
  name2: "",
  s3: "",
  key3: "",
  name3: "",
  cost: 0,
  price: 0,
  stock_num: 0,
  specialPrices: undefined
});
const buyNum = ref(1);
const specType = ref("");
const showSpecPopup = ref(false);
export function useProductSpec() {
  return {
    spec,
    buyNum,
    showSpecPopup,
    specType
  };
}
