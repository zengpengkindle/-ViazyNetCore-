export type MenuItem = {
  title: string;
  path: string;
  icon: string;
  desc?: string;
  descColor?: string;
  checkLogin?: boolean;
};
export type MenuGroup = MenuItem[];
