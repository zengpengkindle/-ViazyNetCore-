import { http } from "@/utils/http";
import { ComStatus, PageData, PageFindArgs } from "./model";
export interface DicFindAllArgs extends PageFindArgs {
  name: string | null;
}

export interface DictionaryFindAllModel {
  createTime: string;
  id: number;
  code: string;
  name: string;
  status: ComStatus;
}

export interface DictionaryTypeModel {
  id: number;
  code: string;
  name: string;
  status: ComStatus;
  description: string | null;
}
export interface DictionaryValueModel {
  id: number;
  code: string;
  name: string;
  status: ComStatus;
  description: string | null;
  dictionaryTypeId: number;
}

export interface DictionaryValueFindAllModel {
  createTime: string;
  id: number;
  code: string;
  name: string;
  status: ComStatus;
  dictionaryTypeId: number;
}
export interface DicValueFindAllArgs extends PageFindArgs {
  name: string | null;
  dictionaryTypeId: number;
}

export class DictionaryApi {
  public remove(id: number): Promise<boolean> {
    return http.request({
      url: "/api/dictionary/remove",
      method: "post",
      params: { id }
    });
  }

  public findAll(
    param1: DicFindAllArgs
  ): Promise<PageData<DictionaryFindAllModel>> {
    return http.request({
      url: "/api/dictionary/findall",
      method: "post",
      data: param1
    });
  }

  public apiGet(id: number): Promise<DictionaryTypeModel> {
    return http.request({
      url: "/api/dictionary/get",
      method: "post",
      params: { id }
    });
  }

  public apiAdd(param1: DictionaryTypeModel): Promise<boolean> {
    return http.request({
      url: "/api/dictionary/add",
      method: "post",
      data: param1
    });
  }
  public apiUpdate(param1: DictionaryTypeModel): Promise<boolean> {
    return http.request({
      url: "/api/dictionary/update",
      method: "post",
      data: param1
    });
  }

  public findValues(
    param1: DicFindAllArgs
  ): Promise<PageData<DictionaryValueModel>> {
    return http.request({
      url: "/api/dictionary/findValues",
      method: "post",
      data: param1
    });
  }

  public apiGetValue(id: number): Promise<DictionaryValueModel> {
    return http.request({
      url: "/api/dictionary/getValue",
      method: "post",
      params: { id }
    });
  }

  public apiAddValue(param1: DictionaryValueModel): Promise<boolean> {
    return http.request({
      url: "/api/dictionary/addValue",
      method: "post",
      data: param1
    });
  }
  public apiUpdateValue(param1: DictionaryValueModel): Promise<boolean> {
    return http.request({
      url: "/api/dictionary/updateValue",
      method: "post",
      data: param1
    });
  }
  public removeValue(id: number): Promise<boolean> {
    return http.request({
      url: "/api/dictionary/removeValue",
      method: "post",
      params: { id }
    });
  }
}

export default new DictionaryApi();
