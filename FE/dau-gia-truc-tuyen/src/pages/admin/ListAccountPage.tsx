import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getListAccount, lockUser, unLockUser } from '../../queries/AdminAPI';
import { useMessage } from '@contexts/MessageContext';
import { Account } from '../../types/auth.type';

const ListAccountPage = () => {
  const [accounts, setAccounts] = useState<Account[]>([]);
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const navigate = useNavigate();
  const [searchUser, setSearchUser] = useState('');
  
  const headings = [
    
    { key: 'userName', value: 'User Name' },
    { key: 'email', value: 'Email' },
    { key: 'fullName', value: 'Full Name' },
    { key: 'phone', value: 'Phone' },
    { key: 'action', value: 'Action' },
  ];

  const [currentPage, setCurrenPage] = useState(1);
  const pageSize = 4;

  useEffect(() => {
    const fetchListAccount = async () => {
      const response = await getListAccount();
      if (response?.isSucceed) {
        setAccounts(Array.isArray(response.result) ? response.result : []);
      } else {
        setErrorMessage('fetch list fail');
        setAccounts([]);
      }
    };
    fetchListAccount();
  }, []);

  // filter username
  const filterUser = accounts.filter((account) => 
    account.userName.toLowerCase().includes(searchUser.toLocaleLowerCase())
  );

  // Pagination logic
  const totalPages = Math.ceil(filterUser.length / pageSize);
  const currentAccounts = filterUser.slice(
    (currentPage - 1) * pageSize,
    currentPage * pageSize
  );

  const handlePrev = () => {
    if (currentPage > 1) {
      setCurrenPage((prev) => prev - 1);
    }
  };

  const handleNext = () => {
    if (currentPage < totalPages) {
      setCurrenPage((prev) => prev + 1);
    }
  };

  const renderPaginationBtn = () => {
    const btns = [];
    const startPage = Math.max(1, currentPage - 1);
    const endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
      btns.push(
        <button
          key={i}
          type="button"
          className={`min-h-[38px] min-w-[38px] flex justify-center items-center border py-2 px-3 text-sm rounded-lg focus:outline-none ${
            i === currentPage
              ? 'border-blue-600 text-blue-600 bg-blue-100'
              : 'bg-slate-300 text-gray-800 hover:bg-gray-100'
          }`}
          onClick={() => setCurrenPage(i)}
        >
          {i}
        </button>
      );
    }

    return btns;
  };

  // Lock account
  const handleLock = async (accountId: string) => {
    try {
      const response = await lockUser(accountId);
      if (response.isSucceed) {
        const updatedAccounts = await getListAccount();
        setAccounts(Array.isArray(updatedAccounts.result) ? updatedAccounts.result : []);
        setSuccessMessage('Lock');
      } else {
        console.error('Lock user operation was not successful:', response.message);
      }
    } catch (error) {
      console.error('Error locking account:', error);
    }
  };

  // Unlock account
  const handleUnlock = async (accountId: string) => {
    try {
      const response = await unLockUser(accountId);
      if (response.isSucceed) {
        const updatedAccounts = await getListAccount();
        setAccounts(Array.isArray(updatedAccounts.result) ? updatedAccounts.result : []);
        setSuccessMessage('UnLock');
      } else {
        console.error('Unlock user operation was not successful:', response.message);
      }
    } catch (error) {
      console.error('Error unlocking account:', error);
    }
  };

  

  return (
    <div className="container mx-auto py-24 px-32">
      <div className="mb-4 flex justify-between items-center  pb-5">
        <div className="flex-1 pr-4">
          <div className="relative md:w-1/6">
            <input
              type="search"
              value={searchUser}
              onChange={(e) => setSearchUser(e.target.value)}
              className="bg-slate-300 w-full pl-10 pr-4 py-2 rounded-lg shadow focus:outline-none focus:shadow-outline text-gray-600 font-medium"
              placeholder="Search..."
            />
            <div className="absolute top-0 left-0 inline-flex items-center p-2">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="w-6 h-6 text-gray-400"
                viewBox="0 0 24 24"
                strokeWidth="2"
                stroke="currentColor"
                fill="none"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <circle cx="10" cy="10" r="7" />
                <line x1="21" y1="21" x2="15" y2="15" />
              </svg>
            </div>
          </div>
        </div>
      </div>

      <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase dark:text-gray-400">
            <tr>
              {headings.map((heading) => (
                <th
                  key={heading.key}
                  scope="col"
                  className={`px-6 py-3 bg-gray-50 dark:bg-gray-800 ${heading.key === 'action' ? 'text-center' : ''}`}
                >
                  {heading.value}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {currentAccounts.map((account, index) => (
              <tr
                key={account.accountId}
                className={`border-b border-gray-200 dark:border-gray-700 ${index % 2 === 0 ? 'bg-white' : 'bg-gray-100'}`}
              >
              
                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3 flex items-center space-x-4 w-auto">
                  <img
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${account.avatar}`}
                        alt="Front CCCD"
                        className="flex-shrink-0 w-10 h-10 object-cover rounded-full mr-2"
                      />
                  {account.userName}
                </td>
                <td className="border-dashed border-t border-gray-200 w-1/4 px-6 py-3">{account.email}</td>
                <td className="border-dashed border-t border-gray-200 w-1/4 px-6 py-3">{account.fullName}</td>
                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3">{account.phone}</td>
                <td className="border-dashed border-t border-gray-200 w-1/6 px-6 py-3 flex ">
                  {account.status ? (
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
                  <button
                    type="button"
                    className="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-2 py-1.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
                    onClick={() =>
                      navigate('/inforUser', { state: { iduser: account.accountId, status: true } })
                    }
                  >
                    View
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      <div className="flex justify-center mt-4 space-x-2 pt-8">
        <button
          onClick={handlePrev}
          className="px-4 py-2 bg-gray-200 rounded-md text-sm"
          disabled={currentPage === 1}
        >
          <svg className="shrink-0 size-3.5" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="m15 18-6-6 6-6"></path>
          </svg>
        </button>
        {renderPaginationBtn()}
        <button
          onClick={handleNext}
          className="px-4 py-2 bg-gray-200 rounded-md text-sm"
          disabled={currentPage === totalPages}
        >
          <svg className="shrink-0 size-3.5" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="m9 18 6-6-6-6"></path>
          </svg>
        </button>
      </div>
    </div>
  );
};

export default ListAccountPage;
