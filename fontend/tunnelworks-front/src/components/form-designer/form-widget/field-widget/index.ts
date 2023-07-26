// const modules = import.meta.globEager("./*.vue");

// export default {
//   install(app) {
//     console.log("modules", modules);
//     for (const path in modules) {
//       const cname = modules[path].default.name;
//       app.component(cname, modules[path].default);
//     }
//   }
// };

const comps = {};

const modules = import.meta.globEager("./*.vue");
for (const path in modules) {
  const cname = modules[path].default.name;
  comps[cname] = modules[path].default;
}

export default comps;
