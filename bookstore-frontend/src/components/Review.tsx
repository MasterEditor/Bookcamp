import React, { useEffect } from "react";
import { useCookies } from "react-cookie";
import { FaUserCircle } from "react-icons/fa";
import { IReview } from "../models/IReview";
import { ImCross } from "react-icons/im";
import { ADMIN } from "../constants/roles";
import { booksApi } from "../services/booksApi";

function Review(props: IReview) {
  const [cookies] = useCookies(["bc_role"]);

  const [deleteReview, { isSuccess }] = booksApi.useDeleteReviewMutation();

  useEffect(() => {
    if (isSuccess) {
      window.location.reload();
    }
  }, [isSuccess]);

  const handleDeleteReview = () => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteReview(props.id!);
    }
  };

  return (
    <div className="flex flex-row justify-start my-8">
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
        <p className="text-sm max-w-[50rem] break-words">{props.review}</p>
      </div>
      {cookies.bc_role === ADMIN && (
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

export default Review;
