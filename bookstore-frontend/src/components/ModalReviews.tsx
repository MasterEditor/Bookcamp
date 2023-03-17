import classNames from "classnames";
import React, { useEffect, useState } from "react";
import { FaSearch, FaSortAmountDown, FaSortAmountUpAlt } from "react-icons/fa";
import { useActions } from "../hooks/useActions";
import { useAppSelector } from "../hooks/useAppSelector";
import { ReviewType } from "../models/ReviewType";
import { booksApi } from "../services/booksApi";
import {
  sortReviewsByAsc,
  sortReviewsAndCommentsByDateByAsc,
  sortReviewsAndCommentsByDateByDesc,
  sortReviewsByDesc,
} from "../services/reviewsSlice";
import Review from "./Review";

interface ModalReviewsProps {
  id: string;
}

function ModalReviews({ id }: ModalReviewsProps) {
  const { fillFullReviews, fillShownReviews, fillSearchReviews } = useActions();
  const { reviews, reviewsFull, searchIndex } = useAppSelector(
    (y) => y.reviews
  );
  const [byDesc, setByDesc] = useState(true);
  const [option, setOption] = useState("1");
  const [chosenType, setChosenType] = useState(0);
  const [search, setSearch] = useState("");
  const [searchReviews, { isSuccess, data }] =
    booksApi.useLazySearchReviewsQuery();

  const { data: reviewsData } = booksApi.useGetReviewsQuery({
    bookId: id,
  });

  useEffect(() => {
    fillFullReviews(reviewsData?.body!.slice()!.sort(sortReviewsByDesc)!);
  }, [reviewsData]);

  useEffect(() => {
    setByOption();
  }, [byDesc]);

  useEffect(() => {
    setByOption();
  }, [option]);

  useEffect(() => {
    if (isSuccess) {
      fillSearchReviews(data?.body.slice()!);
    }
  }, [isSuccess, data]);

  useEffect(() => {
    if (searchIndex > 0) {
      if (chosenType === 0) {
        setByOptionFull();
      } else {
        setByOptionAndType(chosenType);
      }
    }
  }, [searchIndex]);

  const handleReviewTypeClick = (value: ReviewType) => {
    if (chosenType == value) {
      setByOptionFull();
      setChosenType(0);
    } else {
      setByOptionAndType(value);
      setChosenType(value);
    }
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    searchReviews({ keywords: search, bookId: id });
  };

  function setByOption() {
    switch (option) {
      case "1":
        if (byDesc) {
          fillShownReviews(reviews!.slice().sort(sortReviewsByDesc));
        } else {
          fillShownReviews(reviews!.slice().sort(sortReviewsByAsc));
        }
        break;
      case "2":
        if (byDesc) {
          fillShownReviews(
            reviews!.slice().sort(sortReviewsAndCommentsByDateByDesc)
          );
        } else {
          fillShownReviews(
            reviews!.slice().sort(sortReviewsAndCommentsByDateByAsc)
          );
        }
        break;
    }
  }

  function setByOptionFull() {
    switch (option) {
      case "1":
        if (byDesc) {
          fillShownReviews(reviewsFull!.slice().sort(sortReviewsByDesc));
        } else {
          fillShownReviews(reviewsFull!.slice().sort(sortReviewsByAsc));
        }
        break;
      case "2":
        if (byDesc) {
          fillShownReviews(
            reviewsFull!.slice().sort(sortReviewsAndCommentsByDateByDesc)
          );
        } else {
          fillShownReviews(
            reviewsFull!.slice().sort(sortReviewsAndCommentsByDateByAsc)
          );
        }
        break;
    }
  }

  function setByOptionAndType(value: ReviewType) {
    switch (option) {
      case "1":
        if (byDesc) {
          fillShownReviews(
            reviewsFull!
              .slice()
              .filter((x) => x.type === value)
              .sort(sortReviewsByDesc)
          );
        } else {
          fillShownReviews(
            reviewsFull!
              .slice()
              .filter((x) => x.type === value)
              .sort(sortReviewsByAsc)
          );
        }
        break;
      case "2":
        if (byDesc) {
          fillShownReviews(
            reviewsFull!
              .slice()
              .filter((x) => x.type === value)
              .sort(sortReviewsAndCommentsByDateByDesc)
          );
        } else {
          fillShownReviews(
            reviewsFull!
              .slice()
              .filter((x) => x.type === value)
              .sort(sortReviewsAndCommentsByDateByAsc)
          );
        }
        break;
    }
  }

  return (
    <div className="flex flex-col my-6 mx-16">
      <form className="relative w-full mb-4" onSubmit={handleSubmit}>
        <input
          type="search"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          id="searchModal"
          placeholder="Search..."
          className="h-[2.5rem] text-sm w-[19rem] bg-white rounded-lg focus:outline-none focus:ring-[0.1rem] focus:ring-slate-600"
        />
        <button type="submit" className="absolute left-[17rem] top-[0.7rem]">
          <FaSearch className="text-[#1d3557] but-anim text-lg hover:translate-y-[-0.1rem]" />
        </button>
      </form>
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
        <select
          className="bg-gray-50 text-base w-40 border border-gray-300 text-gray-900 text-sm rounded-lg"
          value={option}
          onChange={(x) => setOption(x.target.value)}
        >
          <option value={1}>Top reviews</option>
          <option value={2}>Most recent</option>
        </select>
        <p
          className={classNames(
            "bg-[#b7e4c7] px-4 py-2 rounded-xl cursor-pointer hover:animate-wiggle",
            chosenType === ReviewType.Positive && "border border-gray-800"
          )}
          onClick={() => handleReviewTypeClick(ReviewType.Positive)}
        >
          Positive
        </p>
        <p
          className={classNames(
            "bg-[#f7cad0] px-4 py-2 rounded-xl cursor-pointer hover:animate-wiggle",
            chosenType === ReviewType.Negative && "border border-gray-800"
          )}
          onClick={() => handleReviewTypeClick(ReviewType.Negative)}
        >
          Negative
        </p>
        <p
          className={classNames(
            "bg-[#e0e1dd] px-4 py-2 rounded-xl cursor-pointer hover:animate-wiggle",
            chosenType === ReviewType.Neutral && "border border-gray-800"
          )}
          onClick={() => handleReviewTypeClick(ReviewType.Neutral)}
        >
          Neutral
        </p>
      </div>
      <div className="mt-10 mb-10 flex flex-col gap-y-4">
        {reviews ? (
          reviews.map((item) => <Review {...item} key={item.id} />)
        ) : (
          <p>No reviews added</p>
        )}
      </div>
    </div>
  );
}

export default ModalReviews;
