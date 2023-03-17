import React, { useEffect, useState } from "react";
import { AiOutlinePlusCircle } from "react-icons/ai";
import { HashLink } from "react-router-hash-link";
import Comment from "../components/Comment";
import { IComment } from "../models/IComment";
import { IReview } from "../models/IReview";
import { booksApi } from "../services/booksApi";
import CommentInput from "./CommentInput";
import Modal from "./Modal";
import ModalComments from "./ModalComments";
import ModalReviews from "./ModalReviews";
import Review from "./Review";
import ReviewInput from "./ReviewInput";

interface OneBookFeedbackProps {
  id: string;
  handleShowAlert: () => void;
}

function OneBookFeedback({ id, handleShowAlert }: OneBookFeedbackProps) {
  const [isCommentModal, setCommentModal] = useState(false);
  const [isReviewModal, setReviewModal] = useState(false);
  const [comments, setComments] = useState<IComment[]>([]);
  const [reviews, setReviews] = useState<IReview[]>([]);

  const { data: commentsData } = booksApi.useGetCommentsQuery({
    bookId: id,
    amount: 4,
  });

  const { data: reviewsData } = booksApi.useGetReviewsQuery({
    bookId: id,
    amount: 4,
  });

  useEffect(() => {
    setComments(commentsData?.body!);
  }, [commentsData]);

  useEffect(() => {
    setReviews(reviewsData?.body!);
  }, [reviewsData]);

  const onReviewClose = () => setReviewModal(false);
  const onCommentClose = () => setCommentModal(false);

  const handleShowAllComments = () => {
    setCommentModal(true);
  };

  const handleShowAllReviews = () => {
    setReviewModal(true);
  };

  return (
    <>
      <Modal
        visible={isReviewModal}
        title="Reviews"
        content={<ModalReviews id={id} />}
        onClose={onReviewClose}
      />
      <Modal
        visible={isCommentModal}
        title="Comments"
        content={<ModalComments id={id} />}
        onClose={onCommentClose}
      />
      <div className="flex flex-col my-6 mx-16" id="comments">
        <p
          className="mt-5 mb-5 text-xl lg:text-3xl cursor-pointer hover:underline w-fit"
          onClick={handleShowAllComments}
        >
          Comments &#62;
        </p>
        <div className="mb-10 flex flex-row gap-x-5 flex-wrap">
          {comments ? (
            comments.map((item) => <Comment {...item} key={item.id} />)
          ) : (
            <p>No comments added</p>
          )}
        </div>
        <CommentInput id={id} handleShowAlert={handleShowAlert} />
      </div>
      <div className="flex flex-col bg-[#f3f3f3] px-16 pb-6">
        <p
          className="mt-5 text-xl lg:text-3xl cursor-pointer hover:underline w-fit"
          onClick={handleShowAllReviews}
        >
          Reviews &#62;
        </p>
        <HashLink smooth to="#reviewInput">
          <button className="mt-5 w-fit flex flex-row items-center bg-white px-4 py-2 rounded-lg hover:animate-wiggle">
            <AiOutlinePlusCircle />
            <p className="ml-2">Write a review</p>
          </button>
        </HashLink>
        <div className="mt-10 mb-10 flex flex-col gap-y-4">
          {reviews ? (
            reviews.map((item) => <Review {...item} key={item.id} />)
          ) : (
            <p>No reviews added</p>
          )}
        </div>
        <ReviewInput id={id} handleShowAlert={handleShowAlert} />
      </div>
    </>
  );
}

export default OneBookFeedback;
