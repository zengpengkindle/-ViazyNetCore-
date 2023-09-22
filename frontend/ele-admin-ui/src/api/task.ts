import { http } from "@/utils/http";

export class TaskApi {
  /**
   * 分页获取
   */
  public getList(
    page?: number,
    limit?: number,
    key?: string
  ): Promise<TasksQzPageData> {
    return http.request({
      url: "/api/Task/getList",
      method: "post",
      params: { Page: page, Limit: limit, key }
    });
  }

  public get(jobId?: number): Promise<TasksQz> {
    return http.request({
      url: "/api/Task/get",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 修改计划任务
   */
  public updateTask(param1?: TasksQz): Promise<void> {
    return http.request({
      url: "/api/Task/updateTask",
      method: "post",
      data: param1
    });
  }
  /**
   * 新增计划任务
   */
  public addTask(param1?: TasksQz): Promise<void> {
    return http.request({
      url: "/api/Task/addTask",
      method: "post",
      data: param1
    });
  }
  /**
   * 删除一个任务
   */
  public delete(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/delete",
      method: "delete",
      params: { jobId }
    });
  }
  /**
   * 启动计划任务
   */
  public startJob(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/startJob",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 停止一个计划任务
   */
  public stopJob(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/stopJob",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 暂停一个计划任务
   */
  public pauseJob(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/pauseJob",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 恢复一个计划任务
   */
  public resumeJob(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/resumeJob",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 重启一个计划任务
   */
  public reCovery(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/reCovery",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 获取任务命名空间
   */
  public getTaskNameSpace(): Promise<Array<QuartzReflectionItem>> {
    return http.request({
      url: "/api/Task/getTaskNameSpace",
      method: "get"
    });
  }
  /**
   * 立即执行任务
   */
  public executeJob(jobId?: number): Promise<void> {
    return http.request({
      url: "/api/Task/executeJob",
      method: "get",
      params: { jobId }
    });
  }
  /**
   * 获取任务运行日志
   */
  public getTaskLogs(
    page?: number,
    limit?: number,
    jobId?: number,
    runTimeStart?: string,
    runTimeEnd?: string
  ): Promise<TasksLogPageData> {
    return http.request({
      url: "/api/Task/getTaskLogs",
      method: "post",
      params: { Page: page, Limit: limit, jobId, runTimeStart, runTimeEnd }
    });
  }
  /**
   * 停止一个Trigger
   */
  public stopTrigger(jobId?: number, triggerId?: string): Promise<void> {
    return http.request({
      url: "/api/Task/stopTrigger",
      method: "get",
      params: { jobId, triggerId }
    });
  }
  /**
   * 启用一个计划任务
   */
  public startTrigger(jobId?: number, triggerId?: string): Promise<void> {
    return http.request({
      url: "/api/Task/startTrigger",
      method: "get",
      params: { jobId, triggerId }
    });
  }
}
export default new TaskApi();
/**
 * TasksQzPageData
 */
export interface TasksQzPageData {
  rows: Array<TasksQz>;
  total: number;
}

/**
 * TasksQzPageData
 */
export interface TasksQzPageData {
  rows: Array<TasksQz>;
  total: number;
}

/**
 * TasksQz
 */
export interface TasksQz {
  triggerCount: number | null;
  id: number | null;
  /** 任务名称 */ name: string | null;
  /** 任务分组 */ jobGroup: string | null;
  /** 任务运行时间表达式 */ cron: string | null;
  /** 任务所在DLL对应的程序集名称 */ assemblyName: string | null;
  /** 任务所在类 */ className: string | null;
  /** 任务描述 */ remark: string | null;
  /** 执行次数 */ runTimes: number | null;
  /** 开始时间 */ beginTime: string | null;
  /** 结束时间 */ endTime: string | null;
  /** 触发器类型（0、simple 1、cron） */ triggerType: number | null;
  /** 执行间隔时间, 秒为单位 */ intervalSecond: number | null;
  /** 循环执行次数 */ cycleRunTimes: number | null;
  /** 已循环次数 */ cycleHasRunTimes: number | null;
  /** 是否启动 */ isStart: boolean | null;
  /** 执行传参 */ jobParams: string | null;
  isDeleted: boolean | null;
  /** 任务内存中的状态 */ triggers: Array<TaskInfoDto> | null;
  updateUserId: number | null;
  updateUserName: string | null;
  updateTime: string | null;
  createUserId: number | null;
  createUserName: string | null;
  createTime: string | null;
}

/**
 * TaskInfoDto
 */
export interface TaskInfoDto {
  /** 任务ID */ jobId: string;
  /** 任务名称 */ jobName: string;
  /** 任务分组 */ jobGroup: string;
  /** 触发器ID */ triggerId: string;
  /** 触发器名称 */ triggerName: string;
  /** 触发器分组 */ triggerGroup: string;
  /** 触发器状态 */ triggerStatus: string;
}
/**
 * QuartzReflectionItem
 */
export interface QuartzReflectionItem {
  /** 命名空间 */ nameSpace: string;
  /** 类名 */ nameClass: string;
  /** 备注 */ remark: string;
}

/**
 * TasksLogPageData
 */
export interface TasksLogPageData {
  rows: Array<TasksLog>;
  total: number;
}

/**
 * 任务日志表
 */
export interface TasksLog {
  id: number;
  /** 任务ID */ jobId: number;
  /** 任务耗时 */ totalTime: number;
  /** 执行结果(0-失败 1-成功) */ runResult: boolean;
  /** 运行时间 */ runTime: string;
  /** 结束时间 */ endTime: string;
  /** 执行参数 */ runPars: string;
  /** 异常信息 */ errMessage: string;
  /** 异常堆栈 */ errStackTrace: string;
  /** 任务名称 */ name: string;
  /** 任务分组 */ jobGroup: string;
  createUserId: number;
  createUserName: string;
  createTime: string;
}
