import { nextTick, reactive, Ref, ref } from "vue";
import ModelTitle from "@/assets/images/model-title.png";
import EmptyBanner from "@/assets/images/empty-banner.png";
import Empty from "@/assets/images/empty.png";
import ImangeFourColumn from "@/assets/images/image-four-column.png";
import ImangeOneColumn from "@/assets/images/image-one-column.png";
import ImangeOneLeft from "@/assets/images/image-one-left.png";
import ImangeThreeColumn from "@/assets/images/image-three-column.png";
import IcCar from "@/assets/images/ic-car.png";
import { ElMessageBox } from "element-plus";

export interface ComponentItem {
    newIndex?: number;
    key: string;
    type: 'search' | 'record' | 'coupon' | 'blank' | 'goods' | 'choose'
    | 'goodTabBar' | 'article' | 'articleClassify' | 'video'
    | 'imgSlide' | 'imgSingle' | 'imgWindow' | 'notice'
    | 'navBar' | 'textarea';
    value: any;
    placeholder?: string;
}
export const allWidget = {
    "mediaComponents": [{
        "type": "imgSlide",
        "name": "图片轮播",
        "value": {
            "duration": 2500,
            "list": [{
                "image": EmptyBanner,
                "linkType": '',
                "linkValue": ''
            },
            {
                "image": EmptyBanner,
                "linkType": '',
                "linkValue": ''
            }
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "imgSingle",
        "name": "图片",
        "value": {
            "list": [{
                "image": EmptyBanner,
                "linkType": '',
                "linkValue": '',
                "buttonShow": false,
                "buttonText": '',
                "buttonColor": "#FFFFFF",
                "textColor": "#000000"
            }]
        }, "icon": "ep:add-location"
    },
    {
        "type": "imgWindow",
        "name": "图片分组",
        "value": {
            "style": 2,  // 0 橱窗  2 两列 3三列 4四列
            "margin": 0,
            "list": [
                {
                    "image": EmptyBanner,
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": EmptyBanner,
                    "linkType": '',
                    "linkValue": ''
                }, {
                    "image": EmptyBanner,
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": EmptyBanner,
                    "linkType": '',
                    "linkValue": ''
                }
            ]
        }, "icon": "ep:add-location"
    },
    {
        "type": "video",
        "name": "视频组",
        "value": {
            "autoplay": "false",
            "list": [{
                "image": EmptyBanner,
                "url": "",
                "linkType": '',
                "linkValue": ''
            }]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "article",
        "name": "文章组",
        "value": {
            "list": [
                {
                    "title": ''
                }
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "articleClassify",
        "name": "文章分类",
        "value": {
            "limit": 3,
            "articleClassifyId": ''
        },
        "icon": "ep:add-location"
    }
    ],
    "storeComponents": [{
        "type": "search",
        "name": "搜索框",
        "value": {
            "keywords": '请输入关键字搜索',
            "style": 'round' // round:圆弧 radius:圆角 square:方形
        },
        "icon": "ep:add-location"
    },
    {
        "type": "notice",
        "name": "公告组",
        "value": {
            "type": 'auto', //choose手动选择， auto 自动获取
            "list": [
                {
                    "title": "这里是第一条公告的标题",
                    "content": "",
                    "id": ''
                }
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "navBar",
        "name": "导航组",
        "value": {
            "limit": 4,
            "list": [
                {
                    "image": Empty,
                    "text": "按钮1",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": Empty,
                    "text": "按钮2",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": Empty,
                    "text": "按钮3",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": Empty,
                    "text": "按钮4",
                    "linkType": '',
                    "linkValue": ''
                }
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "goods",
        "name": "商品组",
        "icon": "ep:add-location",
        "value": {
            "title": '商品组名称',
            "lookMore": "true",
            "type": "auto", //auto自动获取  choose 手动选择
            "classifyId": '', //所选分类id
            "brandId": '', //所选品牌id
            "limit": 10,
            "display": "list", //list , slide
            "column": 2, //分裂数量
            "list": [
                {
                    "image": EmptyBanner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": EmptyBanner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": EmptyBanner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": EmptyBanner,
                    "name": '',
                    "price": ''
                }
            ]
        },
    },
    {
        "type": "goodTabBar",
        "name": "商品选项卡",
        "icon": "ep:add-location",
        "value": {
            "isFixedHead": "true",//是否固定头部
            "list": [
                {
                    "title": '选项卡名称一',
                    "subTitle": '子标题一',
                    "type": "auto", //auto自动获取  choose 手动选择
                    "classifyId": '', //所选分类id
                    "brandId": '', //所选品牌id
                    "limit": 10,
                    "column": 2, //分裂数量
                    "isShow": true,
                    "list": [
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        }
                    ],
                    "hasChooseGoods": [],
                },
                {
                    "title": '选项卡名称二',
                    "subTitle": '子标题二',
                    "type": "auto", //auto自动获取  choose 手动选择
                    "classifyId": '', //所选分类id
                    "brandId": '', //所选品牌id
                    "limit": 10,
                    "column": 2, //分裂数量
                    "isShow": true,
                    "list": [
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": EmptyBanner,
                            "name": '',
                            "price": ''
                        }
                    ],
                    "hasChooseGoods": [],
                }
            ]
        },
    },
    {
        "type": "groupPurchase",
        "name": "团购秒杀",
        "value": {
            "title": '活动名称',
            "limit": '10',
            "list": [
                {
                    "image": EmptyBanner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": EmptyBanner,
                    "name": '',
                    "price": ''
                },
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "pinTuan",
        "name": "拼团",
        "value": {
            "title": '活动名称',
            "limit": '10',
            "list": [
                {
                    "goodsImage": EmptyBanner,
                    "name": '',
                    "price": ''
                },
                {
                    "goodsImage": EmptyBanner,
                    "name": '',
                    "price": ''
                },
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "coupon",
        "name": "优惠券组",
        "value": {
            "limit": '2'
        },
        "icon": "ep:add-location"
    },
    {
        "type": "service",
        "name": "服务组",
        "value": {
            "title": '推荐服务卡',
            "limit": '10',
            "list": [
                {
                    "thumbnail": EmptyBanner,
                    "title": '',
                    "money": ''
                },
                {
                    "thumbnail": EmptyBanner,
                    "title": '',
                    "money": ''
                },
            ]
        },
        "icon": "ep:add-location"
    },
    {
        "type": "record",
        "name": "购买记录",
        "value": {
            "style": {
                "top": 20,
                "left": 0
            }
        },
        "icon": "ep:add-location"
    }
    ],
    "utilsComponents": [
        {
            "type": "blank",
            "name": "辅助空白",
            "icon": "ep:add-location",
            "value": {
                "height": 20,
                "backgroundColor": "#FFFFFF"
            },
        },
        {
            "type": "textarea",
            "name": "文本域",
            "value": '',
            "icon": "ep:add-location"
        }]
};

var deepClone = function (obj) {
    let result = Array.isArray(obj) ? [] : {}
    for (let key in obj) {
        if (obj.hasOwnProperty(key)) {
            if (typeof obj[key] === 'object') {
                result[key] = deepClone(obj[key]) //递归复制
            } else {
                result[key] = obj[key]
            }
        }
    }
    return result
}

export function useWidget() {
    const pageData = ref([]);
    const selectWg: Ref<ComponentItem> = ref()
    function setSelectWg(data: ComponentItem) {
        selectWg.value = data;
    }
    function handleWidgetAdd(evt) {
        if (evt.added) {
            const element=evt.added;
            console.log("handleWidgetAdd", evt.added.element);
            console.log("handleWidget pageData", pageData.value);
            var newIndex = element.newIndex;
            var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
            var newObj = deepClone(pageData.value[newIndex]) as any;
            newObj.key = pageData.value[newIndex].type + '_' + elKey
            pageData.value[newIndex] = newObj
            setSelectWg(pageData.value[newIndex])
        }
    }
    function handleClickAdd(obj: ComponentItem) {
        var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
        var newObj = deepClone(obj) as any;
        newObj.key = obj.type + '_' + elKey;
        var newIndex = pageData.value.length || 0;
        pageData.value[newIndex] = newObj
        setSelectWg(pageData.value[newIndex])
    }
    function handleSelectWidget(element) {
        setSelectWg(element);
    }
    function handleSelectRecord(index) {
        setSelectWg(pageData.value[index])
    }
    function deleteWidget(index) {
        if (pageData.value.length - 1 === index) {
            if (index === 0) {
                setSelectWg(null)
            } else {
                setSelectWg(pageData.value[index - 1])
            }
        } else {
            setSelectWg(pageData.value[index + 1])
        }
        nextTick(() => {
            pageData.value.splice(index, 1)
        })
    }
    function handleWidgetDelete(deleteIndex) {
        ElMessageBox.confirm('确定要删除吗？','提示', {
            confirmButtonText: '确认',
            cancelButtonText: '取消',
            type: 'warning',
          }).then(()=>{
            deleteWidget(deleteIndex);
          })
        // layer.open({
        //     title: '提示',
        //     content: '确定要删除吗？',
        //     btn: ['确定', '取消'],
        //     yes: function (index, layero) {
        //         that.deleteWidget(deleteIndex);
        //         layer.close(index)
        //     },
        //     btn2: function () {
        //         return
        //     }
        // });

    }
    function handleWidgetClone(index: number) {
        let cloneData = deepClone(pageData.value[index]) as any;
        cloneData.key =
            pageData.value[index].type +
            '_' +
            Date.now() +
            '_' +
            Math.ceil(Math.random() * 1000000)
        pageData.value.splice(index, 0, cloneData)
        nextTick(() => {
            setSelectWg(pageData.value[index + 1])
        })
    }
    function handleDragRemove(evt) {
        setSelectWg(null);
    }
    function datadragEnd(evt) {

    }
    const selectWidget = (type: string) => {
        for (var key in allWidget) {
            for (var index = 0; index < allWidget[key].length; index++) {
                var element = allWidget[key][index];
                if (element.type == type) {
                    handleClickAdd(element)
                }
            }
        }
    }
    return {
        pageData,
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
    }
}