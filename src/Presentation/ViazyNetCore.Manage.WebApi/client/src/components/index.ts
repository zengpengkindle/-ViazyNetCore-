import { Plugin, defineAsyncComponent } from 'vue';
import { Column } from './column';

export default function createComponents(): Plugin {
    return {
        install(app) {
            const vueUIs = import.meta.glob('./ui/*.vue');
            Object.keys(vueUIs).forEach(path => {
                let name = path.substr(5);
                name = name.substr(0, name.length - 4);
                app.component('x-' + name, defineAsyncComponent(vueUIs[path]));
            });

            const tsUIs = import.meta.glob('./ui/*.ts');

            Object.keys(tsUIs).forEach(path => {
                let name = path.substr(5);
                name = name.substr(0, name.length - 3);
                app.component('x-' + name, defineAsyncComponent(tsUIs[path]));
            });

            //- 若不采用此方法，多级表头将会无法显示
            app.component('x-column', Column);
        }
    }
}