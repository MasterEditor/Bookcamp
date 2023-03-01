import React, { useEffect, useState } from "react";
import { FaUserCircle } from "react-icons/fa";
import Comment from "../components/Comment";
import { useAppSelector } from "../hooks/useAppSelector";
import { IComment } from "../models/IComment";
import { booksApi } from "../services/booksApi";
import TextAreaInput from "./TextAreaInput";

interface OneBookFeedbackProps {
  id: string;
  handleShowAlert: () => void;
}

function OneBookFeedback({ id, handleShowAlert }: OneBookFeedbackProps) {
  const [comments, setComments] = useState<IComment[]>([]);

  const { data: commentsData } = booksApi.useGetCommentsQuery({
    bookId: id!,
    amount: 4,
  });

  useEffect(() => {
    setComments(commentsData?.body!);
  }, [commentsData]);

  return (
    <div className="flex flex-col mt-6 mx-16" id="comments">
      <p className="mt-5 mb-10 text-xl lg:text-3xl">Comments</p>
      <TextAreaInput id={id} handleShowAlert={handleShowAlert} />
      <div className="mt-10">
        {comments ? (
          comments.map((item) => <Comment {...item} key={item.id} />)
        ) : (
          <p>No comments added</p>
        )}
      </div>
      <button className="">Show all comments</button>
    </div>
  );
}

export default OneBookFeedback;
