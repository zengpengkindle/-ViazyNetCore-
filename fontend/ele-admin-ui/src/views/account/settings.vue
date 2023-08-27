<template>
  <section
    class="main"
    v-loading="loading"
    :class="['my-flex', { pc: isPc }]"
    style="padding: 20px; height: 100%"
  >
    <el-tabs :tab-position="tabsPosition" style="height: 100%">
      <el-tab-pane label="基本设置" style="padding: 8px 30px">
        <div class="title">基本设置</div>
        <el-form
          ref="editFormRef"
          :model="editForm"
          :rules="editFormRules"
          label-position="top"
          label-width="80px"
          @submit.prevent
        >
          <el-row :gutter="50">
            <el-col :sm="12" :xs="24">
              <el-form-item label="姓名" prop="name">
                <el-input v-model="editForm.username" readonly />
              </el-form-item>
              <el-form-item label="我的昵称" prop="nickName">
                <el-input v-model="editForm.nickname" />
              </el-form-item>
            </el-col>
            <el-col :sm="12" :xs="24">
              <el-form-item label="头像">
                <x-image v-model="avatar" />
              </el-form-item>
            </el-col>
          </el-row>
          <!-- <el-row>
            <el-col :sm="12" :xs="24">
              <el-form-item>
                <my-confirm-button
                  :disabled="disabled"
                  :validate="editFormvalidate"
                  :placement="'top-end'"
                  :loading="editLoading"
                  style="margin-left: 0px"
                  @click="onSubmit"
                >
                  <template #content>
                    <p>确定要更新基本信息吗？</p>
                  </template>
                  更新基本信息
                </my-confirm-button>
              </el-form-item>
            </el-col>
          </el-row> -->
        </el-form>
      </el-tab-pane>
      <el-tab-pane label="安全设置" style="padding: 8px 30px">
        <div class="title">安全设置</div>
        <el-form
          ref="editPwdFormRef"
          :model="editPwdForm"
          :rules="editPwdFormRules"
          label-position="top"
          label-width="80px"
          @submit.prevent
        >
          <el-form-item label="旧密码" prop="oldPassword">
            <el-input
              v-model="editPwdForm.oldPassword"
              show-password
              auto-complete="off"
            />
          </el-form-item>
          <el-form-item label="新密码" prop="newPassword">
            <el-input
              v-model="editPwdForm.newPassword"
              show-password
              auto-complete="off"
            />
          </el-form-item>
          <el-form-item label="确认密码" prop="confirmPassword">
            <el-input
              v-model="editPwdForm.confirmPassword"
              show-password
              auto-complete="off"
            />
          </el-form-item>
          <el-form-item>
            <x-cofirm-button
              :disabled="disabled"
              :placement="'top-end'"
              :loading="editPwdLoading"
              style="margin-left: 0px"
              @click="onSubmitPwd"
            >
              <template #content>
                <p>确定要更新密码吗？</p>
              </template>
              更新密码
            </x-cofirm-button>
          </el-form-item>
        </el-form>
      </el-tab-pane>
    </el-tabs>
  </section>
</template>

<script lang="ts" setup>
import AccountApi from "@/api/account";
import DefaultAvatar from "@/assets/login/avatar.svg?url";

import { Ref, computed, onMounted, ref } from "vue";
import { FormInstance } from "element-plus";
import { message } from "@/utils/message";

const editFormRef: Ref<FormInstance> = ref();
const editPwdFormRef: Ref<FormInstance> = ref();
const validateNewPassword = (rule, value, callback) => {
  if (value === "") {
    callback(new Error("请输入新密码"));
  } else {
    if (editPwdForm.value.confirmPassword !== "") {
      editPwdFormRef.value.validateField("confirmPassword");
    }
    callback();
  }
};

const validateConfirmPassword = (rule, value, callback) => {
  if (value === "") {
    callback(new Error("请输入确认密码"));
  } else if (value !== editPwdForm.value.newPassword) {
    callback(new Error("新密码和确认密码不一致!"));
  } else {
    callback();
  }
};

const isPc = ref(true);
const tabsPosition: Ref<"left" | "top"> = ref("left");

onMounted(async () => {
  isPc.value = window.innerWidth >= 768;
  tabsPosition.value = isPc.value ? "left" : "top";
  loading.value = true;
  const res = await AccountApi.getIdentity();
  loading.value = false;
  editForm.value = { ...res, avatar: null };
  editPwdForm.value.id = res.id;
});

const editFormRules = {
  nickname: [{ required: true, message: "请输入姓名", trigger: "blur" }]
};
const editForm = ref({
  id: 0,
  username: "",
  nickname: "",
  avatar: ""
});
const disabled = false;
const loading = ref(false);
// const editLoading = ref(false);
const avatarDefault = DefaultAvatar;

const avatar = computed(() => {
  const path = editForm.value.avatar ? editForm.value.avatar : avatarDefault;
  return path;
});
const editFormvalidate = () => {
  let isValid = false;
  editFormRef.value.validate(valid => {
    isValid = valid;
  });
  return isValid;
};
// async onSubmit() {
//   editLoading.value = true;
//   const para = { ...editForm.value };
//   const res = await AccountApi.updateBasic(para);
//   this.editLoading = false;

//   if (!res?.success) {
//     if (!res.msg) {
//       msg.info({
//         message:"确认更新信息",
//         type: "error"
//       });
//     }
//     return;
//   }

//   this.$message({
//     message: this.$t("admin.updateOk"),
//     type: "success"
//   });

//   this.$store.commit("user/setName", para.nickName || para.name);
//   this.$store.commit("user/setAvatar", para.avatar);
// },

const editPwdLoading = ref(false);
const editPwdFormRules = {
  oldPassword: [{ required: true, message: "请输入旧密码", trigger: "blur" }],
  newPassword: [
    { required: true, validator: validateNewPassword, trigger: "blur" }
  ],
  confirmPassword: [
    {
      required: true,
      validator: validateConfirmPassword,
      trigger: "blur"
    }
  ]
};
const editPwdForm = ref({
  id: 0,
  password: "",
  oldPassword: "",
  newPassword: "",
  confirmPassword: ""
});
const onSubmitPwd = async () => {
  if (!editPwdFormRef.value) return;
  editPwdFormRef.value.validate(async valid => {
    if (!valid) return false;
    editPwdLoading.value = true;
    const res = await AccountApi.modifyPassword({
      oldPassword: editPwdForm.value.oldPassword,
      newPassword: editPwdForm.value.newPassword
    });
    if (res) {
      editPwdLoading.value = false;
      message("修改成功", { type: "success" });
      editPwdFormRef.value.resetFields();
    }
  });
};
</script>

<style scoped>
.title {
  margin-bottom: 22px;
  color: rgba(0, 0, 0, 0.85);
  font-weight: 500;
  font-size: 20px;
  line-height: 28px;
}
.pc :deep(.el-tabs--left .el-tabs__header.is-left) {
  margin-right: -1px;
}
.pc :deep(.el-tabs__content::before) {
  content: "";
  position: absolute;
  bottom: 0;
  width: 2px;
  height: 100%;
  background-color: #e4e7ed;
  z-index: 0;
  top: 0;
}

.avatar-uploader :deep(.el-loading-spinner .circular) {
  width: 26px;
  height: 26px;
}

.pc :deep(.el-tabs__content) {
  overflow: auto;
  height: 100%;
}
.main {
  background-color: #fff;
}
</style>
