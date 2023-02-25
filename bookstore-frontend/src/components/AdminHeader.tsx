import React from "react";
import { Link, useNavigate } from "react-router-dom";
import logo from "../pics/logo.svg";
import { authApi } from "../services/authApi";

function AdminHeader() {
  const [logoutUserQuery] = authApi.useLazyLogoutUserQuery();
  const navigate = useNavigate();

  const signoutHandler = () => {
    logoutUserQuery();
    navigate("/login");
    // window.location.reload();
  };

  return (
    <div className="sticky top-0 z-50 w-full px-5 flex flex-row items-center bg-[#caf0f8] h-[3rem] lg:h-[5rem]">
      <div className="flex items-center flex-row justify-start flex-1">
        <img src={logo} alt="logo" className="w-[60px] lg:w-[100px]" />
        <Link
          to="/admin-books"
          className="text-[1.3rem] lg:text-[2.3rem] font-bold"
        >
          bookcamp
        </Link>
      </div>
      <div className="flex flex-row h-full">
        <Link
          to="/admin-books"
          className="flex items-center text-sm lg:text-base"
        >
          <span className="px-4">Books</span>
        </Link>
        <Link to="/users" className="flex items-center text-sm lg:text-base">
          <span className="px-4">Users</span>
        </Link>
        <Link to="/add-book" className="flex items-center text-sm lg:text-base">
          <span className="px-4">Add book</span>
        </Link>
      </div>
      <div className="flex flex-row justify-end flex-1 text-sm lg:text-base">
        <button onClick={signoutHandler}>Sign out</button>
      </div>
    </div>
  );
}

export default AdminHeader;
