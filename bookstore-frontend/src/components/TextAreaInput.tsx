import React, { useEffect, useState } from "react";
import { FaUserCircle } from "react-icons/fa";
import { useAppSelector } from "../hooks/useAppSelector";
import { booksApi } from "../services/booksApi";

interface TextAreaInputProps {
  id: string;
  handleShowAlert: () => void;
}

function TextAreaInput({ id, handleShowAlert }: TextAreaInputProps) {
  const { favourites, imageUrl } = useAppSelector((state) => state.user.user);

  const [comment, setComment] = useState("");
  const [validComment, setValidComment] = useState(false);
  const [isCommentAdded, setIsCommentAdded] = useState(false);
  const [commentFilled, setCommentFilled] = useState(false);

  const { data: userComment } = booksApi.useGetCommentQuery(id!);
  const [addComment, { isSuccess: addCommentSuccess }] =
    booksApi.useAddCommentMutation();

  useEffect(() => {
    if (addCommentSuccess) {
      window.location.reload();
    }
  }, [addCommentSuccess]);

  useEffect(() => {
    setIsCommentAdded(userComment?.body!);
  }, [userComment]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (comment.length < 10) {
      setValidComment(false);
      setCommentFilled(true);
      return;
    }

    if (!isCommentAdded) {
      addComment({ bookId: id!, comment: comment });
    }
  };

  return (
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
          className="rounded 2xl:w-[50rem] min-h-[5rem] h-[5rem] max-h-[8rem] overflow-auto"
          placeholder="Your comment"
          value={comment}
          onChange={(e) => setComment(e.target.value)}
          minLength={10}
          maxLength={256}
        ></textarea>
        {!validComment && commentFilled && (
          <p className="mt-2 text-red-600 text-sm">
            Comment must be no shorter than 10 symbols and no longer than 256
            symbols
          </p>
        )}
        <div className="flex justify-end mt-4 2xl:mr-8">
          {favourites ? (
            <button
              type="submit"
              className="text-white bg-black px-5 py-2 rounded-lg but-anim hover:opacity-80"
            >
              <p>Add comment</p>
            </button>
          ) : (
            <button
              className="text-white bg-black px-5 py-2 rounded-lg but-anim hover:opacity-80"
              onClick={handleShowAlert}
            >
              Add comment
            </button>
          )}
        </div>
      </form>
    </div>
  );
}

export default TextAreaInput;
