export const fakeAuth = (username: string, password: string) => {
    if (username === 'admin' && password === 'admin') {
      return 'fake-jwt-token-for-admin';
    } else if (username === 'user' && password === 'user') {
      return 'fake-jwt-token-for-user';
    } else {
      return null;
    }
  };