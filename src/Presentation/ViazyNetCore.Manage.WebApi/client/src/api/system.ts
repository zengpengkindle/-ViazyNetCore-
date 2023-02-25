import { http } from "@/utils/http";
import { ComStatus, PageData, PageFindArgs } from "./model";
export interface DicFindAllArgs extends PageFindArgs {
    nameLike: string | null;
}

export interface DictionaryFindAllModel {
    createTime: string;
    id: string;
    key: string;
    name: string;
    status: ComStatus;
}
export class DictionaryApi {
    public remove(id: string): Promise<boolean> {
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
}

export default new DictionaryApi();