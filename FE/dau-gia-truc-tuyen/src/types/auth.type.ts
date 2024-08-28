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
export interface AuthContextType {
  token: string | null;
  username: string | null;
  login: (data: LoginRequest) => Promise<boolean>;
  logout: () => void;
  signUp: (data: SignUpRequest) => Promise<boolean>;
  isAuthenticated: () => boolean;
}
