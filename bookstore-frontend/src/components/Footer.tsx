import React from "react";
import { Link } from "react-router-dom";

function Footer() {
  return (
    <div className="w-full text-gray-300 px-5 flex flex-col bg-[#001219] h-[15rem]">
      <div className="flex flex-row basis-2/3 pt-10 pb-2">
        <Link
          to="/"
          className="basis-1/5 text-[2rem] font-bold text-gray-100 mr-[2.3rem]"
        >
          bookcamp
        </Link>
        <div className="basis-1/5 flex flex-col mr-[11.6rem]">
          <p>Service</p>
          <p>Shipping info</p>
          <p>Payments</p>
          <p>FAQ</p>
        </div>
        <div className="basis-1/5 flex flex-col">
          <p>Contacts</p>
          <p>Email us</p>
        </div>
        <div className="basis-1/5"></div>
      </div>
      <div className="flex flex-row font-extralight items-center basis-1/3 border-t-2 border-[#313131]">
        <p className="text-sm basis-1/3 text-right">
          2022. All rights reserved.
        </p>
        <div className="flex flex-row justify-center basis-2/3 text-sm">
          <p>Terms & Conditions</p>
          <p className="px-10">Privacy policy</p>
          <p>Cookies</p>
        </div>
      </div>
    </div>
  );
}

export default Footer;
