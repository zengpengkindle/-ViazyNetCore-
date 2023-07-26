// const modules = import.meta.globEager("./*.vue");

// export default {
//   install(app) {
//     for (const path in modules) {
//       const cname = modules[path].default.name;
//       app.component(cname, modules[path].default);
//     }
//   }
// };
const modules = import.meta.glob("./*.vue");

export default {
  install(app) {
    Object.keys(modules).forEach(key => {
      const cname = key;
      app.component(cname, modules[key].default);
    });
  }
};
