import React, { useEffect } from "react";
import { FaUserCircle } from "react-icons/fa";
import { IComment } from "../models/IComment";
import { ImCross } from "react-icons/im";
import { ADMIN } from "../constants/roles";
import { booksApi } from "../services/booksApi";
import { useAppSelector } from "../hooks/useAppSelector";

function Comment(props: IComment) {
  const { role } = useAppSelector((state) => state.user.user);

  const [deleteComment, { isSuccess }] = booksApi.useDeleteCommentMutation();

  useEffect(() => {
    if (isSuccess) {
      window.location.reload();
    }
  }, [isSuccess]);

  const handleDeleteReview = () => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteComment(props.id!);
    }
  };

  return (
    <div className="flex flex-row justify-start w-[25rem] p-5 h-[15rem] bg-[#edf2f4]">
      {props.imageUrl ? (
        <img src={props.imageUrl} className="rounded-full h-12 w-12" />
      ) : (
        <FaUserCircle className="text-[3rem]" />
      )}
      <div className="flex flex-col ml-5">
        <div className="flex flex-row">
          <span className="font-bold">{props.userName}</span>
          <span className="font-bold mx-2">-</span>
          <span className="font-bold">{props.addedTime}</span>
        </div>
        <p className="text-sm max-w-[15rem] break-words">{props.comment}</p>
      </div>
      {role === ADMIN && (
        <div className="flex items-center">
          <ImCross
            className="text-red-700 cursor-pointer ml-10 but-anim hover:rotate-90"
            onClick={handleDeleteReview}
          />
        </div>
      )}
    </div>
  );
}

export default Comment;
