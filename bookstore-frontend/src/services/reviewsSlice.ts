import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { IComment } from "../models/IComment";
import { IReview } from "../models/IReview";

export interface IReviewsState {
  reviews?: IReview[];
  reviewsFull?: IReview[];
  searchIndex: number;
}

const initialState: IReviewsState = {
  reviews: [],
  reviewsFull: [],
  searchIndex: 0,
};

export function sortReviewsByDesc(a: IReview, b: IReview) {
  return b.likes?.length! - a.likes?.length!;
}

export function sortReviewsByAsc(a: IReview, b: IReview) {
  return a.likes?.length! - b.likes?.length!;
}

export function sortReviewsAndCommentsByDateByDesc(
  a: IReview | IComment,
  b: IReview | IComment
) {
  return new Date(b.addedAt!).getTime() - new Date(a.addedAt!).getTime();
}

export function sortReviewsAndCommentsByDateByAsc(
  a: IReview | IComment,
  b: IReview | IComment
) {
  return new Date(a.addedAt!).getTime() - new Date(b.addedAt!).getTime();
}

export const reviewsSlice = createSlice({
  name: "reviews",
  initialState,
  reducers: {
    fillFullReviews(state, action: PayloadAction<IReview[]>) {
      state.reviews = action.payload;
      state.reviewsFull = action.payload;
      state.searchIndex = 0;
    },
    fillShownReviews(state, action: PayloadAction<IReview[]>) {
      state.reviews = action.payload;
    },
    fillSearchReviews(state, action: PayloadAction<IReview[]>) {
      state.reviewsFull = action.payload;
      state.searchIndex = state.searchIndex + 1;
    },
  },
});

export const reviewsActions = reviewsSlice.actions;
export const reviewsReducer = reviewsSlice.reducer;
