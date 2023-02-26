import { http } from "@/utils/http";
import { ComStatus, PageData, PageFindArgs } from "./model";
export interface DicFindAllArgs extends PageFindArgs {
    nameLike: string | null;
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

export class DictionaryApi {
    public remove(id: number): Promise<boolean> {
        return http.request({
            url: "/api/dictionary/remove",
            method: "post",
            params: { id },
        })
    }

    public findAll(param1: DicFindAllArgs): Promise<PageData<DictionaryFindAllModel>> {
        return http.request({
            url: "/api/dictionary/findall",
            method: "post",
            data: param1,
        })
    }

    public apiGet(id: number): Promise<DictionaryTypeModel> {
        return http.request({
            url: "/api/dictionary/get",
            method: "post",
            params: { id }
        })
    }

    public apiAdd(param1 : DictionaryTypeModel): Promise<boolean> {
        return http.request({
            url: "/api/dictionary/add",
            method: "post",
            data: param1
        })
    }
    public apiUpdate(param1 : DictionaryTypeModel): Promise<boolean> {
        return http.request({
            url: "/api/dictionary/update",
            method: "post",
            data: param1
        })
    }
}

export default new DictionaryApi();