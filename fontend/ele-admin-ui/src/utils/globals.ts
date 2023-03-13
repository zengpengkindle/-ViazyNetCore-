import { ref, Plugin, ComponentPublicInstance, h, DirectiveBinding } from "vue";
import {
  ElMessage,
  ElMessageBox,
  ElLoading,
  ElNotification,
  NotificationHandle
} from "element-plus";
import moment from "moment";
import type { ElForm, ElFormItem } from "element-plus";
import { RouteLocationRaw } from "vue-router";
import useClipboard from "vue-clipboard3";

function toString(message?: any) {
  if (message === null) return "";

  if (typeof message === "string") {
    return h("div", { innerHTML: message });
  }
  var kind = typeof message;

  switch (kind) {
    case "undefined":
      return "";
    case "number":
    case "boolean":
    case "bigint":
      return message.toString();
  }
  return message;
}

Date.prototype.clearTime = function () {
  this.setHours(0);
  this.setMinutes(0);
  this.setSeconds(0);
  this.setMilliseconds(0);
  return this;
};

Date.today = function () {
  var now = new Date();
  return now.clearTime();
};

var DAY_MS = 3600 * 1000 * 24;

Date.prototype.lastWeek = function (weeks) {
  return this.addDate((weeks || 1) * -7)
};
Date.prototype.lastMonth = function (months) {
  return this.addDate((months || 1) * -30);
};
Date.prototype.lastYear = function (years) {
  return this.addDate((years || 1) * -365);
};
Date.prototype.thisWeek = function (weeks) {
  var start = this;
  var weekDay = start.getDay();
  if (weekDay == 0) weekDay = 7;
  weekDay -= 1;
  return this.addDate(-weekDay).addDate((weeks || 0) * 7);
};
Date.prototype.thisMonth = function (months) {
  var start = this;
  var monthDay = start.getDate();
  monthDay -= 1;
  start = this.addDate(-monthDay);
  if (months) {
      start.setMonth(start.getMonth() + months);
  }
  return start;
};
Date.prototype.addDate = function (days) {
  var start = new Date(this.getTime());
  if (days) start.setTime(start.getTime() + DAY_MS * (days || 0));
  return start;
};

window.msg = {
  confirm(message?: any) {
    return new Promise(resolve => {
      ElMessageBox.confirm(toString(message))
        .then(action => {
          resolve(action === "confirm");
        })
        .catch(() => {
          resolve(false);
        });
    });
  },
  alert(message, title, type) {
    return ElMessageBox.alert(toString(message), title, { type });
  },
  success(message?: any) {
    return ElMessage.success({ message: toString(message) });
  },
  info(message?: any) {
    return ElMessage.info({ message: toString(message) });
  },
  warning(message?: any) {
    return ElMessage.warning({ message: toString(message) });
  },
  error(message?: any) {
    return ElMessage.error({ message: toString(message) });
  },
  async prompt(message?: any, value?: string) {
    return new Promise(resolve => {
      ElMessageBox.prompt(toString(message), { inputValue: value })
        .then(data => {
          if (data.action == "confirm") resolve(data.value);
          else resolve(null);
        })
        .catch(() => {
          resolve(null);
        });
    });
  },
  loading(message?: any, runAfterMS = 1100) {
    const my = this;
    if (my.$loading) {
      my.$loading.text.value = toString(message);
      return;
    }
    my.$loadingTimeout = setTimeout(() => {
      my.$loading = ElLoading.service({
        lock: true,
        text: ref<string>(toString(message || "加载中……")),
        fullscreen: true,
        background: "rgba(0, 0, 0, 0.333)"
      });
    }, runAfterMS);
  },
  loadingClose() {
    const my: any = this;
    my.$loadingTimeout && clearTimeout(my.$loadingTimeout);
    my.$loading && my.$loading.close();
    my.$loadingTimeout = null;
    my.$loading = null;
  }
};

function showNotify(
  type: "" | "success" | "warning" | "info" | "error",
  message: string,
  title?: string
): NotificationHandle {
  return ElNotification({
    type,
    title: title == "#" ? "" : title,
    message: toString(message),
    duration: 8000,
    position: "top-right",
    dangerouslyUseHTMLString: true
  });
}

window.notify = {
  alert(message, title) {
    return showNotify("", message, title);
  },
  success(message, title) {
    return showNotify("success", message, title || "Success");
  },
  info(message, title) {
    return showNotify("info", message, title || "提示");
  },
  warning(message, title) {
    return showNotify("warning", message, title || "警示");
  },
  error(message, title) {
    return showNotify("error", message, title || "错误");
  }
};

function parseFixed(fixed?: number | string) {
  if (fixed === 0 || fixed === "0") return 0;
  if (!fixed) return 2;
  return +fixed;
}

function toThousands(isNegative: boolean, value: number) {
  if (value == 0) return isNegative ? "-0" : "0";
  let num = (value || 0).toString(),
    result = "";
  if (isNegative) num = num.substr(1);

  while (num.length > 3) {
    result = "," + num.slice(-3) + result;
    num = num.slice(0, num.length - 3);
  }
  if (num) {
    result = num + result;
  }
  return (isNegative ? "-" : "") + result;
}

window.filters = {
  number(val, fixed) {
    if (val === undefined || val === null || val === "") return "N/A";
    const a = +val;
    if (a == 0) return "N/A";
    const r = a.toFixed(
      parseFixed(fixed === undefined || fixed === null ? 2 : fixed)
    );
    const segs = r.split(".");
    let ir = toThousands(a < 0, +segs[0]);
    if (segs.length == 2) {
      ir += "." + segs[1];
    }

    return ir;
  },
  date(val, format) {
    if (!val) return val;
    if (typeof val === "string" && val.startsWith("0001-01-01T")) return "";
    if (!format) format = "L LTS";

    if (format == "now") return moment(val).fromNow();
    return moment(val).format(format);
  },
  zone(val, timeOffset, format) {
    if (!val) return val;
    if (!format) format = "L LTS";
    let segs = timeOffset.split(":");
    timeOffset = segs[0] + segs[1];
    if (timeOffset.length < 5) timeOffset = "+" + timeOffset;

    if (format == "now") return moment(val).utcOffset(timeOffset).fromNow();
    return moment(val).utcOffset(timeOffset).format(format);
  },
  calendar(val, format) {
    if (!val) return val;

    let options = {
      sameDay: `[今日]LTS`,
      nextDay: `[明日]LTS`,
      nextWeek: "L LTS",
      lastDay: `[昨日]LTS`,
      lastWeek: "L LTS",
      sameElse: "L LTS"
    };
    if (format == "date") {
      options = {
        sameDay: `[今日]`,
        nextDay: `[明日]`,
        nextWeek: "L",
        lastDay: `[昨天]`,
        lastWeek: "L",
        sameElse: "L"
      };
    } else if (format == "time") {
      return moment(val).format("LTS");
    }

    return moment(val).calendar(null, options);
  }
};

export default (): Plugin => {
  return {
    install(app) {
      app.config.globalProperties.$filters = window.filters;

      app.config.globalProperties.$ref = function <TElement>(
        this: ComponentPublicInstance,
        name: string
      ) {
        return this.$refs[name] as TElement;
      };
      app.config.globalProperties.$pickerOptions = {
        shortcuts: [{
          text: '今天',
          value: () => {
            var end = Date.today();
            var start = Date.today();
            return [start, end]
          }
        }, {
          text: '昨天',
          value: () => {
            var end = Date.today().addDate(-1);
            var start = Date.today().addDate(-1);
            return [start, end]
          }
        }, {
          text: '本周',
          value: () => {
            var end = Date.today();
            var start = Date.today().thisWeek();
            return [start, end]
          }
        }, {
          text: '上周',
          value: () => {
            var start = Date.today().thisWeek(-1);
            var end = start.addDate(6);
            return [start, end]
          }
        }, {
          text: '本月',
          value: () => {
            var end = Date.today();
            var start = Date.today().thisMonth();
            return [start, end]
          }
        }, {
          text: '上月',
          value: () => {
            var start = Date.today().thisMonth(-1);
            var end = start.thisMonth(1).addDate(-1);
            return [start, end]
          }
        }, {
          text: '最近一周',
          value: () => {
            var end = Date.today();
            var start = Date.today().lastWeek();
            return [start, end]
          }
        }, {
          text: '最近一个月',
          onClick: function (picker) {
            var end = Date.today();
            var start = Date.today().lastMonth();
            return [start, end]
          }
        }, {
          text: '最近三个月',
          onClick: function (picker) {
            var end = Date.today();
            var start = Date.today().lastMonth(3);
            return [start, end]
          }
        }]
      };
      app.config.globalProperties.$validForm = function (
        this: ComponentPublicInstance,
        name: string,
        onSuccess: Function,
        onFail?: Function
      ): void {
        const elform = this.$ref<typeof ElForm>(name);
        if (!elform) {
          onSuccess.call(this);
        } else {
          elform.validate((success, items) => {
            if (success) onSuccess.call(this);
            else {
              const firstItem = items[Object.keys(items)[0]][0];
              const elfield = (elform["fields"] as (typeof ElFormItem)[]).find(
                f => f.prop == firstItem.field
              );
              if (elfield) {
                msg.error(`[${elfield.label}] ` + firstItem.message).then(r => {
                  onFail && onFail(elfield);
                });
              }
            }
          });
        }
      };
      app.config.globalProperties.$valid = function (
        this: ComponentPublicInstance,
        onSuccess: Function,
        onFail?: Function
      ): void {
        this.$validForm("form", onSuccess, onFail);
      };

      app.config.globalProperties.$run = function <T extends IResult | void>(
        this: LoadingComponentPublicInstance,
        fc: ApiCallback<T>,
        loadingText?: string
      ): boolean {
        const vm = this;
        if (this.loading) {
          return false;
        }
        this.loading = true;
        msg.loading(loadingText);
        const cancelCallback = function () {
          msg.loadingClose();
          setTimeout(() => {
            vm.loading = false;
          }, 366);
        };
        let r;
        try {
          r = fc.call(this);
        } catch (e) {
          console.error(e);
          return false;
        }

        if (typeof r === "string") {
          msg.error(r);
          cancelCallback();
          return false;
          //r = Promise.resolve<IResult>({ message: r, status: -1 });
        }

        if (Promise.resolve(r) == r) {
          r = r.finally(cancelCallback);
        } else {
          cancelCallback();
        }
        return true;
      };

      //app.config.globalProperties.$submitConfirm = function <T extends IResult | void>(this: ComponentPublicInstance, fc: ApiCallback<T>, op?: string) {
      //    let tip = "提示";
      //    if (op) tip = `<div><p style="color:red;text-align:center;font-size:large;"><strong>${op}</strong></p><p>${tip}</p></div>`;
      //    this.$submit(tip, fc);
      //}
      app.config.globalProperties.$n0 = function (
        this: ComponentPublicInstance,
        val: string | number
      ) {
        return this.$filters.number(val, 0);
      };

      app.config.globalProperties.$n6 = function (
        this: ComponentPublicInstance,
        val: string | number
      ) {
        return this.$filters.number(val, 6);
      };
      app.config.globalProperties.$number = function (
        this: ComponentPublicInstance,
        val: string | number,
        fixed?: number | string
      ) {
        return this.$filters.number(val, fixed);
      };
      app.config.globalProperties.$date = function (
        this: ComponentPublicInstance,
        val: any,
        format?: string
      ) {
        return this.$filters.date(val, format);
      };
      app.config.globalProperties.$copyText = function (
        this: ComponentPublicInstance,
        text: string
      ) {
        const { toClipboard } = useClipboard();
        toClipboard(text).then(
          () => {
            msg.success("复制" + " <b>" + text + "<b>");
          },
          () => {
            msg.error("复制失败");
          }
        );
      };

      interface BindingHtmlElement extends HTMLElement {
        __BINDING_EVENT?: boolean;
        binding: DirectiveBinding;
      }

      app.directive("class", function (el: HTMLElement, binding) {
        const key = binding.arg;
        if (key) {
          el.classList.remove(key);
          binding.value && el.classList.add(key);
        }
      });

      app.directive("copy", function (el: BindingHtmlElement, b) {
        el.binding = b;
        if (el.__BINDING_EVENT) return;
        el.__BINDING_EVENT = true;
        const eventName = b.arg == "dbclick" ? "dblclick" : "click";
        el.addEventListener(eventName, $event => {
          $event.preventDefault();
          el.binding.instance?.$copyText(el.binding.value);
        });
      });
      app.directive("router", function (el: BindingHtmlElement, b) {
        el.binding = b;
        if (el.__BINDING_EVENT) return;
        el.__BINDING_EVENT = true;

        el.addEventListener("click", $event => {
          $event.preventDefault();
          if (!el.binding.instance) return;

          const path = el.getAttribute("href") || el.getAttribute("url");
          if (!path) {
            return;
          }

          const routeData: RouteLocationRaw = { path };
          const query = el.binding.value;
          if (query) {
            routeData.query = query;
          }

          if (el.binding.modifiers.top) {
            const { href } = el.binding.instance.$router.resolve(routeData);
            window.open(href, "_blank");
            return;
          }
          el.binding.instance.$router.push(routeData);
        });
      });
    }
  };
};
