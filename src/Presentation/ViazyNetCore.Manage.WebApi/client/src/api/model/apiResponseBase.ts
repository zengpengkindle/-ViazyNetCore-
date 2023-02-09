export interface ApiResponse {
    code: number,
    message: string,
    result?: ApiResponseData
}

interface ApiResponseData {
    success: boolean,
    message?: string,
    err_code: number,
    data: any
}

export class ApiReponseError extends Error {
    code: number;
    err_code: number;
    message: string;
    err_msg?: string;
    constructor(code: number, err_code: number, message?: string, err_msg?: string) {
        super();
        this.code = code;
        this.err_code = err_code;
        this.message = message;
        this.err_msg = err_msg
    }
}