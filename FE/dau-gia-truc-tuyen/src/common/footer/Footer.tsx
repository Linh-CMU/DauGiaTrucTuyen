import React from 'react';

const Footer = () => {
  return (
    <footer className="bg-gray-900 text-white p-8">
      <div className="container mx-auto flex flex-col md:flex-row justify-between gap-8">
        {/* Category Column */}
        <div>
          <h2 className="text-lg font-semibold border-b border-gray-600 pb-2 mb-4">Category</h2>
          <ul className="space-y-2">
            {["Baroque Art", "Rococo Art", "Romanticism", "Renaissance Art", "Impressionism", "Abstract Art", "Pop Art"].map((item, index) => (
              <li key={index} className="hover:text-gray-400">{item}</li>
            ))}
          </ul>
        </div>

        {/* Company & Social Links Column */}
        <div className="text-center">
          <h1 className="text-3xl font-bold mb-2">DAU GIA<span className="text-gray-400"> TRUC TUYEN</span></h1>
          <p className="mb-4">Bid High, Win Big, Smile Bigger</p>
          <p className="text-lg font-semibold mb-2">Social Just You Connected Us!</p>
          <p className="mb-4 text-gray-400">All of update in social</p>
          <div className="flex justify-center space-x-4">
            <a href="#" className="hover:text-gray-400">LinkedIn</a>
            <a href="#" className="hover:text-gray-400">Facebook</a>
            <a href="#" className="hover:text-gray-400">Twitter</a>
            <a href="#" className="hover:text-gray-400">Instagram</a>
          </div>
        </div>

        {/* Newsletter & Payment Column */}
        <div>
          <h2 className="text-lg font-semibold mb-4">Join Our Newsletter & More information.</h2>
          <div className="flex border-b border-gray-600 pb-2 mb-4">
            <input
              type="email"
              placeholder="Email Address"
              className="bg-transparent outline-none flex-1 text-gray-400 placeholder-gray-500"
            />
            <button className="text-white hover:text-gray-400">
              →
            </button>
          </div>
          <p className="font-semibold mb-2">Secured Payment Gateways</p>
          <div className="flex space-x-4">
            <img src="logo.png" alt="VISA" className="h-8" />
            <img src="logo.png" alt="MasterCard" className="h-8" />
            <img src="logo.png" alt="American Express" className="h-8" />
            <img src="logo.png" alt="Maestro" className="h-8" />
          </div>
        </div>
      </div>

      {/* Bottom Section */}
      <div className="border-t border-gray-700 mt-8 pt-4 flex flex-col md:flex-row justify-between text-gray-500 text-sm">
        <p>©Copyright 2024 Probid | Design By Egens Lab</p>
        <div className="flex space-x-4 mt-2 md:mt-0">
          <a href="#" className="hover:text-gray-400">Support Center</a>
          <a href="#" className="hover:text-gray-400">Terms & Conditions</a>
          <a href="#" className="hover:text-gray-400">Privacy Policy</a>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
