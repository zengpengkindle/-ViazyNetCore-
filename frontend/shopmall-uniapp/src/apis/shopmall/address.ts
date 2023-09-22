import { http } from "@/utils/http";

export class AddressApi {
  /**
   * 无
   */
  public findAddress(): Promise<Array<AddressModel>> {
    return http.request({
      url: "/api/Address/FindAddress",
      method: "post"
    });
  }
  public subimtAddress(param1?: AddressModel): Promise<string> {
    return http.request({
      url: "/api/Address/SubimtAddress",
      method: "post",
      data: param1
    });
  }
  public removeAddress(addressId?: string): Promise<boolean> {
    return http.request({
      url: "/api/Address/RemoveAddress",
      method: "post",
      params: { addressId }
    });
  }
}
export default new AddressApi();

/**
 * AddressModel
 */
export interface AddressModel {
  checked: boolean;
  id: string;
  province: string;
  city: string;
  county: string;
  addressDetail: string;
  postalCode: string;
  areaCode: string;
  name: string;
  address: string;
  tel: string;
  isDefault: boolean;
}
