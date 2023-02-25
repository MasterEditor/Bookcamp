import React, { useEffect, useState } from "react";
import logo from "../pics/logo.svg";
import { FaSearch, FaUserCircle } from "react-icons/fa";
import { RiArrowDropDownFill, RiArrowDropUpFill } from "react-icons/ri";
import { createSearchParams, Link, useNavigate } from "react-router-dom";
import { useAppSelector } from "../hooks/useAppSelector";
import { authApi } from "../services/authApi";
import { useActions } from "../hooks/useActions";
import { Menu, Transition } from "@headlessui/react";
import classNames from "classnames";

function Header() {
  const { data, isLoading, isSuccess } = authApi.useMeQuery();

  const [logoutUserQuery, { isSuccess: isLogoutSuccess }] =
    authApi.useLazyLogoutUserQuery();

  const [scrollLimit, setScrollLimit] = useState(false);

  const { updateUser, logoutUser } = useActions();

  const [logouted, setLogouted] = useState(true);

  useEffect(() => {
    if (isSuccess) {
      updateUser(data!);
      setLogouted(false);
    }
  }, [isLoading]);

  useEffect(() => {
    if (isLogoutSuccess) {
      window.location.reload();
    }
  }, [isLogoutSuccess]);

  const [search, setSearch] = useState("");

  const user = useAppSelector((state) => state.user.user);

  const navigate = useNavigate();

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    navigate({
      pathname: "/books",
      search: `?${createSearchParams({ keywords: search })}`,
    });
  };

  const logout = () => {
    logoutUser();
    logoutUserQuery();
    setLogouted(true);
  };

  const handleScroll = (event: any) => {
    if (window.scrollY % 2 !== 0) {
      return;
    }

    if (window.scrollY >= 200) {
      setScrollLimit(true);
    } else {
      setScrollLimit(false);
    }
  };

  window.addEventListener("scroll", handleScroll);

  return (
    <div
      onScroll={handleScroll}
      className={classNames(
        "sticky top-0 z-50 w-full px-5 flex flex-row items-center but-anim",
        scrollLimit
          ? "bg-gradient-to-r from-[rgb(248,180,217,.4)] via-[rgb(202,191,253,.4)] to-[rgb(141,162,251,.4)] h-[3rem]"
          : "bg-gradient-to-r from-pink-300 via-purple-300 to-indigo-400 h-[4rem]"
      )}
    >
      <div className="flex flex-row justify-start flex-1">
        <img
          src={logo}
          alt="logo"
          className={classNames(
            "but-anim",
            scrollLimit ? "w-[2.5rem] lg:w-[3rem]" : "w-[3rem] lg:w-[5rem]"
          )}
        />
        <Link
          to="/"
          className={classNames(
            "font-bold but-anim",
            scrollLimit
              ? "text-[1rem] lg:text-[1.7rem]"
              : "text-[1.3rem] lg:text-[2rem]"
          )}
        >
          bookcamp
        </Link>
      </div>
      <div className="flex ml-10 lg:ml-0 flex-row flex-1">
        {!scrollLimit && (
          <form className="relative w-full" onSubmit={handleSubmit}>
            <input
              type="search"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              id="search"
              placeholder="Search books"
              className="lg:h-[2.5rem] h-[2rem] text-[0.6rem] lg:text-sm w-full pl-3 bg-white rounded-lg focus:outline-none focus:ring-[0.1rem] focus:ring-slate-600"
            />
            <button
              type="submit"
              className="absolute right-4 top-[0.5rem] lg:top-[0.80rem]"
            >
              <FaSearch className="text-[#1d3557] but-anim text-base hover:translate-y-[-0.1rem]" />
            </button>
          </form>
        )}
      </div>
      <div className="flex flex-row justify-end flex-1">
        {logouted ? (
          <Link
            to="/login"
            className={classNames(
              scrollLimit
                ? "bg-none text-black font-bold underline hover:translate-y-[-3px]"
                : "h-7 lg:h-8 bg-slate-800 text-white hover:bg-slate-700",
              "flex items-center px-7 rounded-lg transition duration-300 ease-in-out"
            )}
          >
            <p className="text-[0.7rem] lg:text-[1rem]">Login</p>
          </Link>
        ) : (
          <Menu as="div" className="relative inline-block text-left">
            {({ open }) => (
              <>
                <div>
                  <Menu.Button
                    className={classNames(
                      scrollLimit ? "" : "lg:bg-gray-700 lg:text-white",
                      "flex items-center but-anim justify-center h-10 lg:px-4 lg:py-2 rounded-lg"
                    )}
                  >
                    <p className="text-sm lg:text-base max-w-[10rem] whitespace-nowrap text-ellipsis overflow-hidden">
                      {user.name}
                    </p>
                    {user.imageUrl ? (
                      <img
                        src={user.imageUrl}
                        className="rounded-full h-8 w-8 ml-3 mr-1"
                      />
                    ) : (
                      <FaUserCircle className="text-3xl ml-3 mr-1" />
                    )}
                    {open ? (
                      <RiArrowDropUpFill className="text-lg" />
                    ) : (
                      <RiArrowDropDownFill className="text-lg" />
                    )}
                  </Menu.Button>
                </div>
                <Transition
                  enter="transition ease-out duration-100"
                  enterFrom="transform opacity-0 scale-95"
                  enterTo="transform opacity-100 scale-100"
                  leave="transition ease-in duration-75"
                  leaveFrom="transform opacity-100 scale-100"
                  leaveTo="transform opacity-0 scale-95"
                >
                  <Menu.Items className="absolute mt-1 right-0 w-full origin-top-right bg-gray-700 shadow-lg">
                    <div className="py-1">
                      <Menu.Item>
                        {({ active }) => (
                          <Link
                            to="/profile"
                            className={classNames(
                              active
                                ? "bg-gray-600 text-gray-300"
                                : "text-white",
                              "block w-full text-left px-4 py-2 text-sm"
                            )}
                          >
                            Profile
                          </Link>
                        )}
                      </Menu.Item>
                      <div className="border-t-[1px] border-gray-500">
                        <Menu.Item>
                          {({ active }) => (
                            <button
                              onClick={logout}
                              className={classNames(
                                active
                                  ? "bg-gray-600 text-gray-300"
                                  : "text-white",
                                "block w-full text-left px-4 py-2 text-sm"
                              )}
                            >
                              Sign out
                            </button>
                          )}
                        </Menu.Item>
                      </div>
                    </div>
                  </Menu.Items>
                </Transition>
              </>
            )}
          </Menu>
        )}
      </div>
    </div>
  );
}

export default Header;
