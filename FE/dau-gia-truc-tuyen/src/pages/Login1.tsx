import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';

const Login1 = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const { login } = useAuth();
    const { setIsLoading } = useLoading();

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setIsLoading(true);
        const success = await login({ username, password });
        setIsLoading(false);
        if (success) {
            navigate('/');
            console.log('Login successful!');
          } else {
            console.error('Login failed!');
          }
    };

    return(
        <>
            <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
                <div className="absolute w-80 h-96 bg-blue-400 rounded-lg -rotate-6"></div>
                <div className="relative w-80 h-96 bg-white rounded-lg shadow-lg p-8 z-10">
                    <h2 className="text-xl font-bold mb-6">Đăng nhập</h2>
                    <form onSubmit={handleSubmit}> 
                        <div className="mb-4">
                            <label className="block mb-2 text-sm font-medium">Tên đăng nhập</label>
                            <input
                                className="w-full p-2 border-b border-gray-300 focus:outline-none focus:border-blue-500"
                                placeholder="Ten Dang nhap"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                                required
                            />
                        </div>
                        <div className="mb-6">
                            <label className="block mb-2 text-sm font-medium">Mật khẩu</label>
                            <input
                                type="password"
                                className="w-full p-2 border-b border-gray-300 focus:outline-none focus:border-blue-500"
                                placeholder="Password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                        </div>
                        <button 
                            className="w-full bg-blue-500 text-white p-2 rounded"
                            type='submit'
                        >
                            Dang nhap
                        </button>
                    </form>
                </div>
            </div>
        </>
    )
}

export default Login1;