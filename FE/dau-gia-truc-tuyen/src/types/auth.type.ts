export interface LoginRequest {
  username: string;
  password: string;
}
export interface SignUpRequest {
  username: string;
  password: string;
  email: string;
}
export interface AuthResponse {
  result: string | null;
  isSucceed: boolean;
  message: string;
}

export interface Account {
  accountId: string;
  userName: string;
  email: string;
  fullName: string;
  phone: string;
  city: string;
  ward: string;
  district: string;
  address: string;
  status: boolean;
}

export interface cityResponse {
  code: string;
  name: string;
}

export interface districtResponse {
  code: string
  codename: string;
  province_code: string;
  name: string;
}

export interface wardResponse {
  code:string;
  district_code:string;
  name: string;
}

export interface profileResponse {
  accountId: string,
  fullName: string;
  phone: string;
  city: string;
  ward: string;
  district: string;
  address: string;
  avatar:string;
  frontCCCD: string;
  backsideCCCD: string;
}

export interface listData {
  id: string;
  img: string;
  name: string;
  startDay: string;
  startTime: string;
  endDay: string;
  endTime:string;
  priceStart: number;
  winningBid: number
}



