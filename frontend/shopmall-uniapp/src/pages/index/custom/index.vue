<template>
  <view class="main">
    <!--提示框组件-->
    <u-toast ref="uToast" />
    <!--无网络组件-->
    <u-no-network />
    <!--头部组件-->
    <u-navbar
      id="indexnavbar"
      :is-back="false"
      :title="appTitle"
      :background="customHeaderStyle.background"
      :title-color="customHeaderStyle.titleColor"
      :border-bottom="false"
    />
    <shop-page :data="pageData" />
  </view>
</template>
<script lang="ts" setup>
import ShopPage from "@/components/ShopPage/page.vue";
import { ref, getCurrentInstance, onMounted, type Ref } from "vue";
import PageApi, { type DesginItem } from "@/apis/shopmall/page";
import { GetRect, useHeader } from "@/components/ui/hooks/user-head";
import { onShow } from "@dcloudio/uni-app";
const appTitle = ref("ViazyNetCore");
const pageData: Ref<Array<DesginItem>> = ref([
  { type: "search", value: { keywords: "请输入关键字搜索", style: "round" } },
  {
    type: "imgSlide",
    value: {
      duration: 2500,
      list: [
        {
          image: "/src/assets/images/empty-banner.png",
          linkType: "",
          linkValue: ""
        },
        {
          image: "/src/assets/images/empty-banner.png",
          linkType: "",
          linkValue: ""
        }
      ]
    }
  },
  {
    type: "navBar",
    value: {
      limit: 4,
      list: [
        {
          image: "/src/assets/images/empty.png",
          text: "新品推荐",
          linkType: "1",
          linkValue: "/pages/selection/result/index",
          url: "https://localhost:7277/upload/public/image/2023/04/10/6381676761945089684314620.png"
        },
        {
          image: "/src/assets/images/empty.png",
          text: "限时抢购",
          linkType: "1",
          linkValue: "/pages/selection/result/index",
          url: "https://localhost:7277/upload/public/image/2023/04/10/6381676763408593864006283.png"
        },
        {
          image: "/src/assets/images/empty.png",
          text: "爆品推荐",
          linkType: "1",
          linkValue: "/pages/selection/result/index",
          url: "https://localhost:7277/upload/public/image/2023/04/10/6381676765294845261090523.png"
        },
        {
          image: "/src/assets/images/empty.png",
          text: "推荐好物",
          linkType: "1",
          linkValue: "/pages/selection/result/index",
          url: "https://localhost:7277/upload/public/image/2023/04/10/6381676766308265789439821.png"
        }
      ]
    }
  },
  {
    type: "imgWindow",
    value: {
      style: 0,
      margin: 0,
      list: [
        {
          image:
            "/upload/public/image/2023/03/19/6381486097760473737756926.png",
          linkType: "",
          linkValue: ""
        },
        {
          image: "/src/assets/images/empty-banner.png",
          linkType: "",
          linkValue: ""
        },
        {
          image: "/src/assets/images/empty-banner.png",
          linkType: "",
          linkValue: ""
        },
        {
          image: "/src/assets/images/empty-banner.png",
          linkType: "",
          linkValue: ""
        }
      ]
    }
  },
  {
    type: "goods",
    value: {
      title: "商品组名称",
      type: "auto",
      lookMore: "true",
      classifyId: "",
      brandId: "",
      display: "list",
      limit: 10,
      column: 2,
      list: [
        {
          id: "1H4OZC2R7SW2942",
          brandId: null,
          brandName: null,
          catId: "1GLWG4RKT4W1554",
          catName: "烹饪锅具",
          name: "简约餐盘耐热家用盘子菜盘套装多颜色简约餐盘耐热家用盘子",
          subTitle: null,
          keywords: null,
          description: null,
          price: 129.0,
          openSpec: true,
          image:
            "https://localhost:7277/upload/public/image/2023/04/11/6381685038582121902066009.png"
        },
        {
          id: "1H4OIPMG75S7641",
          brandId: null,
          brandName: null,
          catId: "1GLWD6WZXC01548",
          catName: "手机通讯",
          name: "迷你便携高颜值蓝牙无线耳机立体声只能触控式操作简约立体声耳机",
          subTitle: null,
          keywords: null,
          description: null,
          price: 290.0,
          openSpec: false,
          image:
            "https://localhost:7277/upload/public/image/2023/04/11/6381684984115551305246234.png"
        },
        {
          id: "1FKIBOLF3281851",
          brandId: null,
          brandName: null,
          catId: "11001",
          catName: "奶茶/茶饮",
          name: "小羊咩咩没~~咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩咩",
          subTitle: null,
          keywords: null,
          description: null,
          price: 12.9,
          openSpec: true,
          image:
            "https://localhost:7277/upload/public/image/2023/03/14/6381443164371038354913416.png"
        },
        {
          id: "1FKHSRFF51C1848",
          brandId: null,
          brandName: null,
          catId: "12001",
          catName: "童鞋/婴儿鞋",
          name: "小羊咩咩",
          subTitle: null,
          keywords: null,
          description: null,
          price: 12.0,
          openSpec: false,
          image:
            "https://localhost:7277/upload/public/image/2023/03/14/6381443119233469888795835.png"
        },
        {
          id: "BRQITVPA39C05149",
          brandId: null,
          brandName: null,
          catId: "3",
          catName: "学习用品",
          name: "Test1",
          subTitle: null,
          keywords: null,
          description: null,
          price: 30.0,
          openSpec: false,
          image:
            "https://localhost:7277/upload/public/image/2023/03/14/6381442979251715415542528.png"
        }
      ]
    }
  }
]);
const { boundingRect, customHeaderStyle } = useHeader();
onMounted(async () => {
  pageData.value = await PageApi.getPageData("mobile_home");
});
onShow(async () => {
  const haedRect = (await GetRect(instance, "#indexnavbar")) as UniApp.NodeInfo;
  boundingRect.value.height = haedRect.height;
  boundingRect.value.width = haedRect.width;
  boundingRect.value.top = haedRect.top;
  boundingRect.value.bottom = haedRect.bottom;

  uni.pageScrollTo({ scrollTop: 0 });
});
const instance = getCurrentInstance();
</script>
