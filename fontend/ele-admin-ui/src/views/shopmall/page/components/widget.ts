import { nextTick, reactive, Ref, ref } from "vue";

export interface ComponentItem {
    newIndex?: number;
    key: string;
    type: 'search'|'record'|'coupon'|'blank'|'goods'|'choose'
    |'goodTabBar'|'article'|'articleClassify'|'video'
    |'imgSlide'|'imgSingle'|'imgWindow'|'notice'
    |'navBar'|'textarea';
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
                "image": "/static/images/common/empty-banner.png",
                "linkType": '',
                "linkValue": ''
            },
            {
                "image": "/static/images/common/empty-banner.png",
                "linkType": '',
                "linkValue": ''
            }
            ]
        },
        "icon": "icon-lunbo"
    },
    {
        "type": "imgSingle",
        "name": "图片",
        "value": {
            "list": [{
                "image": "/static/images/common/empty-banner.png",
                "linkType": '',
                "linkValue": '',
                "buttonShow": false,
                "buttonText": '',
                "buttonColor": "#FFFFFF",
                "textColor": "#000000"
            }]
        },
        "icon": "icon-zhaopiantubiao"
    },
    {
        "type": "imgWindow",
        "name": "图片分组",
        "value": {
            "style": 2,  // 0 橱窗  2 两列 3三列 4四列
            "margin": 0,
            "list": [
                {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                }, {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                }
            ]
        },
        "icon": "icon-zidongchuchuang50"
    },
    {
        "type": "video",
        "name": "视频组",
        "value": {
            "autoplay": "false",
            "list": [{
                "image": "/static/images/common/empty-banner.png",
                "url": "",
                "linkType": '',
                "linkValue": ''
            }]
        },
        "icon": "icon-shipin"
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
        "icon": "icon-wenzhang1"
    },
    {
        "type": "articleClassify",
        "name": "文章分类",
        "value": {
            "limit": 3,
            "articleClassifyId": ''
        },
        "icon": "icon-wenzhangfenlei"
    }
    ],
    "storeComponents": [{
        "type": "search",
        "name": "搜索框",
        "value": {
            "keywords": '请输入关键字搜索',
            "style": 'round' // round:圆弧 radius:圆角 square:方形
        },
        "icon": "icon-sousuokuang"
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
        "icon": "icon-gonggao"
    },
    {
        "type": "navBar",
        "name": "导航组",
        "value": {
            "limit": 4,
            "list": [
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮1",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮2",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮3",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮4",
                    "linkType": '',
                    "linkValue": ''
                }
            ]
        },
        "icon": "icon-daohangliebiao"
    },
    {
        "type": "goods",
        "name": "商品组",
        "icon": "icon-shangpin",
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
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                }
            ]
        },
    },
    {
        "type": "goodTabBar",
        "name": "商品选项卡",
        "icon": "icon-shangpin",
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
                    "isShow":true,
                    "list": [
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
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
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
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
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
            ]
        },
        "icon": "icon-tuangou"
    },
    {
        "type": "pinTuan",
        "name": "拼团",
        "value": {
            "title": '活动名称',
            "limit": '10',
            "list": [
                {
                    "goodsImage": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "goodsImage": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
            ]
        },
        "icon": "icon-pinTuan"
    },
    {
        "type": "coupon",
        "name": "优惠券组",
        "value": {
            "limit": '2'
        },
        "icon": "icon-tubiao-youhuiquan"
    },
    {
        "type": "service",
        "name": "服务组",
        "value": {
            "title": '推荐服务卡',
            "limit": '10',
            "list": [
                {
                    "thumbnail": "/static/images/common/empty-banner.png",
                    "title": '',
                    "money": ''
                },
                {
                    "thumbnail": "/static/images/common/empty-banner.png",
                    "title": '',
                    "money": ''
                },
            ]
        },
        "icon": "icon-shangpinzu"
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
        "icon": "icon-jilu"
    }
    ],
    "utilsComponents": [
        {
            "type": "blank",
            "name": "辅助空白",
            "icon": 'icon-kongbai',
            "value": {
                "height": 20,
                "backgroundColor": "#FFFFFF"
            },
        },
        {
            "type": "textarea",
            "name": "文本域",
            "value": '',
            "icon": 'icon-fuwenben',
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

export function useWidget(){
    const pageData=reactive([]);
    const selectWg:Ref<ComponentItem>=ref()
    function setSelectWg(data:ComponentItem) {
        selectWg.value = data;
        console.log(selectWg.value.type)
    }
    function handleWidgetAdd (evt:ComponentItem) {
        var newIndex = evt.newIndex;
        var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
        var newObj = deepClone(pageData[newIndex]) as any;
        newObj.key = pageData[newIndex].type + '_' + elKey
        pageData[newIndex]= newObj
        setSelectWg(pageData[newIndex])
    }
    function handleClickAdd  (obj:ComponentItem) {
        var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
        var newObj = deepClone(obj) as any;
        newObj.key = obj.type + '_' + elKey;
        var newIndex = pageData.length || 0;
        pageData[newIndex]= newObj
        setSelectWg(pageData[newIndex])
    }
    function  handleSelectWidget(index) {
        setSelectWg(pageData[index])
    }
    function handleSelectRecord(index) {
        setSelectWg(pageData[index])
    }
    function  deleteWidget(index) {
        if (pageData.length - 1 === index) {
            if (index === 0) {
                setSelectWg(null)
            } else {
                setSelectWg(pageData[index - 1])
            }
        } else {
            setSelectWg(pageData[index + 1])
        }
        nextTick(() => {
            pageData.splice(index, 1)
        })
    }
    function  handleWidgetDelete(deleteIndex) {
        var that = this;
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
    function handleWidgetClone(index:number) {
        let cloneData = deepClone(pageData[index]) as any;
        cloneData.key =
            pageData[index].type +
            '_' +
            Date.now() +
            '_' +
            Math.ceil(Math.random() * 1000000)
        pageData.splice(index, 0, cloneData)
        nextTick(() => {
            setSelectWg(pageData[index + 1])
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