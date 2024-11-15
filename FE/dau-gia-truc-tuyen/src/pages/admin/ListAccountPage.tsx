import { getListAccount, lockUser, unLockUser } from '../../queries/AdminAPI';
import { useEffect, useState } from "react";
import { useMessage } from '@contexts/MessageContext';
import { Account } from '../../types/auth.type';


const ListAccountPage = () => {
    
    const [accounts, setAccounts] = useState<Account[]>([]);
    const { setErrorMessage, setSuccessMessage } = useMessage();
    const headings = [
        { key: 'userName', value: 'User Name' },
        { key: 'email', value: 'Email' },
        { key: 'fullName', value: 'Full Name' },
        { key: 'phone', value: 'Phone' },
        { key: 'city', value: 'City' },
        { key: 'ward', value: 'Ward' },
        { key: 'district', value: 'District' },
        { key: 'address', value: 'Address' },
        { key: 'status', value: 'Status' },
        { key: 'action', value: 'Action' },
      ];
    
    useEffect(() => {
        const fetchListAccount = async () => {
            const response = await getListAccount(); //  call api
            console.log(response,"data") 
            if(response?.isSucceed){
                setAccounts(Array.isArray(response.result) ? response.result : []);
              }else {
                setErrorMessage("fetch list fail");
                setAccounts([]);
              }
        };
        fetchListAccount();
    }, []);

    // lock account
    const handleLock = async (accountId: string) => {
        try {
            console.log(accountId);
            const response = await lockUser(accountId);
            if (response.isSucceed) {
                const updatedAccounts = await getListAccount();
                setAccounts(Array.isArray(updatedAccounts.result) ? updatedAccounts.result : []);
                setSuccessMessage('Lock');
            } else {
                console.error('Lock user operation was not successful:', response.message);
            }
        } catch (error) {
            console.error('Error unlocking account:', error);
        }
    }

    // unlock account
    const handleUnlock = async (accountId: string) => {
        try {
            const response = await unLockUser(accountId);
            if (response.isSucceed) {
                const updatedAccounts = await getListAccount();
                setAccounts(Array.isArray(updatedAccounts.result) ? updatedAccounts.result : []);
                setSuccessMessage('UnLock');
            } else {
                console.error('Lock user operation was not successful:', response.message);
            }
        } catch (error) {
            console.error('Error unlocking account:', error);
        }
    }


    return (
        <div className='container mx-auto py-6 px-4'>
            <div className="mb-4 flex justify-between items-center">
                <div className="flex-1 pr-4">
                    <div className="relative md:w-1/6">
                        <input 
                            type="search" 
                            className="w-full pl-10 pr-4 py-2 rounded-lg shadow focus:outline-none focus:shadow-outline text-gray-600 font-medium"
                            placeholder="Search..."
                        />
                        <div className="absolute top-0 left-0 inline-flex items-center p-2">
                            <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6 text-gray-400" viewBox="0 0 24 24"
                                stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round"
                                stroke-linejoin="round">
                                <rect x="0" y="0" width="24" height="24" stroke="none"></rect>
                                <circle cx="10" cy="10" r="7" />
                                <line x1="21" y1="21" x2="15" y2="15" />
                            </svg>
                        </div>
                    </div>
                </div>
            </div>
            
            <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className='text-xs text-gray-700 uppercase dark:text-gray-400'>
                        <tr>
                            {headings.map((heading) => (
                                <th key={heading.key} scope="col" className={`px-6 py-3 bg-gray-50 dark:bg-gray-800 ${ heading.key === 'action' ? 'text-center' : ''}`}>
                                    {heading.value}
                                </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody>
                    {accounts.map((account, index) => (
                            <tr key={account.accountId} className={`border-b border-gray-200 dark:border-gray-700 ${ index % 2 === 0 ?  'bg-white': 'bg-gray-100'}`}>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.userName}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/4 px-6 py-3">{account.email}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/4 px-6 py-3">{account.fullName}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.phone}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.city}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.ward}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.district}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.address}</td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">
                                {account.status ? (
                                    <span className="text-red-600">Lock</span>
                                ) : (
                                    <span className="text-green-600">Unlock</span>
                                )}
                                </td>
                                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3 flex">
                                    { account.status ? (
                                        <button 
                                            type="button" 
                                            className="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-2 py-1.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
                                            onClick={() => handleUnlock(account.accountId)}
                                        >
                                            UnLock
                                        </button>
                                    ) : (
                                    <button 
                                        type="button" 
                                        className="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
                                        onClick={() => handleLock(account.accountId)}
                                    >
                                        Lock
                                    </button>
                                    )}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default ListAccountPage;