import { h, defineComponent, VNode } from "vue";

export default defineComponent({
  props: {
    value: {
      type: [String, Number],
      required: true
    },
    fixed: {
      type: [String, Number],
      default: 2
    },
    append: {
      type: String,
      default: ""
    }
  },
  render() {
    const value = this.$props.value;
    const raw = filters.number(value, +this.$props.fixed);
    const segs = raw.split(".");
    const children: Array<string | VNode> = [segs[0]];
    if (value && +this.$props.fixed > 0) {
      children.push(
        h(
          "span",
          {
            class: ["decimals-places-span"],
            style: { fontSize: "50%" }
          },
          "." + (segs.length == 2 ? segs[1] : 0) + this.$props.append
        )
      );
    }
    return h("span", { class: ["decimals-span"] }, children);
  }
});
