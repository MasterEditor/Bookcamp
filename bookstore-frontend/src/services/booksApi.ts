import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { IBook } from "../models/IBook";
import { IGetBooksReq } from "../models/IGetBooksReq";
import { IResponse } from "../models/IResponse";
import { IComment } from "../models/IComment";
import { IReview } from "../models/IReview";

const BASE_URL = process.env.REACT_APP_SERVER_ENDPOINT as string;

const baseQuery = fetchBaseQuery({
  baseUrl: `${BASE_URL}/api/books/`,
  credentials: "include",
});

export const booksApi = createApi({
  reducerPath: "booksApi",
  baseQuery: baseQuery,
  endpoints: (build) => ({
    getBooks: build.query<IResponse<IBook[]>, IGetBooksReq>({
      query: (search) => ({
        url: `?page=${search?.page}&pageSize=${search?.pageSize}&keywords=${search?.keywords}&genre=${search?.genre}`,
        method: "GET",
      }),
      keepUnusedDataFor: 0.1,
    }),
    getGenres: build.query<IResponse<string[]>, void>({
      query: () => ({
        url: `genres`,
        method: "GET",
      }),
    }),
    getPages: build.query<IResponse<number>, number>({
      query: (pageSize) => ({
        url: `pages?pageSize=${pageSize}`,
        method: "GET",
      }),
    }),
    getBook: build.query<IResponse<IBook>, string>({
      query: (id) => ({
        url: `${id}`,
        method: "GET",
      }),
    }),
    getAuthorBooks: build.query<
      IResponse<IBook[]>,
      { author: string; id: string }
    >({
      query: ({ author, id }) => ({
        url: `author/${author}?id=${id}`,
        method: "GET",
      }),
    }),
    getComments: build.query<
      IResponse<IComment[]>,
      { bookId: string; amount?: number }
    >({
      query: (body) => ({
        url: `${body.bookId}/comments?amount=${body.amount ?? ""}`,
        method: "GET",
      }),
      keepUnusedDataFor: 0.0001,
    }),
    getReviews: build.query<
      IResponse<IReview[]>,
      { bookId: string; amount?: number }
    >({
      query: (body) => ({
        url: `${body.bookId}/reviews?amount=${body.amount ?? ""}`,
        method: "GET",
      }),
      keepUnusedDataFor: 0.0001,
    }),
    addComment: build.mutation<void, { comment: string; bookId: string }>({
      query: (body) => ({
        url: `comment`,
        method: "POST",
        body,
      }),
    }),
    addReview: build.mutation<void, IReview>({
      query: (body) => ({
        url: `review`,
        method: "POST",
        body,
      }),
    }),
    getComment: build.query<IResponse<boolean>, string>({
      query: (bookId) => ({
        url: `${bookId}/user-comment`,
        method: "GET",
      }),
    }),
    getReview: build.query<IResponse<boolean>, string>({
      query: (bookId) => ({
        url: `${bookId}/user-review`,
        method: "GET",
      }),
    }),
    addRating: build.mutation<void, { rate: number; bookId: string }>({
      query: (body) => ({
        url: `rate`,
        method: "POST",
        body,
      }),
    }),
    getRating: build.query<IResponse<number>, string>({
      query: (bookId) => ({
        url: `rate?bookId=${bookId}`,
        method: "GET",
      }),
    }),
    getFavourites: build.query<IResponse<IBook[]>, void>({
      query: () => ({
        url: `favourites`,
        method: "GET",
      }),
      keepUnusedDataFor: 1,
    }),
    getNewBooks: build.query<IResponse<IBook[]>, number>({
      query: (num) => ({
        url: `new-books?number=${num}`,
        method: "GET",
      }),
    }),
    getTopRateBooks: build.query<IResponse<IBook[]>, number>({
      query: (num) => ({
        url: `top-rate?number=${num}`,
        method: "GET",
      }),
    }),
    addBook: build.mutation<void, FormData>({
      query: (body) => ({
        url: `book`,
        method: "POST",
        body,
      }),
    }),
    deleteComment: build.mutation<void, string>({
      query: (id) => ({
        url: `comment/${id}`,
        method: "DELETE",
      }),
    }),
    deleteReview: build.mutation<void, string>({
      query: (id) => ({
        url: `review/${id}`,
        method: "DELETE",
      }),
    }),
    deleteBook: build.mutation<void, string>({
      query: (id) => ({
        url: `${id}`,
        method: "DELETE",
      }),
    }),
    deleteFragment: build.mutation<void, { id: string; ext: string }>({
      query: (body) => ({
        url: `${body.id}/delete-fragment/${body.ext}`,
        method: "DELETE",
      }),
    }),
    updateFragment: build.mutation<
      IResponse<string>,
      { id: string; data: FormData }
    >({
      query: (body) => ({
        url: `${body.id}/update-fragment`,
        method: "POST",
        body: body.data,
      }),
    }),
    updateCover: build.mutation<
      IResponse<string>,
      { id: string; data: FormData }
    >({
      query: (body) => ({
        url: `${body.id}/update-cover`,
        method: "POST",
        body: body.data,
      }),
    }),
    addNewFragment: build.mutation<
      IResponse<string>,
      { id: string; data: FormData }
    >({
      query: (body) => ({
        url: `${body.id}/add-fragment`,
        method: "POST",
        body: body.data,
      }),
    }),
    likeReview: build.mutation<void, string>({
      query: (reviewId) => ({
        url: `review/${reviewId}/like`,
        method: "POST",
      }),
    }),
    dislikeReview: build.mutation<void, string>({
      query: (reviewId) => ({
        url: `review/${reviewId}/dislike`,
        method: "POST",
      }),
    }),
    searchReviews: build.query<
      IResponse<IReview[]>,
      { bookId: string; keywords: string }
    >({
      query: (body) => ({
        url: `reviews/search/${body.bookId}?keywords=${body.keywords}`,
        method: "GET",
      }),
      keepUnusedDataFor: 0.0001,
    }),
  }),
});
