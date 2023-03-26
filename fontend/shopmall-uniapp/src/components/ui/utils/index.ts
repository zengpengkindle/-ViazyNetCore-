export * from "./vnode";
function hasOwn(node: any, name: string): boolean {
  return Object.prototype.hasOwnProperty.call.apply(
    Object.prototype.hasOwnProperty,
    [node, name]
  );
}

// 判断是否为 vnode 类型
export function isVNode(node: any): boolean {
  return (
    node !== null &&
    typeof node === "object" &&
    hasOwn(node, "componentOptions")
  );
}
