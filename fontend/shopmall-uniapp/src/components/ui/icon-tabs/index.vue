<template>
  <view
    :style="{
      background: bgColor
    }"
    class="icon-tabs"
  >
    <scroll-view
      class="icon-tabs-view"
      scroll-x="true"
      scroll-y="false"
      style="height: 140rpx; flex: 1; white-space: nowrap"
    >
      <view
        :id="id"
        class="icon-tabs-scroll-box"
        :class="{ 'icon-tabs-scroll-flex': !isScroll }"
      >
        <view
          v-for="(item, index) in list"
          :id="'icon-tab-item-' + index"
          :key="index"
          :style="tabItemStyle(index)"
          :class="{ active: index == currentIndex }"
          @click="clickTab(index)"
        >
          <u-icon
            :key="item.id"
            class="icon-tab-item"
            :name="item.image"
            width="80"
            height="80"
            :label="item.text"
            :label-size="26"
            label-pos="bottom"
            margin-top="10" /></view
      ></view>
    </scroll-view>
    <view class="icon-tabs-more" @click="showU"> 更多</view>
    <u-popup v-model="show" mode="top">
      <view
        v-for="(item, index) in list"
        :id="'icon-tab-item-' + index"
        :key="index"
        :style="tabItemStyle(index)"
        @click="clickTab(index)"
      >
        <u-icon
          :key="item.id"
          class="icon-tab-item"
          :name="item.image"
          width="80"
          height="80"
          :label="item.text"
          :label-size="26"
          label-pos="bottom"
          margin-top="10"
      /></view>
    </u-popup>
  </view>
</template>
<script lang="ts" setup>
import addUnit from "@/uni_modules/vk-uview-ui/libs/function/addUnit";
import {
  ref,
  watch,
  nextTick,
  getCurrentInstance,
  type Ref,
  type CSSProperties,
  onMounted
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
const props = withDefaults(defineProps<IConTabsProps>(), {
  modelValue: 0,
  isScroll: true,
  height: 130,
  fontSize: 30,
  duration: 0.5,
  bgColor: "#ffffff"
});
// tab的样式
const tabItemStyle: (i: number) => CSSProperties = (index: number) => {
  const style = {
    height: props.height + "rpx",
    "line-height": props.height + "rpx",
    "font-size": props.fontSize + "rpx",
    "transition-duration": `${props.duration}s`,
    padding: props.isScroll ? `0 ${props.gutter}rpx` : "",
    flex: props.isScroll ? "auto" : "1",
    width: addUnit(props.itemWidth),
    display: "inline-block",
    "text-align": "center"
  };
  if (index == currentIndex.value) {
    console.log(index);
  }
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
  const tabInfo = tabQueryInfo[currentIndex.value];
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
  const tabRect = await instance.appContext.config.globalProperties.$u.getRect(
    "#" + id.value
  );
  parentLeft.value = tabRect.left;
  componentWidth.value = tabRect.width;
  getTabRect();
};
const instance = getCurrentInstance();
function getTabRect() {
  // 创建节点查询
  const query = uni.createSelectorQuery().in(instance);
  // 历遍所有tab，这里是执行了查询，最终使用exec()会一次性返回查询的数组结果
  for (let i = 0; i < props.list.length; i++) {
    // 只要size和rect两个参数
    query.select(`#u-tab-item-${i}`).fields(
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
      // 初始化滚动条和移动bar的位置
      scrollByIndex();
    }.bind(instance)
  );
}
onMounted(() => {
  id.value = guid();
});
const show = ref(false);
const showU = () => {
  show.value = true;
};
</script>
<style lang="scss" scoped>
.icon-tabs {
  position: relative;
}
.icon-tabs-more {
  width: 30px;
  height: 100%;
  overflow: hidden;
  position: absolute;
  background-color: #ffffff;
  top: 0;
  right: 0;
}
view,
scroll-view {
  box-sizing: border-box;
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

.icon-tabs-scroll-box {
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

.icon-tabs-view {
  width: 100%;
  white-space: nowrap;
  position: relative;
}

.icon-tab-item {
  position: relative;
  /* #ifndef APP-NVUE */
  display: inline-block;
  /* #endif */
  text-align: center;
  transition-property: background-color, color;
}

.icon-tab-bar {
  position: absolute;
  bottom: 0;
}

.icon-tabs-scroll-flex {
  display: flex;
  justify-content: space-between;
}
.active {
  color: #ffffff;

  :deep(image) {
    border-radius: 50%;
    overflow: hidden;
    border: 1px solid blue;
  }
  :deep(text) {
    border-radius: 7px;
    color: #ffffff;
    background-color: blue;
  }
}
</style>
