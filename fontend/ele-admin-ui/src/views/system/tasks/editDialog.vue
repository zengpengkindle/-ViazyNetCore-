<script lang="ts" setup>
import TaskApi, { TasksQz } from "@/api/task";
import { message } from "@/utils/message";
import { FormInstance, FormRules } from "element-plus";
import VueCron from "@/components/ui/cron.vue";
import { ref, watch, Ref, reactive } from "vue";
import Search from "@iconify-icons/ep/search";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";

export interface EditProps {
  modelValue: boolean;
  readonly id: number | null;
}
const props = defineProps<EditProps>();
const visible = ref(false);
const isEdit = ref(false);
const emit = defineEmits(["update:modelValue", "refresh"]);
function init() {
  getDicInfo();
}
watch(
  () => props.modelValue,
  value => {
    visible.value = value;
    if (value) {
      isEdit.value = false;
      init();
    }
  }
);
const handleClose = (done: () => void) => {
  emit("update:modelValue", false);
  done();
};
const defaultTaskQz: TasksQz = {
  id: 0,
  name: "",
  jobGroup: "",
  cron: "0/5 * * * * ?",
  assemblyName: "",
  className: "",
  remark: "",
  runTimes: 0,
  beginTime: "",
  endTime: "",
  triggerType: 0,
  intervalSecond: 0,
  cycleRunTimes: 0,
  cycleHasRunTimes: 0,
  isStart: false,
  jobParams: "",
  isDeleted: false,
  triggers: [],
  updateUserId: 0,
  updateUserName: "",
  updateTime: "",
  createUserId: 0,
  createUserName: "",
  createTime: "",
  triggerCount: 0
};

const editForm: Ref<TasksQz> = ref({ ...defaultTaskQz });

async function getDicInfo() {
  if (props.id) {
    editForm.value = await TaskApi.get(props.id);
  } else {
    editForm.value = { ...defaultTaskQz };
  }
}
const formRef = ref<FormInstance>();
const rules = reactive<FormRules>({
  username: [{ required: true, message: "用户名不能为空" }]
});
const submitForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  formEl.validate(async valid => {
    if (valid) {
      if (props.id) {
        await TaskApi.updateTask({
          ...editForm.value
        });
      } else {
        await TaskApi.addTask({
          ...editForm.value
        });
      }
      message("修改成功", { type: "success" });
      emit("refresh");
      handleClose(() => {});
    } else {
      console.log("error submit!");
      return false;
    }
  });
};
const pickerOptions = {
  shortcuts: [
    {
      text: "今天",
      onClick(picker) {
        picker.$emit("pick", new Date());
      }
    },
    {
      text: "明天",
      onClick(picker) {
        const date = new Date();
        date.setTime(date.getTime() + 3600 * 1000 * 24);
        picker.$emit("pick", date);
      }
    },
    {
      text: "一周后",
      onClick(picker) {
        const date = new Date();
        date.setTime(date.getTime() + 3600 * 1000 * 24 * 7);
        picker.$emit("pick", date);
      }
    },
    {
      text: "一月后(30)",
      onClick(picker) {
        const date = new Date();
        date.setTime(date.getTime() + 3600 * 1000 * 24 * 30);
        picker.$emit("pick", date);
      }
    },
    {
      text: "一年后(365)",
      onClick(picker) {
        const date = new Date();
        date.setTime(date.getTime() + 3600 * 1000 * 24 * 365);
        picker.$emit("pick", date);
      }
    }
  ]
};
const closeForm = () => {
  handleClose(() => {});
};
const expression = ref("");
const showCron = ref(false);
const showDialog = () => {
  expression.value = editForm.value.cron; //传入的 cron 表达式，可以反解析到 UI 上
  showCron.value = true;
};
const crontabFill = (value: string) => {
  editForm.value.cron = value;
};
const handleTask = () => {};
</script>
<template>
  <el-drawer
    v-model="visible"
    size="35%"
    :title="id ? '编辑任务' : '新增任务'"
    :before-close="handleClose"
  >
    <el-form
      ref="formRef"
      :model="editForm"
      :rules="rules"
      label-width="100px"
      class="demo-ruleForm"
    >
      <el-form-item label="任务组" prop="JobGroup">
        <el-input v-model="editForm.jobGroup" auto-complete="off" />
      </el-form-item>
      <el-form-item label="名称" prop="Name">
        <el-input v-model="editForm.name" auto-complete="off" />
      </el-form-item>
      <el-form-item label="程序集" prop="AssemblyName">
        <el-input v-model="editForm.assemblyName" auto-complete="off">
          <el-button
            style="width: 100%; overflow: hidden"
            @click.prevent="handleTask"
            >选择任务</el-button
          >
        </el-input>
      </el-form-item>
      <el-form-item label="执行类名" prop="ClassName">
        <el-input v-model="editForm.className" auto-complete="off" />
      </el-form-item>
      <el-form-item label="执行参数" prop="JobParams">
        <el-input
          class="textarea"
          type="textarea"
          :rows="6"
          v-model="editForm.jobParams"
        />
      </el-form-item>
      <el-form-item prop="triggerType" label="是否Cron" width="" sortable>
        <el-switch
          v-model="editForm.triggerType"
          :inactive-value="0"
          :active-value="1"
        />
        <span style="float: right; color: #aaa"
          >(1：Cron模式，0：Simple模式)</span
        >
      </el-form-item>

      <el-form-item label="Cron表达式" v-if="editForm.triggerType" prop="Cron">
        <el-tooltip placement="top">
          <template #content>
            <div>
              每隔5秒执行一次：*/5 * * * * ?<br />
              每隔1分钟执行一次：0 */1 * * * ?<br />
              每天23点执行一次：0 0 23 * * ?<br />
              每天凌晨1点执行一次：0 0 1 * * ?<br />
              每月1号凌晨1点执行一次：0 0 1 1 * ?<br />
              每月最后一天23点执行一次：0 0 23 L * ?<br />
              每周星期天凌晨1点实行一次：0 0 1 ? * L<br />
              在26分、29分、33分执行一次：0 26,29,33 * * * ?<br />
              每天的0点、13点、18点、21点都执行一次：0 0 0,13,18,21 * * ?<br /></div
          ></template>
          <el-input
            style="margin-bottom: 10px"
            v-model="editForm.cron"
            auto-complete="off"
          >
            <template #append>
              <el-button :icon="useRenderIcon(Search)" @click="showDialog" />
            </template>
          </el-input>
        </el-tooltip>
      </el-form-item>
      <el-form-item label="循环周期" v-else prop="IntervalSecond">
        <el-input-number
          v-model="editForm.intervalSecond"
          :min="1"
          style="width: 200px"
          auto-complete="off"
        />
        <span style="float: right; color: #aaa">(单位：秒)</span>
      </el-form-item>
      <el-form-item
        label="循环次数"
        v-if="!editForm.triggerType"
        prop="cycleRunTimes"
      >
        <el-tooltip placement="top">
          <template #content
            ><div>设置成0时,就意味着无限制次数运行</div></template
          >
          <el-input-number
            v-model="editForm.cycleRunTimes"
            :min="0"
            style="width: 200px"
            auto-complete="off"
          />
        </el-tooltip>
        <span style="float: right; color: #aaa">(单位：次)</span>
      </el-form-item>
      <el-form-item
        label="已循环次数"
        v-if="!editForm.triggerType"
        prop="CycleRunTimes"
      >
        <el-input-number
          v-model="editForm.cycleHasRunTimes"
          :min="0"
          style="width: 200px"
          auto-complete="off"
        />
        <span style="float: right; color: #aaa">(单位：次)</span>
      </el-form-item>
      <el-form-item
        label="并行数"
        v-if="!editForm.triggerType"
        prop="triggerCount"
      >
        <el-input-number
          v-model="editForm.triggerCount"
          :min="0"
          style="width: 200px"
          auto-complete="off"
        />
        <span style="float: right; color: #aaa">(单位：次)</span>
      </el-form-item>
      <el-form-item label="是否激活" prop="IsStart">
        <el-switch v-model="editForm.isStart" />
      </el-form-item>

      <el-form-item label="开始时间" prop="BeginTime">
        <el-date-picker
          type="datetime"
          placeholder="选择日期"
          v-model="editForm.beginTime"
          value-format="yyyy-MM-dd HH:mm:ss"
          :picker-options="pickerOptions"
        />
      </el-form-item>
      <el-form-item label="结束时间" prop="EndTime">
        <el-date-picker
          type="datetime"
          placeholder="选择日期"
          v-model="editForm.endTime"
          value-format="yyyy-MM-dd HH:mm:ss"
          :picker-options="pickerOptions"
        />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm(formRef)">提交</el-button>
        <el-button @click="closeForm">取消</el-button>
      </el-form-item>
    </el-form>
    <el-dialog
      title="cron表达式设置"
      v-model="showCron"
      :close-on-click-modal="false"
      width="65%"
    >
      <VueCron
        @close="showCron = false"
        @change="crontabFill"
        :form-data="expression"
      />
    </el-dialog>
  </el-drawer>
</template>
