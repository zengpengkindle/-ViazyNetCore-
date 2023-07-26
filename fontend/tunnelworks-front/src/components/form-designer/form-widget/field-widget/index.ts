const modules = import.meta.glob("./*.vue");

export default {
  install(app) {
    // for (const path in modules) {
    //   const cname = modules[path].name;
    //   app.component(cname, modules[path].default);
    // }
    Object.keys(modules).forEach(key => {
      // routes.push(modules[key].default);
      const cname = key;
      app.component(cname, modules[key].default);
    });
  }
};
