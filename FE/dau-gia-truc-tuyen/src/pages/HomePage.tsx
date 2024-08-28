import { useAuth } from '@contexts/AuthContext';

const HomePage = () => {
  const { username } = useAuth();
  return <h1 className="flex justify-center items-center h-full text-center">Hi {username}</h1>;
};
export default HomePage;
