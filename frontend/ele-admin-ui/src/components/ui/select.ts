import { h, defineComponent, ComponentPublicInstance, VNode } from "vue";
import { ElSelect, ElOption } from "element-plus";

export function renderSelect(
  component: ComponentPublicInstance,
  createOption: (key: string, value: any) => VNode
) {
  const $props: Record<string, any> = component.$props;

  const children: VNode[] = [];

  //data.props = Object.assign({}, context.props);
  if ($props.block) {
    if (!$props.style) $props.style = {};
    $props.style["width"] = "100%";
  }
  const valid =
    $props.valid ||
    function () {
      return true;
    };

  for (const key in $props.options) {
    const value = $props.options[key];
    if (valid(key, value)) children.push(createOption(key, value));
  }
  const slots: Record<string, (props: Record<string, any>) => VNode[]> = {
    default(props) {
      if (component.$slots.default)
        return [...component.$slots.default(props), ...children];
      return children;
    }
  };
  return h(ElSelect, Object.assign({}, $props, component.$attrs), slots);
}

export const Select = defineComponent({
  props: {
    options: Object,
    placeholder: {
      type: String,
      default: "全部"
    },
    block: Boolean,
    valid: Function
  },
  render() {
    return renderSelect(this, (key, value) =>
      h(ElOption, {
        value: Number.isNaN(+key) ? key : +key,
        label: value
      })
    );
  }
});

export default Select;
