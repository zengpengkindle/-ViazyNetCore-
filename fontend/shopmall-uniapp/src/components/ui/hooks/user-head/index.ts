import { ref } from "vue";

export type BoundingRect = {
  width: number;
  height: number;
  left: number;
  right: number;
  top: number;
  bottom: number;
};

const boundingRect = ref<BoundingRect>({
  bottom: 0,
  height: 0,
  left: 0,
  right: 0,
  top: 0,
  width: 0
});

export function useHeader() {
  return {
    boundingRect
  };
}
export function GetRect(
  instance: any,
  selector: any,
  all = null
): Promise<UniApp.NodeInfo | UniApp.NodeInfo[]> {
  return new Promise(resolve => {
    uni
      .createSelectorQuery()
      .in(instance)
      [all ? "selectAll" : "select"](selector)
      .boundingClientRect(rect => {
        if (all && Array.isArray(rect) && rect.length) {
          resolve(rect as UniApp.NodeInfo[]);
        }
        if (!all && rect) {
          resolve(rect as UniApp.NodeInfo);
        }
      })
      .exec();
  });
}
