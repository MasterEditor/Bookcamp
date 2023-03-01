import React, { useEffect, useState } from "react";
import { createSearchParams, useNavigate } from "react-router-dom";
import { IBook } from "../models/IBook";
import { booksApi } from "../services/booksApi";
import SingleBook from "./SingleBook";

interface FromAuthorProps {
  id: string;
  author: string;
}

function FromAuthor({ id, author }: FromAuthorProps) {
  const { data: authorBooksData } = booksApi.useGetAuthorBooksQuery({
    author,
    id,
  });

  const navigate = useNavigate();
  const [authorBooks, setAuthorBooks] = useState<IBook[]>([]);

  useEffect(() => {
    setAuthorBooks(authorBooksData?.body!);
  }, [authorBooksData]);

  const handleBookClick = (bookId: string) => {
    navigate(`/book/${bookId}`);
  };

  const handleAuthorBooks = () => {
    navigate({
      pathname: "/books",
      search: `?${createSearchParams({ keywords: author })}`,
    });
  };

  return (
    <>
      <div className="flex flex-col mt-6 px-16 border-b border-[#ced4da] pb-10 bg-[#ededed]">
        <p className="mt-5 mb-10 text-xl lg:text-3xl">From the publisher</p>
        <div className="flex w-full h-full flex-row justify-center">
          {authorBooks &&
            authorBooks.map((item) => (
              <div
                className="basis-1/6 bg-[#f3f3f3] m-5 cursor-pointer but-anim
         hover:translate-y-[-0.5rem]"
                key={item.id}
                onClick={() => handleBookClick(item.id)}
              >
                <SingleBook {...item} />
              </div>
            ))}
        </div>
        <div className="flex w-full justify-end">
          <button
            className="learn-more text-sm mt-7 text-left w-[10rem]"
            onClick={handleAuthorBooks}
          >
            <span className="circle" aria-hidden="true">
              <span className="icon arrow"></span>
            </span>
            <span className="button-text">See all</span>
          </button>
        </div>
      </div>
    </>
  );
}

export default FromAuthor;
