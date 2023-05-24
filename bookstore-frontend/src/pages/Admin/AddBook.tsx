import classNames from "classnames";
import { Alert } from "flowbite-react";
import React, { ChangeEvent, useEffect, useState } from "react";
import AdminHeader from "../../components/AdminHeader";
import FormInput from "../../components/FormInput";
import { useInput } from "../../hooks/useInput";
import { REQUIRED } from "../../hooks/useValidation";
import { booksApi } from "../../services/booksApi";

function AddBook() {
  const bookId = useInput("", REQUIRED);
  const [alertHidden, setAlertHidden] = useState(true);
  const [fragments, setFragments] = useState<FileList>();
  const [cover, setCover] = useState<File>();

  const [addBook, { isLoading, isSuccess, isError }] =
    booksApi.useAddBookMutation();

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const formData = new FormData();

    formData.append("bookId", bookId.bind.value);
    for (let i = 0; i < fragments?.length!; i++) {
      formData.append("fragments", fragments![i]);
    }

    formData.append("cover", cover!);

    addBook(formData);
  };

  useEffect(() => {
    if (isSuccess || isError) {
      setAlertHidden(false);
    }
  }, [isLoading, isSuccess, isError]);

  const onFragmentsChange = (event: ChangeEvent<HTMLInputElement>) => {
    const frags = event.target.files;
    if (!frags) {
      return;
    }

    setFragments(frags);
  };

  const alertDissmiss = () => {
    setAlertHidden(true);
  };

  const onCoverChange = (event: ChangeEvent<HTMLInputElement>) => {
    const fileCover = event.target.files && event.target.files[0];
    if (!fileCover) {
      return;
    }

    setCover(fileCover);
  };

  return (
    <>
      <AdminHeader />
      <Alert
        color={isSuccess ? "success" : "failure"}
        onDismiss={alertDissmiss}
        className={classNames(
          alertHidden ? "hidden" : "block",
          "absolute top-24 z-10 right-2"
        )}
      >
        <span className="font-medium">
          {isSuccess ? "Book successfully added" : "Something went wrong!"}
        </span>
      </Alert>
      <div className="mx-auto w-[400px]">
        <div className="flex justify-center mt-20">
          <form
            onSubmit={handleSubmit}
            className="bg-white shadow-md w-full rounded-lg px-8 pt-6 pb-8 mb-4"
          >
            <h2 className="text-xl mb-8 mt-6 text-center font-bold">
              Add new book
            </h2>
            <FormInput
              id="bookId"
              type="text"
              placeholder="Book id from google api"
              input={bookId}
            />

            <label
              htmlFor="file"
              className="block text-gray-700 text-sm font-bold mb-2"
            >
              Choose book fragments
            </label>
            <input
              id="file"
              type="file"
              accept=".pdf, .epub, .txt, .fb2"
              className="mb-6 w-[15rem]"
              onChange={onFragmentsChange}
              multiple={true}
            />

            <label
              htmlFor="cover"
              className="block text-gray-700 text-sm font-bold mb-2"
            >
              Choose the cover
            </label>
            <input
              id="cover"
              type="file"
              accept=".png, .jpg, .jpeg"
              className="mb-6 w-[15rem]"
              onChange={onCoverChange}
              multiple={false}
            />

            <button
              disabled={!bookId.valid.isRegexPass || isLoading}
              type="submit"
              className="rounded-xl bg-[#2c968f] text-white p-2 w-full mt-3 mb-4 transition duration-300 ease-in-out hover:opacity-90"
            >
              <p>Add</p>
            </button>
          </form>
        </div>
      </div>
    </>
  );
}

export default AddBook;
