
import { http } from '@/utils/http'
import { UserLoginArgs, JwtTokenResult } from './model'

export class AccountApi  {
  /**
   * 无
   */
  public accountLogin(param1: UserLoginArgs): Promise<JwtTokenResult> {
    return http.request({
      url: '/account/login',
      method: 'post',
      data: param1
    })
  }
}
export default new AccountApi()