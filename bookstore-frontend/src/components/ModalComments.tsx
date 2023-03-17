import React, { useEffect, useState } from "react";
import { IComment } from "../models/IComment";
import { booksApi } from "../services/booksApi";
import Comment from "../components/Comment";
import { FaSortAmountDown, FaSortAmountUpAlt } from "react-icons/fa";
import {
  sortReviewsAndCommentsByDateByAsc,
  sortReviewsAndCommentsByDateByDesc,
} from "../services/reviewsSlice";

interface ModalCommentsProps {
  id: string;
}

function ModalComments({ id }: ModalCommentsProps) {
  const [comments, setComments] = useState<IComment[]>([]);
  const [byDesc, setByDesc] = useState(true);

  const { data: commentsData } = booksApi.useGetCommentsQuery({
    bookId: id,
  });

  useEffect(() => {
    setComments(
      commentsData?.body.slice().sort(sortReviewsAndCommentsByDateByDesc)!
    );
  }, [commentsData]);

  useEffect(() => {
    if (comments.length > 0) {
      if (byDesc) {
        setComments(comments.slice().sort(sortReviewsAndCommentsByDateByDesc));
      } else {
        setComments(comments.slice().sort(sortReviewsAndCommentsByDateByAsc));
      }
    }
  }, [byDesc]);

  return (
    <div className="flex flex-col my-6 mx-16">
      <div className="flex flex-row gap-x-4 items-center">
        {byDesc ? (
          <FaSortAmountDown
            className="cursor-pointer text-lg"
            onClick={() => setByDesc(!byDesc)}
          />
        ) : (
          <FaSortAmountUpAlt
            className="cursor-pointer text-lg"
            onClick={() => setByDesc(!byDesc)}
          />
        )}
        <p>Most recent</p>
      </div>
      <div className="mt-10 mb-10 flex flex-row gap-y-5 gap-x-5 flex-wrap">
        {comments ? (
          comments.map((item) => <Comment {...item} key={item.id} />)
        ) : (
          <p>No comments added</p>
        )}
      </div>
    </div>
  );
}

export default ModalComments;
