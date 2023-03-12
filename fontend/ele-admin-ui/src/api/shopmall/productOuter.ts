
  import { ComStatus,Pagination } from '../model'

  
  /**
  * ProductOuterPageData
  */
  export interface ProductOuterPageData {
    
    rows: Array<ProductOuter>
    
    total: number
  }
  
  /**
  * ProductOuter
  */
  export interface ProductOuter {
    
    outerName: string
    
    outerType: string
    
    status: ComStatus
    
    description: string
    
    beginTime: string
    
    endTime: string
    
    createTime?: string
    
    id?: string
  }
  

  import { http } from '@/utils/http';
  
  export class ProductOuterApi {
    /**
     * 无
     */
    public apiProductOuterFindAll(param1?: Pagination): Promise<ProductOuterPageData> {
      return http.request({
        url: '/api/ProductOuter/FindAll',
        method: 'post',
        data: param1
      })
    }
    public apiProductOuterModifyStatus(outerId?: string, status?: ComStatus): Promise<boolean> {
        return http.request({
          url: '/api/ProductOuter/ModifyStatus',
          method: 'post',
          params: { outerId, status }
        })
      }
      public apiProductOuterMangerProductOuter(param1?: ProductOuter): Promise<boolean> {
        return http.request({
          url: '/api/ProductOuter/MangerProductOuter',
          method: 'post',
          data: param1
        })
      }
  }
  export default new ProductOuterApi()
  