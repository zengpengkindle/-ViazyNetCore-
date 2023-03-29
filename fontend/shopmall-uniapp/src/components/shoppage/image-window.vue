<template>
  <view class="u-margin-bottom-20">
    <u-toast ref="uToast" />
    <u-grid
      v-if="
        parameters.style == 2 || parameters.style == 3 || parameters.style == 4
      "
      :border="false"
      :align="center"
      :col="parameters.style"
    >
      <u-grid-item
        v-for="(item, index) in parameters.list"
        :key="index"
        bg-color="transparent"
        :custom-style="{ padding: parameters.margin + 'rpx' }"
      >
        <u-lazy-load
          :image="item.image"
          :index="index"
          style="width: 100%"
          img-mode="widthFix"
          @click="showSliderInfo(item.linkType, item.linkValue)"
        />
      </u-grid-item>
    </u-grid>
    <u-grid
      v-if="parameters.style == 0"
      :border="false"
      :align="center"
      :col="2"
    >
      <u-grid-item
        bg-color="transparent"
        :custom-style="{ padding: parameters.margin + 'rpx' }"
      >
        <u-lazy-load
          v-if="parameters.list[0]"
          :image="parameters.list[0].image"
          threshold="-150"
          style="width: 100%"
          img-mode="widthFix"
          @click="
            showSliderInfo(
              parameters.list[0].linkType,
              parameters.list[0].linkValue
            )
          "
        />
      </u-grid-item>
      <u-grid-item
        bg-color="transparent"
        :custom-style="{ padding: parameters.margin + 'rpx' }"
      >
        <u-lazy-load
          v-if="parameters.list[1]"
          :image="parameters.list[1].image"
          threshold="-150"
          img-mode="widthFix"
          class="img-window-img-top"
          style="width: 100%; box-sizing: border-box"
          @click="
            showSliderInfo(
              parameters.list[1].linkType,
              parameters.list[1].linkValue
            )
          "
        />
        <u-grid :border="false" :align="center" :col="2">
          <u-grid-item
            bg-color="transparent"
            :custom-style="{ padding: parameters.margin + 'rpx' }"
          >
            <u-lazy-load
              v-if="parameters.list[2]"
              :image="parameters.list[2].image"
              threshold="-150"
              img-mode="widthFix"
              style="width: 100%"
              @click="
                showSliderInfo(
                  parameters.list[2].linkType,
                  parameters.list[2].linkValue
                )
              "
            />
          </u-grid-item>
          <u-grid-item
            bg-color="transparent"
            :custom-style="{ padding: parameters.margin + 'rpx' }"
          >
            <u-lazy-load
              v-if="parameters.list[3]"
              :image="parameters.list[3].image"
              threshold="-150"
              img-mode="widthFix"
              style="width: 100%"
              @click="
                showSliderInfo(
                  parameters.list[3].linkType,
                  parameters.list[3].linkValue
                )
              "
            />
          </u-grid-item>
        </u-grid>
      </u-grid-item>
    </u-grid>
  </view>
</template>
<script lang="ts" setup>
import { ref } from "vue";

export interface ImageWindowParameter {
  style: number;
  margin: number;
  list: Array<any>;
}
export interface ImageWindowProps {
  parameters: ImageWindowParameter;
}
defineProps<ImageWindowProps>();
const center = ref("center");
const showSliderInfo = (type: any, val: any) => {
  console.log(type);
  console.log(val);
};
</script>
<style lang="scss" scoped>
.img-window-img-top {
  position: relative;
  width: 100%;
  height: 0;
  padding-bottom: 50%;
  overflow: hidden;
  :deep(.u-lazy-item) {
    position: absolute;
    width: 100%;
    height: 100%;
  }
}
</style>
