import { ApiResponse, ApiResponseData } from "@/api/model/apiResponseBase";
export class ApiResponseMockDefault implements ApiResponse {
  code: number;
  message: string;
  data: ApiResponseData;
  /**
   *
   */
  constructor(result: any) {
    this.code = 200;
    this.message = "This is a mock response.";
    this.data = {
      success: true,
      result: result
    };
  }
}
