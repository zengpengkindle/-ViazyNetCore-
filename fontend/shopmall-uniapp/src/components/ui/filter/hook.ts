import { ref } from "vue";

export type FilterData = {
  overall: number;
  layout: number;
  sorts: "asc" | "desc" | "";
};

const sorts = ref("");
const color = ref("#1989fa");
const layout = ref(1);
const overall = ref(1);
const changeFilter = (filter: FilterData) => {
  sorts.value = filter.sorts;
  layout.value = filter.layout;
  overall.value = filter.overall;
};
export function useFilter() {
  return {
    sorts,
    color,
    layout,
    overall,
    changeFilter
  };
}
