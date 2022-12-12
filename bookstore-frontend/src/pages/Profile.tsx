import React, { ChangeEvent, useEffect, useRef, useState } from "react";
import Header from "../components/Header";
import { useAppSelector } from "../hooks/useAppSelector";
import { FaUserCircle } from "react-icons/fa";
import { BiEditAlt } from "react-icons/bi";
import { useActions } from "../hooks/useActions";
import { authApi, BASE_URL } from "../services/authApi";
import DotLoader from "react-spinners/DotLoader";
import { FadeLoader } from "react-spinners";
import SingleBook from "../components/SingleBook";
import { booksApi } from "../services/booksApi";
import { IBook } from "../models/IBook";
import { useNavigate } from "react-router-dom";

function Profile() {
  const user = useAppSelector((state) => state.user.user);

  const navigate = useNavigate();

  const {
    data: booksData,
    isSuccess: booksSuccess,
    isLoading: booksLoading,
  } = booksApi.useGetFavouritesQuery();

  const [books, setBooks] = useState<IBook[]>([]);

  useEffect(() => {
    if (booksSuccess) {
      setBooks(booksData?.body!);
    }
  }, [booksData, booksSuccess]);

  const { storeImage, updateName } = useActions();

  const [isEditing, setIsEditing] = useState(false);

  const [updateImage, { isLoading, isSuccess, data }] =
    authApi.useUpdateImageMutation();

  const [updateUserName] = authApi.useUpdateUserNameMutation();

  const imageRef: any = useRef(null);

  const handleImageClick = () => {
    imageRef.current.click();
  };

  const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
    const fileObj = event.target.files && event.target.files[0];
    if (!fileObj) {
      return;
    }

    event.target.value = "";

    const formData = new FormData();
    formData.append("file", fileObj);

    updateImage(formData);
  };

  useEffect(() => {
    if (isSuccess) {
      const url = `${BASE_URL}/api/user/images/${data!.body}`;

      storeImage(url);
      window.location.reload();
    }
  }, [isLoading]);

  const handleEditProfile = () => {
    setIsEditing(!isEditing);

    if (isEditing) {
      const request = {
        name: user.name!,
      };

      updateUserName(request);
    }
  };

  const handleNameChange = (event: ChangeEvent<HTMLInputElement>) => {
    updateName(event.target.value);
  };

  const handleBookClick = (bookId: string) => {
    navigate(`/book/${bookId}`);
  };

  return (
    <>
      <Header />
      <div className="flex flex-col h-full">
        <div className="flex flex-row pt-10 mb-10 px-16 bg-gradient-to-bl from-rose-100 to-teal-100">
          <div
            onClick={handleImageClick}
            className="relative cursor-pointer but-anim w-[15rem]"
          >
            <div className="opacity-0 rounded-full h-52 w-52 bg-[rgba(0,0,0,0.4)] hover:opacity-100 but-anim absolute inset-0 z-10 flex flex-col justify-center items-center text-white font-semibold">
              <BiEditAlt className="text-4xl" />
              <p>Choose a photo</p>
            </div>
            <div className="absolute w-full h-full left-6 top-6">
              <DotLoader color="#34a0a4" size={50} loading={isLoading} />
            </div>
            <input
              ref={imageRef}
              onChange={handleFileChange}
              type="file"
              accept=".png, .jpg, .jpeg, .gif, .tiff"
              className="hidden"
              multiple={false}
            />
            {user.imageUrl ? (
              <img
                src={user.imageUrl}
                className="rounded-full mb-6 h-52 w-52"
                alt="user_image"
              />
            ) : (
              <FaUserCircle className="text-[6rem] mb-6 h-52 w-52" />
            )}
          </div>
          <div className="flex flex-col justify-center ml-5">
            <p className="font-semibold">PROFILE</p>
            <h1 className="text-[3rem] font-bold">{user.name}</h1>
          </div>
        </div>
        <div className="flex flex-col space-y-4 px-16">
          <div className="flex text-[#adb5bd] flex-row justify-between border-b border-[#adb5bd]">
            <p className={!isEditing ? "" : "pt-3 font-bold text-black"}>
              user_name
            </p>
            {!isEditing ? (
              <p>{user.name}</p>
            ) : (
              <input
                type="text"
                maxLength={32}
                minLength={2}
                className="text-black w-[25rem] font-bold pr-2 text-right border-none bg-inherit focus:outline-none focus:ring-0"
                value={user.name}
                placeholder="Your name"
                onChange={handleNameChange}
              />
            )}
          </div>
          <div className="flex text-[#adb5bd] flex-row justify-between border-b border-[#adb5bd]">
            <p className="">e-mail</p>
            <p>{user.email}</p>
          </div>
          <div className="flex text-[#adb5bd] flex-row justify-between border-b border-[#adb5bd]">
            <p className="">birthday</p>
            <p>{user.birth}</p>
          </div>
          <div className="flex text-[#adb5bd] flex-row justify-between border-b border-[#adb5bd]">
            <p className="">register date</p>
            <p>{user.registerDate}</p>
          </div>
          <div className="flex text-[#adb5bd] flex-row justify-between border-b border-[#adb5bd]">
            <p className="">gender</p>
            <p>{user.gender === 1 ? "Man" : "Woman"}</p>
          </div>
          <button
            onClick={handleEditProfile}
            className="flex flex-row justify-center items-center w-[10rem] bg-black text-white px-4 py-2 but-anim  hover:animate-wiggle"
          >
            {!isEditing ? (
              <>
                <BiEditAlt className="mr-1" />
                <p>Edit profile</p>
              </>
            ) : (
              <p>Update</p>
            )}
          </button>
        </div>
        <h1 className="ml-16 mt-10 mb-5 text-3xl">Favourite books</h1>
        <div className="w-full flex flex-row flex-wrap justify-center">
          {booksLoading ? (
            <FadeLoader className="mt-10" />
          ) : books.length === 0 ? (
            <h1 className="text-left text-xl">Empty</h1>
          ) : (
            books.map((item) => (
              <div
                className="basis-1/5 bg-[#f3f3f3] m-5 cursor-pointer but-anim
               hover:translate-y-[-0.5rem] hover:rotate-1 hover:shadow-[5px_5px_0px_0px_rgba(213,180,175)]"
                key={item.id}
                onClick={() => handleBookClick(item.id)}
              >
                <SingleBook {...item} />
              </div>
            ))
          )}
        </div>
      </div>
    </>
  );
}

export default Profile;
