export interface LoginRequest {
  username: string;
  password: string;
}
export interface SignUpRequest {
  username: string;
  password: string;
  email: string;
}

export interface CredentialResponse {
  credential?: string; // JWT do Google Identity Services cung cấp
  select_by?: string;  // Cách người dùng chọn tài khoản (tùy chọn)
  clientId?: string;   // Client ID được sử dụng
}
export interface GoogleLoginResponse {
  tokenId: string; // ID token được Google cung cấp (JWT token)
  accessToken: string; // Access token từ Google
  profileObj: {
    email: string; // Email người dùng
    name: string; // Tên người dùng
    imageUrl: string; // URL ảnh đại diện
    googleId: string; // Google ID của người dùng
  };
  tokenObj: {
    id_token: string; // JWT token
    expires_at: number; // Thời gian hết hạn của token (timestamp)
    expires_in: number; // Thời gian còn lại (giây)
    first_issued_at: number; // Lần đầu được cấp (timestamp)
    login_hint: string;
    scope: string; // Phạm vi của token
  };
};
export interface GoogleLoginFailureResponse {
  error: string; // Mô tả lỗi (e.g., "idpiframe_initialization_failed")
  details?: string; // Chi tiết bổ sung về lỗi
};

export interface AuthResponse {
  result: any | null;
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
  avatar: string;
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
  categoryId: number | null;
  ward: string;
  district: string;
  address: string;
  avatar:string;
  signature:string;
  frontCCCD: string;
  backsideCCCD: string;
  dateOfIssue: string;
  placeOfIssue: string;
  gender: boolean;
  birthdate: string;
  placeOfResidence: string;
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



