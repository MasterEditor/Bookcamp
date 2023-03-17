import { Menu, Transition } from "@headlessui/react";
import { Alert, Breadcrumb } from "flowbite-react";
import React, { ChangeEvent, useEffect, useRef, useState } from "react";
import { AiFillStar } from "react-icons/ai";
import { BiBookHeart } from "react-icons/bi";
import { RiArrowDropDownFill, RiArrowDropUpFill } from "react-icons/ri";
import { createSearchParams, useNavigate, useParams } from "react-router-dom";
import { HashLink } from "react-router-hash-link";
import { ADMIN } from "../constants/roles";
import { useAppSelector } from "../hooks/useAppSelector";
import { IBook } from "../models/IBook";
import { booksApi } from "../services/booksApi";
import parse from "html-react-parser";
import classNames from "classnames";
import { authApi } from "../services/authApi";
import { IComment } from "../models/IComment";

interface OneBookInfoProps {
  id: string;
  book: IBook;
  handleShowAlert: () => void;
  showAlert: boolean;
}

function OneBookInfo({
  id,
  book,
  handleShowAlert,
  showAlert,
}: OneBookInfoProps) {
  const { favourites, name, role } = useAppSelector((state) => state.user.user);
  const rating: number[] = [1, 2, 3, 4, 5];
  const navigate = useNavigate();
  const fragmentRef = useRef<HTMLInputElement>(null);
  const [isCoverUpdate, setIsCoverUpdate] = useState(false);
  const [isAddFragment, setIsAddFragment] = useState(false);
  const [isInFavourite, setIsInFavourite] = useState(false);
  const [comments, setComments] = useState<IComment[]>([]);
  const [userRate, setUserRate] = useState(0);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const { data: commentsData } = booksApi.useGetCommentsQuery({ bookId: id! });
  const { data: userRateData } = booksApi.useGetRatingQuery(id!);
  const [deleteBook, { isSuccess: bookDeleteSuccess }] =
    booksApi.useDeleteBookMutation();
  const [addToFavourite] = authApi.useAddToFavouriteMutation();
  const [removeFromFavourite] = authApi.useRemoveFromFavouriteMutation();
  const [addRating] = booksApi.useAddRatingMutation();
  const [updateFragment, { isSuccess: updateFragmentSuccess }] =
    booksApi.useUpdateFragmentMutation();
  const [updateCover, { isSuccess: updateCoverSuccess }] =
    booksApi.useUpdateCoverMutation();
  const [addFragment, { isSuccess: addFragmentSuccess }] =
    booksApi.useAddNewFragmentMutation();
  const [deleteFragment, { isSuccess: deleteFragmentSuccess }] =
    booksApi.useDeleteFragmentMutation();

  useEffect(() => {
    if (book && favourites?.includes(book.id)) {
      setIsInFavourite(true);
    }
  }, [book]);

  useEffect(() => {
    setComments(commentsData?.body!);
  }, [commentsData]);

  useEffect(() => {
    setUserRate(userRateData?.body!);
  }, [userRateData]);

  useEffect(() => {
    if (name) {
      setIsLoggedIn(true);
    }
  }, [name]);

  useEffect(() => {
    if (bookDeleteSuccess) {
      navigate("/admin-books");
    }
  }, [bookDeleteSuccess]);

  useEffect(() => {
    if (
      updateFragmentSuccess ||
      deleteFragmentSuccess ||
      updateCoverSuccess ||
      addFragmentSuccess
    ) {
      window.location.reload();
    }
  }, [
    updateFragmentSuccess,
    deleteFragmentSuccess,
    updateCoverSuccess,
    addFragmentSuccess,
  ]);

  const handleBack = (back: string) => {
    navigate({
      pathname: "/books",
      search: `?${createSearchParams({ genres: back })}`,
    });
  };

  const handleDeleteBook = () => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteBook(book?.id!);
    }
  };

  const coverUpdateClick = () => {
    setIsCoverUpdate(true);
    setIsAddFragment(false);
    fragmentRef.current!.accept = ".png, .jpg, .jpeg, .gif, .tiff";
    fragmentRef.current!.click();
  };

  const handleFavourite = () => {
    if (favourites) {
      if (!isInFavourite) {
        addToFavourite({ bookId: id! });
      } else {
        removeFromFavourite({ bookId: id! });
      }
      setIsInFavourite(!isInFavourite);
    } else {
      handleShowAlert();
    }
  };

  const handleRateClick = (rate: number) => {
    if (!favourites) {
      handleShowAlert();
      return;
    }

    addRating({ rate: rate, bookId: id! });
    window.location.reload();
  };

  const handleFragmentChange = (event: ChangeEvent<HTMLInputElement>) => {
    const fileObj = event.target.files && event.target.files[0];
    if (!fileObj) {
      return;
    }

    event.target.value = "";

    const formData = new FormData();
    formData.append("file", fileObj);

    if (isCoverUpdate) {
      updateCover({ id: book!.id, data: formData });
    } else {
      if (isAddFragment) {
        addFragment({ id: book!.id, data: formData });
      } else {
        updateFragment({ id: book!.id, data: formData });
      }
    }
  };

  const handleFragmentClick = (ext: string) => {
    setIsCoverUpdate(false);
    setIsAddFragment(false);
    fragmentRef.current!.accept = ext;
    fragmentRef.current!.click();
  };

  const handleFragmentDelete = (ext: string) => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteFragment({ id: book!.id, ext: ext });
    }
  };

  const handleAddNewFragment = () => {
    setIsAddFragment(true);
    fragmentRef.current!.accept = ".pdf, .epub, .txt, .fb2";
    fragmentRef.current!.click();
  };

  return (
    <>
      <div className="flex flex-row justify-between">
        <Breadcrumb className="mt-5 ml-5">
          <Breadcrumb.Item onClick={() => handleBack("")}>
            <p className="cursor-pointer hover:font-bold">Books</p>
          </Breadcrumb.Item>
          <Breadcrumb.Item onClick={() => handleBack(book?.genre!)}>
            <p className="cursor-pointer hover:font-bold">{book?.genre}</p>
          </Breadcrumb.Item>
          <Breadcrumb.Item>{book?.name}</Breadcrumb.Item>
        </Breadcrumb>
        {role === ADMIN && (
          <p
            className="underline text-red-800 cursor-pointer mt-5 mr-5 hover:translate-y-[-3px] but-anim"
            onClick={handleDeleteBook}
          >
            Delete
          </p>
        )}
      </div>
      <div className="flex flex-col mb-10">
        <div className="flex flex-col lg:flex-row justify-start mt-16 mx-5 2xl:mx-16 border-b border-[#ced4da] pb-10">
          <div className="basis-2/4 flex flex-col items-center justify-center mb-10 lg:mb-0 mr-5 xl:mr-0">
            <img
              src={book?.cover && book?.cover}
              className="w-[25rem] h-[35rem] mt-3"
            />
            {role === ADMIN && (
              <button
                className="mt-2 underline text-yellow-400"
                onClick={coverUpdateClick}
              >
                Update cover
              </button>
            )}
          </div>
          <div className="flex flex-col basis-3/4 xl:mr-[6rem]">
            {showAlert && (
              <Alert color="info">
                <span>
                  <span className="font-medium">Not a user!</span> Please login
                  and try again.
                </span>
              </Alert>
            )}
            <div className="flex flex-row justify-between">
              <h1 className="font-light text-[1.5rem] lg:text-[2.5rem] lg:w-[30rem]">
                {book?.name}
              </h1>
              <div className="flex flex-row justify-center items-center">
                <button onClick={handleFavourite}>
                  <BiBookHeart
                    className={classNames(
                      isInFavourite ? "text-red-600" : "",
                      "text-3xl mr-2 but-anim hover:translate-y-[-3px]"
                    )}
                  />
                </button>
                <p className={isInFavourite ? "text-red-600" : ""}>
                  {isInFavourite ? "In favourites" : "Add to favourite"}
                </p>
              </div>
            </div>
            <h2 className="font-medium text-xl mt-2 text-[#6c757d]">
              by {book?.author}
            </h2>
            <div className="mt-5 flex flex-row items-center">
              <div className="flex flex-row items-center rate">
                {rating.map((item) => (
                  <AiFillStar
                    key={item}
                    className={classNames(
                      book?.rating! >= item
                        ? "text-[#ffb703]"
                        : "text-[#d3d3d3]",
                      "rate-star cursor-pointer text-2xl"
                    )}
                    onClick={() => handleRateClick(item)}
                  />
                ))}
              </div>
              <p className="ml-2 text-sm font-bold text-gray-900 dark:text-white">
                {book?.rating}
              </p>
              <span className="mx-1.5 h-1 w-1 rounded-full bg-gray-500 dark:bg-gray-400" />
              <HashLink
                smooth
                to="#comments"
                className="text-sm font-medium text-gray-900 underline hover:no-underline dark:text-white"
              >
                {comments?.length} comments
              </HashLink>
            </div>
            {userRate !== undefined && userRate !== 0 && (
              <p className="mt-2 text-sm text-[#ffb703] italic">
                Your rate is {userRate}
              </p>
            )}
            <div className="flex flex-row justify-start items-start mt-3 mb-5">
              <div className="flex flex-col text-left font-bold">
                <p>Publish date</p>
                <p className="text-center font-normal text-sm text-[#495057]">
                  {book?.publishDate}
                </p>
              </div>
              <div className="flex flex-col text-left mx-10 font-bold">
                <p>Language</p>
                <p className="text-center font-normal text-sm text-[#495057]">
                  {book?.language}
                </p>
              </div>
              <div className="flex flex-col text-left font-bold">
                <p>Pages</p>
                <p className="text-center font-normal text-sm text-[#495057]">
                  {book?.pages}
                </p>
              </div>
            </div>
            <p className="text-sm mt-2 break-words">
              {book?.about && parse(book?.about!)}
            </p>
            <div className="flex justify-center items-center my-5 lg:mb-10 lg:mt-10 w-fit h-fit py-3 px-5 font-bold text-2xl rounded-full bg-[#e8c1ff]">
              <p>{book?.price !== 0 ? `${book?.price}$` : "free"}</p>
            </div>
            <input
              ref={fragmentRef}
              onChange={handleFragmentChange}
              id="file"
              type="file"
              className="hidden"
              multiple={false}
            />
            <div className="flex flex-row gap-4 lg:mt-4">
              <Menu as="div" className="relative inline-block text-left">
                {({ open }) => (
                  <>
                    <div>
                      <Menu.Button className="flex flex-row justify-between pl-3 text-white bg-black py-2 w-[14rem]">
                        <p className="text-base">Download fragment</p>
                        {open ? (
                          <div>
                            <RiArrowDropUpFill className="text-2xl" />
                          </div>
                        ) : (
                          <RiArrowDropDownFill className="text-2xl" />
                        )}
                      </Menu.Button>
                    </div>
                    <Transition
                      enter="transition ease-out duration-300"
                      enterFrom="transform opacity-0 scale-y-95"
                      enterTo="transform opacity-100 scale-y-100"
                      leave="transition ease-in duration-75"
                      leaveFrom="transform opacity-100 scale-y-100"
                      leaveTo="transform opacity-0 scale-y-95"
                    >
                      <Menu.Items className="absolute left-0 min-w-[14rem] text-white origin-top-left bg-black shadow-lg">
                        <div className="py-1 border-t border-[#e8c1ff]">
                          <Menu.Item>
                            {({ active }) =>
                              book?.fragmentPaths ? (
                                <div className="flex flex-col items-start">
                                  {book.fragmentPaths.map((item, key) => (
                                    <button
                                      className="flex flex-row justify-start w-full but-anim"
                                      key={key}
                                    >
                                      {isLoggedIn ? (
                                        <a
                                          href={item}
                                          className="w-full hover:text-[#ae2012]"
                                          download={item}
                                        >
                                          {item.substring(
                                            item.lastIndexOf("/") + 1
                                          )}
                                        </a>
                                      ) : (
                                        <>
                                          <p
                                            className="w-full"
                                            onClick={handleShowAlert}
                                          >
                                            {item.substring(
                                              item.lastIndexOf("/") + 1
                                            )}
                                          </p>
                                          {role === ADMIN && (
                                            <div className="flex">
                                              <p
                                                className="hover:text-yellow-400"
                                                onClick={() =>
                                                  handleFragmentClick(
                                                    item.substring(
                                                      item.lastIndexOf("/") + 1
                                                    )
                                                  )
                                                }
                                              >
                                                Update
                                              </p>
                                              <p
                                                className="mx-2 hover:text-red-800"
                                                onClick={() =>
                                                  handleFragmentDelete(
                                                    item.substring(
                                                      item.lastIndexOf("/") + 1
                                                    )
                                                  )
                                                }
                                              >
                                                Delete
                                              </p>
                                            </div>
                                          )}
                                        </>
                                      )}
                                    </button>
                                  ))}
                                </div>
                              ) : (
                                <p></p>
                              )
                            }
                          </Menu.Item>
                        </div>
                      </Menu.Items>
                    </Transition>
                  </>
                )}
              </Menu>
              {role === ADMIN && (
                <button
                  className="mt-2 underline text-yellow-400"
                  onClick={handleAddNewFragment}
                >
                  Add new fragment
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default OneBookInfo;
