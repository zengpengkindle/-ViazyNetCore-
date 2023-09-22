import AddressApi, { type AddressModel } from "@/apis/shopmall/address";
import { ref, watch, type Ref } from "vue";
const addressList: Ref<Array<AddressModel>> = ref([]);
const selectAddress: Ref<AddressModel> = ref();
const init = async () => {
  addressList.value = await AddressApi.findAddress();
  if (addressList.value.length > 0) selectAddress.value = addressList.value[0];
};
const refreshAddress = async () => {
  await init();
};
const SelectAddress = (select: AddressModel) => {
  selectAddress.value = select;
};
watch(
  () => addressList.value,
  nval => {
    if (nval.length > 0) selectAddress.value = nval[0];
    else selectAddress.value = null;
  }
);
export function useAddress() {
  return { addressList, selectAddress, refreshAddress, SelectAddress };
}
