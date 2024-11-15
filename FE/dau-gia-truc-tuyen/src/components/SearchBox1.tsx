import { useState } from 'react';

const SearchBox1 = () => {
    const [dropDown, setDropDown] = useState(false);

    const toggleDropDown = () => {
        setDropDown(!dropDown);
    };

    return(
        <div className='w-full max-w-[1120px] h-20 bg-[#F19B40] p-4  absolute m-auto bottom-[-50px] left-0 right-0 sm:max-w-screen-sm md:max-w-screen-md lg:max-w-screen-lg'>
            <form className="max-w-lg mx-auto space-y-4"> 
                <div className="flex relative space-x-4">
                    <button 
                        className="flex-shrink-0 z-10 inline-flex items-center py-2.5 px-4 text-sm font-medium text-center text-gray-900 bg-gray-100 border border-gray-300  hover:bg-gray-200 focus:ring-4 focus:outline-none focus:ring-gray-100 dark:bg-gray-700 dark:hover:bg-gray-600 dark:focus:ring-gray-700 dark:text-white dark:border-gray-600" 
                        type="button"
                        onClick={toggleDropDown}
                    >
                        Tất cả 
                        <svg className="w-2.5 h-2.5 ms-2.5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 10 6">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 4 4 4-4"/>
                        </svg>
                    </button>
                    
                    <div className={`absolute top-full mt-1 left-0 z-10 ${dropDown ? '' : 'hidden'} bg-white divide-y divide-gray-100  shadow w-44 dark:bg-gray-700`}>
                        <ul className="py-2 text-sm text-gray-700 dark:text-gray-200">
                            <li><button type="button" className="inline-flex w-full px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Tất cả</button></li>
                            <li><button type="button" className="inline-flex w-full px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Đang diễn ra</button></li>
                            <li><button type="button" className="inline-flex w-full px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Sắp diễn ra</button></li>
                            <li><button type="button" className="inline-flex w-full px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Đã kết thúc</button></li>
                        </ul>
                    </div>
                    
                    <div className="relative w-full">
                        <input 
                            type="search" 
                            id="search-dropdown" 
                            className="block p-2.5 w-full z-20 text-sm text-gray-900 bg-gray-50 border-s-gray-50 border-s-2 border border-gray-300 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-s-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:border-blue-500" 
                            placeholder="Nhập từ khoá tìm kiếm (tên, trạng thái, mã số)" 
                            required 
                        />
                    </div>
                    <button type="submit" className="absolute top-0 end-0 p-2.5 text-sm font-medium h-full w-14 text-white bg-blue-700 border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                        <svg className="w-4 h-4" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"/>
                        </svg>
                        <span className="sr-only">Search</span>
                    </button>
                </div>
            </form>
      </div>
      
    )
}
export default SearchBox1