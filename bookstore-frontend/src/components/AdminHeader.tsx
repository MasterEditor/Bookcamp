import React from "react";
import { useCookies } from "react-cookie";
import { Link } from "react-router-dom";
import { useAppSelector } from "../hooks/useAppSelector";
import logo from "../pics/logo.svg";

function AdminHeader() {
  const user = useAppSelector((state) => state.user.user);

  const [, , removeCookies] = useCookies(["bc_role"]);

  const signoutHandler = () => {
    removeCookies("bc_role", { path: "/" });
    // window.location.reload();
  };

  return (
    <div className="sticky top-0 z-50 w-full px-5 flex flex-row items-center bg-[#caf0f8] h-[5rem]">
      <div className="flex flex-row justify-start flex-1">
        <img src={logo} alt="logo" width="100px" />
        <Link to="/admin-books" className="text-[2.3rem] font-bold">
          bookcamp
        </Link>
      </div>
      <div className="flex flex-row h-full">
        <Link to="/admin-books" className="flex items-center">
          <span className="px-4">Books</span>
        </Link>
        <Link to="/users" className="flex items-center">
          <span className="px-4">Users</span>
        </Link>
        <Link to="/add-book" className="flex items-center">
          <span className="px-4">Add book</span>
        </Link>
      </div>
      <div className="flex flex-row justify-end flex-1">
        <button onClick={signoutHandler}>Sign out</button>
      </div>
    </div>
  );
}

export default AdminHeader;
