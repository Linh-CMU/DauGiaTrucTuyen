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
