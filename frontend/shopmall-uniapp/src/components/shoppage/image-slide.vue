<template>
  <view class="u-margin-20">
    <u-swiper
      :list="swiperItems"
      :effect3d="false"
      :title="false"
      bg-color="transparency"
      @click="taped"
    />
    <u-toast ref="uToast" />
  </view>
</template>
<script lang="ts" setup>
import { onMounted, ref } from "vue";
export interface ImgSliderParameter {
  list: Array<any>;
}
export interface ImgSliderProps {
  parameters: ImgSliderParameter;
}
const prop = defineProps<ImgSliderProps>();
const swiperItems = ref([]);

onMounted(() => {
  const data = prop.parameters.list;
  for (let i = 0; i < data.length; i++) {
    const moder = {
      image:
        data[i].image == "/src/assets/images/empty-banner.png"
          ? "/static/images/common/empty-banner.png"
          : data[i].image,
      opentype: "click",
      url: "",
      title: data[i].linkType,
      linkType: data[i].linkType,
      linkValue: data[i].linkValue
    };
    swiperItems.value.push(moder);
  }
});
function taped(e: number) {
  showSliderInfo(swiperItems[e].linkType, swiperItems[e].linkValue);
}
const showSliderInfo = (type, val) => {
  if (!val) {
    return;
  }
  if (type == 1) {
    if (val.indexOf("http") != -1) {
      // #ifdef H5
      window.location.href = val;
      // #endif
    } else {
      // #ifdef H5 || APP-PLUS || APP-PLUS-NVUE || MP
      if (
        val == "/pages/index/default/default" ||
        val == "/pages/category/index/index" ||
        val == "/pages/index/cart/cart" ||
        val == "/pages/index/member/member"
      ) {
        // $u.route({ type: "switchTab", url: val });
        return;
      } else if (val.indexOf("/pages/coupon/coupon") > -1) {
        const id = val.replace("/pages/coupon/coupon?id=", "");
        receiveCoupon(id);
      } else {
        // $u.route(val);
        return;
      }
      // #endif
    }
  } else if (type == 2) {
    //goGoodsDetail(val);
  } else if (type == 3) {
    // $u.route("/pages/article/details/details?id=" + val + "&idType=1");
  } else if (type == 4) {
    // $u.route("/pages/article/list/list?cid=" + val);
  } else if (type == 5) {
    // $u.route("/pages/form/details/details?id=" + val);
  }
};
const receiveCoupon = couponId => {
  let data = {
    promotion_id: couponId
  };
  //   api.getCoupon(data).then(res => {
  //     if (res.status) {
  //       this.$refs.uToast.show({ title: res.msg, type: "success", back: false });
  //     } else {
  //       this.$u.toast(res.msg);
  //     }
  //   });
};
</script>
<style></style>
