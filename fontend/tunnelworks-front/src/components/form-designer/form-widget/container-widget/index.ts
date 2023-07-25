const modules = import.meta.globEager("./*.vue");

export default {
  install(app) {
    for (const path in modules) {
      const cname = modules[path].default.name;
      app.component(cname, modules[path].default);
    }
  }
};
