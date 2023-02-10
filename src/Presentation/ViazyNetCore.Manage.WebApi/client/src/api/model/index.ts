/**
 * UserLoginArgs
 */
export interface UserLoginArgs {
  username: string | null;
  password: string | null;
  code?: string | null;
  mark?: string | null;
  auditor?: number | null;
}

/**
 * JwtTokenResult
 */
export interface JwtTokenResult {
  accessToken: string;
  expiresIn: number;
}
