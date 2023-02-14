
import { http } from '@/utils/http';
import { ComStatus } from './model'
/**
* FindRolesParameters
*/
export interface FindRolesParameters {

    nameLike: string | null

    page: number | null

    limit: number | null
}

/**
* RoleFindAllModelPageData
*/
export interface RoleFindAllModelPageData {

    rows: Array<RoleFindAllModel>

    total: number
}

/**
* RoleFindAllModel
*/
export interface RoleFindAllModel {

    status: ComStatus

    createTime: string

    modifyTime: string

    id: string

    name: string
}


export class RoleApi {
    /**
     * 无
     */
    public apiRoleFindRoles(param1?: FindRolesParameters): Promise<RoleFindAllModelPageData> {
        return http.request({
          url: '/api/Role/findRoles',
          method: 'post',
          data: param1
        })
      }
}
export default new RoleApi()
