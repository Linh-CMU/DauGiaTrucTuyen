export interface UserType {
    role: string;
    [key: string]: any;
  }
  
  export type AuthContextType = {
    user: UserType | null;
    login: (username: string, password: string) => void;
    logout: () => void;
  };