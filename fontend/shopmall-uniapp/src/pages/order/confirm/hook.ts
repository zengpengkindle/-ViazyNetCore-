import { ref, type Ref } from "vue";
import type { CreateTradeSetModel } from "@/apis/shopmall/trade";
const tradeSet: Ref<CreateTradeSetModel> = ref();
export function useTradeCash() {
  return {
    tradeSet
  };
}
