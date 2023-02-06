module.exports = {
  publicPath: './knife4j',
  //outputDir: "dist",
  outputDir: '../../src/Infrastructure/ViazyNetCore.Swagger/knife4j',
  lintOnSave: false,
  css: {
    loaderOptions: {
      less: {
        javascriptEnabled: true
      }
    }
  },
  devServer: {
    proxy: {
      '/': {
        target: 'http://localhost:7284/',
        ws: true,
        changeOrigin: true
      }
    }
  }
}
