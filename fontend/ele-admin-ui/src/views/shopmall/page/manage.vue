<script lang="ts" setup>
import { ref, Ref } from 'vue';
import ModelTitle from "@/assets/images/model-title.png";
import EmptyBanner from "@/assets/images/empty-banner.png";
import ImangeFourColumn from "@/assets/images/image-four-column.png";
import ImangeOneColumn from "@/assets/images/image-one-column.png";
import ImangeOneLeft from "@/assets/images/image-one-left.png";
import ImangeThreeColumn from "@/assets/images/image-three-column.png";
import IcCar from "@/assets/images/ic-car.png";
import { useRenderIcon } from '@/components/ReIcon/src/hooks';
import SelectLink from "./components/SelectLink.vue";
import {
  useTools
} from "./components/toolhook";
import {
  ComponentItem,
  useWidget
} from "./components/widget";

export interface Props {
  modelValue: boolean;
  readonly id: number;
}

const {
  catList,
  brandList,
  articleTypeList,
  imgWindowStyle,
  getSelectWgName,
  resetColor,
  changeGoodsType,
  handleDeleteGoods,
  selectGoods,
  handleRemoveTabBar,
  changeTabBarGoodsType,
  handleDeleteTabBarGoods,
  selectTabBarGoods,
  handleAddTabBarGoods,
  article_list,
  upImage,
  chooseLink,
  handleAddSlide,
  handleSlideRemove,
  slectTplStyle,
  handleDeleteNotice,
  selectNotice,
  handleAddNav
} = useTools();

const {
  selectWg,
  selectWidget,
  setSelectWg,
  handleWidgetAdd,
  handleClickAdd,
  handleSelectWidget,
  handleSelectRecord,
  deleteWidget,
  handleWidgetDelete,
  handleWidgetClone,
  handleDragRemove,
  datadragEnd
} = useWidget();

const props = defineProps<Props>();
const pageData: Ref<Array<ComponentItem>> = ref([]);

interface TabBarComponent {
  list: Array<GoodItem>;
  column: number;
  title: string;
}

interface GoodItem {
  name: string;
  image: string;
  price: number;
}

interface ServiceComponent {
  title: string;
  list: Array<ServiceItem>;
}
interface ServiceItem {
  title: string;
  thumbnail: string;
  money: number;
}

interface ToolGroup {
  name: string;
  tools: Array<ToolComponent>;
}
interface ToolComponent {
  name: string;
  icon: any;
  type: string;
}
const tools: Array<ToolGroup> = [
  {
    name: "媒体组件",
    tools: [
      { name: "图片轮播", icon: "ep:add-location", type: "imgSlide" },
      { name: "图片", icon: "ep:add-location", type: "imgSingle" },
      { name: "图片分组", icon: "ep:add-location", type: "imgWindow" },
      { name: "视频组", icon: "ep:add-location", type: "video" },
      { name: "文章组", icon: "ep:add-location", type: "article" },
      { name: "文章分类", icon: "ep:add-location", type: "articleClassify" }
    ]
  }, {
    name: "商城组件",
    tools: [
      { name: "搜索框", icon: "ep:add-location", type: "search" },
      { name: "公告组", icon: "ep:add-location", type: "notice" },
      { name: "导航组", icon: "ep:add-location", type: "navBar" },
      { name: "商品组", icon: "ep:add-location", type: "goods" },
      { name: "商品选项卡", icon: "ep:add-location", type: "goodTabBar" }
    ]
  }, {
    name: "工具组件",
    tools: [
      { name: "辅助空白", icon: "ep:add-location", type: "blank" },
      { name: "文本域", icon: "ep:add-location", type: "textarea" }
    ]
  }
]
</script>
<template>
  <div class="main">
    <el-row :gutter="20">
      <el-col :span="6">
        <el-card>
          <template #header>
            组件库
          </template>
          <template v-for="group in tools" v-bind:key="group.name">
            <el-divider content-position="left">{{ group.name }}</el-divider>
            <el-row :gutter="10">
              <el-col :span="6" v-for="item in group.tools">
                <button class="component-item" @click=selectWidget(item.type)>
                  <div class="p-1">
                    <i class="component-icon">
                      <component :is="useRenderIcon(item.icon, { width: 16 })" />
                    </i>
                  </div>
                  <p class="text">{{ item.name }}</p>
                </button>
              </el-col>
            </el-row>
          </template>
          <el-divider />
          <el-button type="primary">保存页面</el-button>
        </el-card>
      </el-col>
      <el-col :span="6">
        <div class="center-container">
          <div class="model-title">
            <img :src="ModelTitle">
          </div>
          <div>
            <div v-for="(item, index) in pageData" v-bind:key="item.key">
              <div class="layout-main" :key="item.key"
                :class="{ active: selectWg.key === item.key, npr: item.type == 'record' }"
                @click="handleSelectWidget(item)">
                <!-- 搜索框 -->
                <div v-if="item.type === 'search'" class="drag lay-item lay-search">
                  <div class="lay-search-c">
                    <input v-model="item.value.keywords" class="lay-search-input" :class="item.value.style" />
                    <i class="iconfontCustom icon-sousuokuang"></i>
                  </div>
                </div>
                <!-- 购买记录 -->
                <div v-if="item.type === 'record'" class="drag lay-record" :class="item.value.style.align"
                  @click="handleSelectRecord(item)" :style="{ top: item.value.style.top + '%' }">
                  <div class="record-item">
                    <i class="layui-icon layui-icon-user"></i>
                    <span class="text">xxx刚刚0.01元买到了xxx</span>
                  </div>
                  <div @click.stop="handleWidgetDelete(item)" class="btn-delete" v-if="selectWg.key === item.key">删除
                  </div>
                </div>
                <!-- 商品组 -->
                <div v-if="item.type === 'goods'" class="drag clearfix lay-goods" :class="item.value.display">
                  <div class="goods-head">
                    <div>{{ item.value.title }}</div>
                    <div v-if="item.value.lookMore">查看更多></div>
                  </div>
                  <div class="goods-item" v-for="(goods, key) in item.value.list" :key="key"
                    :class="'column' + item.value.column">
                    <div class="goods-image">
                      <img :src="goods.image_url || goods.image" alt="">
                    </div>
                    <div class="goods-detail">
                      <p class="goods-name twolist-hidden">
                        {{ goods.name || '此处显示商品名称' }}
                      </p>
                      <p class="goods-price">{{ goods.price || '99.00' }}</p>
                    </div>
                  </div>
                </div>
                <!-- 商品选项卡 -->
                <div v-if="item.type === 'goodTabBar'" class="drag clearfix lay-goods list">
                  <div class="goods-tab-head">
                    <div v-for="(goods, key) in item.value.list" :key="key">{{ goods.title }}</div>
                  </div>
                  <div v-for="(goods, key) in item.value.list" :key="key" v-show="key == 0">
                    <div class="goods-item" :class="'column' + goods.column" v-for="(goodsitem, itemkey) in goods.list"
                      :key="itemkey">
                      <div class="goods-image">
                        <img :src="goodsitem.image" alt="">
                      </div>
                      <div class="goods-detail">
                        <p class="goods-name twolist-hidden">
                          {{ goodsitem.name || '此处显示商品名称' }}
                        </p>
                        <p class="goods-price">{{ goodsitem.price || '99.00' }}</p>
                      </div>
                    </div>
                  </div>
                </div>
                <!-- 图片轮播 -->
                <div v-if="item.type === 'imgSlide'" class="drag lay-item lay-imgSlide">
                  <el-carousel :interval="item.value.duration" arrow="never" :autoplay="false">
                    <el-carousel-item v-for="(list, key) in item.value.list" :key="key">
                      <img :src="list.image" alt="banner" style="width:100%;height:100%">
                    </el-carousel-item>
                  </el-carousel>
                </div>
                <!-- 置顶轮播 -->
                <!--<div v-if="item.type==='topImgSlide'" class="drag lay-item lay-imgSlide">
                                                                      <el-carousel :interval="item.value.duration" arrow="never" :autoplay="false">
                                                                          <el-carousel-item v-for="(list,key) in item.value.list" :key="key">
                                                                              <div style="width: 100%; height: 100%; position: relative">
                                                                                  <img :src="list.image" alt="banner" style="max-width: 80%; max-height: 80%; bottom: 10%; position: absolute; left: 10%; z-index: 10; border: 1px solid salmon;">
                                                                                  <img :src="list.bg" alt="bannerbg" style="width: 100%; height: 100%; position: absolute; top: 0; left: 0; z-index: 1; border: 1px solid salmon;">
                                                                              </div>
                                                                          </el-carousel-item>
                                                                      </el-carousel>
                                                                  </div>-->
                <!-- 单图组 -->
                <div v-if="item.type === 'imgSingle'" class="drag lay-imgSingle">
                  <div class="img-wrap" v-for="(img, key) in item.value.list" :key="key">
                    <img :src="img.image" alt="">
                    <div class="img-btn" :style="{ backgroundColor: img.buttonColor, color: img.textColor }"
                      v-show="img.buttonShow">{{ img.buttonText }}</div>
                  </div>
                </div>
                <!-- 图片橱窗 -->
                <div v-if="item.type === 'imgWindow'" class="drag lay-imgWindow clearfix"
                  :class="'row' + item.value.style" :style="{}">
                  <template v-if="item.value.style == 0">
                    <div class="display">
                      <div class="display-left">
                        <img :src="item.value.list[0].image" alt="">
                      </div>
                      <div class="display-right">
                        <div class="display-right1">
                          <img :src="item.value.list[1].image" alt="">
                        </div>
                        <div class="display-right2">
                          <div class="left">
                            <img :src="item.value.list[2].image" alt="">
                          </div>
                          <div class="right">
                            <img :src="item.value.list[3].image" alt="">
                          </div>
                        </div>
                      </div>
                    </div>
                  </template>
                  <template v-else>
                    <div class="img-wrap" v-for="(img, key) in item.value.list" :key="key"
                      :style="{ padding: item.value.margin + 'px' }">
                      <img :src="img.image" alt="">
                    </div>
                  </template>
                </div>
                <!-- 视频组 -->
                <div v-if="item.type === 'video'" class="drag lay-item lay-video">
                  <div class="video-wrap" v-for="(video, key) in item.value.list">
                    <video :src="video.url" :poster="video.image" height="200px;"></video>
                  </div>
                </div>
                <!-- 文章组 -->
                <div v-if="item.type === 'article'" class="drag lay-article">
                  <div class="article-wrap clearfix" v-for="(article, key) in item.value.list">
                    <div class="article-left">
                      <div class="article-left-title">
                        {{ article.title || '此处显示文章标题' }}
                      </div>
                    </div>
                    <div class="article-img">
                      <img :src="article.cover || EmptyBanner" alt="">
                    </div>
                  </div>
                </div>
                <!-- 文章分类 -->
                <div v-if="item.type === 'articleClassify'" class="drag lay-article">
                  <div class="article-wrap clearfix">
                    <div class="article-left">
                      <div class="article-left-title">
                        此处显示文章标题
                      </div>
                    </div>
                    <div class="article-img">
                      <img :src="EmptyBanner" alt="">
                    </div>
                  </div>
                  <div class="article-wrap clearfix">
                    <div class="article-left">
                      <div class="article-left-title">
                        此处显示文章标题
                      </div>
                    </div>
                    <div class="article-img">
                      <img :src="EmptyBanner" alt="">
                    </div>
                  </div>
                </div>
                <!-- 公告组 -->
                <div v-if="item.type === 'notice'" class="drag lay-item lay-notice">
                  <i class="iconfontCustom icon-gonggao"></i>
                  <div class="notice-right">
                    <div v-for="(notice, key) in item.value.list" class="notice-text">{{ notice.title }}</div>
                  </div>
                </div>
                <!-- 优惠券组 -->
                <div v-if="item.type === 'coupon'" class="drag lay-item lay-coupon">
                  <div class="coupon-item">
                    <div class="coupon-left">
                      <p>满300减30</p>
                    </div>
                    <div class="coupon-right">
                      <p class="conpon-f">订单减1.44元 减100元 </p>
                      <p>购买订单满2元 </p>
                      <p>2019-05-01 - 2019-05-31</p>
                    </div>
                    <div class="coupon-btn">
                      立即领取
                    </div>
                  </div>
                </div>
                <!-- 导航组 -->
                <div v-if="item.type === 'navBar'" class="drag lay-navBar clearfix" :class="'row' + item.value.limit">
                  <div class="item" v-for="(nav, key) in item.value.list" :key="key">
                    <div class="item-image">
                      <img :src="nav.image" alt="">
                    </div>
                    <p class="item-text">{{ nav.text }}</p>
                  </div>
                </div>
                <!-- 辅助空白 -->
                <div v-if="item.type === 'blank'" class="drag lay-item lay-blank"
                  :style="{ height: item.value.height + 'px', backgroundColor: item.value.backgroundColor }">
                </div>
                <!-- 文本域 -->
                <div v-if="item.type === 'textarea'" class="drag lay-item lay-textarea">
                  <div class="lay-search-c">
                    <el-input type="textarea" autosize v-html="item.value" resize="none"></el-input>
                  </div>
                </div>
                <div @click.stop="handleWidgetDelete(item)" class="btn-delete" v-if="selectWg.key === item.key">删除</div>
                <div @click.stop="handleWidgetClone(index)" class="btn-clone" v-if="selectWg.key === item.key">复制</div>
              </div>
            </div>
          </div>
        </div>
      </el-col>
      <el-col :span="12">
        <el-card v-if="selectWg && Object.keys(selectWg).length > 0">
          <div class="custom-item main-body" id="editbody">
            <el-form ref="form" label-width="100px" label-position="left">
              <div class="custom-item-t">
                <div class="custom-item-t-c">{{ getSelectWgName(selectWg.type) }} </div>
              </div>
              <template v-if="selectWg.type == 'search'">
                <el-form-item label="提示内容">
                  <el-input v-model="selectWg.value.keywords" :placeholder="selectWg.placeholder"></el-input>
                </el-form-item>
                <el-form-item label="搜索框样式">
                  <el-radio-group v-model="selectWg.value.style">
                    <el-radio label="square">方形</el-radio>
                    <el-radio label="radius">圆角</el-radio>
                    <el-radio label="round">圆弧</el-radio>
                  </el-radio-group>
                </el-form-item>
              </template>
              <!-- 购买记录 -->
              <template v-if="selectWg.type == 'record'">
                <div class="content-item">
                  <el-form-item label="位置：">
                    <el-radio-group v-model="selectWg.value.style.align">
                      <el-radio label="left">居左</el-radio>
                      <el-radio label="right">居右</el-radio>
                    </el-radio-group>
                  </el-form-item>
                </div>
                <div class="content-item">
                  <el-form-item label="上边距：">
                    <el-slider v-model="selectWg.value.style.top" :min="0" :max="100"></el-slider>
                    <span>{{ selectWg.value.style.top }}%</span>
                  </el-form-item>
                </div>
              </template>
              <!-- 优惠券 -->
              <template v-if="selectWg.type == 'coupon'">
                <div>
                  <div class="content-item">
                    <span class="item-label">显示数量：</span>
                    <div class="number-content ml20">
                      <input type="number" v-model.number="selectWg.value.limit" min="0" class="number-input">
                    </div>
                  </div>
                  <div class="pl25">
                    <p class="layout-tip">
                      优惠券数据请到 促销管理 - <a href="javascript:;" lay-href="/promotion/coupon/">优惠券列表</a>中管理
                    </p>
                  </div>
                </div>
              </template>
              <!-- 辅助空白 -->
              <template v-if="selectWg.type == 'blank'">
                <el-form-item label="背景颜色">
                  <el-color-picker v-model="selectWg.value.backgroundColor"></el-color-picker>
                  <a class="reset-color" href="javascript:;" @click="resetColor">重置</a>
                </el-form-item>
                <el-form-item label="组件高度">
                  <el-slider v-model="selectWg.value.height" :min="1" :max="200"></el-slider>
                </el-form-item>
              </template>
              <!-- 商品组 -->
              <template v-if="selectWg.type == 'goods'">
                <div>
                  <el-form-item label="商品来源">
                    <el-radio-group v-model="selectWg.value.type" @change="changeGoodsType">
                      <el-radio label="auto">自动获取</el-radio>
                      <el-radio label="choose">手动选择</el-radio>
                    </el-radio-group>
                  </el-form-item>
                  <div v-show="selectWg.value.type == 'auto'">
                    <el-form-item label="商品分类">
                      <el-select v-model="selectWg.value.classifyId" placeholder="请选择分类">
                        <el-option value=" " label="请选择分类"></el-option>
                        <template v-for="item in catList">
                          <el-option :value="item.id" :label="item.title"></el-option>
                          <template v-if="item.children">
                            <el-option v-for="second in item.children" :key="second.id" :label="second.title"
                              :value="second.id" class="second">
                            </el-option>
                          </template>
                        </template>
                      </el-select>
                    </el-form-item>
                    <el-form-item label="品牌分类">
                      <el-select v-model="selectWg.value.brandId" placeholder="请选择品牌">
                        <el-option value=" " label="请选择品牌"></el-option>
                        <el-option v-for="item in brandList" :value="item.id" :label="item.name"
                          :key="item.id"></el-option>
                      </el-select>
                    </el-form-item>
                    <el-form-item label="显示数量">
                      <input type="number" v-model.number="selectWg.value.limit" :min="1" class="number-input">
                    </el-form-item>
                  </div>
                  <div v-show="selectWg.value.type == 'choose'">
                    <div class="select_seller_goods_box">
                      <input type="hidden" name="params[goodsId]" value="">
                      <ul id="selectGoods" class="sellect_seller_goods_list clearfix">
                        <el-select element="ul" :list="selectWg.value.list"
                          :options="{ group: { name: 'selectGoodsList' }, ghostClass: 'ghost', animation: 150 }">
                          <el-option :value="goods.key" v-for="(goods, key) in selectWg.value.list" :key="key">
                            <i class="layui-icon layui-icon-close-fill icon-delete" @click="handleDeleteGoods(key)"></i>
                            <img :src="goods.image" alt="">
                          </el-option>
                        </el-select>
                      </ul>
                      <div class="addImg" @click="selectGoods">
                        <i class="iconfontCustom icon-icon-test"></i>
                        <span>选择商品</span>
                      </div>
                    </div>
                  </div>
                  <el-divider />
                  <el-form-item label="显示类型">
                    <el-radio-group v-model="selectWg.value.display">
                      <el-radio label="list">列表平铺</el-radio>
                      <el-radio label="slide" :disabled="selectWg.value.column == 1">横向滚动</el-radio>
                    </el-radio-group>
                  </el-form-item>
                  <el-form-item label="分列数量">
                    <el-radio-group v-model="selectWg.value.column">
                      <el-radio :label="1" :disabled="selectWg.value.display == 'slide'">单列</el-radio>
                      <el-radio :label="2">两列</el-radio>
                      <el-radio :label="3">三列</el-radio>
                    </el-radio-group>
                  </el-form-item>
                  <el-form-item label="商品组名称">
                    <input type="text" v-model="selectWg.value.title" class="selectLinkVal">
                  </el-form-item>
                  <el-form-item label="是否查看更多">
                    <el-radio-group v-model="selectWg.value.lookMore">
                      <el-radio label="true">是</el-radio>
                      <el-radio label="false">否</el-radio>
                    </el-radio-group>
                  </el-form-item>
                  <div class="pl25">
                    <p class="layout-tip">
                      商品数据请到 商品管理 - <a href="javascript:;" lay-href="/good/goods/">商品列表</a> 中管理
                    </p>
                  </div>
                </div>
              </template>
              <!-- 商品选项卡 -->
              <template v-if="selectWg.type == 'goodTabBar'">
                <div>
                  <el-form-item label="是否固定头部">
                    <el-radio-group v-model="selectWg.value.isFixedHead">
                      <el-radio label="true">是</el-radio>
                      <el-radio label="false">否</el-radio>
                    </el-radio-group>
                  </el-form-item>

                  <div v-for="(child, key) in selectWg.value.list">
                    <div class="drag-block">
                      <div class="handle-icon" title="删除这一项">
                        <i class="iconfontCustom icon-cuohao" @click="handleRemoveTabBar(key)"></i>
                      </div>
                    </div>
                    <div class="tabBarItem">
                      <el-form-item label="商品来源">
                        <el-radio-group v-model="child.type" @change="changeTabBarGoodsType(child.type, key)">
                          <el-radio label="auto">自动获取</el-radio>
                          <el-radio label="choose">手动选择</el-radio>
                        </el-radio-group>
                      </el-form-item>
                      <div v-show="child.type == 'auto'">
                        <el-form-item label="商品分类">
                          <el-select v-model="child.classifyId" placeholder="请选择分类">
                            <el-option value=" " label="请选择分类"></el-option>
                            <template v-for="itemCat in catList">
                              <el-option :value="itemCat.id" :label="itemCat.title"></el-option>
                              <template v-if="itemCat.children">
                                <el-option v-for="second in itemCat.children" :key="second.id" :label="second.title"
                                  :value="second.id" class="second">
                                </el-option>
                              </template>
                            </template>
                          </el-select>
                        </el-form-item>
                        <el-form-item label="品牌分类">
                          <el-select v-model="child.brandId" placeholder="请选择品牌">
                            <el-option value=" " label="请选择品牌"></el-option>
                            <el-option v-for="itemBrand in brandList" :value="itemBrand.id" :label="itemBrand.name"
                              :key="itemBrand.id"></el-option>
                          </el-select>
                        </el-form-item>
                        <el-form-item label="显示数量">
                          <input type="number" v-model.number="child.limit" :min="1" class="number-input">
                        </el-form-item>
                      </div>
                      <div v-show="child.type == 'choose'">
                        <div class="select_seller_goods_box">
                          <input type="hidden" name="params[goodsId]" value="">
                          <ul id="selectGoods" class="sellect_seller_goods_list clearfix">
                            <el-select element="ul" :list="child.list"
                              :options="{ group: { name: 'selectGoodsList' }, ghostClass: 'ghost', animation: 150 }">
                              <el-option :value="goods.key" v-for="(goods, index) in child.list" :key="index">
                                <i class="layui-icon layui-icon-close-fill icon-delete"
                                  @click="handleDeleteTabBarGoods(key, index)"></i>
                                <img :src="goods.image" alt="">
                              </el-option>
                            </el-select>
                          </ul>
                          <div class="addImg" @click="selectTabBarGoods(key)">
                            <i class="iconfontCustom icon-icon-test"></i>
                            <span>选择商品</span>
                          </div>
                        </div>
                      </div>
                      <hr class="divider" />
                      <el-form-item label="分列数量">
                        <el-radio-group v-model="child.column">
                          <el-radio :label="1" :disabled="child.display == 'slide'">单列</el-radio>
                          <el-radio :label="2">两列</el-radio>
                          <el-radio :label="3">三列</el-radio>
                        </el-radio-group>
                      </el-form-item>
                      <el-form-item label="Tab大标题名称">
                        <input type="text" v-model="child.title" class="selectLinkVal">
                      </el-form-item>
                      <el-form-item label="Tab子标题名称">
                        <input type="text" v-model="child.subTitle" class="selectLinkVal">
                      </el-form-item>

                    </div>
                  </div>
                  <div class="addImg" @click="handleAddTabBarGoods">
                    <i class="iconfontCustom icon-icon-test"></i>
                    <span>添加一个Tab</span>
                  </div>
                  <div class="pl25">
                    <p class="layout-tip">
                      商品数据请到 商品管理 - <a href="javascript:;" lay-href="/good/goods/">商品列表</a> 中管理
                    </p>
                  </div>
                </div>
              </template>
              <!-- 文章组 -->
              <template v-if="selectWg.type == 'article'">
                <div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">添加文章：</label>
                    <div class="layui-input-inline seller-inline-5">
                      <input type="text" v-model="selectWg.value.list[0].title" placeholder="请选择广告文章" readonly
                        class="layui-input" @click="article_list">
                    </div>
                  </div>
                  <div class="pl25">
                    <p class="layout-tip">
                      文章数据请到 运营管理 - <a href="javascript:;" lay-href="content/article/articles/">文章列表</a> 中管理
                    </p>
                  </div>
                </div>
              </template>
              <!-- 文章分类 -->
              <template v-if="selectWg.type == 'articleClassify'">
                <div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">文章分类：</label>
                    <div class="layui-input-inline seller-inline-5">
                      <el-select v-model="selectWg.value.articleClassifyId" placeholder="请选择分类">
                        <template v-for="item in articleTypeList">
                          <el-option :value="item.id" :label="item.name"></el-option>
                        </template>
                      </el-select>
                    </div>
                  </div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">显示数量：</label>
                    <div class="layui-input-inline seller-inline-5">
                      <input type="number" v-model.number="selectWg.value.limit" :min="1" class="layui-input">
                    </div>
                  </div>
                  <div class="pl25">
                    <p class="layout-tip">
                      文章分类数据请到 运营管理 - <a href="javascript:;" lay-href="content/article/articletype/">文章列表</a> 中管理
                    </p>
                  </div>
                </div>
              </template>
              <!-- 视频组 -->
              <template v-if="selectWg.type == 'video'">
                <el-form-item label="自动播放">
                  <el-switch v-model="selectWg.value.autoplay" active-value="true" inactive-value="false"
                    active-color="#13ce66" inactive-color="#ff4949">
                  </el-switch>
                </el-form-item>
                <el-option :value="item.key" v-for="(item, index) in selectWg.value.list" :key="index">
                  <div class="content">
                    <div class="content-item">
                      <span class="item-label">视频封面:</span>
                      <x-image v-model="item.url"></x-image>
                    </div>
                    <div class="content-item">
                      <span class="item-label">视频地址：</span>
                      <el-input size="small" placeholder="请输入视频地址" v-model="item.url"></el-input>
                    </div>
                  </div>
                </el-option>
              </template>
              <!-- 轮播图 -->
              <template v-if="selectWg.type == 'imgSlide'">
                <div>
                  <div class="content-item">
                    <span class="item-label">切换时间：</span>
                    <div class="number-content ml20">
                      <input type="number" :step="500" v-model.number="selectWg.value.duration" :min="0"
                        class="number-input">
                      <p class="layout-tip">轮播图自动切换的间隔时间，单位：毫秒</p>
                    </div>
                  </div>
                  <el-select element="ul" :list="selectWg.value.list"
                    :options="{ group: { name: 'slideList' }, ghostClass: 'ghost', animation: 150, handle: '.drag-block' }">
                    <el-option :value="item.key" v-for="(item, index) in selectWg.value.list" :key="index">
                      <div class="drag-block">
                        <div class="handle-icon" title="删除这一项">
                          <i class="iconfontCustom icon-cuohao" @click="handleSlideRemove(index)"></i>
                        </div>
                      </div>
                      <div class="content">
                        <div class="content-item">
                          <el-form-item label="图片：">
                            <x-image v-model="item.url"></x-image>
                          </el-form-item>
                        </div>
                        <select-link @choose-link="chooseLink(index, item.linkType)" :index="index"
                          :type.sync="item.linkType" :id.sync="item.linkValue"></select-link>
                      </div>
                    </el-option>
                  </el-select>
                  <div class="addImg" @click="handleAddSlide">
                    <i class="iconfontCustom icon-icon-test"></i>
                    <span>添加一个图片</span>
                  </div>
                </div>
              </template>
              <!-- 单图组 -->
              <template v-if="selectWg.type == 'imgSingle'">
                <el-option :value="item.key" v-for="(item, index) in selectWg.value.list" :key="index">
                  <div class="content">
                    <div class="content-item">
                      <span class="item-label">图片:</span>
                      <x-image v-model="item.url"></x-image>
                    </div>
                    <select-link @choose-link="chooseLink(index, item.linkType)" :index="index" :type.sync="item.linkType"
                      :id.sync="item.linkValue"></select-link>
                    <div class="content-item">
                      <el-form-item label="添加按钮：">
                        <el-switch v-model="item.buttonShow" active-color="#13ce66">
                        </el-switch>
                      </el-form-item>
                    </div>
                    <div class="content-item" v-show="item.buttonShow">
                      <el-form-item label="按钮颜色：">
                        <el-color-picker v-model="item.buttonColor"></el-color-picker>
                      </el-form-item>
                    </div>
                    <div class="content-item" v-show="item.buttonShow">
                      <el-form-item label="按钮文字：">
                        <input type="text" class="selectLinkVal" v-model="item.buttonText">
                      </el-form-item>
                    </div>
                    <div class="content-item" v-show="item.buttonShow">
                      <el-form-item label="文字颜色：">
                        <el-color-picker v-model="item.textColor"></el-color-picker>
                      </el-form-item>
                    </div>
                  </div>
                </el-option>
              </template>
              <!-- 图片橱窗 -->
              <template v-if="selectWg.type == 'imgWindow'">
                <div>
                  <div class="content-item">
                    <span class="item-label">布局方式:</span>
                    <div class="tpl-block">
                      <div class="tpl-item" v-for="(item, i) in imgWindowStyle" :key="i" @click="slectTplStyle(item)"
                        :class="{ active: selectWg.value.style == item.value }">
                        <div class="tpl-item-image">
                          <img :src="item.image" alt="">
                        </div>
                        <div class="tpl-item-text">
                          {{ item.title }}
                        </div>
                      </div>
                      <p class="layout-tip">建议添加比例/尺寸一致的图片</p>
                    </div>
                  </div>
                  <div class="content-item">
                    <el-form-item label="图片间距：">
                      <el-slider v-model="selectWg.value.margin" :min="0" :max="30">
                      </el-slider>
                    </el-form-item>
                    <span style="display:inline-block;height:30px;line-height:30px;font-size:12px;margin-left:10px;">{{
                      selectWg.value.margin }}px</span>
                  </div>
                  <el-select>
                    <el-option value="item.key" v-for="(item, index) in selectWg.value.list" v-bind:key="index">
                      <div class="drag-block">
                        <div class="handle-icon" title="删除这一项">
                          <i class="iconfontCustom icon-cuohao" @click="handleSlideRemove(item)"></i>
                        </div>
                      </div>
                      <div class="content">
                        <div class="content-item">
                          <el-form-item label="图片：">
                            <x-image v-model="item.url"></x-image>
                          </el-form-item>
                        </div>
                        <select-link @choose-link="chooseLink(index, item.linkType)" :index="index"
                          :type.sync="item.linkType" :id.sync="item.linkValue"></select-link>
                      </div>
                    </el-option>
                  </el-select>
                  <div class="addImg" @click="handleAddSlide">
                    <i class="iconfontCustom icon-icon-test"></i>
                    <span>添加一个图片</span>
                  </div>
                </div>
              </template>
              <!-- 公告 -->
              <template v-if="selectWg.type == 'notice'">
                <el-form-item label="公告获取">
                  <el-radio-group v-model="selectWg.value.type">
                    <el-radio label="auto">自动获取</el-radio>
                    <el-radio label="choose">手动选择</el-radio>
                  </el-radio-group>
                </el-form-item>
                <div v-if="selectWg.value.type == 'choose'">
                  <div id="n15578855354_box" class="select_seller_notice_box">
                    <el-select id="n15578855354_list" class="sellect_seller_brands_list">
                     
                        <el-option :value="notice.key" v-for="(notice, key) in selectWg.value.list" :key="key">
                          <span>
                            <i class="layui-icon layui-icon-close" @click="handleDeleteNotice(key)"></i>
                          </span>{{ notice.title }}
                        </el-option>
                      </el-select>
                  </div>
                  <div>
                    <a href="javascript:;" class="layui-btn layui-btn-xs" @click="selectNotice">
                      <i class="iconfontCustom icon-choose1"></i>选择公告
                    </a>
                  </div>
                </div>
                <div class="pl25">
                  <p class="layout-tip">
                    公告数据请到 运营管理 - <a href="javascript:;" lay-href="content/notice/">公告列表</a> 中管理
                  </p>
                </div>
              </template>
              <!-- 导航组-->
              <template v-if="selectWg.type == 'navBar'">
                <div>
                  <el-form-item label="每行数量">
                    <el-radio-group v-model="selectWg.value.limit">
                      <el-radio :label="3">3个</el-radio>
                      <el-radio :label="4">4个</el-radio>
                      <el-radio :label="5">5个</el-radio>
                    </el-radio-group>
                  </el-form-item>
                  <el-select element="ul" :list="selectWg.value.list"
                    :options="{ group: { name: 'slideList' }, ghostClass: 'ghost', animation: 150, handle: '.drag-block' }">
                    <el-option :value="item.key" v-for="(item, index) in selectWg.value.list" :key="index">
                      <div class="drag-block">
                        <div class="handle-icon" title="删除这一项">
                          <i class="iconfontCustom icon-cuohao" @click="handleSlideRemove(index)"></i>
                        </div>
                      </div>
                      <div class="content">
                        <div class="content-item">
                          <el-form-item label="图片：">
                            <x-image v-model="item.url" />
                          </el-form-item>
                        </div>
                        <div class="content-item">
                          <el-form-item label="按钮文字：">
                            <input type="text" class="selectLinkVal" v-model="item.text">
                          </el-form-item>
                        </div>
                        <select-link @choose-link="chooseLink(index, item.linkType)" :index="index"
                          :type.sync="item.linkType" :id.sync="item.linkValue"></select-link>
                      </div>
                    </el-option>
                  </el-select>
                  <div class="addImg" @click="handleAddNav">
                    <i class="iconfontCustom icon-icon-test"></i>
                    <span>添加一个导航组</span>
                  </div>
                </div>
              </template>
              <!-- 文本域 -->
              <template v-if="selectWg.type == 'textarea'">
                <div>
                  <div class="document-editor">
                    <div class="toolbar-container" id="toolbar-container"></div>
                    <div class="content-container">
                      <div id="container" class="core-editor"></div>
                    </div>
                  </div>
                  <!--<textarea id="container"></textarea>-->
                </div>
              </template>
            </el-form>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
<style scoped lang="scss">
.component-item {
  width: 100%;
  padding: 3px 0;
  margin: 5px;
  text-align: center;
  display: block;
  background-color: #f5f5f5;
  border: 1px solid #ddd;
  font-size: 12px;
  color: #333;
  cursor: pointer;
  -webkit-transition: all 0.5s;
  transition: all 0.5s;
  height: auto;
  text-align: center;

  .component-icon {

    *,
    *:before,
    *:after {
      box-sizing: inherit;
    }

    >svg {
      display: inline;
    }
  }
}

.center-container {
  width: 375px;
  height: 806px;
  border: 1px solid #e9e9e9;
  box-shadow: 0 3px 10px #dcdcdc;
  position: relative;

  >.model-title {
    height: 88px;
    width: 100%;
    position: relative;

  }

  .layout-list {
    height: 710px;
    overflow-y: scroll;
  }

  .lay-imgWindow {
    min-height: 100px;
  }
}
</style>