import React, { useEffect, useState } from "react";
import { FaUserCircle } from "react-icons/fa";
import { useAppSelector } from "../hooks/useAppSelector";
import { useInput } from "../hooks/useInput";
import { TEXT_REGEX } from "../hooks/useValidation";
import { IReview } from "../models/IReview";
import { booksApi } from "../services/booksApi";
import FormInput from "./FormInput";

interface ReviewInputProps {
  id: string;
  handleShowAlert: () => void;
}

function ReviewInput({ id, handleShowAlert }: ReviewInputProps) {
  const { imageUrl, name } = useAppSelector((state) => state.user.user);
  const { data: userReview } = booksApi.useGetReviewQuery(id!);
  const [addReview, { isSuccess }] = booksApi.useAddReviewMutation();

  const title = useInput("", TEXT_REGEX);
  const [body, setBody] = useState("");
  const [type, setType] = useState("1");
  const [validBody, setValidBody] = useState(false);
  const [isReviewAdded, setIsReviewAdded] = useState(false);
  const [bodyFilled, setBodyFilled] = useState(false);

  useEffect(() => {
    setIsReviewAdded(userReview?.body!);
  }, [userReview]);

  useEffect(() => {
    if (isSuccess) {
      window.location.reload();
    }
  }, [isSuccess]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (body.length < 128) {
      setValidBody(false);
      setBodyFilled(true);
      return;
    }

    if (!title.valid.isRegexPass) {
      return;
    }

    const review: IReview = {
      title: title.bind.value,
      body: body,
      bookId: id,
      type: parseInt(type),
    };

    if (!isReviewAdded) {
      addReview(review);
    }
  };

  return (
    <div className="flex flex-row justify-start" id="reviewInput">
      {imageUrl ? (
        <img
          src={imageUrl}
          className="hidden lg:block rounded-full h-12 w-12 mr-5"
        />
      ) : (
        <FaUserCircle className="hidden lg:block text-[3rem] mr-5" />
      )}
      <form
        className="flex flex-col w-[100%] xl:w-[50%]"
        onSubmit={handleSubmit}
      >
        <label
          className="block text-gray-700 text-sm font-bold mb-2"
          htmlFor="type"
        >
          Type
        </label>
        <select
          id="type"
          className="mb-4 shadow appearance-none text-[16px] border rounded w-full px-3 py-2 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          value={type}
          onChange={(e) => setType(e.target.value)}
        >
          <option value="1">Positive</option>
          <option value="2">Negative</option>
          <option value="3">Neutral</option>
        </select>
        <FormInput id="title" type="text" placeholder="Title" input={title} />
        <label
          className="block text-gray-700 text-sm font-bold mb-2"
          htmlFor="body"
        >
          Body
        </label>
        <textarea
          id="body"
          className="rounded min-h-[5rem] h-[5rem] max-h-[25rem] overflow-auto"
          placeholder="Body"
          value={body}
          onChange={(e) => setBody(e.target.value)}
          minLength={256}
          maxLength={2048}
        ></textarea>
        {!validBody && bodyFilled && (
          <p className="mt-2 text-red-600 text-sm">
            Review must be no shorter than 256 symbols and no longer than 2048
            symbols
          </p>
        )}
        <div className="flex justify-end mt-4">
          {name ? (
            <button
              type="submit"
              className="text-white bg-black px-5 py-2 rounded-lg but-anim hover:opacity-80"
            >
              <p>Publish review</p>
            </button>
          ) : (
            <button
              type="button"
              className="text-white bg-black px-5 py-2 rounded-lg but-anim hover:opacity-80"
              onClick={handleShowAlert}
            >
              Publish review
            </button>
          )}
        </div>
      </form>
    </div>
  );
}

export default ReviewInput;
