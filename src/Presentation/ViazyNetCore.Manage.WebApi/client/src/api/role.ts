
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

/**
* BmsRole
*/
export interface BmsRole {

    name: string

    status: ComStatus

    extraData: string

    createTime: string

    modifyTime: string

    id: string
}
/**
* UpdateBmsRoleArgs
*/
export interface UpdateBmsRoleArgs {

    id: string | null

    name: string | null

    status: ComStatus | null

    extraData: string | null

    keys: Array<string> | null
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
    /**
    * 无
    */
    public apiRoleFindRole(id?: string): Promise<BmsRole> {
        return http.request({
            url: '/api/Role/findRole',
            method: 'post',
            params: { id }
        })
    }

    public apiRoleUpdateRole(param1?: UpdateBmsRoleArgs): Promise<boolean> {
        return http.request({
            url: '/api/Role/updateRole',
            method: 'post',
            data: param1
        })
    }
}
export default new RoleApi()
