import React, { useState, useEffect } from "react";
import {
  createSearchParams,
  Link,
  useNavigate,
  useParams,
} from "react-router-dom";
import { HashLink } from "react-router-hash-link";
import { BiBookHeart } from "react-icons/bi";
import Header from "../components/Header";
import { FaUserCircle } from "react-icons/fa";
import Footer from "../components/Footer";
import { Menu, Transition } from "@headlessui/react";
import { RiArrowDropDownFill, RiArrowDropUpFill } from "react-icons/ri";
import { AiFillStar } from "react-icons/ai";
import { Alert, Breadcrumb } from "flowbite-react";
import "../hover.scss";
import SingleBook from "../components/SingleBook";
import { booksApi } from "../services/booksApi";
import { IBook } from "../models/IBook";
import { useAppSelector } from "../hooks/useAppSelector";
import classNames from "classnames";
import { authApi } from "../services/authApi";
import { IReview } from "../models/IReview";
import Review from "../components/Review";
import parse from "html-react-parser";
import { useCookies } from "react-cookie";
import { ADMIN } from "../constants/roles";
import AdminHeader from "../components/AdminHeader";

function OneBook() {
  const { id } = useParams();

  const { favourites, imageUrl, name } = useAppSelector(
    (state) => state.user.user
  );
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const [cookies] = useCookies(["bc_role"]);

  const [showAlert, setShowAlert] = useState(false);
  const [userRate, setUserRate] = useState(0);

  const [isInFavourite, setIsInFavourite] = useState(false);

  const [review, setReview] = useState("");
  const [reviews, setReviews] = useState<IReview[]>([]);
  const [validReview, setValidReview] = useState(false);
  const [reviewFilled, setReviewFilled] = useState(false);

  const [deleteBook, { isSuccess: bookDeleteSuccess }] =
    booksApi.useDeleteBookMutation();
  const [addToFavourite] = authApi.useAddToFavouriteMutation();
  const [removeFromFavourite] = authApi.useRemoveFromFavouriteMutation();
  const [addReview, { isSuccess: addReviewSuccess }] =
    booksApi.useAddReviewMutation();
  const [addRating] = booksApi.useAddRatingMutation();
  const { data: userRateData } = booksApi.useGetRatingQuery(id!);
  const { data: userReview } = booksApi.useGetReviewQuery(id!);
  const { data: reviewsData } = booksApi.useGetReviewsQuery(id!);

  const [isReviewAdded, setIsReviedAdded] = useState(false);

  useEffect(() => {
    if (favourites?.find((x) => x === id) !== undefined) {
      setIsInFavourite(true);
    }
  }, [favourites]);

  useEffect(() => {
    if (name) {
      setIsLoggedIn(true);
    }
  }, [name]);

  useEffect(() => {
    setReviews(reviewsData?.body!);
  }, [reviewsData]);

  useEffect(() => {
    setUserRate(userRateData?.body!);
  }, [userRateData]);

  useEffect(() => {
    setIsReviedAdded(userReview?.body!);
  }, [userReview]);

  useEffect(() => {
    if (bookDeleteSuccess) {
      navigate("/admin-books");
    }
  }, [bookDeleteSuccess]);

  useEffect(() => {
    if (addReviewSuccess) {
      window.location.reload();
    }
  }, [addReviewSuccess]);

  useEffect(() => {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  }, [id, showAlert]);

  const { data: bookData } = booksApi.useGetBookQuery(id!);

  const [getAuthorBooks, { data: authorBooksData }] =
    booksApi.useLazyGetAuthorBooksQuery();

  const [authorBooks, setAuthorBooks] = useState<IBook[]>([]);

  const [book, setBook] = useState<IBook>();

  const rating: number[] = [1, 2, 3, 4, 5];

  useEffect(() => {
    setBook(bookData?.body!);

    getAuthorBooks({ author: bookData?.body.author!, id: id! });
  }, [bookData]);

  useEffect(() => {
    setAuthorBooks(authorBooksData?.body!);
  }, [authorBooksData]);

  const navigate = useNavigate();

  const handleBookClick = (bookId: string) => {
    navigate(`/book/${bookId}`);
  };

  const handleBack = (back: string) => {
    navigate({
      pathname: "/books",
      search: `?${createSearchParams({ genres: back })}`,
    });
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
      setShowAlert(true);
    }
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (review.length < 10) {
      setValidReview(false);
      setReviewFilled(true);
      return;
    }

    if (!isReviewAdded) {
      addReview({ bookId: id!, review: review });
    }
  };

  const handleRateClick = (rate: number) => {
    if (!favourites) {
      setShowAlert(true);
      return;
    }

    addRating({ rate: rate, bookId: id! });
    window.location.reload();
  };

  const handleShowAlert = () => {
    if (showAlert) {
      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    } else {
      setShowAlert(true);
    }
  };

  const handleDeleteBook = () => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteBook(book?.id!);
    }
  };

  const handleAuthorBooks = () => {
    navigate({
      pathname: "/books",
      search: `?${createSearchParams({ keywords: book?.author! })}`,
    });
  };

  return (
    <>
      {cookies.bc_role === ADMIN ? <AdminHeader /> : <Header />}
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
        {cookies.bc_role === ADMIN && (
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
          <div className="basis-2/4 flex justify-center mb-10 lg:mb-0 mr-5 xl:mr-0">
            <img
              src={book?.cover && book?.cover}
              className="w-[25rem] h-[35rem] mt-3"
            />
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
                to="/book/:id#reviews"
                className="text-sm font-medium text-gray-900 underline hover:no-underline dark:text-white"
              >
                {reviews?.length} reviews
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
            <Menu as="div" className="relative inline-block text-left lg:mt-4">
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
                                    className="flex flex-row justify-start w-full hover:text-[#ae2012] but-anim"
                                    key={key}
                                  >
                                    {isLoggedIn ? (
                                      <a
                                        href={item}
                                        className="w-full"
                                        download={item}
                                      >
                                        {item.substring(
                                          item.lastIndexOf("/") + 1
                                        )}
                                      </a>
                                    ) : (
                                      <p
                                        className="w-full"
                                        onClick={handleShowAlert}
                                      >
                                        {item.substring(
                                          item.lastIndexOf("/") + 1
                                        )}
                                      </p>
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
          </div>
        </div>
        {/* Books */}
        <div className="flex flex-col mt-6 px-16 border-b border-[#ced4da] pb-10 bg-[#ededed]">
          <p className="mt-5 mb-10 text-xl lg:text-3xl">From the publisher</p>
          <div className="flex w-full h-full flex-row justify-center">
            {authorBooks &&
              authorBooks.map((item) => (
                <div
                  className="basis-1/6 bg-[#f3f3f3] m-5 cursor-pointer but-anim
               hover:translate-y-[-0.5rem]"
                  key={item.id}
                  onClick={() => handleBookClick(item.id)}
                >
                  <SingleBook {...item} />
                </div>
              ))}
          </div>
          <div className="flex w-full justify-end">
            <button
              className="learn-more text-sm mt-7 text-left w-[10rem]"
              onClick={handleAuthorBooks}
            >
              <span className="circle" aria-hidden="true">
                <span className="icon arrow"></span>
              </span>
              <span className="button-text">See all</span>
            </button>
          </div>
        </div>
        {/* Reviews */}
        <div className="flex flex-col mt-6 mx-16" id="reviews">
          <p className="mt-5 mb-10 text-xl lg:text-3xl">Reviews</p>
          <div className="flex flex-row justify-start">
            {imageUrl ? (
              <img
                src={imageUrl}
                className="hidden lg:block rounded-full h-12 w-12 mr-5"
              />
            ) : (
              <FaUserCircle className="hidden lg:block text-[3rem] mr-5" />
            )}
            <form
              className="flex flex-col w-[100%] xl:w-[60%]"
              onSubmit={handleSubmit}
            >
              <textarea
                className="rounded 2xl:w-[50rem] h-[5rem] overflow-auto"
                placeholder="Your review"
                value={review}
                onChange={(e) => setReview(e.target.value)}
                minLength={10}
                maxLength={256}
              ></textarea>
              {!validReview && reviewFilled && (
                <p className="mt-2 text-red-600 text-sm">
                  Review must be no shorter than 10 symbols and no longer than
                  256 symbols
                </p>
              )}
              <div className="flex justify-end mt-4 2xl:mr-8">
                {favourites ? (
                  <button
                    type="submit"
                    className="text-white bg-black px-5 py-2 rounded-lg but-anim hover:opacity-80"
                  >
                    <p>Add review</p>
                  </button>
                ) : (
                  <button
                    className="text-white bg-black px-5 py-2 rounded-lg but-anim hover:opacity-80"
                    onClick={handleShowAlert}
                  >
                    Add review
                  </button>
                )}
              </div>
            </form>
          </div>
          <div className="mt-10">
            {reviews ? (
              reviews.map((item) => <Review {...item} key={item.id} />)
            ) : (
              <p>No reviews added</p>
            )}
          </div>
        </div>
      </div>
      <Footer />
    </>
  );
}

export default OneBook;
