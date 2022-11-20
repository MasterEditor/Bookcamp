import { Breadcrumb, Pagination } from "flowbite-react";
import React, { useEffect, useState } from "react";
import SingleBook from "./SingleBook";
import classNames from "classnames";
import { useNavigate, useSearchParams } from "react-router-dom";
import { booksApi } from "../services/booksApi";
import { IBook } from "../models/IBook";
import FadeLoader from "react-spinners/FadeLoader";

interface BooksCompProps {
  genre?: string;
  keywords?: string;
}

function BooksComp({ genre, keywords }: BooksCompProps) {
  const pageSize = 20;

  const [, setSearchParams] = useSearchParams();

  const [page, setPage] = useState(1);

  const [books, setBooks] = useState<IBook[]>([]);
  const [pages, setPages] = useState(0);

  const {
    isLoading: pagesLoading,
    isSuccess: pagesSuccess,
    data: pagesData,
  } = booksApi.useGetPagesQuery(pageSize);

  const {
    isLoading,
    isSuccess,
    data: genresData,
  } = booksApi.useGetGenresQuery();

  const [
    getBooks,
    { isLoading: booksLoading, isSuccess: booksSuccess, data: booksData },
  ] = booksApi.useLazyGetBooksQuery();

  const [genres, setGenres] = useState<string[]>([]);

  useEffect(() => {
    if (isSuccess) {
      setGenres(["All books", ...genresData.body!]);
      getBooks({ page, pageSize, keywords, genre });
    }
  }, [isLoading]);

  useEffect(() => {
    if (pagesSuccess) {
      setPages(pagesData.body!);
    }
  }, [pagesLoading]);

  useEffect(() => {
    if (booksSuccess) {
      setBooks(booksData?.body!);
    }
  }, [booksLoading, booksSuccess, booksData]);

  const navigate = useNavigate();

  const onPageChange = (page: number) => {
    setPage(page);
    getBooks({ page, pageSize, keywords, genre });
  };

  const initialBook = genres.findIndex((x) => x === genre);

  const [selectedBook, setSelectedBook] = useState(0);

  useEffect(() => {
    setSelectedBook(initialBook === -1 ? 0 : initialBook);
  }, [initialBook]);

  const handleBookClick = (bookId: string) => {
    navigate(`/book/${bookId}`);
  };

  const onGenreChanged = (genre: number) => {
    setSelectedBook(genre);
    setSearchParams({ genres: genre === 0 ? "" : genres[genre] });
    getBooks({
      page,
      pageSize,
      keywords,
      genre: genre === 0 ? "" : genres[genre],
    });
  };

  return (
    <>
      <Breadcrumb className="mt-5 ml-5">
        <Breadcrumb.Item>Books</Breadcrumb.Item>
        <Breadcrumb.Item>{genres[selectedBook]}</Breadcrumb.Item>
      </Breadcrumb>
      <div className="flex flex-row min-h-screen">
        <section
          className={classNames(
            books.length === 0 ? "mb-10" : "",
            "flex flex-col pt-10 w-[15rem] min-h-full"
          )}
        >
          {genres.map((item, key) => (
            <p
              onClick={() => onGenreChanged(key)}
              className={classNames(
                "pl-5 pt-5 cursor-pointer transition-opacity",
                selectedBook === key ? "font-bold" : "hover:opacity-60"
              )}
              key={key}
            >
              <span
                className={classNames(
                  selectedBook === key ? "inline" : "hidden",
                  "mr-1"
                )}
              >
                &mdash;
              </span>
              {item}
            </p>
          ))}
        </section>
        <div className="flex flex-col w-full">
          <h1 className="ml-[3rem] mt-10 text-[3rem]">All Books</h1>
          <div className="w-full flex flex-row flex-wrap justify-center">
            {booksLoading ? (
              <FadeLoader className="mt-16" />
            ) : books.length === 0 ? (
              <h1 className="text-left text-xl">No books found</h1>
            ) : (
              books.map((item) => (
                <div
                  className="basis-1/5 bg-[#f3f3f3] m-5 cursor-pointer but-anim
               hover:translate-y-[-0.5rem] hover:rotate-1 hover:shadow-[5px_5px_0px_0px_rgba(213,180,175)]"
                  key={item.id}
                  onClick={() => handleBookClick(item.id)}
                >
                  <SingleBook {...item} />
                </div>
              ))
            )}
          </div>
          <Pagination
            className="py-5 flex flex-col items-center"
            currentPage={page}
            onPageChange={onPageChange}
            showIcons={true}
            totalPages={pages}
          />
        </div>
      </div>
    </>
  );
}

export default BooksComp;
