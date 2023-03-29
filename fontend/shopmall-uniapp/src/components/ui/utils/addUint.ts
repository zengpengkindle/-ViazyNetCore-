function number(value) {
  return /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
}

export default function addUnit(
  value: string | number = "auto",
  unit = "rpx"
): string {
  value = String(value);
  // 用uView内置验证规则中的number判断是否为数值
  return number(value) ? `${value}${unit}` : value;
}
