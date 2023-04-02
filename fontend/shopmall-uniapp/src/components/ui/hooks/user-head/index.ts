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
