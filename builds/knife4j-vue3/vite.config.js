import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import Components from 'unplugin-vue-components/vite'
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers'
import viteCompression from 'vite-plugin-compression';
import removeConsole from 'vite-plugin-remove-console';
import { resolve } from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  base: './',
  plugins: [
    vue(),
    vueJsx(),
    Components({
      resolvers: [AntDesignVueResolver()]
    })
    // viteCompression({
    //   deleteOriginFile: false, //删除源文件
    //   threshold: 10240, //压缩前最小文件大小
    //   algorithm: 'gzip', //压缩算法
    //   ext: '.gz', //文件类型
    // }),
    // removeConsole()
  ],
  define:{
    'process.env':{}
  },
  resolve: {
    alias: [
      { find: '@', replacement: resolve(__dirname, 'src') },
      { find: /^~/, replacement: '' },
    ]
  },
  // 开启less支持
  css: {
    preprocessorOptions: {
      less: {
        javascriptEnabled: true
      }
    }
  },
  server: {
    port: 5173,
    host: true,
    proxy: {
      '^/proxyApi': {
        target: `https://localhost:7284/`,
        secure: false,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/proxyApi/, '/swagger')
      }
    }
  },
  build: {
    outDir: '../../src/Infrastructure/ViazyNetCore.Swagger/',
    rollupOptions: {
      input: 'doc.html',
      output: {
        chunkFileNames: 'knife4j/js/[name]-[hash].js',
        entryFileNames: 'knife4j/js/[name]-[hash].js',
        assetFileNames: 'knife4j/[ext]/[name]-[hash].[ext]'
      }
    }
  }
})
