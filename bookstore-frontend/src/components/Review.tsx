import classNames from "classnames";
import React, { useEffect, useState } from "react";
import { AiTwotoneDislike, AiTwotoneLike } from "react-icons/ai";
import { FaUserCircle } from "react-icons/fa";
import { useAppSelector } from "../hooks/useAppSelector";
import { IReview } from "../models/IReview";
import { booksApi } from "../services/booksApi";
import { ImCross } from "react-icons/im";
import { ADMIN } from "../constants/roles";

function Review(props: IReview) {
  const { role } = useAppSelector((state) => state.user.user);

  const [deleteReview, { isSuccess }] = booksApi.useDeleteReviewMutation();
  const userId = useAppSelector((x) => x.user.user.id);
  const [reviewBg, setReviewBg] = useState("");
  const [addLike] = booksApi.useLikeReviewMutation();
  const [addDislike] = booksApi.useDislikeReviewMutation();

  const [likes, setLikes] = useState(props.likes);
  const [dislikes, setDislikes] = useState(props.dislikes);
  const [isLiked, setIsLiked] = useState(false);
  const [isDisliked, setIsDisliked] = useState(false);

  useEffect(() => {
    switch (props.type) {
      case 1:
        setReviewBg("bg-[#b7e4c7]");
        break;
      case 2:
        setReviewBg("bg-[#f7cad0]");
        break;
      case 3:
        setReviewBg("bg-[#e0e1dd]");
        break;
    }
  }, []);

  useEffect(() => {
    if (isSuccess) {
      window.location.reload();
    }
  }, [isSuccess]);

  const handleLikeClick = () => {
    if (!userId) {
      return;
    }

    if (likes?.find((x) => x === userId) || isLiked) {
      return;
    }

    addLike(props.id!);
    setLikes((prev) => [...prev!, userId]);
    setIsLiked(true);

    if (dislikes?.find((x) => x === userId)) {
      setDislikes(dislikes.filter((x) => x !== userId));
      setIsDisliked(false);
    }
  };
  const handleDislikeClick = () => {
    if (!userId) {
      return;
    }

    if (dislikes?.find((x) => x === userId) || isDisliked) {
      return;
    }

    addDislike(props.id!);
    setDislikes((prev) => [...prev!, userId]);
    setIsDisliked(true);

    if (likes?.find((x) => x === userId)) {
      setLikes(likes.filter((x) => x !== userId));
      setIsLiked(false);
    }
  };

  const handleDeleteReview = () => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteReview(props.id!);
    }
  };

  return (
    <div
      className={classNames(
        "flex flex-row justify-start w-[50rem] p-5 h-fit",
        reviewBg
      )}
    >
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
        <h2 className="font-bold text-lg my-4">{props.title}</h2>
        <p className="text-sm max-w-[40rem] break-words whitespace-pre-wrap">
          {props.body}
        </p>
        <div className="flex flex-row gap-x-2 justify-end mt-8">
          {(dislikes?.find((x) => x === userId) || isDisliked) && (
            <span>You disliked</span>
          )}
          {(likes?.find((x) => x === userId) || isLiked) && (
            <span>You liked</span>
          )}
          <AiTwotoneLike
            className="text-[#25a18e] text-xl cursor-pointer hover:translate-y-[-3px] but-anim"
            onClick={handleLikeClick}
          />
          <span>{likes?.length}</span>
          <AiTwotoneDislike
            className="text-[#ba181b] text-xl cursor-pointer hover:translate-y-[-3px] but-anim"
            onClick={handleDislikeClick}
          />
          <span>{dislikes?.length}</span>
        </div>
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

export default Review;
