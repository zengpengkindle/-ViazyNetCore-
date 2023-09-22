export async function asyncLogin() {
  return new Promise<UniApp.LoginRes>((resolve, reject) => {
    uni.login({
      provider: "weixin",
      success(res) {
        resolve(res);
      },
      fail() {
        reject();
      }
    });
  });
}
export async function asyncGetUserProfile() {
  return new Promise<UniApp.GetUserProfileRes>((resolve, reject) => {
    uni.getUserProfile({
      desc: "授权昵称、头像",
      success(res) {
        resolve(res);
      },
      fail() {
        reject();
      }
    });
  });
}

class ConcurrentCtrl {
  concurrentList: Array<any>;
  concurrent: number;
  running: number;

  constructor(concurrent: number) {
    this.concurrentList = []; // 并发列表
    this.concurrent = concurrent; // 并发数量
    this.running = 0;
  }
  run(fun: any) {
    return new Promise((resolve, reject) => {
      this.concurrentList.push({
        fun,
        resolve,
        reject
      });
      this.check();
    });
  }
  async check() {
    if (this.running >= this.concurrent) {
      return;
    }
    if (this.concurrentList.length === 0) {
      return;
    }
    const item = this.concurrentList.shift() as any;
    this.running++;
    try {
      const res = await item.fun();
      item.resolve(res);
    } catch (err) {
      item.reject(err);
    } finally {
      this.running--;
      this.check();
    }
  }
}

/**
 * 获取文件base64
 * @param path 文件地址
 * @returns
 */
export async function asyncReadFile(path: string) {
  return new Promise<UniApp.ReadFileSuccessCallbackResult>(
    (resolve, reject) => {
      uni.getFileSystemManager().readFile({
        filePath: path,
        encoding: "base64",
        success: r => {
          resolve(r);
        },
        fail: e => {
          reject(e);
        }
      });
    }
  );
}
const uploadImageConcurrentCtrl = new ConcurrentCtrl(2);
async function concurrentUploadImage(b64: string) {
  return await uploadImageConcurrentCtrl.run(() => {
    //return uploadImage(b64);
  });
}

export async function downloadSaveFile(url: string) {
  const dres = (await uni.downloadFile({
    url
  })) as any;
  if (!/:ok$/.test(dres.errMsg)) {
    throw new Error(dres.errMsg);
  }
  const sres = (await uni.saveImageToPhotosAlbum({
    filePath: dres.tempFilePath
  })) as any;
  if (!/:ok$/.test(sres.errMsg)) {
    throw new Error(sres.errMsg);
  }
  return sres;
}

/**
 * uploadFiles
 * originFile
 * type fpath/base64/url
 */
export async function uploadFile(
  originFile: string,
  type = "fpath",
  maxSize = 5 * (1024 * 1024)
) {
  // 1 文件大小检查
  let b64 = "";
  if (type === "fpath") {
    const res: any = await readFile(originFile);
    if (res.size > maxSize) {
      return {
        success: false,
        msg: `文件大小超过${maxSize / 1024 / 1024}M`
      };
    }
    b64 = res.data;
  }
  if (type === "url") {
    //
  }

  // 文件上传
  const index = b64.indexOf(",");
  const upRes = await concurrentUploadImage(b64.slice(index + 1));
  return {
    ...upRes,
    success: true
  };
}

export async function readFile(path: string) {
  return new Promise<UniApp.ReadFileSuccessCallbackResult>(
    (resolve, reject) => {
      uni.getFileSystemManager().readFile({
        filePath: path,
        encoding: "base64",
        success: r => {
          resolve(r);
        },
        fail: e => {
          reject(e);
        }
      });
    }
  );
}
