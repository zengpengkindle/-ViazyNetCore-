import type { Themes } from "@/utils/themes";

declare module "@vue/runtime-core" {
  export interface ComponentCustomProperties {
    $themes: Themes;
  }
}
