

import { http } from '@/utils/http'
import { UserFindAllArgs, UserFindAllModelPageData } from './model'

export class UserApi {
  /**
   * 无
   */
  public apiUserFindAll(param1: UserFindAllArgs): Promise<UserFindAllModelPageData> {
    return http.request({
      url: '/api/User/findAll',
      method: 'post',
      data: param1
    })
  }
}
export default new UserApi()
