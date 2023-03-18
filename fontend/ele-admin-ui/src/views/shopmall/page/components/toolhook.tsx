import { Ref, ref } from "vue";


export function useTools() {
    const catList: Ref<any> = ref([]);
    const brandList: Ref<any> = ref([]);

    function getSelectWgName(type: string) {

    }

    function resetColor() {

    }
    // 商品
    function changeGoodsType() {

    }
    function handleDeleteGoods(key: number) {

    }

    function selectGoods() {

    }

    // 商品选项卡
    function handleRemoveTabBar(key: number) {

    }
    function changeTabBarGoodsType(type: any, key: number) {

    }

    function handleDeleteTabBarGoods(key: number, index: number) {

    }

    function selectTabBarGoods(key: number) {

    }

    function handleAddTabBarGoods() {

    }

    // 文章
    function article_list() {

    }
    const articleTypeList: Ref<any> = ref([]);
    // 视频组
    function upImage(index: number, item: any) {

    }
    // 轮播图
    function chooseLink(index: number, item: any) {

    }
    function handleSlideRemove(index: number) {

    }
    function handleAddSlide() {

    }
    // 图片橱窗
    const imgWindowStyle: Ref<Array<any>> = ref([])
    function slectTplStyle(item:any){

    }
    // 公告
    function handleDeleteNotice(key: number) {

    }
    function selectNotice() {
    }

    // 导航组
    function handleAddNav(){

    }
    return {
        catList,
        brandList,
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
        articleTypeList,
        upImage,
        chooseLink,
        handleAddSlide,
        handleSlideRemove,
        imgWindowStyle,
        slectTplStyle,
        handleDeleteNotice,
        selectNotice,
        handleAddNav
    }
}

