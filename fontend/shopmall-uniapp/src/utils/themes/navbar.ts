//通用头部背景样式
export const mainNavBar: MainNavBar = {
  background: {
    //颜色
    // backgroundColor: "#007aff",
    // 导航栏背景图
    // background: 'url(https://cdn.uviewui.com/uview/swiper/1.jpg) no-repeat',
    // 还可以设置背景图size属性
    // backgroundSize: 'cover',

    // 渐变色
    backgroundImage: "linear-gradient(45deg, #007aff, #005acf)"
  },
  //通用头部文字颜色
  titleColor: "#ffffff",
  //通用头部文字颜色
  backIconColor: "#ffffff"
};
export interface MainNavBar {
  background?: BackGround;
  titleColor?: string;
  backIconColor?: string;
}
interface BackGround {
  backgroundColor?: string;
  background?: string;
  backgroundSize?: string;
  backgroundImage?: string;
}
