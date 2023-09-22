<template>
  <view class="form-address">
    <view class="container">
      <u-form
        class="form-content"
        label-width="140rpx"
        label-align="right"
        :label-style="{ fontSize: '28rpx', alignItems: 'flex-start' }"
      >
        <u-form-item class="form-cell" label="收货人">
          <u-input
            v-model="address.name"
            borderless
            maxlength="20"
            type="text"
            placeholder="您的姓名"
          />
        </u-form-item>
        <u-form-item class="form-cell" label="手机号">
          <u-input
            v-model="address.tel"
            borderless
            type="number"
            maxlength="11"
            placeholder="联系您的手机号"
          />
        </u-form-item>
        <u-form-item class="form-cell" label="地区" right-icon="map">
          <u-input
            v-model="area"
            borderless
            placeholder="省/市/区"
            data-item="address"
            disabled
            @click="showAreaPicker = true"
          />
          <u-picker
            v-model="showAreaPicker"
            mode="region"
            :params="areaParams"
            :default-region="[address.province, address.city, address.county]"
            @confirm="areaPicker"
          />
        </u-form-item>
        <u-form-item
          class="form-cell"
          t-class-title="t-cell-title"
          label="详细地址"
        >
          <view class="textarea__wrapper">
            <u-input
              v-model="address.addressDetail"
              type="textarea"
              placeholder="门牌号等(例如:10栋1001号)"
              auto-height
            />
          </view>
        </u-form-item>
        <u-form-item label="默认" :border-bottom="false">
          <u-switch v-model="address.isDefault" />
          设置为默认收货地址
        </u-form-item>
        <view class="submit">
          <u-button
            ripple
            type="primary"
            shape="circle"
            block
            :disabled="!submitActive"
            @click="saveSubmit"
          >
            保存
          </u-button>
        </view>
      </u-form>
    </view>
  </view>
</template>
<script lang="ts" setup>
import AddressApi, { type AddressModel } from "@/apis/shopmall/address";
import { ref, computed, type Ref } from "vue";
const address: Ref<AddressModel> = ref({
  checked: false,
  id: "",
  province: "",
  city: "",
  county: "",
  addressDetail: "",
  postalCode: "",
  areaCode: "",
  name: "",
  address: "",
  tel: "",
  isDefault: false
});
const areaParams = {
  province: true,
  city: true,
  area: true
};
interface AreaPickerCallback {
  area: {
    name: string;
    code: number;
  };
  city: {
    name: string;
    code: number;
  };
  province: {
    name: string;
    code: number;
  };
}
const area = computed(
  () =>
    (address.value.province || "省") +
    "/" +
    (address.value.city || "市") +
    "/" +
    (address.value.county || "区")
);
const areaPicker = (callback: AreaPickerCallback) => {
  address.value.province = callback.province.name;
  address.value.city = callback.city.name;
  address.value.county = callback.area.name;
};
const submitActive = ref(true);
const showAreaPicker = ref(false);
const saveSubmit = async () => {
  submitActive.value = true;
  await AddressApi.subimtAddress(address.value);
  submitActive.value = false;
  uni.navigateBack();
  uni.showToast({ title: "修改成功" });
};
</script>
<style lang="scss" scoped>
.form-address {
  min-height: 100vh;
  background-color: #f2f2f2;
  box-sizing: border-box;
  overflow: hidden;
  :deep(.u-form-item) {
    padding: 10rpx;
  }
}
.container {
  margin: 20rpx 20rpx 0;
  padding: 20rpx 20rpx;
  border-radius: 20rpx;
  background-color: #ffffff;
  overflow: hidden;
  box-sizing: border-box;
}
</style>
