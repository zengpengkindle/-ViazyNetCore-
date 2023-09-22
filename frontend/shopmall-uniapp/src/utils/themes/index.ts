import { mainNavBar, type MainNavBar } from "./navbar";

export * from "./navbar";
export interface Themes {
  MainNavBar: MainNavBar;
}
export const theme: Themes = {
  MainNavBar: mainNavBar
};
