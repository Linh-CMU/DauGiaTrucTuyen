import React, { useEffect, useState } from 'react';
import { Bar, Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { moneyStatistics, productStatistics } from '@queries/AdminAPI';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  Tooltip,
  Legend
);
interface DataChart {
  labels: string[]; // Array of strings for labels
  datasets: {
    label: string;
    data: number[]; // Array of numbers for data points
    backgroundColor: string;
  }[];
}

interface DataMoneyChart {
  labels: string[]; // Array of strings for labels
  datasets: {
    label: string;
    data: number[]; // Array of numbers for data points
    fill: boolean;
    borderColor: string,
    backgroundColor: string,
  }[];
}

const Home = () => {
  const [activeTab, setActiveTab] = useState(1);
  const [dailyData, setDailyData] = useState<DataChart>({
    labels: [],
    datasets: [{ label: "", data: [], backgroundColor: "" }],
  });
  const [monthlyData, setMonthlyData] = useState<DataMoneyChart>({
    labels: [],
    datasets: [{ label: "", data: [], fill: false, borderColor: "", backgroundColor: "" }],
  });

  useEffect(() => {
    const fetchDailyData = async () => {
      try {
        const response = await productStatistics();

        const transformedData = {
          labels: response.labels,
          datasets: response.datasets.map((dataset: any) => ({
            label: dataset.label,
            ...dataset,
            backgroundColor: 'rgba(75, 192, 192, 0.6)',
          })),
        };
        setDailyData(transformedData);
      } catch (error) {
        console.error('Error fetching daily data:', error);
      }
    };
    fetchDailyData();
  });

  useEffect(() => {
    const fetchMoney = async () => {
      try {
        const response = await moneyStatistics();

        const transformedData = {
          labels: response.labels,
          datasets: response.datasets.map((dataset: any) => ({
            label: dataset.label,
            ...dataset,
            backgroundColor: dataset.backgroundColor,
          })),
        };

        setMonthlyData(transformedData);
      } catch (error) {
        console.error('Error fetching money data:', error);
      }
    };
    fetchMoney();
  });

  const yearlyData = {
    labels: ['2020', '2021', '2022', '2023', '2024'],
    datasets: [
      {
        label: 'Yearly Money',
        data: [450000, 600000, 700000, 800000, 900000],
        fill: false,
        borderColor: 'rgba(153, 102, 255, 0.6)',
        backgroundColor: 'rgba(153, 102, 255, 0.6)',
      },
    ],
  };

  return (
    <div className="container mx-auto p-6 bg-gray-100 min-h-screen mt-16 flex">
      <div className="w-1/6 p-4 bg-white rounded-lg shadow-sm h-52 mt-6">
        <div className="space-y-2">
          <button
            className={`w-full p-2 text-left rounded-lg cursor-pointer ${
              activeTab === 1 ? 'bg-blue-600 text-white' : 'bg-gray-100 text-gray-800'
            }`}
            onClick={() => setActiveTab(1)}
          >
            Daily Money
          </button>
          <button
            className={`w-full p-2 text-left rounded-lg cursor-pointer ${
              activeTab === 2 ? 'bg-blue-600 text-white' : 'bg-gray-100 text-gray-800'
            }`}
            onClick={() => setActiveTab(2)}
          >
            Monthly Money
          </button>
          <button
            className={`w-full p-2 text-left rounded-lg cursor-pointer ${
              activeTab === 3 ? 'bg-blue-600 text-white' : 'bg-gray-100 text-gray-800'
            }`}
            onClick={() => setActiveTab(3)}
          >
            Yearly Money
          </button>
        </div>
      </div>

      <div className="flex-1 p-6">
        <div className="bg-white p-4 rounded-lg shadow-sm h-[80vh]">
          {activeTab === 1 && (
            <>
              <div className="grid grid-cols-2 gap-4 mb-6">
                <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
                  <svg
                    className="w-6 h-6 text-gray-800 dark:text-white mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      fillRule="evenodd"
                      d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z"
                      clipRule="evenodd"
                    />
                    <path
                      fillRule="evenodd"
                      d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z"
                      clipRule="evenodd"
                    />
                    <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z" />
                  </svg>
                  <div>
                    <div className="text-3xl font-bold text-gray-800">500.000 vnđ</div>
                    <div className="text-sm text-gray-500">Today's Money</div>
                  </div>
                </div>

                <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
                  <svg
                    className="w-6 h-6 text-gray-800 dark:text-white mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      fillRule="evenodd"
                      d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z"
                      clipRule="evenodd"
                    />
                    <path
                      fillRule="evenodd"
                      d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z"
                      clipRule="evenodd"
                    />
                    <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z" />
                  </svg>
                  <div>
                    <div className="text-3xl font-bold text-gray-800">500.000 vnđ</div>
                    <div className="text-sm text-gray-500">Today's Money</div>
                  </div>
                </div>

                <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
                  <svg
                    className="w-6 h-6 text-gray-800 dark:text-white mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      fillRule="evenodd"
                      d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z"
                      clipRule="evenodd"
                    />
                    <path
                      fillRule="evenodd"
                      d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z"
                      clipRule="evenodd"
                    />
                    <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z" />
                  </svg>
                  <div>
                    <div className="text-3xl font-bold text-gray-800">500.000 vnđ</div>
                    <div className="text-sm text-gray-500">Today's Money</div>
                  </div>
                </div>

                <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
                  <svg
                    className="w-6 h-6 text-gray-800 dark:text-white mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      fillRule="evenodd"
                      d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z"
                      clipRule="evenodd"
                    />
                    <path
                      fillRule="evenodd"
                      d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z"
                      clipRule="evenodd"
                    />
                    <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z" />
                  </svg>
                  <div>
                    <div className="text-3xl font-bold text-gray-800">500.000 vnđ</div>
                    <div className="text-sm text-gray-500">Today's Money</div>
                  </div>
                </div>
              </div>
            </>
          )}
          {activeTab === 2 && (
            <>
              <h2 className="text-lg font-semibold">Monthly Money</h2>
              <Line
                data={dailyData}
                options={{
                  responsive: true,
                  maintainAspectRatio: false, 
                  plugins: { legend: { display: true } },
                }}
              />
            </>
          )}
          {activeTab === 3 && (
            <>
              <h2 className="text-lg font-semibold">Yearly Money</h2>
              <Line
                data={monthlyData}
                options={{ responsive: true, plugins: { legend: { display: true } } }}
              />
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default Home;
