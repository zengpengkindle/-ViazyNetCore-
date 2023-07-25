<template>
  <el-upload
    class="avatar-uploader"
    :show-file-list="false"
    :on-success="handleAvatarSuccess"
    :before-upload="beforeAvatarUpload"
    :on-preview="handlePictureCardPreview"
    :on-remove="handleRemove"
    :http-request="httpRequestHandler"
    action="/api/common/upload/image"
  >
    <img v-if="props.modelValue" :src="props.modelValue" class="avatar" />
    <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
  </el-upload>
  <el-dialog v-model="dialogVisible">
    <img w-full :src="dialogImageUrl" alt="图片预览" />
  </el-dialog>
</template>
<script lang="ts" setup>
import { ElMessage, UploadRequestOptions } from "element-plus";
import { Plus } from "@element-plus/icons-vue";

import type { UploadProps } from "element-plus";
import { reactive, ref } from "vue";
// import { ApiResponse } from "@/api/model/apiResponseBase";
import { http } from "@/utils/http";
export interface ImageProps {
  modelValue: string | null;
  height?: string;
  width?: string;
}
const emits = defineEmits(["update:modelValue"]);
const props = defineProps<ImageProps>();

const handleAvatarSuccess: UploadProps["onSuccess"] = (
  response,
  uploadFile
) => {
  // const apiResponse = response as ApiResponse;
  // let imageUrl = "";
  // if (apiResponse.code == 200) {
  //   if (apiResponse.data.success) {
  //     imageUrl = apiResponse.data.result as string;
  //   }
  // }
  URL.createObjectURL(uploadFile.raw!);
  emits("update:modelValue", response);
};
const uploadTypes = reactive(["image/jpeg", "image/png", "image/webp"]);
const beforeAvatarUpload: UploadProps["beforeUpload"] = rawFile => {
  console.log(rawFile.type);
  if (!uploadTypes.includes(rawFile.type)) {
    ElMessage.error("图像必须是 JPG/PNG 格式!");
    return false;
  } else if (rawFile.size / 1024 / 1024 > 2) {
    ElMessage.error("图像大小不能超过 2MB!");
    return false;
  }
  return true;
};

const handleRemove: UploadProps["onRemove"] = (uploadFile, uploadFiles) => {
  console.log(uploadFile, uploadFiles);
};
const dialogImageUrl = ref("");
const dialogVisible = ref(false);
const handlePictureCardPreview: UploadProps["onPreview"] = uploadFile => {
  dialogImageUrl.value = uploadFile.url!;
  dialogVisible.value = true;
};
const httpRequestHandler = (
  options: UploadRequestOptions
): XMLHttpRequest | Promise<unknown> => {
  const param = new FormData(); // 创建form对象
  //注意files是对应后台的参数名哦
  param.append(options.filename ?? "files", options.file);
  return http.request({
    url: options.action,
    method: "post",
    headers: { "Content-Type": "multipart/form-data" },
    data: param
  });
};
</script>

<style scoped>
.avatar-uploader .avatar {
  width: 100px;
  height: 100px;
  display: block;
}
</style>

<style>
.avatar-uploader .el-upload {
  border: 1px dashed var(--el-border-color);
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: var(--el-transition-duration-fast);
}

.avatar-uploader .el-upload:hover {
  border-color: var(--el-color-primary);
}

.el-icon.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 100px;
  height: 100px;
  text-align: center;
}
</style>
