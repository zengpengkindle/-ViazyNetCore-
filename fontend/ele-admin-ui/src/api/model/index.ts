export interface PageFindArgs {
  sort: 0 | 1;
  sortField: string | null;
  page: number;
  limit: number;
}
export interface PageData<TModel> {
  rows: Array<TModel>;
  total: number;
}
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
  nickname: string;
}

/**
 * UserFindAllArgs
 */
export interface UserFindAllArgs {
  usernameLike: string | null;

  roleId: string | null;

  status: ComStatus | null;

  sort: number | null;

  sortField: string | null;

  page: number | null;

  limit: number | null;
  orgId: number | null;
}

/**
 * UserFindAllModelPageData
 */
export interface UserFindAllModelPageData {
  rows: Array<UserFindAllModel>;

  total: number;
}

/**
 * UserFindAllModel
 */
export interface UserFindAllModel {
  createTime: string;

  modifyTime: string;

  roleName: string;

  googleKey: string;

  extraData: string;

  id: string;

  username: string;

  nickname: string;

  status: ComStatus;

  roleId: number;
}

//
export enum ComStatus {
  Disabled = 0,
  Enabled = 1,
  UnChecked = 2,
  Deleted = -1
}
/**
 * UserFindModel
 */
export interface UserFindModel {
  roleName?: string;

  googleKey?: string;

  extraData: string;

  id: string;

  username: string;

  nickname: string;

  status: ComStatus;

  roleId?: number;
}

/**
 * UserModel
 */
export interface UserModel {
  extraData: string | null;

  id: string | null;

  username: string | null;

  nickname: string | null;

  status: ComStatus | null;

  roleId: number | null;
}

/**
 * UserManageDto
 */
export interface UserManageDto {
  userId: string;

  userName: string;

  password: string;
}
/**
* Pagination
*/
export interface Pagination {
  
  page: number | null
  
  limit: number | null
}