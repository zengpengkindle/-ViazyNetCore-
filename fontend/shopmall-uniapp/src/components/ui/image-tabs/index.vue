<template>
  <view
    :style="{
      background: bgColor
    }"
    class="image-tabs"
  >
    <scroll-view
      class="image-tabs-view"
      scroll-x="true"
      scroll-y="false"
      style="flex: 1; white-space: nowrap"
      :scroll-left="scrollLeft"
      scroll-with-animation
    >
      <view
        :id="id"
        class="image-tabs-scroll-box"
        :class="{ 'image-tabs-scroll-flex': !isScroll }"
      >
        <view
          v-for="(item, index) in list"
          :id="'image-tab-item-' + index"
          :key="index"
          :style="tabItemStyle(index)"
          :class="{ active: index == currentIndex }"
          @click="clickTab(index)"
        >
          <x-icon
            :key="item.id"
            :active="index == currentIndex"
            class="image-tab-item"
            :url="item.image"
            width="90"
            height="90"
            :label="item.text"
            :label-size="22"
            label-pos="bottom"
            margin-top="10"
          />
        </view>
      </view>
    </scroll-view>
    <view
      class="image-tabs-more"
      :style="{
        boxSizing: 'content-box'
      }"
      @click="showU"
    >
      <view class="image-tabs-more-content">
        更多<u-icon name="list-dot" />
      </view>
    </view>
    <u-popup v-model="show" mode="top" closeable :border-radius="18">
      <view :style="{ width: '100%', height: statusBarHeight + 'px' }" />
      <view class="image-tabs-popup-header">全部分类</view>
      <view
        v-for="(item, index) in list"
        :id="'image-tab-item-' + index"
        :key="index"
        :style="tabItemStyle(index)"
        @click="clickTab(index)"
      >
        <x-icon
          :key="item.id"
          class="image-tab-item"
          :active="index == currentIndex"
          :url="item.image"
          width="90"
          height="90"
          :label="item.text"
          :label-size="22"
          label-pos="bottom"
          margin-top="10"
        />
      </view>
    </u-popup>
  </view>
</template>
<script lang="ts" setup>
import XIcon from "../icon/index.vue";
import addUnit from "@/uni_modules/vk-uview-ui/libs/function/addUnit";
import {
  ref,
  watch,
  nextTick,
  getCurrentInstance,
  type Ref,
  type CSSProperties,
  onMounted,
  onBeforeMount
} from "vue";
import guid from "../utils/guid";

export interface IConTabsProps {
  modelValue: number;
  isScroll?: boolean;
  list: Array<any>;
  bgColor?: string;
  height?: number;
  fontSize?: number;
  duration?: number;
  activeClass?: string;
  inactiveClass?: string;
  showBar?: boolean;
  itemWidth?: string;
  gutter?: number;
  barWidth?: number;
}

const systemInfo = uni.getSystemInfoSync();
const statusBarHeight = ref(systemInfo.statusBarHeight);

const props = withDefaults(defineProps<IConTabsProps>(), {
  modelValue: 0,
  isScroll: true,
  height: 130,
  fontSize: 30,
  duration: 0.5,
  gutter: 12
});
// tab的样式
const tabItemStyle: (i: number) => CSSProperties = () => {
  const style: CSSProperties = {
    "line-height": props.height + "rpx",
    "font-size": props.fontSize + "rpx",
    "transition-duration": `${props.duration}s`,
    padding: props.isScroll ? `${props.gutter}rpx` : "",
    flex: props.isScroll ? "auto" : "1",
    width: addUnit(props.itemWidth),
    display: "inline-block",
    "text-align": "center"
  };
  return style;
};
const currentIndex: Ref<number> = ref(0);
watch(
  () => props.list,
  (n, o) => {
    if (n.length !== o.length) currentIndex.value = 0;
    nextTick(() => {
      init();
    });
  }
);
watch(
  () => props.modelValue,
  nval => {
    currentIndex.value = nval;
    scrollByIndex();
  }
);
const emits = defineEmits(["update:modelValue", "change"]);
const clickTab = index => {
  // 点击当前活动tab，不触发事件
  if (index == props.modelValue) return;
  // 发送事件给父组件
  emits("change", { index, item: props.list[index] });
  emits("update:modelValue", index);
};

const tabQueryInfo = ref([]);
const parentLeft = ref(0);
const componentWidth = ref(0);
const scrollLeft = ref(0);
const scrollBarLeft = ref(0);
const barFirstTimeMove = ref(true);
function scrollByIndex() {
  // 当前活动tab的布局信息，有tab菜单的width和left(为元素左边界到父元素左边界的距离)等信息
  const tabInfo = tabQueryInfo.value[currentIndex.value];
  if (!tabInfo) return;
  // 活动tab的宽度
  const tabWidth = tabInfo.width;
  // 活动item的左边到tabs组件左边的距离，用item的left减去tabs的left
  const offsetLeft = tabInfo.left - parentLeft.value;
  // 将活动的tabs-item移动到屏幕正中间，实际上是对scroll-view的移动
  const m_scrollLeft = offsetLeft - (componentWidth.value - tabWidth) / 2;
  scrollLeft.value = m_scrollLeft < 0 ? 0 : m_scrollLeft;
  // 当前活动item的中点点到左边的距离减去滑块宽度的一半，即可得到滑块所需的移动距离
  const left = tabInfo.left + tabInfo.width / 2 - parentLeft.value;
  // 计算当前活跃item到组件左边的距离
  scrollBarLeft.value = left - uni.upx2px(props.barWidth) / 2;
  // 第一次移动滑块的时候，barFirstTimeMove为true，放到延时中将其设置false
  // 延时是因为scrollBarLeft作用于computed计算时，需要一个过程需，否则导致出错
  if (barFirstTimeMove.value == true) {
    setTimeout(() => {
      barFirstTimeMove.value = false;
    }, 100);
  }
}

const id = ref("");
const init = async () => {
  const tabRect = await GetRect("#" + id.value);
  parentLeft.value = tabRect.left;
  componentWidth.value = tabRect.width;
  getTabRect();
};
const instance = getCurrentInstance();
function GetRect(selector, all = null): any {
  return new Promise(resolve => {
    uni
      .createSelectorQuery()
      .in(instance)
      [all ? "selectAll" : "select"](selector)
      .boundingClientRect(rect => {
        if (all && Array.isArray(rect) && rect.length) {
          resolve(rect);
        }
        if (!all && rect) {
          resolve(rect);
        }
      })
      .exec();
  });
}
function getTabRect() {
  // 创建节点查询
  const query = uni.createSelectorQuery().in(instance);
  // 历遍所有tab，这里是执行了查询，最终使用exec()会一次性返回查询的数组结果
  for (let i = 0; i < props.list.length; i++) {
    // 只要size和rect两个参数
    query.select(`#image-tab-item-${i}`).fields(
      {
        size: true,
        rect: true
      },
      () => {}
    );
  }
  // 执行查询，一次性获取多个结果
  query.exec(
    function (res) {
      tabQueryInfo.value = res;
      console.log("query", res);
      // 初始化滚动条和移动bar的位置
      scrollByIndex();
    }.bind(instance)
  );
}
onBeforeMount(() => {
  id.value = guid();
});
onMounted(() => {
  currentIndex.value = props.modelValue;
  nextTick(() => init());
});
const show = ref(false);
const showU = () => {
  show.value = true;
};
</script>
<style lang="scss" scoped>
.image-tabs {
  position: relative;
}
.image-tabs-more {
  position: absolute;
  width: 70rpx;
  top: 0;
  right: 0;
  bottom: 0;
  font-size: 26rpx;
  text-align: center;
  overflow: hidden;
  .image-tabs-more-content {
    position: absolute;
    width: 100%;
    padding: 10rpx 15rpx;
    top: 50%;
    transform: translateY(-50%);
  }
}

view,
scroll-view {
  box-sizing: border-box;
}
::-webkit-scrollbar {
  width: 0;
  height: 0;
  color: transparent;
}
/* #ifndef APP-NVUE */
::-webkit-scrollbar,
::-webkit-scrollbar,
::-webkit-scrollbar {
  display: none;
  width: 0 !important;
  height: 0 !important;
  -webkit-appearance: none;
  background: transparent;
}
/* #endif */

.image-tabs-scroll-box {
  position: relative;
  /* #ifdef MP-TOUTIAO */
  white-space: nowrap;
  /* #endif */
}

/* #ifdef H5 */
// 通过样式穿透，隐藏H5下，scroll-view下的滚动条
scroll-view ::v-deep ::-webkit-scrollbar {
  display: none;
  width: 0 !important;
  height: 0 !important;
  -webkit-appearance: none;
  background: transparent;
}
/* #endif */

.image-tabs-view {
  width: 100%;
  padding-right: 70rpx;
  white-space: nowrap;
  position: relative;
}

.image-tab-item {
  position: relative;
  /* #ifndef APP-NVUE */
  display: inline-block;
  /* #endif */
  text-align: center;
  transition-property: background-color, color;
}

.image-tab-bar {
  position: absolute;
  bottom: 0;
}

.image-tabs-scroll-flex {
  display: flex;
  justify-content: space-between;
}

.image-tabs-popup-header {
  padding: 24rpx 36rpx;
  font-size: 28rpx;
  font-weight: 600;
}
:deep(.u-drawer-content) {
  border-bottom-left-radius: 24rpx;
  border-bottom-right-radius: 24rpx;
  overflow: hidden;
}
</style>
